using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
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
        Destroy(gameObject);
    }
}
