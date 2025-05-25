using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{   
    float hAxis;
    float vAxis;
    public float speed;
    public GameObject[] weapons;
    public bool[] hasWeapons;
    public GameObject[] grenades;
    public GameObject grenadeObj; 
    public int hasGrenades;
    public int maxHasGrenades;
    public Camera followCam;
    public int ammo;
    public int coin;
    public int health;
    public int score;
    
    public int maxAmmo;
    public int maxCoin;
    public int maxHealth;

    bool wDown;
    bool jDown;
    bool fDown;
    bool gDown;
    bool rDown;
    bool isJump;
    bool isDodge;
    bool iDown;
    bool sDown1;
    bool sDown2;
    bool sDown3;
    bool isSwap;
    bool isReload;
    bool isFireReady = true;
    bool isBorder;
    bool isDamage;

    Vector3 moveVec;
    Vector3 dodgeVec;
    Rigidbody rigid;
    Animator anim;
    MeshRenderer[] meshs;
    GameObject nearObject;
    public Weapon equipWeapon;
    int equipWeaponIndex = -1;
    float fireDelay;
    

    void Awake(){
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        Application.targetFrameRate = 60;
        meshs = GetComponentsInChildren<MeshRenderer>();
        PlayerPrefs.SetInt("MaxScore", 124);
    }


    // Update is called once per frame
    void Update()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");
        jDown = Input.GetButtonDown("Jump");
        iDown = Input.GetButtonDown("Interation");
        fDown = Input.GetButton("Fire1");
        gDown = Input.GetButtonDown("Fire2");
        rDown = Input.GetButtonDown("Reload");
        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
        sDown3 = Input.GetButtonDown("Swap3");

        moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        if(isDodge){
            moveVec = dodgeVec;
        }
        if(isSwap || isReload || !isFireReady){
            moveVec = Vector3.zero;
        }
        if(!isBorder){
            if(wDown){
                transform.position += moveVec * speed * 0.3f * Time.deltaTime;
            }else{        
                transform.position += moveVec * speed * Time.deltaTime;
            }
        }

        if(jDown && moveVec == Vector3.zero && !isJump && !isDodge && !isJump){
            rigid.AddForce(Vector3.up * 15, ForceMode.Impulse);
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump = true;
        }
        anim.SetBool("isRun", moveVec != Vector3.zero);
        anim.SetBool("isWalk", wDown);

        if(jDown && moveVec != Vector3.zero && !isJump && !isDodge && !isSwap){
            dodgeVec = moveVec;
            speed *= 2;
            anim.SetTrigger("doDodge");
            isDodge = true;

            Invoke("DodgeOut", 0.6f);
        }


        transform.LookAt(transform.position + moveVec);
        if(fDown){
        Ray ray = followCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayHit;
        
        if(Physics.Raycast(ray, out rayHit, 100)){
            Vector3 nextVec = rayHit.point - transform.position;
            nextVec.y = 0;
            transform.LookAt(transform.position + nextVec);
        }
        }
        if(iDown && nearObject != null && !isJump && !isDodge){
            if(nearObject.tag == "Weapon"){
                Item item = nearObject.GetComponent<Item>();
                int weaponIndex = item.value;
                hasWeapons[weaponIndex] = true;

                Destroy(nearObject);
            }
        }
        if(health <= 0){
            GameOver();
        }
    
    
        Swap();
        SwapOut();
        Attack();
        Reload();
        Grenade();
    }

    void DodgeOut(){
        speed *= 0.5f;
        isDodge = false;
    }

    void GameOver(){

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    void Reload(){
        if(equipWeapon == null){
            return;
        }
        if(equipWeapon.type == Weapon.Type.Melee){
            return;
        }
        if(ammo == 0){
            return;
        }
        if(rDown && !isJump && !isDodge && ! !isSwap && !isFireReady){
            anim.SetTrigger("doReload");
            isReload = true;

            Invoke("ReloadOut", 1.5f);
        }
    }

    void ReloadOut(){
        int reAmmo = ammo < equipWeapon.maxAmmo ? ammo : equipWeapon.maxAmmo;
        equipWeapon.curAmmo = reAmmo;
        ammo -= reAmmo;
        isReload = false;
    }

    void Grenade(){
        if(hasGrenades == 0)
            return;
        if(gDown && !isReload && !isSwap){
            Ray ray = followCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;
        
            if(Physics.Raycast(ray, out rayHit, 100)){
            Vector3 nextVec = rayHit.point - transform.position;
            nextVec.y = 10;
            
            GameObject instantGrenade = Instantiate(grenadeObj, transform.position, transform.rotation);
            Rigidbody rigidGrenade = instantGrenade.GetComponent<Rigidbody>();
            rigidGrenade.AddForce(nextVec, ForceMode.Impulse);
            rigidGrenade.AddTorque(Vector3.back * 10 , ForceMode.Impulse);

            hasGrenades--;
            grenades[hasGrenades].SetActive(false);
            }
        }
    }
    

    void FreezeRotation(){
        rigid.angularVelocity = Vector3.zero;
    }

    void StopToWall(){
        Debug.DrawRay(transform.position, transform.forward *5 ,Color.green);
        isBorder = Physics.Raycast(transform.position, transform.forward, 5, LayerMask.GetMask("Wall"));
    }
    void FixedUpdate(){
        FreezeRotation();
        StopToWall();
    }
    void OnCollisionEnter(Collision collision){
        if(collision.gameObject.tag == "Floor"){
            anim.SetBool("isJump", false);
            isJump = false;
        }
    }
    
    void OnTriggerEnter(Collider other){
        if(other.tag == "Item"){
            Item item = other.GetComponent<Item>();
            switch(item.type){
                case Item.Type.Ammo:
                    ammo += item.value;
                    if(ammo > maxAmmo){
                        ammo = maxAmmo;
                    }
                    break;
                case Item.Type.Coin:
                    coin += item.value;
                    if(coin > maxCoin){
                        coin = maxCoin;
                    }
                    break;
                case Item.Type.Heart:
                    health += item.value;
                    if(health > maxHealth){
                        health = maxHealth;
                    }
                    break;
                case Item.Type.Grenade:
                    if(hasGrenades == maxHasGrenades)
                        return;
                    grenades[hasGrenades].SetActive(true);
                    hasGrenades += item.value;
                    Debug.Log($"Before Adding: hasGrenades = {hasGrenades}, Item Value = {item.value}");
                    break;
            }
            Destroy(other.gameObject);
        }
        else if (other.tag == "EnemyBullet"){
            if(!isDamage){
                Bullet enemyBullet = other.GetComponent<Bullet>();
                health -= enemyBullet.damage;
                if(other.GetComponent<Rigidbody>() != null){
                    Destroy(other.gameObject);
                }
                StartCoroutine(OnDamage());
            }
        }
    }

    IEnumerator OnDamage(){
        isDamage = true;
        foreach(MeshRenderer mesh in meshs){
            mesh.material.color = Color.yellow;
        }
        yield return new WaitForSeconds(0.5f);
        isDamage = false;
        foreach(MeshRenderer mesh in meshs){
            mesh.material.color = Color.white;
        }
    }
    void OnTriggerStay(Collider other){
        if(other.tag == "Weapon"){
            nearObject = other.gameObject;
        }
    }
    void OnTriggerExit(Collider other){
        if(other.tag == "Weapon"){
            nearObject = null;
        }
    }

    void Swap(){
        if(sDown1 && (!hasWeapons[0] || equipWeaponIndex==0 )){
            return;
        }
        if(sDown2 && (!hasWeapons[1] || equipWeaponIndex==1 )){
            return;
        }
        if(sDown3 && (!hasWeapons[2] || equipWeaponIndex==2 )){
            return;
        }
        int weaponIndex = -1;
        if(sDown1) weaponIndex =0;
        if(sDown2) weaponIndex =1;
        if(sDown3) weaponIndex =2;
        if((sDown1 || sDown2 || sDown3) && !isJump && !isDodge){
            if(equipWeapon != null){
                equipWeapon.gameObject.SetActive(false);
            }
            equipWeaponIndex = weaponIndex;
            equipWeapon = weapons[weaponIndex].GetComponent<Weapon>();
            equipWeapon.gameObject.SetActive(true);
            
            anim.SetTrigger("doSwap");
            isSwap = true;
            Invoke("SwapOut", 0.4f);
        }
    }
    void SwapOut(){
        isSwap = false;
    }

    void Attack(){
        if(equipWeapon == null){
            return;
        }
        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < fireDelay;

        if(fDown && isFireReady && !isDodge && !isSwap){
            equipWeapon.Use();
            anim.SetTrigger(equipWeapon.type == Weapon.Type.Melee ? "doSwing" : "doShot");
            fireDelay = 0;
        }
    }
}
