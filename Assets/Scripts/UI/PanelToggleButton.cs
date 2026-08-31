using UnityEngine;

public class PanelToggleButton : MonoBehaviour
{
    [SerializeField] GameObject panel;

    void Start() => panel.SetActive(false);

    public void Toggle() => panel.SetActive(!panel.activeSelf);
}
