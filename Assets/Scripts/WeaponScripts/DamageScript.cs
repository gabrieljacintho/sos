using Photon.Pun;
using System.Collections;
using UnityEngine;

public class DamageScript : MonoBehaviour
{
    [SerializeField]
    private PhotonView photonView;
    [HideInInspector]
    public PlayerScript playerScript;

    [Space]
    [SerializeField]
    private float damage = 25.0f;
    [SerializeField]
    private float timeToDestroy = 6.0f;
    [SerializeField]
    private bool isDamageArea;

    private void Start()
    {
        if (photonView.IsMine)
        {
            if (playerScript != null) damage *= Mathf.Clamp(playerScript.GetPower() / playerScript.GetMaximumPower(), 0.2f, 1);
            StartCoroutine(Destroy(gameObject, timeToDestroy));
        }
    }
    
    public PhotonView GetPhotonView()
    {
        return photonView;
    }

    public PlayerScript GetPlayerScript()
    {
        return playerScript;
    }

    public void SetDamage(float value)
    {
        damage = value;
    }

    public float GetDamage()
    {
        return damage;
    }

    public bool IsDamageArea()
    {
        return isDamageArea;
    }

    private IEnumerator Destroy(GameObject gameObject, float time)
    {
        yield return new WaitForSeconds(time);
        PhotonNetwork.Destroy(gameObject);
    }
}
