using Photon.Pun;
using System.Collections;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager instance;

    private bool matchInProgress;
    private int round = 1;


    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Update()
    {
        GameObject[] playersAlive = GameObject.FindGameObjectsWithTag("Player"); // Otimizar!!

        if (!GetMatchInProgress() && playersAlive.Length > 0)
        {
            foreach (GameObject playerAlive in playersAlive)
            {
                if (playerAlive.transform.position.x > 16) continue;
                else return;
            }

            StartMatch();
        }
        else if (GetMatchInProgress() && playersAlive.Length == 0 && PhotonNetwork.InRoom) StartCoroutine(RestartMatch());
    }

    public void StartMatch()
    {
        SetMatchInProgress(true);
        UIManager.instance.ActivePanel("GameplayPanel");
        //SpawnManager.instance.StartSpawnEnemies(); ------------------------------TESTE!!
        SpawnManager.instance.StartSpawnLifes();
    }

    private IEnumerator RestartMatch()
    {
        SetMatchInProgress(false);
        //ClientManager.instance.UpdateAll();
        UIManager.instance.ActivePanel("EndPanel");
        yield return new WaitForSeconds(5);
        //UIManager.instance.ActivePanel("EndCoinPanel");
        //yield return new WaitForSeconds(5);
        CleanScene();
        SpawnManager.instance.SpawnPlayer();
        UIManager.instance.ActivePanel("GameplayPanel");
        NextRound(true);
    }

    public void NextRound(bool reset)
    {
        if (reset) round = 1;
        else round++;

        SpawnManager.instance.ResetEnemiesCount();
        UIManager.instance.SetRoundText(round.ToString());
    }

    private void CleanScene()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            //GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            //foreach (GameObject enemy in enemies) PhotonNetwork.Destroy(enemy); ------------------------------TESTE!!

            GameObject[] lifes = GameObject.FindGameObjectsWithTag("Life");
            foreach (GameObject life in lifes) PhotonNetwork.Destroy(life.transform.parent.gameObject);
        }
    }

#region Setters
    public void SetRoomAsPublic() // Feature FUTURA!!
    {
        /*if (!PhotonNetwork.IsMasterClient) return;
        // Definir sala como pública
        UIManager.instance.SetRoomButtonText(PhotonNetwork.CurrentRoom.Name);
        UIManager.instance.SetRoomButtonImageColor(Color.green);*/
    }

    public void SetRoomAsPrivate() // Feature FUTURA!!
    {
        /*if (!PhotonNetwork.IsMasterClient) return;
        // Definir sala como privada
        UIManager.instance.SetRoomButtonText("PRIVATE");
        UIManager.instance.SetRoomButtonImageColor(Color.red);*/
    }
#endregion

    public void SetMatchInProgress(bool value)
    {
        matchInProgress = value;
    }

    public bool GetMatchInProgress()
    {
        return matchInProgress;
    }

    public int GetRound()
    {
        return round;
    }
}
