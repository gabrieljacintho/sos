using UnityEngine;

public class ClimbScript : MonoBehaviour
{
    [SerializeField]
    private bool checkCanClimb;
    [SerializeField]
    private ClimbScript checkCanClimbScript;

    private bool canClimb = true;


    public bool CanClimb()
    {
        return canClimb;
    }

    private void Climb()
    {
        transform.parent.position = new Vector2(transform.parent.position.x + (transform.parent.localScale.x * 0.35f), transform.parent.position.y + 1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Floor"))
        {
            if (checkCanClimb) canClimb = false;
            else if (!checkCanClimb && checkCanClimbScript.CanClimb()) Climb();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Floor") && checkCanClimb) canClimb = true;
    }
}
