using UnityEngine;
using UnityEngine.UI;

public class CoinTextScript : MonoBehaviour
{
    private Text coinText;

    private void Awake()
    {
        coinText = GetComponent<Text>();
    }

    private void OnEnable()
    {
        UpdateCoinText();
    }

    public void UpdateCoinText()
    {
        coinText.text = PlayerPrefs.GetInt("Coins", 0).ToString();
    }
}
