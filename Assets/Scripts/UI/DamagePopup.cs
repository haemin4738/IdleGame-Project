using System.Collections;
using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] TMP_Text label;

    public void Show(float damage, bool isCrit)
    {
        label.text  = $"{damage:F0}";
        label.color = isCrit ? Color.red : Color.yellow;
        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        var rt        = GetComponent<RectTransform>();
        float elapsed = 0f;
        Vector2 start = rt.anchoredPosition;
        Color color   = label.color;

        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime;
            rt.anchoredPosition = start + Vector2.up * (elapsed * 80f);
            color.a    = 1f - elapsed;
            label.color = color;
            yield return null;
        }
        Destroy(gameObject);
    }
}
