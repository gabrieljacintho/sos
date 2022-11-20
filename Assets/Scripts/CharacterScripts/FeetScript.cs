using UnityEngine;

public class FeetScript : MonoBehaviour
{
    private bool onFloor;


    public bool OnFloor()
    {
        return onFloor;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Floor")) onFloor = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Floor")) onFloor = false;
    }
}
