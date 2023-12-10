using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    private AudioClip currentAudioClip;
    GameObject audioObject;

    [SerializeField] private AudioClip ambiance;
    private AudioSource layerPlayer;


    [Range(0.0f, 1.0f)] public float soundtrackVolume = 1f;
    [Range(0.0f, 1.0f)] public float audioEntityVolume = 1f;

    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>();

                if (instance == null)
                {
                    GameObject obj = new GameObject("AudioManager");
                    instance = obj.AddComponent<AudioManager>();
                }
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject); 
        }
        else
        {
            //Destroy(gameObject);
        }

        layerPlayer = GetComponent<AudioSource>();
        LayerPlayer(ambiance);
    }

    // Dictionary to store the currently playing instances of each AudioClip
    private Dictionary<AudioClip, List<AudioSource>> playingInstances = new Dictionary<AudioClip, List<AudioSource>>();

    public void PlaySound(AudioClip clip, Vector3 initialPosition, int maxConcurrentInstances = 3, float volume = 1)
    {

        if (clip == null)
        {
            Debug.LogWarning("Audio clip is null.");
            return;
        }



        // Check if the audio clip is already playing the maximum allowed instances
        if (playingInstances.ContainsKey(clip) && playingInstances[clip].Count >= maxConcurrentInstances)
        {
            Debug.Log($"Max instances reached for audio clip: {clip.name}");
            return;
        }

        audioObject = new GameObject("AudioSourceObject");
        audioObject.transform.position = initialPosition;

        AudioSource audioSource = audioObject.AddComponent<AudioSource>();
        audioSource.volume = volume;
        audioSource.clip = clip;
        audioSource.spatialBlend = 1f; // Set to 1 for 3D spatialization

        audioSource.Play();

        // Add the audio source to the dictionary for tracking
        if (!playingInstances.ContainsKey(clip))
        {
            playingInstances[clip] = new List<AudioSource>();
        }
        playingInstances[clip].Add(audioSource);

        // Destroy the audio object after the clip has finished playing
        StartCoroutine(DestroyAudioObjectDelayed(audioObject, clip.length));

    }

    private System.Collections.IEnumerator DestroyAudioObjectDelayed(GameObject audioObject, float delay)
    {
        yield return new WaitForSeconds(delay);

        // Remove the AudioSource from the list
        AudioSource audioSource = audioObject.GetComponent<AudioSource>();
        AudioClip clipToRemove = audioSource.clip;

        if (playingInstances.ContainsKey(clipToRemove))
        {
            playingInstances[clipToRemove].Remove(audioSource);

            // If there are no more instances of this clip, remove it from the dictionary
            if (playingInstances[clipToRemove].Count == 0)
            {
                playingInstances.Remove(clipToRemove);
            }
        }

        // Destroy the audio object
        Destroy(audioObject);
    }

    public void UpdateSoundPosition(Vector3 newPosition)
    {
        if (audioObject != null)
        {
            audioObject.transform.position = newPosition;
        }
    }

    private void LayerPlayer(AudioClip clip)
    {
        layerPlayer.clip = clip;
        layerPlayer.volume = soundtrackVolume;
        layerPlayer.loop = true;
        layerPlayer.Play();
    }

    private List<AudioEntity> audioEntities = new List<AudioEntity>();

    public AudioEntity AddAsAnAudioEntity(GameObject gameObject)
    {
        AudioEntity audioEntity = gameObject.AddComponent<AudioEntity>();
        
        audioEntity.Initialize(audioEntityVolume);
        audioEntities.Add(audioEntity);
        return audioEntity;
    }
}

public class AudioEntity : MonoBehaviour
{
    private float _audioEntityVolume;
    private AudioSource audioSource;
    private string currentClipName;

    // Initialize the audio entity with a shared AudioSource
    public void Initialize(float volume)
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        _audioEntityVolume = volume;
        audioSource.spatialBlend = 1f; // Set to 1 for 3D spatialization

    }

    public void PlayState(AudioClip audioClip, float volume, bool overwrite = false, bool isLooping = false)
    {
        if (currentClipName == audioClip.name && overwrite == false) return;

        audioSource.clip = audioClip;
        audioSource.volume = volume * _audioEntityVolume;
        audioSource.loop = isLooping;
        audioSource.Play();

        currentClipName = audioClip.name;
    }
}