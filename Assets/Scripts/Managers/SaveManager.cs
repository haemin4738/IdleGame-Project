using System.Collections;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class SaveManager : MonoBehaviour
{
    static SaveManager _instance;
    public static SaveManager Instance
    {
        get
        {
            if (_instance == null)
            {
                var go = new GameObject("[SaveManager]");
                _instance = go.AddComponent<SaveManager>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    public SaveData Data { get; private set; } = new();

    ISaveProvider _provider;
    const float AUTO_SAVE_INTERVAL = 30f;

    void Awake()
    {
        if (_instance != null && _instance != this) { Destroy(gameObject); return; }
        _instance = this;
        DontDestroyOnLoad(gameObject);
        _provider = new JsonSaveProvider();
        Load();
    }

    void Start() => StartCoroutine(AutoSaveRoutine());

    public void Save()
    {
        Data.lastSaveTimestamp = System.DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        _provider.Save(Data);
    }

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
