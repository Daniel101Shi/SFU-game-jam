// Assets/Scripts/AudioManager.cs
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1f;
    [Range(0.1f, 3f)] public float pitch = 1f;
    public bool loop = false;
    
    [HideInInspector] public AudioSource source;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    [Header("Sound Effects")]
    public Sound[] sounds;
    
    [Header("Music")]
    public AudioClip backgroundMusic;
    public float musicVolume = 0.5f;
    private AudioSource musicSource;
    
    [Header("Settings")]
    [Range(0f, 1f)] public float masterVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;
    
    private Dictionary<string, Sound> soundDictionary = new Dictionary<string, Sound>();

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudio();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        PlayBackgroundMusic();
    }
    
    private void InitializeAudio()
    {
        // Create audio sources for each sound
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
            
            // Add to dictionary for quick lookup
            if (!soundDictionary.ContainsKey(sound.name))
            {
                soundDictionary.Add(sound.name, sound);
            }
        }
        
        // Setup music source
        if (backgroundMusic != null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.clip = backgroundMusic;
            musicSource.volume = musicVolume;
            musicSource.loop = true;
        }
    }
    
    public void PlaySound(string soundName)
    {
        if (soundDictionary.TryGetValue(soundName, out Sound sound))
        {
            if (sound.source != null)
            {
                sound.source.volume = sound.volume * sfxVolume * masterVolume;
                sound.source.Play();
            }
        }
        else
        {
            Debug.LogWarning($"Sound '{soundName}' not found in AudioManager!");
        }
    }
    
    public void PlaySoundWithPitch(string soundName, float pitchVariation = 0.1f)
    {
        if (soundDictionary.TryGetValue(soundName, out Sound sound))
        {
            if (sound.source != null)
            {
                sound.source.volume = sound.volume * sfxVolume * masterVolume;
                sound.source.pitch = sound.pitch + Random.Range(-pitchVariation, pitchVariation);
                sound.source.Play();
            }
        }
    }
    
    public void StopSound(string soundName)
    {
        if (soundDictionary.TryGetValue(soundName, out Sound sound))
        {
            if (sound.source != null && sound.source.isPlaying)
            {
                sound.source.Stop();
            }
        }
    }
    
    public void PlayBackgroundMusic()
    {
        if (musicSource != null && !musicSource.isPlaying)
        {
            musicSource.volume = musicVolume * masterVolume;
            musicSource.Play();
        }
    }
    
    public void StopBackgroundMusic()
    {
        if (musicSource != null && musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }
    
    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        
        // Update all audio sources
        foreach (Sound sound in sounds)
        {
            if (sound.source != null)
            {
                sound.source.volume = sound.volume * sfxVolume * masterVolume;
            }
        }
        
        if (musicSource != null)
        {
            musicSource.volume = musicVolume * masterVolume;
        }
    }
    
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        
        // Update all sound effect sources
        foreach (Sound sound in sounds)
        {
            if (sound.source != null)
            {
                sound.source.volume = sound.volume * sfxVolume * masterVolume;
            }
        }
    }
    
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        
        if (musicSource != null)
        {
            musicSource.volume = musicVolume * masterVolume;
        }
    }
    
    public bool IsSoundPlaying(string soundName)
    {
        if (soundDictionary.TryGetValue(soundName, out Sound sound))
        {
            return sound.source != null && sound.source.isPlaying;
        }
        return false;
    }
}