using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField]
    private Canvas foregroundCanvas;

    [Header("Panels")]
    [SerializeField]
    private GameObject menuPanelObject;
    [SerializeField]
    private GameObject storePanelObject;
    [SerializeField]
    private GameObject podiumPanelObject;
    [SerializeField]
    private GameObject selectPanelObject;
    [SerializeField]
    private GameObject gameplayPanelObject;
    [SerializeField]
    private GameObject controllerPanelObject;
    [SerializeField]
    private GameObject statusPanelObject;
    [SerializeField]
    private GameObject pausePanelObject;
    [SerializeField]
    private GameObject endCoinPanelObject;
    [SerializeField]
    private GameObject endPanelObject;
    [SerializeField]
    private GameObject levelPanelObject;
    [SerializeField]
    private GameObject settingsPanelObject;
    [SerializeField]
    private GameObject insufficientCoinsPanelObject;
    [SerializeField]
    private GameObject loadingPanelObject;

    [Header("MenuPanelElements")]
    [SerializeField]
    private Text codeInputText;

    [Header("GameplayPanelElements")]
    [SerializeField]
    private GameObject roomButtonObject;
    [SerializeField]
    private GameObject pauseButtonObject;

    [Header("StatusPanelElements")]
    [SerializeField]
    private Text roundText;
    [SerializeField]
    private RectTransform lifeBarRectTransform;
    [SerializeField]
    private Image lifeBarFillImage;
    [SerializeField]
    private RectTransform powerBarRectTransform;
    [SerializeField]
    private Image powerBarFillImage;

    [Header("RoomButtonElements")]
    [SerializeField]
    private Image roomButtonImage;
    [SerializeField]
    private Text roomButtonText;

    [Header("SettingsPanelElements")]
    [SerializeField]
    private Slider musicSlider;
    [SerializeField]
    private Slider soundEffectsSlider;
    [SerializeField]
    private Text regionButtonText;
    [SerializeField]
    private GameObject removeAdsObject;

    [Header("LevelPanelElements")]
    [SerializeField]
    private Text levelText;
    [SerializeField]
    private Image levelBarFill;

    [Space]
    [SerializeField]
    private JoystickScript joystickScript;
    private PlayerScript playerScript;


    private void Awake()
    {
        if (instance == null) instance = this;
    }

    #region PanelElements
    #region MenuPanelElements
    public void SettingsButton()
    {
        ActivePanel("SettingsPanel");
        SoundsManager.instance.PlayButtonSoundEffect();
    }

    public void StoreButton()
    {
        ActivePanel("StorePanel");
        SoundsManager.instance.PlayButtonSoundEffect();
    }

    public void PodiumButton()
    {
        ActivePanel("PodiumPanel");
        SoundsManager.instance.PlayButtonSoundEffect();
    }

    public void CreateRoomButton()
    {
        PhotonConnection.instance.CreateRoom();
        SoundsManager.instance.PlayButtonSoundEffect();
    }

    public void FindRoomButton()
    {
        PhotonConnection.instance.FindRoom();
        SoundsManager.instance.PlayButtonSoundEffect();
    }

    public void CodeInputButton()
    {
        PhotonConnection.instance.JoinRoom(GetCodeInputText());
        SoundsManager.instance.PlayButtonSoundEffect();
    }
    #endregion

    #region PodiumPanelElements
    public void ChangePodiumButton()
    {
        SoundsManager.instance.PlayButtonSoundEffect();
    }

    public void EnterWithFacebookButton()
    {
        SoundsManager.instance.PlayButtonSoundEffect();
    }
    #endregion

    #region GameplayPanelElements
    #region ControllerPanelElements
    public void JumpButtonDown()
    {
        if (playerScript != null) playerScript.JumpButtonDown();
    }

    public void JumpButtonUp()
    {
        if (playerScript != null) playerScript.JumpButtonUp();
    }

    public void ShootButtonDown()
    {
        if (playerScript != null) playerScript.BeatButtonDown();
    }

    public void ShootButtonUp()
    {
        if (playerScript != null) playerScript.BeatButtonUp();
    }
    #endregion

    #region PausePanelElements
    public void ContinueButton()
    {
        ActivePanel("GameplayPanel");
        SoundsManager.instance.PlayButtonSoundEffect();
    }

    public void ExitButton()
    {
        PhotonConnection.instance.LeaveRoom();
        SoundsManager.instance.PlayButtonSoundEffect();
    }
    #endregion

    #region RoomButtonElements
    public void RoomButton() // Arrumar!!
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (roomButtonText.text == "PRIVATE") RoomManager.instance.SetRoomAsPublic();
            else RoomManager.instance.SetRoomAsPrivate();
            SoundsManager.instance.PlayButtonSoundEffect();
        }
    }

    public void ShareRoomButton()
    {
        SoundsManager.instance.PlayButtonSoundEffect();
    }
    #endregion

    public void PauseButton()
    {
        ActivePanel("PausePanel");
        SoundsManager.instance.PlayButtonSoundEffect();
    }
    #endregion

    #region SettingsPanelElements
    public void MusicSlider()
    {
        SoundsManager.instance.SetMusicVolumeScale(musicSlider.value);
    }

    public void SoundEffectsSlider()
    {
        SoundsManager.instance.SetSoundEffectsVolumeScale(soundEffectsSlider.value);
    }

    public void RegionButton()
    {
        SoundsManager.instance.PlayButtonSoundEffect();
    }

    public void RemoveAdsButton()
    {
        SoundsManager.instance.PlayButtonSoundEffect();
    }

    public void CloseSettingsPanelButton()
    {
        PlayerPrefs.SetFloat("MusicVolumeScale", SoundsManager.instance.GetMusicVolumeScale());
        PlayerPrefs.SetFloat("SoundEffectsVolumeScale", SoundsManager.instance.GetSoundEffectsVolumeScale());
        PlayerPrefs.Save();
        SetActiveSettingsPanel(false);
        SoundsManager.instance.PlayButtonSoundEffect();
    }
    #endregion

    #region InsufficientCoinsPanel
    public void CloseInsufficientCoinsPanelButton()
    {
        SetActiveInsufficientCoinsPanel(false);
    }
    #endregion

    public void MenuButton()
    {
        ActivePanel("MenuPanel");
        SoundsManager.instance.PlayButtonSoundEffect();
    }

    public void ProductButton()
    {
        SoundsManager.instance.PlayButtonSoundEffect();
    }
    #endregion

    #region Setters
    #region PanelSetters
    private void SetActiveMenuPanel(bool value)
    {
        menuPanelObject.SetActive(value);
    }

    private void SetActiveStorePanel(bool value)
    {
        storePanelObject.SetActive(value);
    }

    private void SetActivePodiumPanel(bool value)
    {
        podiumPanelObject.SetActive(value);
    }

    private void SetActiveSettingsPanel(bool value)
    {
        settingsPanelObject.SetActive(value);
    }

    private void SetActiveSelectPanel(bool value)
    {
        selectPanelObject.SetActive(value);
    }

    private void SetActiveInsufficientCoinsPanel(bool value)
    {
        insufficientCoinsPanelObject.SetActive(value);
    }

    private void SetActiveGameplayPanel(bool value)
    {
        gameplayPanelObject.SetActive(value);
        if (value) UpdateGameplayPanel();
    }

    public void SetActiveControllerPanel(bool value)
    {
        if (value != controllerPanelObject.activeInHierarchy) ResetPlayerController();
        controllerPanelObject.SetActive(value);
    }

    public void SetActiveStatusPanel(bool value)
    {
        statusPanelObject.SetActive(value);
        if (value)
        {
            SetActiveLifeAndPowerBar(playerScript != null);
            SetLifeBarWidth(playerScript.GetMaximumLife());
            SetLifeBarFillImageFillAmount(playerScript.GetLife() / playerScript.GetMaximumLife());
            SetPowerBarWidth(playerScript.GetMaximumPower());
            SetPowerBarFillImageFillAmount(playerScript.GetPower() / playerScript.GetMaximumPower());
        }
    }

    private void SetActivePausePanel(bool value)
    {
        pausePanelObject.SetActive(value);
    }

    private void SetActiveEndCoinPanel(bool value)
    {
        endCoinPanelObject.SetActive(value);
    }

    private void SetActiveEndPanel(bool value)
    {
        endPanelObject.SetActive(value);
    }

    private void SetActiveLevelPanel(bool value)
    {
        levelPanelObject.SetActive(value);
        if (value) UpdateLevelPanel();
    }

    private void SetActiveLoadingPanel(bool value)
    {
        loadingPanelObject.SetActive(value);
    }
    #endregion

    #region GameplayPanelSetters
    #region StatusPanelSetters
    public void SetRoundText(string text)
    {
        roundText.text = text;
    }

    public void SetActiveLifeAndPowerBar(bool value)
    {
        lifeBarRectTransform.gameObject.SetActive(value);
        powerBarRectTransform.gameObject.SetActive(value);
    }

    private void SetLifeBarWidth(float width)
    {
        lifeBarRectTransform.sizeDelta = new Vector2(width, lifeBarRectTransform.sizeDelta.y);
    }

    public void SetLifeBarFillImageFillAmount(float fillAmount)
    {
        lifeBarFillImage.fillAmount = fillAmount;
    }

    private void SetPowerBarWidth(float width)
    {
        powerBarRectTransform.sizeDelta = new Vector2(width, powerBarRectTransform.sizeDelta.y);
    }

    public void SetPowerBarFillImageFillAmount(float fillAmount)
    {
        powerBarFillImage.fillAmount = fillAmount;
    }
    #endregion

    #region RoomButtonSetters
    public void SetActiveRoomButton(bool value)
    {
        roomButtonObject.SetActive(value);
    }

    public void SetRoomButtonImageColor(Color color)
    {
        roomButtonImage.color = color;
    }

    public void SetRoomButtonText(string text)
    {
        roomButtonText.text = text;
    }
    #endregion

    public void UpdateGameplayPanel()
    {
        SetActiveControllerPanel(playerScript != null);
        SetActiveRoomButton(!RoomManager.instance.GetMatchInProgress());
        SetActiveStatusPanel(RoomManager.instance.GetMatchInProgress());
    }

    public void ResetPlayerController()
    {
        if (playerScript != null)
        {
            playerScript.BeatButtonUp();
            playerScript.JumpButtonUp();
        }
        joystickScript.ResetJoystick();
    }

    public void SetActivePauseButtonObject(bool value)
    {
        pauseButtonObject.SetActive(value);
    }
    #endregion

    #region SettingsPanelSetters
    public void SetMusicSliderValue(float value)
    {
        musicSlider.value = value;
    }

    public void SetSoundEffectsSliderValue(float value)
    {
        soundEffectsSlider.value = value;
    }

    public void SetRegionButtonText(string text)
    {
        regionButtonText.text = text;
    }

    public void SetActiveRemoveAdsObject(bool value)
    {
        removeAdsObject.SetActive(value);
    }
    #endregion

    #region LevelPanelSetters
    public void SetLevelText(string text)
    {
        levelText.text = text;
    }

    public void SetLevelBarFillAmount(float fillAmount)
    {
        levelBarFill.fillAmount = fillAmount;
    }

    public void UpdateLevelPanel()
    {
        SetLevelText("LEVEL " + PlayerPrefs.GetInt("Level", 1).ToString());
        SetLevelBarFillAmount(PlayerPrefs.GetInt("XP", 0) / (Mathf.Pow(1.1f, PlayerPrefs.GetInt("Level", 1) + 10) * 10));
    }
    #endregion

    public void SetPlayerScript(PlayerScript playerScript)
    {
        this.playerScript = playerScript;
    }
    #endregion

    #region Getters
    public JoystickScript GetJoystickScript()
    {
        return joystickScript;
    }

    public GameObject GetPlayerObject()
    {
        if (playerScript == null) return null;
        return playerScript.gameObject;
    }

    public string GetCodeInputText()
    {
        return codeInputText.text;
    }
    #endregion

    public void ActivePanel(string panelName)
    {
        /*if (settingsPanelObject.name == panelName)
        {
            SetActiveSettingsPanel(true);
            return;
        }
        else if (insufficientCoinsPanelObject.name == panelName)
        {
            SetActiveInsufficientCoinsPanel(true);
            return;
        }*/
        SetActiveMenuPanel(menuPanelObject.name == panelName);
        //SetActiveStorePanel(storePanelObject.name == panelName);
        //SetActivePodiumPanel(podiumPanelObject.name == panelName);
        SetActiveSelectPanel(selectPanelObject.name == panelName);
        SetActiveGameplayPanel(gameplayPanelObject.name == panelName);
        SetActivePausePanel(pausePanelObject.name == panelName);
        //SetActiveEndCoinPanel(endCoinPanelObject.name == panelName);
        SetActiveEndPanel(endPanelObject.name == panelName);
        /*SetActiveLevelPanel(menuPanelObject.name == panelName
            || podiumPanelObject.name == panelName
            || pausePanelObject.name == panelName);*/
        SetActiveLoadingPanel(loadingPanelObject.name == panelName);
    }
}
