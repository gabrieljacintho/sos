using Photon.Pun;
using System.Collections;
using UnityEngine;

public class DestroyObjectScript : MonoBehaviour
{
    [SerializeField]
    private float minTime = 10.0f, maxTime = 30.0f;

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient) StartCoroutine(DestroyObject());
    }

    private IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(Random.Range(minTime, maxTime));
        PhotonNetwork.Destroy(gameObject);
    }
}
