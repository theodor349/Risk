using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance;
    private AudioSource audioSource;
    private float volume;

    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
        volume = audioSource.volume;
    }

    public void PlayNext()
    {
        audioSource.pitch = Random.Range(0.5f, 1.5f);
        audioSource.volume = volume + Random.Range(-0.1f, 0.1f);
        audioSource.Play();
    }
}
