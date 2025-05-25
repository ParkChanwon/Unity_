# 게임제작기초실습 프로젝트 - Quad Action
<img width="977" alt="Image" src="https://github.com/user-attachments/assets/07c5d95d-b282-4621-a0cf-3ff6ae785784" />


---
## 게임소개 
`Quad Action`는 Unity 기반으로 게임 제작 기초 실습을 학습하며 만든 **프로젝트**입니다.  
핵심 목표는
- 짧은 플레이 타임의 손쉬운 플레이 게임
- 캐릭터를 직접 움직이며 사냥하여 성취감 획득
- 단순하지만 높은 게임성
- 몬스터별 난이도 설정


</br>


##  게임 특징

-  각기 다른 능력을 가진 적으로 난이도 조절
-  권총, 라이플 등 다양한 무기를 활용하여 적을 소탕 
-  사운드 이펙트, 무기의 파티클 효과
- 걷기, 달리기, 구르기로 다양한 애니메이션을 적용해 적의 공격을 회피하며 공격

</br>

<img width="253" alt="Image" src="https://github.com/user-attachments/assets/dc517d97-8924-4cbf-b715-28988f4b4532" />

왼쪽 상단 부터 시계 방향으로 적, 무기, 캐릭터, 움직임입니다.

<img width="300" alt="Image" src="https://github.com/user-attachments/assets/be9a033f-86d5-4a31-88bd-3d3eab59918b" />

적은 각기 다른 능력을 가지고 있으며, 초록색 적의 경우 플레이어를 향해 다가오기만 하지만, 보라색 적은 빠르게 달려오며, 노란색 적은 원거리에서 미사일을 발사합니다.

<img width="1028" alt="image" src="https://github.com/user-attachments/assets/6c595c98-55e2-4875-8a50-f1a49704fbe5" />

시작 메뉴입니다. 화면 중앙에는 로고와 함께 전에 플레이 했던 최고 기록을 보여주며, 하단에는 Game Start 버튼이 있고, 누르게 되면 바로 게임 시작이 됩니다.

<img width="1030" alt="image" src="https://github.com/user-attachments/assets/007825ea-c7ea-4cf5-a7e8-c943cdd9c091" />

맵을 돌아다니면 각기 스폰되는 무기를 획득하여 다가오는, 공격하는 적들을 물리쳐야 합니다.

<img width="1039" alt="image" src="https://github.com/user-attachments/assets/d5abc66e-948e-4a26-9cc2-09067269d2e4" />

화면 왼쪽 하단을 보면 생명력과 총알의 개수, 적에게 떨어진 코인을 한 눈에 볼 수 있으며 화면 오른쪽 위에는 스테이지와 함께 경과된 시간을 표시해줍니다.

<img width="962" alt="image" src="https://github.com/user-attachments/assets/291e3a49-2d60-4558-b46a-25cb31e458ef" />

던진 무기에서 파티클 효과가 나타나는 것을 확인할 수 있습니다.

</br>

## 프로젝트 핵심 구현 코드

<img width="738" alt="image" src="https://github.com/user-attachments/assets/c01c18c4-e579-4949-8337-7c8c0d5f178c" />

<img width="983" alt="image" src="https://github.com/user-attachments/assets/2cba8b64-d04b-4e46-8f23-f461d39e875d" />

isWalk, isRun 등 Bool/Trigger로 행동 전환을 제어하였으며, doShot, doReload같은 트리거를 사용하여 공격, 재장전 등의 애니메이션을 사용하였습니다.
Idle이 기본 상태이며, Walk와 Run은 언제든 전환될 수 있습니다. 
공격, 스왑, 재장전 등 애니메이션이 끝나면 원래 다시 기본 상태로 돌아오게 하였습니다.

<img width="753" alt="image" src="https://github.com/user-attachments/assets/866470a0-5af4-4f4c-bc62-c0a466b22c87" />

벽에 닿아서 튕겨나가는 현상을 막고자 isBorder를 사용하였으며, W키를 누르는 것에 따라 속도를 조절하고, 움직이지 않고 점프를 하면 위로 점프를 하고 
잔여 총알이 0일 때 Reload를 하게 하습니다.

<img width="363" alt="image" src="https://github.com/user-attachments/assets/366a7b77-4654-452f-afae-57e188ac3390" />
<img width="785" alt="image" src="https://github.com/user-attachments/assets/9fd5f2e5-da26-4366-aa0a-388d8e482398" />
<img width="321" alt="image" src="https://github.com/user-attachments/assets/77c2e8a7-5be0-4bb2-8ced-b8f241fdc5da" />


수류탄을 사용할 때에는 마우스포인터로부터 Raycast를 계산하여 투사체 방향을 계산하였으며, Rigidbody에 Impulse 힘과 회전 토크를 주어 자연스러운 낙하 궤적을 만들었습니다.
숫자키 1,2,3 등을 눌러 획득한 무기를 번갈아가며 사용할 수 있습니다.
또한 적에게는 공격 범위와, 타겟팅범위를 다르게 주어 근접 ~ 장거리를 설정하였습니다.



## 프로젝트 시연 영상 

[프로젝트 시연 영상](https://youtu.be/oG8DHEGL7q4)
