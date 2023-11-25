using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    private AudioClip currentAudioClip;
    GameObject audioObject;

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
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Dictionary to store the currently playing instances of each AudioClip
    private Dictionary<AudioClip, List<AudioSource>> playingInstances = new Dictionary<AudioClip, List<AudioSource>>();

    public void PlaySound(AudioClip clip, Vector3 initialPosition, int maxConcurrentInstances = 3)
    {
        if (currentAudioClip == clip) return;

        if (clip == null)
        {
            Debug.LogWarning("Audio clip is null.");
            return;
        }

        currentAudioClip = clip;

        // Check if the audio clip is already playing the maximum allowed instances
        if (playingInstances.ContainsKey(clip) && playingInstances[clip].Count >= maxConcurrentInstances)
        {
            Debug.Log($"Max instances reached for audio clip: {clip.name}");
            return;
        }

        audioObject = new GameObject("AudioSourceObject");
        audioObject.transform.position = initialPosition;

        AudioSource audioSource = audioObject.AddComponent<AudioSource>();
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
        Destroy(audioObject, clip.length);
    }

    public void UpdateSoundPosition(Vector3 newPosition)
    {
        if (audioObject != null)
        {
            audioObject.transform.position = newPosition;
        }
    }
}