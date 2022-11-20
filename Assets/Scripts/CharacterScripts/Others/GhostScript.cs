using UnityEngine;

public class GhostScript : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private EnemyScript enemyScript;
    
    [Header("Attributes")]
    [SerializeField]
    private float flyForce = 1.0f;
    private float minimumFlyForce = 0.5f;
    private float maximumFlyForce = 1.5f;

    private float t = 0.0f;

    private bool spin = true;
    private bool nearTargetPosition;


    private void Start()
    {
        if (enemyScript.GetPhotonView().IsMine)
        {
            minimumFlyForce = flyForce - 0.5f;
            maximumFlyForce = flyForce - 0.5f;
        }
    }

    private void Update()
    {
        if (enemyScript.GetPhotonView().IsMine)
        {
            if (enemyScript.GetTargetPlayerObject() != null)
            {
                nearTargetPosition = Vector2.Distance(transform.position, enemyScript.GetTargetPosition()) < 0.5f;

                if (!spin) enemyScript.SetTargetPosition(enemyScript.GetTargetPlayerObject().transform.position);
                else if (nearTargetPosition) spin = false;

                LookToTarget();
                if (BelowTargetPosition() || enemyScript.GetFeetScript().OnFloor()) Fly();
            }
        }
    }

    private void Fly()
    {
        float targetForce = 0.1f;

        t += 0.5f * Time.deltaTime;

        if (t > 1.0f)
        {
            if (flyForce > maximumFlyForce - 0.1f) targetForce = minimumFlyForce;
            else targetForce = maximumFlyForce;
            t = 0.0f;
        }

        flyForce = Mathf.Lerp(targetForce == maximumFlyForce ? minimumFlyForce : maximumFlyForce, targetForce, t);
        if (enemyScript.GetRigidbody().velocity.y < 0) flyForce += -enemyScript.GetRigidbody().velocity.y;

        enemyScript.GetRigidbody().AddForce(Vector2.up * flyForce, ForceMode2D.Force);
    }

    private void LookToTarget()
    {
        Vector3 direction = new Vector3(enemyScript.GetTargetPosition().x - transform.position.x,enemyScript.GetTargetPosition().y - transform.position.y, 0);
        if (direction.magnitude > 1) direction = direction.normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 270.0f;

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void Spin()
    {
        spin = true;

        Vector2 targetPosition = new Vector2(enemyScript.GetTargetPlayerObject().transform.position.x+ (enemyScript.GetTargetPlayerObject().transform.position.x <= transform.position.x ? -3.0f : 3.0f),
            enemyScript.GetTargetPlayerObject().transform.position.y + 2.0f);

        enemyScript.SetTargetPosition(targetPosition);
    }

    private bool BelowTargetPosition()
    {
        return transform.position.y < enemyScript.GetTargetPosition().y - 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (enemyScript.GetPhotonView().IsMine && collision.CompareTag("Player") && collision == enemyScript.GetTargetPlayerObject()) Spin();
    }
}
