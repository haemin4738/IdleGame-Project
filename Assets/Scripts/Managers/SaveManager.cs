using System.Collections;
using UnityEngine;

// SaveManager가 GameManager보다 먼저 Awake되도록 실행 순서 보장
[DefaultExecutionOrder(-100)]
public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    public SaveData Data { get; private set; } = new();

    ISaveProvider _provider;
    const float AUTO_SAVE_INTERVAL = 30f;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        _provider = new JsonSaveProvider();
    }

    void Start() => StartCoroutine(AutoSaveRoutine());

    public void Save() => _provider.Save(Data);

    public void Load() => Data = _provider.Load();

    IEnumerator AutoSaveRoutine()
    {
        var wait = new WaitForSeconds(AUTO_SAVE_INTERVAL);
        while (true)
        {
            yield return wait;
            Save();
        }
    }
}
