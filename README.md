# Project21
 게임콘텐츠과 2학년 1학기

# TO DO LIST

1. 적 랜덤 스폰 기능 추가 < 로그라이크 요소 추가
> 플레이어 일부 데이터를 저장하여 스테이지 클리어 여부에 따라 적 생성 제한 및 위치 플레이어 위치 변동
ex)1-N 클리어 시 씬 전환 될때 1-N 스테이지 전투 진입 시점 좌표 데이터로 플레이어 이동 1-(N ~ n) 스테이지의 적 생성 X 

# 버그 픽스 (예정)
1. 카드 중복 사용 버그
> 타겟 지정 카드 사용 후 타겟을 지정하지 않은 채로 다른 타겟 지정 카드를 사용하는 경우 가장 마지막에 사용한 카드의 효과만 적용됨. 마나는 그대로 소모함.
2. 적 턴 행동 양식 변경
> 현재 적 턴이 되면 모든 적이 한꺼번에 행동하는 상태, 이를 적 턴이 되면 적 하나씩 행동하도록 변경 예정


# 24.05.27
@cms
- 플레이어&적 스테이터스 클래스 생성
- 스킬(카드) 시스템 클래스 생성

# 24.05.28
@cms 
- 카드 리스트 추가 (스크립터블 오브젝트)
- 카드 뽑기 기능 및 펼치기 기능 추가
- 메인 메뉴 임시 추가
- 카드 크게 보기, 카드 드래그 기능 추가

# 24.05.29
@cms
- Enemy Entity 기획 및 추가
- 카드 데이터 조정 (스크립터블 내용 수정)

# 24.05.30
@cms
- Entity 관리를 통한 적 생성
- 적 생성 후 이미지 정렬
- 임시 턴 종료 버튼 추가
- 카드 세부 내용 추가 (스크립터블 오브젝트 추가 필요)
- 카드 사용 스크립트 수정

# 24.06.03
@cms
- Card 스크립터블 오브젝트 수정 및 CardManager를 통한 리스트 관리 방식 변경
- 카드 드래그&드랍을 통한 카드 사용 이벤트 제작, 카드 사용을 통한 플레이어, 적 스테이터스 변동 확인
- 적 체력 변동 값에 따른 체력바 UI 제작
- 플레이어 + 적의 턴제 매커니즘 정리
> 플레이어 턴 종료 > 적1 행동 > 적 2 행동> 적 턴 종료> 플레이어 턴 시작(반복)
- 오브젝트 풀링을 통한 카드 손패 생성 및 제거

# 24.06.05
@cms
- 임시 UI 추가
- 공격 카드 사용 시 임의의 적을 선택하기 전까지 사용 X, 선택 시 해당 적에게 카드 효과 발생
- 적 행동 추가 
> 공격 or 방어 행동만 가능
- 적 행동 전 어떤 행동을 할 지 이미지를 통한 사전 표시

# 24.06.08
@cms
- 적 죽음 애니메이션 추가
- 적 죽을 시 해당 게임 오브젝트+UI 제거, 관리 리스트에서 삭제

# 24.06.11
@cms
- 마나, 방어도, 체력 UI 추가 및 수정
- 마나 시스템 추가
- 현재 스테이터스에 따른 각 파라미터 UI 변동 추가
- 적 임시 방어도 UI 추가
- 플레이어 사망 & 적 전체 사망(스테이지 클리어) 임시 구현

# 24.06.12 ~ 24.06.15
@cms
- UIManager 스크립트 분할
- 중간 발표용 빌드 파일 제작
> 스테이지 -> 전투 돌입 루프 테스트
> 적 이미지 수정
> 적 행동 딜레이 조정
> 임시 bgm 추가
> 임시 카드 추가/삭제 이벤트 구현
> 카드 효과 테스트 및 추가
- 효과음 추가
> 적 피격, 공격, 죽음, 방어도
> 카드 뽑기, 사용, 선택
> 플레이어 피격, 공격, 방어, 힐, 승리 ,패배
- 단일공격 시 파티클 추가
- 턴 종료 버튼 추가
- 마나 부족, 나의 턴 알림창 추가

# 24.06.17
@cms
- 포스트 프로세싱 작업
- 빌드용 라이팅 설정 변경
- 전투 배경용 파티클 추가 (모래바람, 먼지 등)
- 적 구분용 하이라이트(윤곽선) 추가 및 타겟 설정 시 색상 변경
- 플레이어 피격 파티클 + 화면 흔들림 추가
- 전투 승리 / 패배 UI 수정
- 카드 하단 부분에 마우스 위치 시 반복적으로 카드가 확대되고 줄어드는 현상 수정