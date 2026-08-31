using TMPro;
using UnityEngine;

public class OfflineRewardPopup : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] TMP_Text goldText;
    [SerializeField] TMP_Text timeText;

    void Start()
    {
        if (OfflineManager.Instance == null) return;

        var reward = OfflineManager.Instance.Calculate();
        if (reward.Gold <= 0) return;

        var h = reward.Seconds / 3600;
        var m = (reward.Seconds % 3600) / 60;
        goldText.text = $"+{reward.Gold:N0} 골드";
        timeText.text = $"{h:D2}:{m:D2} 동안 수익";
        panel.SetActive(true);
    }

    public void OnClose() => panel.SetActive(false);
}
