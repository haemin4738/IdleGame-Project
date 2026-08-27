using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    const string SCENE_MAIN = "Main";
    const string SCENE_BATTLE = "Battle";

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SaveManager.Instance.Load();
    }

    public void GoToBattle()
    {
        EventBus.Clear();
        SceneManager.LoadScene(SCENE_BATTLE);
    }

    public void GoToMain()
    {
        EventBus.Clear();
        SceneManager.LoadScene(SCENE_MAIN);
    }

    void OnApplicationPause(bool paused)
    {
        if (paused) SaveManager.Instance.Save();
    }

    void OnApplicationQuit()
    {
        SaveManager.Instance.Save();
    }
}
