using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundFx : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip footSteps;

    public AudioClip jumpSfx;
    public AudioClip dieSFX;
    public void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

    }

    // Update is called once per frame
    public void PlayFootSteps()
    {
        if (footSteps != null)
        {
            audioSource.PlayOneShot(footSteps);

        }
    }
    public void PlayJump()
    {
        if (jumpSfx != null)
        {
            audioSource.PlayOneShot(jumpSfx);

        }

    }
    public void DieSound()
    {
        if (dieSFX != null)
        {
            audioSource.PlayOneShot(dieSFX);
    }
   }
    
}
