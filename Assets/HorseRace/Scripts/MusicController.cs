using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{

    [SerializeField] AudioClip GameMusic;
    [SerializeField] AudioClip GameOverMusic;
    AudioSource startMusic;
    AudioSource endMusic;

    void Start()
    {
        startMusic = this.gameObject.AddComponent<AudioSource>();
        startMusic.clip = GameMusic;
        startMusic.playOnAwake = false;
        startMusic.bypassEffects = true;
        startMusic.bypassListenerEffects = true;
        startMusic.spatialBlend = 0.0f;
        startMusic.loop = true;

        endMusic = this.gameObject.AddComponent<AudioSource>();
        endMusic.clip = GameOverMusic;
        endMusic.playOnAwake = false;
        endMusic.bypassEffects = true;
        endMusic.bypassListenerEffects = true;
        endMusic.spatialBlend = 0.0f;

    }

    public void GameStartMusic()
    {
        startMusic.volume = 1.0f;
        startMusic.loop = true;
        startMusic.Play();
    }

    public void GameEndMusic()
    {
        startMusic.Stop();
        endMusic.Play();
    }

}
