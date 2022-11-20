using Photon.Pun;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    private float xLimit = 1.5f;
    [SerializeField]
    private float yLimit = 0.75f;

    private Vector3 velocity = Vector3.zero;
    private GameObject playerObject;
    private Vector3 initialPosition = new Vector3(-7, 1, -10);
    private Vector3 targetPosition;


    private void Update()
    {
        if (playerObject == null)
        {
            if (!PhotonNetwork.InRoom && targetPosition != initialPosition) targetPosition = initialPosition;
            else if (UIManager.instance.GetPlayerObject() != null) playerObject = UIManager.instance.GetPlayerObject();
            else SelectAPlayerAtScene();
        }
        if (playerObject != null)
        {
            if (playerObject.transform.position.x - transform.position.x > xLimit)
            {
                targetPosition = new Vector3(playerObject.transform.position.x - xLimit, targetPosition.y, -10);
            }
            else if (transform.position.x - playerObject.transform.position.x > xLimit)
            {
                targetPosition = new Vector3(playerObject.transform.position.x + xLimit, targetPosition.y, -10);
            }

            if (playerObject.transform.position.y - transform.position.y > yLimit)
            {
                targetPosition = new Vector3(targetPosition.x, playerObject.transform.position.y - yLimit, -10);
            }
            else if (transform.position.y - playerObject.transform.position.y > yLimit)
            {
                targetPosition = new Vector3(targetPosition.x, playerObject.transform.position.y + yLimit, -10);
            }
        }

        targetPosition = new Vector3(targetPosition.x, Mathf.Clamp(targetPosition.y, 1, Mathf.Infinity), -10);

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, 0.3f);
    }

    private bool SelectAPlayerAtScene()
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

        if (playerObjects.Length > 0)
        {
            foreach (GameObject playerObject in playerObjects)
            {
                if (this.playerObject == null
                    || Vector2.Distance(playerObject.transform.position, transform.position) < Vector2.Distance(targetPosition, transform.position))
                {
                    this.playerObject = playerObject;
                    targetPosition = this.playerObject.transform.position;
                }
            }
            return true;
        }
        return false;
    }
}
