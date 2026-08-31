using System.Collections;
using TMPro;
using UnityEngine;

public class ToastPopup : MonoBehaviour
{
    public static ToastPopup Instance { get; private set; }

    [SerializeField] TMP_Text label;
    [SerializeField] CanvasGroup canvasGroup;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        canvasGroup.alpha = 0f;
    }

    public void Show(string message)
    {
        StopAllCoroutines();
        label.text = message;
        transform.SetAsLastSibling();
        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        canvasGroup.alpha = 1f;
        yield return new WaitForSeconds(1.2f);

        float elapsed = 0f;
        while (elapsed < 0.5f)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = 1f - (elapsed / 0.5f);
            yield return null;
        }
        canvasGroup.alpha = 0f;
    }
}
