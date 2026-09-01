using System.IO;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public static class CharacterAssetBuilder
{
    static readonly (string charName, string folder)[] Chars =
    {
        ("Cappi", "Assets/CA_Assets/Character/Cappi"),
        ("Dao",   "Assets/CA_Assets/Character/Dao"),
        ("Marid", "Assets/CA_Assets/Character/Marid"),
    };

    [MenuItem("IdleCA/Build Character Assets")]
    static void BuildAll()
    {
        EnsureFolder("Assets/Prefabs/Characters");
        foreach (var (charName, folder) in Chars)
            Build(charName, folder);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("[CharacterAssetBuilder] Done.");
    }

    static void Build(string n, string folder)
    {
        var idleSprite = AssetDatabase.LoadAssetAtPath<Sprite>($"{folder}/{n}RightIdle.png");
        var walkSprites = new Sprite[4];
        for (int i = 0; i < 4; i++)
            walkSprites[i] = AssetDatabase.LoadAssetAtPath<Sprite>($"{folder}/{n}RightWalk_{i + 1}.png");

        if (idleSprite == null)
        {
            Debug.LogError($"[CharacterAssetBuilder] {n}RightIdle.png not found at {folder}");
            return;
        }

        var outDir = $"Assets/Prefabs/Characters/{n}";
        EnsureFolder(outDir);

        var idleClip   = CreateClip($"{outDir}/{n}_Idle.anim", loop: true,  idleSprite);
        var walkClip   = CreateClip($"{outDir}/{n}_Walk.anim", loop: false, walkSprites);
        var controller = CreateController($"{outDir}/{n}.controller", idleClip, walkClip);
        CreatePrefab($"{outDir}/{n}_Character.prefab", n, controller, idleSprite);
    }

    static AnimationClip CreateClip(string path, bool loop, params Sprite[] sprites)
    {
        var clip = new AnimationClip { frameRate = 8 };
        var settings = AnimationUtility.GetAnimationClipSettings(clip);
        settings.loopTime = loop;
        AnimationUtility.SetAnimationClipSettings(clip, settings);

        var binding = EditorCurveBinding.PPtrCurve("", typeof(SpriteRenderer), "m_Sprite");
        var keys = new ObjectReferenceKeyframe[sprites.Length];
        for (int i = 0; i < sprites.Length; i++)
            keys[i] = new ObjectReferenceKeyframe { time = i / 8f, value = sprites[i] };
        AnimationUtility.SetObjectReferenceCurve(clip, binding, keys);

        AssetDatabase.CreateAsset(clip, path);
        return clip;
    }

    static AnimatorController CreateController(string path, AnimationClip idle, AnimationClip walk)
    {
        var ctrl = AnimatorController.CreateAnimatorControllerAtPath(path);
        ctrl.AddParameter("Attack", AnimatorControllerParameterType.Trigger);

        var sm = ctrl.layers[0].stateMachine;
        var idleState = sm.AddState("Idle");
        idleState.motion = idle;
        sm.defaultState = idleState;

        var walkState = sm.AddState("Walk");
        walkState.motion = walk;

        var toWalk = idleState.AddTransition(walkState);
        toWalk.AddCondition(AnimatorConditionMode.If, 0, "Attack");
        toWalk.hasExitTime = false;
        toWalk.duration = 0f;

        var toIdle = walkState.AddTransition(idleState);
        toIdle.hasExitTime = true;
        toIdle.exitTime = 1f;
        toIdle.hasFixedDuration = false;
        toIdle.duration = 0f;

        return ctrl;
    }

    static void CreatePrefab(string path, string charName, RuntimeAnimatorController ctrl, Sprite defaultSprite)
    {
        var go = new GameObject($"{charName}_Character");
        var sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = defaultSprite;
        go.AddComponent<Animator>().runtimeAnimatorController = ctrl;
        PrefabUtility.SaveAsPrefabAsset(go, path);
        Object.DestroyImmediate(go);
    }

    static void EnsureFolder(string path)
    {
        if (AssetDatabase.IsValidFolder(path)) return;
        var parent = Path.GetDirectoryName(path)?.Replace('\\', '/') ?? "Assets";
        EnsureFolder(parent);
        AssetDatabase.CreateFolder(parent, Path.GetFileName(path));
    }
}
