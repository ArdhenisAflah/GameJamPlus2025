using System;
using UnityEngine;
using UnityEngine.Audio;

public class GameAudioManager : MonoBehaviour
{
    public static GameAudioManager Instance;

    [Header("Mixer (optional)")]
    public AudioMixer mixer;

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("Sound Library")]
    public Sound[] sounds;

    void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Assign audio data to sources if needed
        foreach (Sound s in sounds)
        {
            // You can expand: assign to bgmSource or sfxSource based on type
        }
    }

    // ------------------------------------------------------------
    // BGM
    // ------------------------------------------------------------

    public void PlayBGM(string soundName)
    {
        Sound s = Array.Find(sounds, snd => snd.name == soundName);

        if (s == null)
        {
            Debug.LogWarning("BGM not found: " + soundName);
            return;
        }

        bgmSource.clip = s.clip;
        bgmSource.loop = true;
        bgmSource.volume = s.volume;
        bgmSource.pitch = s.pitch;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    // ------------------------------------------------------------
    // SFX
    // ------------------------------------------------------------

    public void PlaySFX(string soundName)
    {
        Sound s = Array.Find(sounds, snd => snd.name == soundName);

        if (s == null)
        {
            Debug.LogWarning("SFX not found: " + soundName);
            return;
        }

        sfxSource.PlayOneShot(s.clip, s.volume);
    }

    public void PlaySFXPitch(string soundName, float pitch)
    {
        Sound s = Array.Find(sounds, snd => snd.name == soundName);

        if (s == null)
        {
            Debug.LogWarning("SFX not found: " + soundName);
            return;
        }

        sfxSource.pitch = pitch;
        sfxSource.PlayOneShot(s.clip, s.volume);
        sfxSource.pitch = 1f; // reset
    }

    // ------------------------------------------------------------
    // Fade BGM (optional)
    // ------------------------------------------------------------
    public void FadeBGM(float targetVolume, float duration)
    {
        StartCoroutine(FadeRoutine(targetVolume, duration));
    }

    private System.Collections.IEnumerator FadeRoutine(float target, float time)
    {
        float start = bgmSource.volume;
        float timer = 0f;

        while (timer < time)
        {
            timer += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(start, target, timer / time);
            yield return null;
        }
    }
}
