using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic instance;

    public static BackgroundMusic Instance
    {
        get { return instance; }
    }


    [Header("Playlist")]
    [SerializeField] private AudioClip[] musicClips;
    [SerializeField] private int startIndex = 0;

    [Header("Settings")]
    [Range(0f, 1f)][SerializeField] private float volume = 0.35f;
    [SerializeField] private bool playOnStart = true;

    private AudioSource audioSource;
    private int currentIndex = -1;

    private void Awake()
    {
        // Singleton: only one music object exists
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.spatialBlend = 0f; // 2D :contentReference[oaicite:4]{index=4}
        audioSource.playOnAwake = false;
        audioSource.volume = volume;

        if (playOnStart)
        {
            SelectIndex(startIndex, true);
        }
        else
        {
            if (IsValidIndex(startIndex))
            {
                currentIndex = startIndex;
                audioSource.clip = musicClips[startIndex];
            }
        }
    }

    public void SelectIndex(int index, bool restartIfSame = false)
    {
        if (!IsValidIndex(index))
        {
            Debug.LogWarning("BackgroundMusic: Invalid music index " + index + ". Check your playlist in the Inspector.");
            return;
        }

        AudioClip nextClip = musicClips[index];

        if (nextClip == null)
        {
            Debug.LogWarning("BackgroundMusic: musicClips[" + index + "] is NULL.");
            return;
        }

        if (index == currentIndex)
        {
            // Unity: calling Play when the same clip is already playing restarts it. :contentReference[oaicite:5]{index=5}
            if (!restartIfSame)
            {
                return;
            }
        }

        currentIndex = index;

        // Unity: assigning a new clip replaces the old one; it does not auto-play. :contentReference[oaicite:6]{index=6}
        audioSource.clip = nextClip;
        audioSource.volume = volume;
        audioSource.Play(); // :contentReference[oaicite:7]{index=7}
    }

    public int GetCurrentIndex()
    {
        return currentIndex;
    }

    public void StopMusic()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }

    public void SetVolume(float newVolume)
    {
        volume = Mathf.Clamp01(newVolume);

        if (audioSource != null)
        {
            audioSource.volume = volume;
        }
    }

    private bool IsValidIndex(int index)
    {
        if (musicClips == null)
        {
            return false;
        }

        if (musicClips.Length == 0)
        {
            return false;
        }

        if (index < 0)
        {
            return false;
        }

        if (index >= musicClips.Length)
        {
            return false;
        }

        return true;
    }
}
