using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSoundPlay : MonoBehaviour
{
    public AudioSource AudioSource;

    public void PlayAudioClip(AudioClip audioClip)
    {
        AudioSource.PlayOneShot(audioClip);
    }
}
