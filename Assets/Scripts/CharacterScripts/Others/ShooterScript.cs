using Photon.Pun;
using System.Collections;
using UnityEngine;

public class ShooterScript : MonoBehaviour
{
    [SerializeField]
    private PlayerScript playerScript;

    [Header("Components")]
    [SerializeField]
    private GameObject armObject;
    [SerializeField]
    private GameObject gunBarrel;
    [SerializeField]
    private GameObject bulletPrefab;

    [Header("Attributes")]
    [SerializeField]
    private float fireRatePerMinute = 300.0f;

    private bool loadedGun = true, aiming, beatingUp;

    private void Start()
    {
        if (playerScript.GetPhotonView().IsMine)
        {
            //playerScript.aimWeapon = AimWeapon;
            //playerScript.stopAimingWeapon = StopAimingWeapon;
        }
    }

    private void AimWeapon(Vector3 direction)
    {
        float angleCorrection = 0;
        if (direction.x < 0) angleCorrection += 180.0f;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - angleCorrection;

        armObject.transform.rotation = Quaternion.Euler(0, 0, angle);

        if (!aiming) aiming = true;
    }

    private void StopAimingWeapon()
    {
        if (aiming)
        {
            armObject.transform.rotation = Quaternion.Euler(Vector3.zero);
            aiming = false;
        }
    }

    private void Beat()
    {
        if (loadedGun && beatingUp)
        {
            float angleCorrection = 90.0f;

            if (transform.localScale.x < 0)
            {
                angleCorrection *= -1;
                playerScript.Kick(Vector2.right);
            }
            else playerScript.Kick(Vector2.left);

            PhotonNetwork.Instantiate(bulletPrefab.name, gunBarrel.transform.position, Quaternion.Euler(Vector3.forward * (armObject.transform.eulerAngles.z - angleCorrection)));

            loadedGun = false;
            StartCoroutine(LoadWeapon());
        }
    }

    private IEnumerator LoadWeapon()
    {
        yield return new WaitForSeconds(60 / fireRatePerMinute);
        loadedGun = true;
    }
}
