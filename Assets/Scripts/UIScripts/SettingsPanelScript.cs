using UnityEngine;

public class SettingsPanelScript : MonoBehaviour
{
    private void OnEnable()
    {
        UpdateSettingsWindows();
    }

    public void UpdateSettingsWindows()
    {
        UIManager.instance.SetMusicSliderValue(PlayerPrefs.GetFloat("MusicVolumeScale", 1.0f));
        UIManager.instance.SetSoundEffectsSliderValue(PlayerPrefs.GetFloat("SoundEffectsVolumeScale", 0.5f));
        UIManager.instance.SetRegionButtonText(PlayerPrefs.GetString("RegionText", "EUA"));
        UIManager.instance.SetActiveRemoveAdsObject(PlayerPrefs.GetInt("RemoveAds", 0) == 1 ? false : true);
    }
}
