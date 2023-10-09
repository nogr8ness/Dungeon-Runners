using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;

    public void SetVolume(float vol)
    {
        mixer.SetFloat("volume", vol);
    }
}
