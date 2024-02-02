using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioSource[] audioSources;
    public AudioClip[] audioClips;
    public AudioSource musicSource;
    public AudioMixerGroup audioMixerGroup;
    public float pitch, timer, piss;

    // Start is called before the first frame update


  void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        audioSources = new AudioSource[audioClips.Length];
        for (int i = 0; i < audioClips.Length; i++)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.outputAudioMixerGroup = audioMixerGroup;
            audioSource.clip = audioClips[i];
            audioSources[i] = audioSource;
        }
    }
    void Start()
    {
        pitch = 1;
    }

    // Update is called once per frame
    void Update()
    {
        timer+= Time.deltaTime * 0.2f;
        musicSource.pitch = Mathf.Lerp(piss,pitch, timer);

    }

   public void PlaySound(int clipIndex)
    {
        if (clipIndex < 0 || clipIndex >= audioSources.Length)
        {
            Debug.LogError("Clip index out of range");
            return;
        }

        audioSources[clipIndex].Play();
    }
   public void StopSound(int clipIndex)
    {
        if (clipIndex < 0 || clipIndex >= audioSources.Length)
        {
            Debug.LogError("Clip index out of range");
            return;
        }

        audioSources[clipIndex].Stop();
    }

    public void PauseSound()
    {
        for (int i = 0; i < audioSources.Length; i++)
        {

            audioSources[i].Pause();
        }
    }

    public void SetPitch(float pitch)
    {
        for (int i = 0; i < audioSources.Length; i++)
        {

            audioSources[i].pitch = pitch;
        }
    }



}
