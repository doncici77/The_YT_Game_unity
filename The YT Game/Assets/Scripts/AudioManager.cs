using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioClip backgroundMusic1;
    public AudioClip backgroundMusic2;
    public AudioClip backgroundMusic3;
    public AudioClip backgroundMusic4;
    public AudioClip choiceSound;

    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayBackgroundMusic(int trackNumber)
    {
        switch (trackNumber)
        {
            case 1:
                audioSource.clip = backgroundMusic1;
                break;
            case 2:
                audioSource.clip = backgroundMusic2;
                break;
            case 3:
                audioSource.clip = backgroundMusic3;
                break;
            case 4:
                audioSource.clip = backgroundMusic4;
                break;
        }
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
