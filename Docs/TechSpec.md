# IdleCA — 기술 스펙 (Technical Specification)

> 버전: 1.0 | 작성일: 2026-08-27 | 엔진: Unity 2022+ | 플랫폼: Windows PC

---

## 1. 프로젝트 개요

| 항목 | 내용 |
|------|------|
| 장르 | 방치형 RPG (Idle RPG) |
| 테마 | 크레이지아케이드 IP (캐릭터·몬스터·맵 원작 사용) |
| 레퍼런스 | 메이플 키우기 스타일 |
| 플랫폼 | PC (Windows) |
| 엔진 | Unity |
| 목적 | 포트폴리오 |

---

## 2. 코어 게임플레이

### 2.1 핵심 루프

```
[로비] ──→ [전투 씬] ──→ 자동 전투 (실시간)
  ↑              │
  └── 업그레이드 ←┘ + 오프라인 수익 (최대 12시간)
```

- **온라인**: 실시간 자동 전투, 골드/경험치 획득
- **오프라인**: 경과시간 기반 수익 계산, 최대 캡 **12시간**

### 2.2 전투 화면

- **사이드뷰** — 캐릭터 왼쪽, 몬스터 오른쪽
- 자동 공격 + 스킬 자동 발동
- 몬스터 처치 시 드롭 + 다음 몬스터 자동 스폰
- 보스 처치 → 스테이지 클리어
- 사망 시 자동 부활 (일정 딜레이)

### 2.3 MVP 범위

스킬 트리 + 펫 + 업적 + 다수 캐릭터 선택 포함 (Full MVP)

---

## 3. 아키텍처

### 3.1 전체 구조

```
ScriptableObject (데이터)
        ↓
Manager 클래스들 (로직)
        ↓        ↕ EventBus
   UI (View)
```

- **데이터**: ScriptableObject 기반 데이터 드리븐
- **로직**: Manager 패턴 (싱글턴)
- **통신**: EventBus (시스템 간 직접 참조 없음)

### 3.2 씬 구성

| 씬 | 역할 |
|----|------|
| `Main.unity` | 로비 — 업그레이드, 스킬, 펫, 업적 |
| `Battle.unity` | 전투 — 사이드뷰 자동 전투 |

### 3.3 저장 방식

```
ISaveProvider (인터페이스)
    ├── JsonSaveProvider   ← 현재 사용
    └── ServerSaveProvider ← 나중에 서버 연동 시 교체
```

- 저장 위치: `Application.persistentDataPath/save.json`
- 자동 저장: **30초마다** + **앱 포즈/종료 시**

---

## 4. ScriptableObject 데이터 구조

### 4.1 공통 구조체

```csharp
[Serializable]
public struct StatBonus {
    public float atkPercent;
    public float hpPercent;
    public float goldPercent;
    public float expPercent;
    public float defFlat;
}
```

### 4.2 SO 클래스 목록

| SO 클래스 | 주요 필드 |
|-----------|-----------|
| `CharacterData` | name, portrait, baseHP/ATK/DEF/Speed, attackType, unlockCost, ownedSkills[] |
| `MonsterData` | name, sprite, hp/atk/def, expReward, goldReward, dropTable[], isBoss |
| `SkillData` | name, icon, skillType(Active/Passive), damage, cooldown, effect, maxLevel, statPerLevel |
| `EquipmentData` | name, icon, slot, rarity(일반/희귀/영웅/전설), baseStats, upgradeMaxLevel |
| `PetData` | name, sprite, passiveBonus(StatBonus), unlockCondition |
| `StageData` | name, backgroundSprite, monsterWaves[], bossMonster, recommendedATK, goldMultiplier |
| `AchievementData` | title, description, conditionType, conditionValue, reward |

---

## 5. Manager 시스템

| Manager | 역할 |
|---------|------|
| `GameManager` | 진입점, 씬 전환 관리 |
| `EventBus` | 시스템 간 이벤트 통신 |
| `BattleManager` | 자동 전투 루프, 드롭 처리, 자동 부활 |
| `OfflineManager` | 경과시간 계산, 오프라인 수익 (최대 12시간) |
| `UpgradeManager` | 스탯 업그레이드 (비용: `baseCost × 1.15^level`) |
| `SkillManager` | 스킬 포인트, 스킬 레벨업(최대 10), 패시브/액티브 발동 |
| `PetManager` | 펫 보유 관리, StatBonus 합산 |
| `AchievementManager` | 업적 추적, 보상 지급 |
| `StageManager` | 스테이지 진행, 보스 클리어 조건 |
| `SaveManager` | ISaveProvider 통해 JSON 저장/불러오기 |

### 5.1 EventBus 주요 이벤트

```csharp
MonsterKilled(MonsterData monster)
GoldChanged(long newAmount)
LevelUp(int newLevel)
StageCleared(int stageIndex)
AchievementUnlocked(AchievementData achievement)
```

---

## 6. UI 구조

### 6.1 Main 씬 (로비)

```
┌─────────────────────────────────┐
│  [캐릭터명]  Lv.1  💰 0         │  ← HUD
├─────────────────────────────────┤
│                                 │
│       [캐릭터 스프라이트]        │  ← 중앙
│                                 │
├─────────────────────────────────┤
│ [전투] [업그레이드] [스킬] [펫] [업적] │  ← 하단 탭
└─────────────────────────────────┘
```

### 6.2 Battle 씬

```
┌─────────────────────────────────┐
│  Stage 1-1    💰 12/s    [나가기] │  ← 상단
├─────────────────────────────────┤
│                                 │
│  [캐릭터] ——공격——→ [몬스터]     │  ← 사이드뷰 전투
│  HP ████████           HP ████  │
│                                 │
├─────────────────────────────────┤
│        [◀ 이전]  [다음 ▶]       │  ← 하단
└─────────────────────────────────┘
```

### 6.3 팝업

| 팝업 | 트리거 |
|------|--------|
| `OfflineRewardPopup` | 앱 재시작 시 오프라인 수익 표시 |
| `LevelUpPopup` | 레벨업 |
| 업적 달성 토스트 | 업적 조건 달성 |
| 장비 드롭 알림 | 희귀 등급 이상 드롭 |

> 데미지 팝업: **오브젝트 풀링** 적용

---

## 7. 구현 우선순위

| 순위 | 시스템 | 검증 방법 |
|------|--------|-----------|
| 1 | Core 인프라 (GameManager, EventBus, SaveManager) | 씬 전환 + 저장/불러오기 동작 확인 |
| 2 | ScriptableObject 데이터 클래스 | Inspector에서 데이터 입력 가능 |
| 3 | BattleManager + 전투 씬 | 자동 전투 루프 동작 |
| 4 | UpgradeManager + 로비 UI | 업그레이드 비용 계산 + UI 반영 |
| 5 | OfflineManager | 오프라인 수익 계산 검증 |
| 6 | SkillManager | 스킬 발동 + 레벨업 |
| 7 | PetManager | StatBonus 합산 적용 |
| 8 | AchievementManager | 조건 추적 + 보상 지급 |
| 9 | StageManager | 스테이지 진행 흐름 |
| 10 | 콘텐츠 데이터 입력 | 캐릭터/몬스터/스테이지 SO 에셋 작성 |

---

## 8. 기술 결정 메모

- **애니메이션**: 초기엔 정적 스프라이트만, 이후 단계에서 추가
- **업그레이드 공식**: `cost = baseCost × 1.15^currentLevel`
- **스킬 최대 레벨**: 10
- **오프라인 캡**: 12시간 (초과분 무시)
- **저장 추상화**: `ISaveProvider` → 나중에 서버 전환 시 구현체만 교체
