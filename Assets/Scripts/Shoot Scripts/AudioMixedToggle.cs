using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioMixedToggle : MonoBehaviour
{
    public AudioMixer mixer;
    public string parameter;
    public GameObject disableImage;

    private float unmutedAudioLevel;
    private bool isMuted = false;

    public void ToggleAudio()
    {
        if(!isMuted)
        {
            mixer.GetFloat(parameter, out unmutedAudioLevel);
            mixer.SetFloat(parameter, -80);
            disableImage.SetActive(true);
            isMuted = true;
        }
        else
        {
            mixer.SetFloat(parameter, unmutedAudioLevel);
            disableImage.SetActive(false);
            isMuted = false;
        }
    }
}
