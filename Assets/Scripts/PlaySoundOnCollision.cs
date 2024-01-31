using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnCollision : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip audioClip;
    public bool playOnce = false;
    public bool playOnCollision = true;
    public bool playOnTrigger = false;
    public bool playOnStay = false;
    public bool playOnExit = false;

    private bool hasPlayed = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (playOnCollision && !hasPlayed && collision.gameObject.CompareTag("Player"))
        {
            audioSource.PlayOneShot(audioClip);
            hasPlayed = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playOnTrigger && !hasPlayed && other.CompareTag("Player"))
        {
            audioSource.PlayOneShot(audioClip);
            hasPlayed = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (playOnStay && !hasPlayed && other.CompareTag("Player"))
        {
            audioSource.PlayOneShot(audioClip);
            hasPlayed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (playOnExit && !hasPlayed && other.CompareTag("Player"))
        {
            audioSource.PlayOneShot(audioClip);
            hasPlayed = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (playOnStay && !hasPlayed && collision.gameObject.CompareTag("Player"))
        {
            audioSource.PlayOneShot(audioClip);
            hasPlayed = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (playOnExit && !hasPlayed && collision.gameObject.CompareTag("Player"))
        {
            audioSource.PlayOneShot(audioClip);
            hasPlayed = true;
        }
    }

    private void Update()
    {
        if (playOnce && hasPlayed)
        {
            playOnCollision = false;
            playOnTrigger = false;
            playOnStay = false;
            playOnExit = false;
        }
    }
}
