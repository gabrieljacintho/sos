using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    [SerializeField]
    private DamageScript damageScript;
    [SerializeField]
    private BoxCollider2D thisCollider;
    [SerializeField]
    private Rigidbody2D thisRigidbody;

    [Space]
    [SerializeField]
    private float speed = 10.0f;
    private bool fly = true;


    private void Awake()
    {
        transform.position = new Vector3(transform.position.x + (transform.localScale.x * 2), transform.position.y + 2.3f, 0);
    }

    private void Update()
    {
        if (fly) transform.Translate(Vector2.right * transform.localScale.x * speed * Time.deltaTime, Space.Self);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (damageScript.GetPhotonView().IsMine)
        {
            if (!collision.gameObject.CompareTag("Enemy")) damageScript.SetDamage(0);
            fly = false;
            thisRigidbody.gravityScale = 0;
            thisRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
}
