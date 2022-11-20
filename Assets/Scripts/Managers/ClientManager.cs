using UnityEngine;

public class ClientManager : MonoBehaviour
{
    public static ClientManager instance;
    
    private int pendingXP;
    private int pendingCoins;


    public void Awake()
    {
        if (instance == null) instance = this;
    }

    public void Start()
    {
        UIManager.instance.UpdateLevelPanel();
    }

    private void UpdateLevel()
    {
        int xp = PlayerPrefs.GetInt("XP", 0);
        int level = 1;
        for (int i = 10; xp >= (Mathf.Pow(1.1f, i) * 10); i++) level++;

        PlayerPrefs.SetInt("Level", level);
        PlayerPrefs.Save();

        UIManager.instance.UpdateLevelPanel();
    }

    private void UpdateXP()
    {
        PlayerPrefs.SetInt("XP", PlayerPrefs.GetInt("XP", 0) + GetPendingXP());
        pendingXP = 0;
        UpdateLevel();
    }

    private void UpdateCoins()
    {
        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins", 0) + GetPendingCoins());
        pendingCoins = 0;
    }

    public void UpdateAll()
    {
        UpdateXP();
        UpdateCoins();
    }

    public void IncreasePendingXP(int value)
    {
        pendingXP += value;
    }

    public int GetPendingXP()
    {
        return pendingXP;
    }

    public void IncreasePendingCoins(int value)
    {
        pendingCoins = value;
    }

    public int GetPendingCoins()
    {
        return pendingCoins;
    }
}
