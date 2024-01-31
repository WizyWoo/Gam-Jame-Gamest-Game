using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioSource audioSource;
    public AudioClip[] audioClips;

    // Start is called before the first frame update


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.LogError("Multiple instances of SoundManager detected. Destroying the object.");
            Destroy(gameObject);
        }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlaySound(int clipIndex)
    {
        if (clipIndex < 0 || clipIndex >= audioClips.Length)
        {
            Debug.LogError("Clip index out of range");
            return;
        }

        audioSource.PlayOneShot(audioClips[clipIndex]);
    }
    public void StopSound()
    {
        audioSource.Stop();
    }

    public void PauseSound()
    {
        audioSource.Pause();
    }

    public void SetPitch(float pitch)
    {
        audioSource.pitch = pitch;
    }

}
