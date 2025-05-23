using UnityEngine;
using UnityEngine.UI;

public class AudioSelector : MonoBehaviour
{
    [SerializeField] AudioSource mp3;
    [SerializeField] AudioSource wav;

    [SerializeField] Button mp3_button;
    [SerializeField] Button wav_button;

    [SerializeField] Image bar;

    AudioSource current;

    public void PlayMp3()
    {
        mp3.Play();
        mp3_button.interactable = false;
        wav.Stop();
        wav_button.interactable = true;
        current = mp3;
    }

    public void PlayWav()
    {
        wav.Play();
        wav_button.interactable = false;
        mp3.Stop();
        mp3_button.interactable = true;
        current = wav;
    }

    private void Update()
    {
        if (current == null) return;

        bar.fillAmount = current.time / current.clip.length;
    }
}
