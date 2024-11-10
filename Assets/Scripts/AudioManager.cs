using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private Dictionary<string, AudioSource> audioSources = new Dictionary<string, AudioSource>();
    public AudioSource musicSource;  // Separate AudioSource for background music
    public List<AudioClip> audioClips;  // A list of all your audio clips

    private void Start()
    {
        EventBus.Subscribe<Attack>(OnAttack);
        EventBus.Subscribe<Heal>(OnHeal);
        EventBus.Subscribe<Block>(OnBlock);
        EventBus.Subscribe<TakeDMG>(OnTakeDMG);
        EventBus.Subscribe<Charge>(OnCharge);
        EventBus.Subscribe<ShieldDestroyed>(OnShieldDestroyed);
        EventBus.Subscribe<EventAddShield>(OnEventAddShield);
        EventBus.Subscribe<BeginEncounter>(OnBeginEncounter);
    }
    void Awake()
    {
        // Set up singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Makes sure the AudioManager persists across scenes
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Initialize audio sources
        foreach (var clip in audioClips)
        {
            GameObject soundObject = new GameObject(clip.name);
            soundObject.transform.SetParent(transform);
            AudioSource source = soundObject.AddComponent<AudioSource>();
            source.clip = clip;
            audioSources[clip.name] = source;
        }
    }

    public void PlaySound(string name, float volume = 1.0f)
    {
        if (audioSources.ContainsKey(name))
        {
            audioSources[name].volume = volume;
            audioSources[name].Play();
        }
        else
        {
            Debug.LogWarning("Audio clip not found: " + name);
        }
    }

    public void PlayMusic(AudioClip clip, float volume = 1.0f)
    {
        musicSource.clip = clip;
        musicSource.volume = volume;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopMusic()
    {
        if (musicSource.isPlaying)
            musicSource.Stop();
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void StopSound(string name)
    {
        if (audioSources.ContainsKey(name) && audioSources[name].isPlaying)
        {
            audioSources[name].Stop();
        }
    }

    private void OnAttack(Attack e)
    {
        if (!e.is_ranged)
        {
            PlaySound("Attack", 0.6f);
        }
        else
        {
            PlaySound("FireProjectile", 0.6f);
        }
    }

    private void OnHeal(Heal e)
    {
        PlaySound("Heal", 0.5f);
    }

    private void OnBlock(Block e)
    {
        PlaySound("Block", 0.5f);
    }

    private void OnTakeDMG(TakeDMG e)
    {
        PlaySound("TakeDMG", 0.5f);
    }

    private void OnCharge(Charge e)
    {
        StartCoroutine(PlaySoundFor("Charge", e.duration, 0.5f));
    }
    private void OnShieldDestroyed(ShieldDestroyed e)
    {
        PlaySound("ShieldDestroyed", 0.4f);
    }

    private void OnEventAddShield(EventAddShield e)
    {
        PlaySound("Shield", 0.3f);
    }
    private void OnBeginEncounter(BeginEncounter e)
    {
        StopMusic();
        if (e.is_boss)
        {
            PlayMusic(audioSources["BossBattleMusic"].clip);
        }
        else
        {
            PlayMusic(audioSources["BattleMusic"].clip, 0.5f);
        }
    }

    private void OnEnable()
    {
        // Register to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Unregister from the sceneLoaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Overworld") // Check if it's the correct scene
        {
            PlayMusic(audioSources["OverworldMusic"].clip);
        }
        else if (scene.name == "MainMenu") // Check if it's the correct scene
        {
            StopMusic();
        }
        else if (scene.name == "PvP") // Check if it's the correct scene
        {
            PlayMusic(audioSources["BattleMusic"].clip);
        }
    }


    private IEnumerator PlaySoundFor(string name, float duration, float volume = 1.0f)
    {
        PlaySound(name, volume);
        yield return new WaitForSeconds(duration);
        StopSound(name);
    }
}
