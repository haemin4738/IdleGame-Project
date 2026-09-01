# IdleCA — 크레이지아케이드 테마 방치형 RPG

Unity 6로 제작한 포트폴리오용 방치형 RPG. 크레이지아케이드 캐릭터/비주얼 에셋을 활용해 사이드뷰 자동 전투 루프를 구현했습니다.

## 플레이 영상 / 스크린샷

> _(추가 예정)_

## 핵심 기능

| 기능 | 설명 |
|------|------|
| **자동 전투** | 캐릭터가 자동으로 몬스터를 공격, 처치 시 골드·경험치 획득 |
| **스테이지 진행** | 1-1 ~ 1-5 (5스테이지, 보스 포함), 클리어 시 다음 스테이지 자동 진행 |
| **업그레이드** | 골드로 ATK·HP·DEF·Speed·CritChance·CritDamage 강화 |
| **캐릭터 선택** | Cappi(전사) / Archer(궁수) / Mage(마법사) — 각자 다른 기본 스탯 |
| **스킬 시스템** | 패시브 스킬 7종 (ATK·DEF·HP·EXP·골드·크리티컬) |
| **펫 시스템** | 펫 5종, 각각 스탯 보너스 제공 |
| **장비 시스템** | 장비 6종, 부위별 스탯 보너스 |
| **업적 시스템** | 몬스터 처치·골드·레벨·스테이지·펫 보유 달성 조건 |
| **오프라인 보상** | 앱 종료 시간만큼 골드 자동 적립 (펫·스킬·장비 보너스 반영) |
| **저장 시스템** | JSON 자동 저장 (30초마다 + 앱 포즈/종료 시) |

## 기술 스택

- **엔진**: Unity 6 (6000.0.67f1), URP 2D
- **언어**: C#
- **아키텍처**: ScriptableObject 데이터 드리븐 + Manager 싱글톤 + EventBus (pub/sub)
- **저장**: JSON (`Application.persistentDataPath/save.json`)
- **에셋**: Crazy Arcade 캐릭터 스프라이트, 치비 몬스터 애니메이션 프리팹

## 아키텍처 개요

```
EventBus (pub/sub)
    │
    ├── BattleManager      공격 루프, 데미지 계산, 부활
    ├── StageManager       스테이지 진행, 몬스터 소환 단일 책임
    ├── CharacterManager   캐릭터 선택/구매
    ├── UpgradeManager     스탯 강화 관리
    ├── SkillManager       패시브 스킬
    ├── PetManager         펫 스탯 보너스
    ├── EquipmentManager   장비 스탯 보너스
    ├── AchievementManager 업적 달성 체크
    ├── OfflineManager     오프라인 보상 계산
    └── SaveManager        JSON 저장/불러오기
```

**주요 이벤트 흐름**

```
MonsterKilledEvent → StageManager → 다음 몬스터 소환 (스테이지 클리어 시 0.6s 딜레이)
                   → AchievementManager → 업적 체크

DamageEvent        → BattleView → HP바 갱신 + 데미지 팝업 + 물풍선 탄환 발사

StageChangedEvent  → BattleView → 스테이지 텍스트 + 캐릭터 워크 애니 + 0.6s 후 몬스터 교체
```

## 프로젝트 구조

```
Assets/
  Scripts/
    Core/           EventBus, GameManager, GameEvents
    Data/           ScriptableObject 정의 (CharacterData, MonsterData 등)
    Managers/       BattleManager, StageManager, SaveManager 등
    UI/             BattleView, UpgradePanel, CharacterPanel 등
    Editor/         CharacterAssetBuilder (AnimClip + Controller + Prefab 자동 생성)
  ScriptalbeObjects/
    CharacterData.asset, Character_Archer.asset, Character_Mage.asset
    Monsters/       Slime.asset, SlimeBoss.asset
    Stage/          Stage1_1 ~ Stage1_5.asset
    Pet_*.asset, Skill_*.asset, Equip_*.asset, *Achieve.asset
  CA_Assets/
    Character/      Cappi·Dao·Marid 스프라이트 (방향별 Idle·Walk)
    FX/             Bubble_1 (물풍선 탄환·피격 이펙트)
    Map/            floor.png, 배경 장식 스프라이트
  Prefabs/
    Characters/     CharacterAssetBuilder가 생성한 캐릭터 프리팹
  Scenes/
    Main.unity      로비·전투 통합 씬
```

## 실행 방법

Unity Hub에서 프로젝트 경로를 열고 `Scenes/Main.unity`를 실행합니다.

```bash
# 커맨드라인 빌드 (Windows)
"C:/Program Files/Unity/Hub/Editor/6000.0.67f1/Editor/Unity.exe" \
  -quit -batchmode \
  -projectPath "D:/Unity/Portpolio/IdleCA" \
  -buildWindowsPlayer "Build/IdleCA.exe" \
  -logFile "Build/build.log"
```

## 콘텐츠 현황

| 구분 | 항목 |
|------|------|
| 캐릭터 | Cappi(전사), Archer(궁수), Mage(마법사) |
| 스테이지 | 1-1 / 1-2 / 1-3 / 1-4 / 1-5(보스) |
| 몬스터 | Slime, SlimeBoss |
| 펫 | Cat, Dog, Dragon, Penguin, Tiger |
| 스킬 | AtkUp, DefUp, HpUp, ExpUp, GoldUp, CritChanceUp, CritDamageUp |
| 장비 | IronSword, FlameSword, IronShield, SteelArmor, AncientCharm, LuckyRing |
| 업적 | 몬스터처치, 골드획득, 레벨달성, 스테이지달성, 펫보유 |
