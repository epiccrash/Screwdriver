using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager S;

    // Music
    [SerializeField] AudioSource backgroundMusic;
    [SerializeField] AudioSource margaritaville;
    [SerializeField] AudioSource margaritavilleLoop;
    [SerializeField] AudioSource mashupMusic;
    [SerializeField] AudioSource shotsMusic;
    // DJ sounds
    [SerializeField] AudioSource barIsCooking;
    [SerializeField] AudioSource tryingToGetLow;
    [SerializeField] AudioSource timeForShots;
    [SerializeField] AudioSource lastCall;
    [SerializeField] AudioSource getPartyStarted;
    [SerializeField] AudioSource uhOh;

    private AudioSource currentMusic;

    // Awake is called on the first frame update
    void Awake()
    {
        S = this;
    }

    // Update is called once per frame
    void Update()
    {
        SetCurrentMusic();
    }

    public void PlayBackgroundMusic()
    {
        PlayMusic(backgroundMusic);
    }

    public void PlayMargaritaville()
    {
        PlayMusic(margaritaville);
    }

    public void PlayMargaritavilleLoop()
    {
        PlayMusic(margaritavilleLoop);
    }

    public void PlayMashup()
    {
        PlayMusic(mashupMusic);
    }

    public void PlayShots()
    {
        PlayMusic(shotsMusic);
    }

    // DJ Sounds
    public void PlayBarIsCooking()
    {
        PlaySound(barIsCooking);
    }
    public void PlayTryingToGetLow()
    {
        PlaySound(tryingToGetLow);
    }
    public void PlayTimeForShots()
    {
        PauseCurrentMusic();
        PlaySound(timeForShots);
    }
    public void PlayLastCall()
    {
        PlaySound(lastCall);
    }
    public void PlayGetPartyStarted()
    {
        PauseCurrentMusic();
        PlaySound(getPartyStarted);
    }
    public void PlayUhOh()
    {
        PlaySound(uhOh);
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

    private void PlayMusic(AudioSource aSource)
    {
        PauseCurrentMusic();
        PlaySound(aSource);
    }

    public bool DJIsSpeaking()
    {
        return barIsCooking.isPlaying || tryingToGetLow.isPlaying || timeForShots.isPlaying || lastCall.isPlaying || getPartyStarted.isPlaying || uhOh.isPlaying;
    }

    private void SetCurrentMusic()
    {
        if (backgroundMusic.isPlaying)
        {
            currentMusic = backgroundMusic;
        }
        else if (margaritaville.isPlaying)
        {
            currentMusic = margaritaville;
        }
        else if (margaritavilleLoop.isPlaying)
        {
            currentMusic = margaritavilleLoop;
        }
        else if (mashupMusic.isPlaying)
        {
            currentMusic = mashupMusic;
        }
        else if (shotsMusic.isPlaying)
        {
            currentMusic = shotsMusic;
        }
        else
        {
            currentMusic = null;
        }
    }

    private void PauseCurrentMusic()
    {
        if (currentMusic != null && currentMusic.isPlaying)
        {
            currentMusic.Pause();
        }
    }

    private void StopCurrentMusic()
    {
        if (currentMusic != null && currentMusic.isPlaying)
        {
            currentMusic.Stop();
        }
    }
}
