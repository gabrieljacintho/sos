using Photon.Pun;
using UnityEngine;

public class EnemyTestScript : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private GameObject enemyTestPrefab;

    [Header("Attributes")]
    [SerializeField]
    private float increasePowerToPlayer = 10.0f;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (collision.gameObject.CompareTag("Damage"))
            {
                DamageScript damageScript;
                if (collision.transform.parent != null) damageScript = collision.transform.parent.gameObject.GetComponent<DamageScript>();
                else damageScript = collision.gameObject.GetComponent<DamageScript>();

                damageScript.GetPlayerScript().IncreasePower(increasePowerToPlayer);

                if (!damageScript.IsDamageArea() && PhotonNetwork.IsMasterClient)
                    PhotonNetwork.Destroy(collision.transform.parent != null ? collision.transform.parent.gameObject : collision.gameObject);

                Instantiate(enemyTestPrefab, new Vector2(transform.position.x, transform.position.y + 10), Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}
