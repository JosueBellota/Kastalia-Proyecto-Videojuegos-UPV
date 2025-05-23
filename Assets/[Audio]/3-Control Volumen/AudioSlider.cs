using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Dapasa.Audio
{
    [RequireComponent(typeof(Slider))]
    public class AudioSlider : MonoBehaviour
    {

        [SerializeField] AudioGroups group;

        [Header("Test")]
        [SerializeField] AudioClip _testClip;

        [SerializeField] AudioSource _testSource;

        Slider _slider;

        IEnumerator Start()
        {
            _slider = GetComponent<Slider>();
            yield return null;
            float value = 0f;
            switch (group)
            {
                case AudioGroups.Musica:
                    value = DbToLineal(AudioVolumeManager.Instance.MusicVolume);
                    break;
                case AudioGroups.Sfx:
                    value = DbToLineal(AudioVolumeManager.Instance.SfxVolume);
                    break;
                default:
                    value = 1f;
                    break;
            }
            _slider.value = value;
            _slider.onValueChanged.AddListener(ChangeValue);
        }

        void ChangeValue(float value)
        {
            float vol = value == 0 ? -80f : LinealToDb(value);
            switch (group)
            {
                case AudioGroups.Musica:
                    AudioVolumeManager.Instance.MusicVolume = vol;
                    break;
                case AudioGroups.Sfx:
                    AudioVolumeManager.Instance.SfxVolume = vol;
                    break;
            }
        }

        public void TestVolume()
        {
            if (_testClip != null && _testSource != null)
            {
                _testSource.PlayOneShot(_testClip);
            }
        }

        float DbToLineal(float dB)
        {
            return Mathf.Pow(10f, dB / 20f);
        }

        float LinealToDb(float lineal)
        {
            return Mathf.Log10(lineal) * 20f;
        }
    }
}