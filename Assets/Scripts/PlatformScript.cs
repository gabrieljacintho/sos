using UnityEngine;

public class PlatformScript : MonoBehaviour
{
    private bool toLeft = false;
    private float speed = 2.0f;

    private void Start()
    {
        UpdatePlatform();
    }

    private void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    private void UpdatePlatform()
    {
        toLeft = !toLeft;
        speed = Random.Range(2.0f, 4.0f);
        speed *= toLeft ? -1 : 1;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8) UpdatePlatform();
    }
}
