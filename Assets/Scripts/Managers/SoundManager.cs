using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SoundManager : MonoBehaviour
{

    public static SoundManager instance;

    private AudioSource asForButtons;
    private AudioSource asForBackground;

    private List<AudioSource> asForAnySFX = new();
    [SerializeField] private AudioClip bgSoundClip;


    [HideInInspector] public bool _soundToggle = true;
    [HideInInspector] public bool _musicToggle = true;



    public AudioClip characterButtonSound;
    public AudioClip menuButtonSound;
    public AudioClip wheelButtonSound;
    public AudioClip sketchingSound;
    public AudioClip playButtonAudio;
    public AudioClip cancelButtonAudio;
    public AudioClip cameraButtonSound;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        asForBackground = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
        asForBackground.loop = true;
        asForBackground.playOnAwake = false;

        asForButtons = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
        asForButtons.playOnAwake = false;

        for (int i = 0; i < 5; i++)
        {
            AudioSource source = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
            source.playOnAwake = false;
            asForAnySFX.Add(source);
        }
    }

    public void ToggleSound()
    {
        PlayButtonSound();
        _soundToggle = !_soundToggle;
    }

    public void ToggleMusic()
    {
        PlayButtonSound();
        _musicToggle = !_musicToggle;
        if (_musicToggle)
        {
            PlayBackgroundMusic(bgSoundClip);
        }
        else
        {
            StopBackgroundMusic();
        }
    }


    public void PlayButtonSound(AudioClip audioClip = null)
    {
        if (_soundToggle)
        {
            if (audioClip != null)
            {
                asForButtons.clip = audioClip;
            }
            asForButtons.Play();
        }
    }

    public void PlayCharacterButtonSound()
    {
        if (_soundToggle)
        {
            asForButtons.clip = characterButtonSound;
            asForButtons.Play();
        }
    }

    public void PlayMenuButtonSound()
    {
        if (_soundToggle)
        {
            asForButtons.clip = menuButtonSound;
            asForButtons.Play();
        }
    }

    public void PlayCameraButtonSound()
    {
        if (_soundToggle)
        {
            asForButtons.clip = cameraButtonSound;
            asForButtons.Play();
        }
    }

    public void PlaySFX(AudioSource audioSource)
    {
        if (_soundToggle)
        {
            audioSource.Play();
        }
    }
    public void SetMusicVolume(float volume)
    {
        asForBackground.volume = volume;
    }


    public void PlayBackgroundMusic(AudioClip clip, bool isLoop = true)
    {
        if (_musicToggle)
        {
            asForBackground.clip = clip;
            asForBackground.loop = isLoop;
            asForBackground.Play();
        }

    }

    public void StopBackgroundMusic()
    {
        asForBackground.Stop();
    }

    public void PlaySfxSound(AudioClip audioClip, bool isLoop = false, bool canPlaySameAudio = false, float delay = 0, float volume = 1.0f, float volumeEaseInTime = 0f)
    {
        if (_soundToggle)
        {
            AudioSource audioSource = CheckFreeAudioSource();
            if (!canPlaySameAudio)
            {
                if (IsAnyOnePlayingSameAudio(audioClip))
                {
                    return;
                }
            }
            audioSource.clip = audioClip;
            audioSource.loop = isLoop;
            audioSource.PlayDelayed(delay);
            if (volumeEaseInTime > 0)
            {
                audioSource.DOFade(volume, volumeEaseInTime).From(0f);
            }
            else
            {
                audioSource.volume = volume;
            }
        }
    }

    public void StopSfx(AudioClip clip)
    {
        for (int i = 0; i < asForAnySFX.Count; i++)
        {
            if (asForAnySFX[i].isPlaying && asForAnySFX[i].clip == clip)
            {
                asForAnySFX[i].loop = false;
                asForAnySFX[i].Stop();

            }
        }
    }
    public void PauseSfx(AudioSource audioSource)
    {
        audioSource.Pause();
    }
    public void ResumeSfx(AudioSource audioSource)
    {
        audioSource.UnPause();
    }


    private AudioSource CheckFreeAudioSource()
    {
        for (int i = 0; i < asForAnySFX.Count; i++)
        {
            if (!asForAnySFX[i].isPlaying)
            {
                return asForAnySFX[i];
            }
        }
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        asForAnySFX.Add(audioSource);
        return audioSource;
    }
    public bool IsAnyOnePlayingSameAudio(AudioClip clip)
    {
        for (int i = 0; i < asForAnySFX.Count; i++)
        {
            if (asForAnySFX[i].isPlaying && asForAnySFX[i].clip == clip)
            {
                return true;
            }
        }
        return false;
    }

    public void StopAllSfxAndButtonSound()
    {
        for (int i = 0; i < asForAnySFX.Count; i++)
        {
            asForAnySFX[i].Stop();
            asForAnySFX[i].enabled = false;
        }
        asForButtons.Stop();
        Invoke("EnableAllAudioSource", .5f);
    }
    private void EnableAllAudioSource()
    {
        for (int i = 0; i < asForAnySFX.Count; i++)
        {
            asForAnySFX[i].enabled = true;
        }
    }
}
