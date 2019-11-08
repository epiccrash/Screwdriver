using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager S;

    [SerializeField] AudioSource backgroundMusic;
    [SerializeField] AudioSource margaritaville;
    [SerializeField] AudioSource margaritavilleLoop;

    // Awake is called on the first frame update
    void Awake()
    {
        S = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayBackgroundMusic()
    {
        backgroundMusic.Play();
    }

    public void PlayMargaritaville()
    {
        margaritaville.Play();
    }

    public void PlayMargaritavilleLoop()
    {
        margaritavilleLoop.Play();
    }

    /*
     * Plays a sound.
     * AudioSource of the object calling this method required for the sound to be played.
     */
    public void PlaySound(AudioSource aSource)
    {
        if (!aSource.isPlaying)
        {
            aSource.Play();
        }
    }

    /*
     * Plays a sound.
     * AudioSource of the object calling this method required for the sound to be played.
     * Frequency must be a number between 0 and the upper sound boundary, inclusive.
     */
    public void PlaySound(AudioSource aSource, int frequency, int upperBound)
    {
        if (!aSource.isPlaying && frequency > Random.Range(0, upperBound))
        {
            aSource.Play();
        }
    }

    public void StopSound(AudioSource aSource)
    {
        if (aSource.isPlaying)
        {
            aSource.Stop();
        }
    }
}
