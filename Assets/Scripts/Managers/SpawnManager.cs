using Photon.Pun;
using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

    [SerializeField]
    private GameObject ghostPrefab;
    [SerializeField]
    private GameObject lifePrefab;
    [SerializeField]
    private GameObject[] playersPrefab;
    [SerializeField]
    private GameObject roomManager;

    private int enemiesInTheRound;
    private GameObject[] enemiesInTheRoundObjects;
    private int deadEnemiesInTheRound;


    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Update()
    {
        enemiesInTheRoundObjects = GameObject.FindGameObjectsWithTag("Enemy");
        deadEnemiesInTheRound = enemiesInTheRound - enemiesInTheRoundObjects.Length;
    }

    public void SpawnRoomManager()
    {
        PhotonNetwork.Instantiate(roomManager.name, Vector3.zero, Quaternion.identity);
    }

    public void SpawnPlayer()
    {
        PhotonNetwork.Instantiate(playersPrefab[Random.Range(0, playersPrefab.Length)].name, new Vector2(-7, -2), Quaternion.identity);
    }

    public void StartSpawnEnemies()
    {
        StartCoroutine(SpawnEnemies());
    }

    public void StartSpawnLifes()
    {
        if (PhotonNetwork.IsMasterClient) StartCoroutine(SpawnLifes());
    }

    public void IncreaseDeadEnemiesInTheRound()
    {
        deadEnemiesInTheRound++;
    }

    public void ResetEnemiesCount()
    {
        enemiesInTheRound = 0;
        deadEnemiesInTheRound = 0;
    }

    private IEnumerator SpawnEnemies()
    {
        while (RoomManager.instance.GetMatchInProgress())
        {
            if (enemiesInTheRound < RoomManager.instance.GetRound() * 2)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    Vector2 enemyPosition = new Vector2(Random.Range(-21.0f, 52.0f), 20.0f);
                    PhotonNetwork.Instantiate(ghostPrefab.name, enemyPosition, Quaternion.identity);
                }
                enemiesInTheRound++;
            }
            else if (enemiesInTheRound == deadEnemiesInTheRound && deadEnemiesInTheRound > 0) RoomManager.instance.NextRound(false);

            yield return new WaitForSeconds(3.0f);
        }
    }

    private IEnumerator SpawnLifes()
    {
        while (RoomManager.instance.GetMatchInProgress())
        {
            Vector2 lifePosition = new Vector2(Random.Range(-21.0f, 52.0f), 20.0f);
            PhotonNetwork.Instantiate(lifePrefab.name, lifePosition, Quaternion.identity);

            yield return new WaitForSeconds(Random.Range(10.0f, 30.0f));
        }
    }
}