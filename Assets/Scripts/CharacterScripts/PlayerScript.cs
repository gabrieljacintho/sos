using Photon.Pun;
using UnityEngine;

public class PlayerScript : MonoBehaviour
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
    private JoystickScript joystickScript;

    [Header("Attributes")]
    [SerializeField]
    private float life = 100.0f;
    private float maximumLife = 100.0f;
    [SerializeField]
    private float movementSpeed = 12.0f;
    [SerializeField]
    private float jumpForce = 25.0f;
    private float currentJumpForce;
    [SerializeField]
    private float kickForce = 2.5f;
    [SerializeField]
    private float flipCorrection;
    [SerializeField]
    private float power = 100.0f;
    private float maximumPower = 100.0f;

    private bool canMove = true;
    private bool move = true;


    private void Start()
    {
        if (photonView.IsMine)
        {
            maximumLife = life;
            maximumPower = power;
            //maximumLife = Mathf.Clamp(life - 5.0f + (PlayerPrefs.GetInt("Level", 1) * life * 0.05f), 100.0f, 200.0f);
            //maximumPower = Mathf.Clamp(life - 2.5f + (PlayerPrefs.GetInt("Level", 1) * life * 0.025f), 100.0f, 200.0f);
            joystickScript = UIManager.instance.GetJoystickScript();

            UIManager.instance.SetPlayerScript(this);
            UIManager.instance.ActivePanel("GameplayPanel");
        }
    }

    private void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            if (canMove && (joystickScript.Horizontal() != 0 || joystickScript.Vertical() != 0))
            {
                if ((joystickScript.Horizontal() > 0 && transform.localScale.x < 0)
                    || (joystickScript.Horizontal() < 0 && transform.localScale.x > 0)) Flip();

                if (move) Move(joystickScript.Horizontal());
            }
            else if (animator != null) animator.SetBool("Run", false);

            if (currentJumpForce > 0) Jump();

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

    private void Jump()
    {
        if (currentJumpForce == jumpForce) thisRigidbody.AddForce(transform.up * currentJumpForce, ForceMode2D.Impulse);
        else thisRigidbody.AddForce(transform.up * currentJumpForce * 2, ForceMode2D.Force);

        currentJumpForce -= jumpForce * 0.1f * Time.deltaTime;

        if (!animator.GetBool("Jump"))
        {
            animator.SetInteger("JumpBlend", 0);
            animator.SetBool("Jump", true);
        }
    }

    public void Beat(GameObject beatPrefab)
    {
        GameObject beat = PhotonNetwork.Instantiate(beatPrefab.name, transform.position, Quaternion.identity);
        beat.GetComponent<DamageScript>().playerScript = this;
        beat.transform.localScale = transform.localScale;
    }

    private void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        transform.position = new Vector3(transform.position.x + (transform.localScale.x < 0 ? flipCorrection : -flipCorrection), transform.position.y, 0);
    }

    private void Die()
    {
        if (RoomManager.instance.GetMatchInProgress())
        {
            UIManager.instance.SetActiveControllerPanel(false);
            UIManager.instance.SetActiveLifeAndPowerBar(false);
            PhotonNetwork.Destroy(gameObject);
        }
        else
        {
            transform.position = Vector2.left * 7;
            canMove = true;
        }
    }

    public void Kick(Vector2 direction)
    {
        thisRigidbody.AddForce(direction * kickForce, ForceMode2D.Impulse);
    }

    #region Buttons
    public void JumpButtonDown()
    {
        if (feetScript.OnFloor() && !animator.GetBool("Jump")) currentJumpForce = jumpForce;
    }

    public void JumpButtonUp()
    {
        currentJumpForce = 0;
    }

    public void BeatButtonDown()
    {
        move = false;
        if (animator != null) animator.SetBool("Attack", true);
    }

    public void BeatButtonUp()
    {
        move = true;
        if (animator != null) animator.SetBool("Attack", false);
    }
    #endregion

    #region Getters e Setters
    public Animator GetAnimator()
    {
        return animator;
    }

    public PhotonView GetPhotonView()
    {
        return photonView;
    }

    public void IncreaseLife(float life)
    {
        this.life += life;
        this.life = Mathf.Clamp(this.life, 0.0f, maximumLife);

        if (photonView.IsMine) UIManager.instance.SetLifeBarFillImageFillAmount(this.life / maximumLife);

        if (this.life == 0)
        {
            if (animator != null)
            {
                canMove = false;
                animator.SetTrigger("Die");
            }
            else Die();
        }
    }

    public float GetLife()
    {
        return life;
    }

    public float GetMaximumLife()
    {
        return maximumLife;
    }

    public void IncreasePower(float power)
    {
        this.power += power;
        this.power = Mathf.Clamp(this.power, 0.0f, maximumPower);

        if (photonView.IsMine) UIManager.instance.SetPowerBarFillImageFillAmount(this.power / maximumPower);
    }

    public float GetPower()
    {
        return power;
    }

    public float GetMaximumPower()
    {
        return maximumPower;
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (photonView.IsMine)
        {
            if (collision.CompareTag("Damage"))
            {
                IncreaseLife(-25.0f);
                IncreasePower(-maximumPower);
            }
            else if (collision.CompareTag("Life"))
            {
                IncreaseLife(25.0f);
                PhotonNetwork.Destroy(collision.transform.parent.gameObject);
            }
        }
    }
}
