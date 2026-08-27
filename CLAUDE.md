# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## 기술 스택

- **언어**: C# (Unity)
- **엔진**: Unity 2022+ (URP 2D)
- **저장**: JSON (`Application.persistentDataPath/save.json`)
- **아키텍처**: ScriptableObject 기반 데이터 드리븐 + Manager 패턴 + EventBus
- **목적**: 포트폴리오 — 크레이지아케이드 테마 방치형 RPG

## 빌드 및 실행

```bash
# Unity Editor에서 열기 (Unity Hub 사용)
# 프로젝트 경로: D:/Unity/Portpolio/IdleCA

# 커맨드라인 빌드 (Windows)
"C:/Program Files/Unity/Hub/Editor/6000.0.67f1/Editor/Unity.exe" \
  -quit -batchmode \
  -projectPath "D:/Unity/Portpolio/IdleCA" \
  -buildWindowsPlayer "Build/IdleCA.exe" \
  -logFile "Build/build.log"

# 테스트 실행 (Unity Test Runner — Edit Mode)
"C:/Program Files/Unity/Hub/Editor/6000.0.67f1/Editor/Unity.exe" \
  -quit -batchmode \
  -projectPath "D:/Unity/Portpolio/IdleCA" \
  -runTests -testPlatform EditMode \
  -testResults "TestResults.xml"
```

## 프로젝트 구조

```
Assets/
  Scripts/
    Core/           # GameManager, EventBus
    Data/           # ScriptableObject 정의 (CharacterData, MonsterData 등)
    Managers/       # BattleManager, SaveManager, UpgradeManager 등
    UI/             # 각 패널/팝업 컨트롤러
    Utils/          # 공통 유틸리티
  ScriptableObjects/
    Characters/     # CharacterData 에셋
    Monsters/       # MonsterData 에셋
    Skills/         # SkillData 에셋
    Stages/         # StageData 에셋
    Pets/           # PetData 에셋
    Equipment/      # EquipmentData 에셋
  Scenes/
    Main.unity      # 로비 씬
    Battle.unity    # 전투 씬
  Sprites/          # 스프라이트 에셋
  Prefabs/          # UI 프리팹, 이펙트 프리팹
Docs/
  TechSpec.md       # 게임 기술 스펙 전체
```

## 씬 구성

| 씬 | 역할 |
|----|------|
| `Main.unity` | 로비 — 업그레이드, 스킬, 펫, 업적 탭 |
| `Battle.unity` | 사이드뷰 자동 전투 |

## 저장 시스템

```csharp
// 인터페이스 기반 — 나중에 서버 연동 시 구현체만 교체
ISaveProvider
  └── JsonSaveProvider   // 현재 사용: persistentDataPath/save.json
  └── ServerSaveProvider // 추후 추가 예정
```

- 자동 저장: 30초마다 + 앱 포즈/종료 시
- 저장 위치: `Application.persistentDataPath/save.json`

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

- API 키, 서버 주소 등 민감한 값은 절대 커밋하지 않는다
- `Library/`, `Temp/`, `Logs/`, `UserSettings/`는 `.gitignore`로 제외 — 커밋하지 않는다
- `.meta` 파일은 반드시 함께 커밋한다 (Unity 에셋 참조 유지)
- ScriptableObject 에셋(`.asset`) 수정 시 해당 `.meta`도 함께 커밋한다
- 씬 파일(`.unity`) 충돌 방지를 위해 한 번에 한 명만 씬 편집한다
- `[SerializeField]` private 필드를 선호 — `public` 노출 최소화
