using UnityEngine;
using UnityEngine.UI;

public class MusicToggle : MonoBehaviour, IDataPersistence
{
    [SerializeField] private Toggle soundToggle;
    [SerializeField] private Image buttonImage;
    [SerializeField] private Sprite soundOnSprite;
    [SerializeField] private Sprite soundOffSprite;
    [SerializeField] private AudioSource musicSource;

    private void Start()
    {
        UpdateButtonImage(soundToggle.isOn);

        soundToggle.onValueChanged.AddListener(OnSoundToggleChanged);
    }

    private void OnSoundToggleChanged(bool isOn)
    {
        UpdateButtonImage(isOn);

        if (musicSource != null)
        {
            musicSource.mute = !isOn;
        }
    }

    private void UpdateButtonImage(bool isOn)
    {
        buttonImage.sprite = isOn ? soundOnSprite : soundOffSprite;
    }

    public void LoadData(GameData data)
    {
        soundToggle.isOn = data.musicState;
        OnSoundToggleChanged(data.musicState);
    }

    public void SaveData(ref GameData data)
    {
        data.musicState = soundToggle.isOn;
    }


}
