using UnityEngine;
using UnityEngine.UI;

public class AudioManagerButton : MonoBehaviour
{
    public Button toggleButton;
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;
    private bool isMuted;

    private static AudioManagerButton instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (toggleButton != null)
        {
            toggleButton.onClick.AddListener(ToggleAudio);
            isMuted = PlayerPrefs.GetInt("IsMuted", 0) == 1;
            UpdateButtonSprite();
            UpdateAudio();
        }
        else
        {
            Debug.LogError("Toggle button not assigned!");
        }
    }

    private void Update()
    {
        UpdateAudio();
    }

    void ToggleAudio()
    {
        isMuted = !isMuted;
        PlayerPrefs.SetInt("IsMuted", isMuted ? 1 : 0);
        UpdateButtonSprite();
        UpdateAudio();
        Debug.Log(isMuted ? "All audio muted." : "All audio unmuted.");
    }

    void UpdateAudio()
    {
        float volume = isMuted ? 0f : 0.35f;

        GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        foreach (GameObject obj in allObjects)
        {
            ApplyVolumeToAudioSources(obj.transform, volume);
        }
    }

    void ApplyVolumeToAudioSources(Transform parent, float volume)
    {
        AudioSource[] audioSources = parent.GetComponentsInChildren<AudioSource>(true);
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.volume = volume;
        }
    }

    void UpdateButtonSprite()
    {
        if (toggleButton != null && toggleButton.image != null)
        {
            toggleButton.image.sprite = isMuted ? soundOffSprite : soundOnSprite;
        }
        else
        {
            Debug.LogError("Button or button image not assigned!");
        }
    }
}

