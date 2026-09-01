using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Projectile : MonoBehaviour
{
    [SerializeField] Sprite explosionSprite;
    [SerializeField] float explosionSize = 80f;
    [SerializeField] float explosionDuration = 0.35f;

    public void Launch(Vector3 from, Vector3 to, float speed = 800f)
    {
        transform.position = from;
        StartCoroutine(Move(to, speed));
    }

    IEnumerator Move(Vector3 to, float speed)
    {
        while (Vector3.Distance(transform.position, to) > 8f)
        {
            transform.position = Vector3.MoveTowards(transform.position, to, speed * Time.deltaTime);
            transform.Rotate(0f, 0f, 360f * Time.deltaTime);
            yield return null;
        }
        SpawnExplosion();
        Destroy(gameObject);
    }

    void SpawnExplosion()
    {
        if (explosionSprite == null) return;
        var go = new GameObject("FX_Explosion");
        go.transform.SetParent(transform.parent, false);
        go.transform.position = transform.position;
        var img = go.AddComponent<Image>();
        img.sprite = explosionSprite;
        img.GetComponent<RectTransform>().sizeDelta = new Vector2(explosionSize, explosionSize);
        Destroy(go, explosionDuration);
    }
}
