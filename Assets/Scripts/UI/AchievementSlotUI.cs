using TMPro;
using UnityEngine;

public class AchievementSlotUI : MonoBehaviour
{
    [SerializeField] TMP_Text titleText;
    [SerializeField] TMP_Text descText;
    [SerializeField] TMP_Text progressText;
    [SerializeField] GameObject completedMark;

    AchievementData _data;

    public void Init(AchievementData data)
    {
        _data = data;
        titleText.text = data.title;
        descText.text = data.description;
        Refresh();
    }

    public void Refresh()
    {
        bool done = AchievementManager.Instance.IsCompleted(_data);
        long progress = AchievementManager.Instance.GetProgress(_data);
        progressText.text = done ? "완료!" : $"{progress} / {_data.conditionValue}";
        completedMark.SetActive(done);
    }
}
