using UnityEngine;
using UnityEngine.Audio;

namespace Dapasa.Audio
{
    public class AudioVolumeManager : MonoBehaviour
    {

        [Tooltip("Seleccionar para que los valores se mantengan entre sesiones de juego")]
        [SerializeField] bool _saveToPrefs = false;

        [SerializeField] AudioMixer _audioMixer;

        [SerializeField] string _musicParam;

        [SerializeField] string _sfxParam;

        static AudioVolumeManager _instance;

        static public AudioVolumeManager Instance { get => _instance; }

        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                DestroyImmediate(gameObject);
            }
        }

        void Start()
        {
            if (_saveToPrefs)
            {
                MusicVolume = PlayerPrefs.GetFloat(_musicParam, 0f);
                SfxVolume = PlayerPrefs.GetFloat(_sfxParam, 0f);
                Debug.Log($"Música: {MusicVolume} dB");
                Debug.Log($"Sfx: {SfxVolume} dB");
            }
        }

        public float MusicVolume
        {
            get
            {
                if (_audioMixer.GetFloat(_musicParam, out float value)) return value;
                return 0f;
            }
            set
            {
                _audioMixer.SetFloat(_musicParam, value);
                if (_saveToPrefs) PlayerPrefs.SetFloat(_musicParam, value);
            }
        }

        public float SfxVolume
        {
            get
            {
                if (_audioMixer.GetFloat(_sfxParam, out float value))
                {
                    Debug.Log($"get SfxVolume -> Sfx: {value} dB");
                    return value;
                }
                return 0f;
            }
            set
            {
                _audioMixer.SetFloat(_sfxParam, value);
                if (_saveToPrefs) PlayerPrefs.SetFloat(_sfxParam, value);
            }
        }
    }

    public enum AudioGroups
    {
        Musica,
        Sfx
    }
}