using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioSliderController : MonoBehaviour
{
    [Header("Mixer principal")]
    public AudioMixer mainMixer;

    [Header("Sliders UI")]
    public Slider musicaSlider;
    public Slider efectosSlider;

    private void Start()
    {
        // Cargar valores guardados o usar por defecto
        float musicaVol = PlayerPrefs.GetFloat("MusicaVolume", 0.75f);
        float sfxVol = PlayerPrefs.GetFloat("SFXVolume", 0.75f);

        musicaSlider.value = musicaVol;
        efectosSlider.value = sfxVol;

        musicaSlider.onValueChanged.AddListener(SetMusicaVolume);
        efectosSlider.onValueChanged.AddListener(SetSFXVolume);

        SetMusicaVolume(musicaVol);
        SetSFXVolume(sfxVol);
    }

    public void SetMusicaVolume(float value)
    {
        float dB = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        mainMixer.SetFloat("Vol_Musica", dB); // NOMBRE CORREGIDO
        PlayerPrefs.SetFloat("MusicaVolume", value);
    }

    public void SetSFXVolume(float value)
    {
        float dB = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        mainMixer.SetFloat("Vol_Sfx", dB); // NOMBRE CORREGIDO
        PlayerPrefs.SetFloat("SFXVolume", value);
    }
}
