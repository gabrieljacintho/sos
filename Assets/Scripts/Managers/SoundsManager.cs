using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    public static SoundsManager instance;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip[] songs;

    [SerializeField]
    private AudioClip buttonSoundEffect;

    private float musicVolumeScale = 1.0f;
    private float soundEffectsVolumeScale = 0.5f;


    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        musicVolumeScale = PlayerPrefs.GetFloat("MusicVolumeScale", 1.0f);
        soundEffectsVolumeScale = PlayerPrefs.GetFloat("SoundEffectsVolumeScale", 0.5f);
        audioSource.volume = musicVolumeScale;
    }

    private void Update()
    {
        if (!audioSource.isPlaying && songs.Length > 0)
        {
            audioSource.clip = songs[Random.Range(0, songs.Length)];
            audioSource.Play();
        }
    }

    public void PlaySoundEffect(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip, GetSoundEffectsVolumeScale());
    }

    public void SetAudioSourceMute(bool value)
    {
        audioSource.mute = value;
    }

    #region Getters e Setters
    public void SetMusicVolumeScale(float value)
    {
        musicVolumeScale = value;
        audioSource.volume = musicVolumeScale;
    }

    public void SetSoundEffectsVolumeScale(float value)
    {
        soundEffectsVolumeScale = value;
    }

    public float GetMusicVolumeScale()
    {
        return musicVolumeScale;
    }

    public float GetSoundEffectsVolumeScale()
    {
        return soundEffectsVolumeScale;
    }
    #endregion

    public void PlayButtonSoundEffect()
    {
        if (buttonSoundEffect != null) audioSource.PlayOneShot(buttonSoundEffect, GetSoundEffectsVolumeScale());
    }
}
