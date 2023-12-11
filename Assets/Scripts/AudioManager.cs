using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("----- Audio Source -----")]
    [SerializeField] AudioSource ambientSource;
    [SerializeField] AudioSource SFXSource;
    [SerializeField] AudioSource jumpscareSource;

    [Header("----- Audio Clip -----")]
    public AudioClip ambience;
    public AudioClip walkOutside;
    public AudioClip walkInside;
    public AudioClip bearTrap;
    public AudioClip monsterBite;
    public AudioClip creakScare;
    public AudioClip breathScare;
    public AudioClip whisperScare;
    public AudioClip classicScare;
    public AudioClip ratScare;
    public AudioClip crowScare;

    private void Start()
    {
        ambientSource.clip = ambience;
        ambientSource.Play();
    }

    public void PlayJumpscare(AudioClip clip)
    {
        jumpscareSource.PlayOneShot(clip);
    }
    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
