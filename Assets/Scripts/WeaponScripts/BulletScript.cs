using Photon.Pun;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField]
    private DamageScript damageScript;

    [Space]
    [SerializeField]
    private float speed = 10.0f;


    private void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime, Space.Self);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (damageScript.GetPhotonView().IsMine && collision.gameObject.CompareTag("Floor")) PhotonNetwork.Destroy(gameObject);
    }
}
