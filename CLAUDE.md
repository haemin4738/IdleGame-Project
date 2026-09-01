# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## 기술 스택

- **언어**: C# (Unity)
- **엔진**: Unity 6 (6000.0.67f1), URP 2D
- **저장**: JSON (`Application.persistentDataPath/save.json`)
- **아키텍처**: ScriptableObject 기반 데이터 드리븐 + Manager 싱글톤 패턴 + EventBus (pub/sub)
- **목적**: 포트폴리오 — 크레이지아케이드 테마 방치형 RPG

## 빌드 및 실행

```bash
# Unity Editor에서 열기 (Unity Hub 사용)
# 프로젝트 경로: D:/Unity/Portpolio/IdleCA
# 씬: Assets/Scenes/Main.unity

# 커맨드라인 빌드 (Windows)
"C:/Program Files/Unity/Hub/Editor/6000.0.67f1/Editor/Unity.exe" \
  -quit -batchmode \
  -projectPath "D:/Unity/Portpolio/IdleCA" \
  -buildWindowsPlayer "Build/IdleCA.exe" \
  -logFile "Build/build.log"
```

## 프로젝트 구조

```
Assets/
  Scripts/
    Core/           GameManager, EventBus, GameEvents
    Data/           ScriptableObject 정의 (CharacterData, MonsterData 등)
    Managers/       BattleManager, StageManager, SaveManager 등
    UI/             BattleView, UpgradePanel, CharacterPanel 등
    Editor/         CharacterAssetBuilder (AnimClip + Controller + Prefab 자동 생성)
  ScriptalbeObjects/   ← 폴더명 오타 있음 (변경 금지 — Unity GUID 참조 중)
    CharacterData.asset, Character_Archer.asset, Character_Mage.asset
    Monsters/       Slime.asset, SlimeBoss.asset
    Stage/          Stage1_1 ~ Stage1_5.asset
    Pet_*.asset, Skill_*.asset, Equip_*.asset, *Achieve.asset
  CA_Assets/
    Character/      Cappi·Dao·Marid 스프라이트 (방향별 Idle·Walk 각 4프레임)
    FX/             Bubble_1/ (default.png 탄환, Explosion.png 피격 이펙트)
    Map/            floor.png (바닥), 배경 장식 스프라이트
  Prefabs/
    Characters/     CharacterAssetBuilder가 생성한 캐릭터 프리팹 (Cappi/Dao/Marid)
  Scenes/
    Main.unity      로비·전투 통합 씬 (씬은 이것 하나)
```

## 씬 구성

씬은 `Main.unity` 하나. 전투와 로비가 패널 전환으로 동작함.

| 패널 | 역할 |
|------|------|
| BattleView | 사이드뷰 자동 전투, HP바, 데미지 팝업, 탄환, 캐릭터/몬스터 비주얼 |
| UpgradePanel | ATK·HP·DEF·Speed·Crit 강화 |
| CharacterPanel | 캐릭터 선택/구매 |
| SkillPanel | 패시브 스킬 |
| PetPanel | 펫 |
| EquipmentPanel | 장비 |
| AchievementPanel | 업적 |

## 핵심 아키텍처

### EventBus

`EventBus.Publish<T>()` / `EventBus.Subscribe<T>()` 방식. 직접 참조 대신 이벤트로 통신.

주요 이벤트 (`GameEvents.cs`):
- `DamageEvent` — 데미지 발생 (Target: Monster/Character)
- `MonsterKilledEvent` — 몬스터 처치 (골드·경험치 포함)
- `StageChangedEvent` — 스테이지 변경
- `StageClearedEvent` — 스테이지 클리어
- `GoldChangedEvent` — 골드 변경
- `CharacterChangedEvent` — 캐릭터 교체

### 몬스터 소환 책임

**StageManager가 단일 책임.** BattleManager는 자동 재소환하지 않음.
- 킬 수 미달 → `StageManager.OnMonsterKilled()` → 즉시 `BattleManager.LoadMonster()`
- 스테이지 클리어 → `LoadMonsterDelayed()` (0.6s 딜레이 — 워크 애니 4프레임 8fps=0.5s 맞춤)

### 스탯 보너스 계산

`BattleManager`가 `PetBonus`, `SkillBonus`, `EquipBonus`를 집계해 최종 스탯 계산.
`GoldMultiplier`는 `BattleManager`의 public 프로퍼티 — `OfflineManager`가 참조.

### 저장 시스템

```csharp
ISaveProvider
  └── JsonSaveProvider  // persistentDataPath/save.json
```
자동 저장: 30초마다 + 앱 포즈/종료 시.

### CharacterAssetBuilder (Editor 전용)

`IdleCA/Build Character Assets` 메뉴 실행 시 Cappi·Dao·Marid 각각:
- `Assets/Prefabs/Characters/{Name}/` 에 AnimationClip(Idle/Walk) + AnimatorController + Prefab 생성
- Animator: `Attack` 트리거 → Idle→Walk, ExitTime 후 Walk→Idle

## 저장 데이터 구조 (`SaveData.cs`)

```csharp
gold, level, exp
currentStageIndex, maxReachedStageIndex
upgradeLevels[]
ownedSkillIds[], ownedPetIds[], equippedItemIds[]
ownedCharacterNames[], activeCharacterName
achievementProgress[]
lastSaveTime
```

## Git 브랜치 전략

- 새로운 기능 추가 또는 기존 기능 수정 시 **반드시 새 브랜치를 생성**하고 작업한다
- 작업 완료 후 PR을 올린다 (직접 main에 push 금지)
- 브랜치 네이밍: `feat/기능명`, `fix/수정내용` 등 타입 기반

### 커밋 타입

| 이모지 | 타입 | 설명 |
|--------|------|------|
| ✨ | feat | 새로운 기능 추가 |
| 🐛 | fix | 버그 수정 |
| 🎨 | style | 코드 스타일 변경 |
| 🔄 | refactor | 리팩토링 |
| 💄 | design | UI/스프라이트 등 디자인 수정 |
| 💬 | comment | 주석 추가/수정 |
| 📝 | docs | 문서 수정 |
| 🧪 | test | 테스트 추가/수정 |
| 🛠️ | chore | 설정/빌드 등 기타 변경 |
| 📛 | rename | 파일명/폴더명 변경 |
| 🗑️ | remove | 파일 삭제 |

커밋 메시지 형식: `✨ feat: 전투 자동 공격 구현`

## 개발 시 주의사항

- `ScriptalbeObjects/` 폴더명 오타는 수정 금지 — Unity GUID가 이 경로를 참조함
- `.meta` 파일은 반드시 함께 커밋한다 (Unity 에셋 참조 유지)
- ScriptableObject 에셋(`.asset`) 수정 시 해당 `.meta`도 함께 커밋한다
- 씬 파일(`.unity`) 충돌 방지를 위해 한 번에 한 명만 씬 편집한다
- `[SerializeField]` private 필드를 선호 — `public` 노출 최소화
- `Library/`, `Temp/`, `Logs/`, `UserSettings/`는 `.gitignore`로 제외
- API 키, 서버 주소 등 민감한 값은 절대 커밋하지 않는다
