using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementSlotUI : MonoBehaviour
{
    [SerializeField] TMP_Text titleText;
    [SerializeField] TMP_Text progressText;
    [SerializeField] Button claimButton;
    [SerializeField] TMP_Text claimButtonText;

    AchievementData _data;

    public void Init(AchievementData data)
    {
        _data = data;
        titleText.text = data.title;
        claimButton.onClick.AddListener(OnClaim);
        Refresh();
    }

    public void Refresh()
    {
        bool allDone = AchievementManager.Instance.IsAllDone(_data);
        bool canClaim = AchievementManager.Instance.CanClaim(_data);
        int claimed = AchievementManager.Instance.GetClaimedCount(_data);
        long progress = AchievementManager.Instance.GetProgress(_data);

        if (allDone)
        {
            progressText.text = "모두 완료!";
            claimButton.interactable = false;
            claimButtonText.text = "완료";
        }
        else
        {
            long target = _data.milestoneValues[claimed];
            progressText.text = $"{progress} / {target}";
            claimButton.interactable = canClaim;
            claimButtonText.text = "받기";
        }
    }

    void OnClaim()
    {
        AchievementManager.Instance.Claim(_data);
        Refresh();
    }
}
