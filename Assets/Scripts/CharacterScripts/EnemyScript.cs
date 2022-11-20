using Photon.Pun;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private Rigidbody2D thisRigidbody;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private PhotonView photonView;
    [SerializeField]
    private FeetScript feetScript;

    [Header("Attributes")]
    [SerializeField]
    private float life = 100.0f;
    private float maximumLife = 100.0f;
    [SerializeField]
    private float movementSpeed = 12.0f;
    [SerializeField]
    private float jumpForce = 25.0f;
    [SerializeField]
    private float kickForce = 2.5f;
    [SerializeField]
    private float flipCorrection;
    [SerializeField]
    private float increasePowerToPlayer = 10.0f;

    private GameObject targetPlayerObject;
    private Vector2 targetPosition;

    private bool canMove = true;


    private void Start()
    {
        if (photonView.IsMine)
        {
            maximumLife = life;
            movementSpeed = Random.Range(movementSpeed - 1, movementSpeed + 1);
        }
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            if (targetPlayerObject == null) SelectPlayer();
            else if (canMove)
            {
                if ((targetPosition.x > transform.position.x && transform.localScale.x < 0)
                || (targetPosition.x < transform.position.x && transform.localScale.x > 0)) Flip();

                Move(Mathf.Clamp(targetPosition.x - transform.position.x, -1, 1));
            }

            if (animator != null && animator.GetBool("Jump"))
            {
                if (thisRigidbody.velocity.y < 0) animator.SetInteger("JumpBlend", 1);
                else if (feetScript.OnFloor() && thisRigidbody.velocity.y <= 0)
                {
                    animator.SetBool("Jump", false);
                    animator.SetInteger("JumpBlend", 2);
                }
            }

            if (transform.position.y < -6) Die();
        }
    }

    private void Move(float horizontalDirection)
    {
        if (animator != null)
        {
            animator.SetBool("Run", true);
            animator.SetFloat("RunVelocity", Mathf.Abs(horizontalDirection));
        }

        float speed = movementSpeed;
        if (!feetScript.OnFloor()) speed *= 0.75f;
        transform.Translate(Vector2.right * horizontalDirection * speed * Time.deltaTime, Space.World);
    }

    private void SelectPlayer()
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject playerObject in playerObjects)
        {
            if (targetPlayerObject == null
                || Vector2.Distance(playerObject.transform.position, transform.position) < Vector2.Distance(targetPosition, transform.position))
            {
                targetPlayerObject = playerObject;
                targetPosition = targetPlayerObject.transform.position;
            }
        }
    }

    public void Beat(GameObject beatPrefab)
    {
        GameObject beat = PhotonNetwork.Instantiate(beatPrefab.name, transform.position, Quaternion.identity);
        beat.transform.localScale = transform.localScale;
    }

    private void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        transform.position = new Vector3(transform.position.x + (transform.localScale.x < 0 ? flipCorrection : -flipCorrection), transform.position.y, 0);
    }

    private void Die()
    {
        PhotonNetwork.Destroy(gameObject);
    }

    #region Getters e Setters
    public Rigidbody2D GetRigidbody()
    {
        return thisRigidbody;
    }

    public PhotonView GetPhotonView()
    {
        return photonView;
    }

    public FeetScript GetFeetScript()
    {
        return feetScript;
    }

    public Vector2 GetTargetPosition()
    {
        return targetPosition;
    }

    public void DecreaseLife(float life)
    {
        this.life -= life;
        this.life = Mathf.Clamp(this.life, 0.0f, maximumLife);

        if (this.life == 0)
        {
            if (animator != null)
            {
                canMove = false;
                animator.SetTrigger("Die");
            }
            else Die();
        }
        //else if (life > 0) thisRigidbody.AddForce(collision.transform. * 2.5f, ForceMode2D.Impulse);
    }

    public void SetTargetPosition(Vector2 position)
    {
        targetPosition = position;
    }

    public GameObject GetTargetPlayerObject()
    {
        return targetPlayerObject;
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (photonView.IsMine)
        {
            if (collision.CompareTag("Damage"))
            {
                DamageScript damageScript;
                if (collision.transform.parent != null) damageScript = collision.transform.parent.gameObject.GetComponent<DamageScript>();
                else damageScript = collision.gameObject.GetComponent<DamageScript>();

                damageScript.GetPlayerScript().IncreasePower(increasePowerToPlayer);

                if (!damageScript.IsDamageArea()) PhotonNetwork.Destroy(collision.transform.parent != null ? collision.transform.parent.gameObject : collision.gameObject);

                DecreaseLife(damageScript.GetDamage());
            }
        }
    }
}
