using UnityEngine;
using UnityEngine.UI;

public class FullscreenToggle : MonoBehaviour
{
    public Toggle toggle;

    void Start()
    {
        if (toggle == null) toggle = GetComponent<Toggle>();
        toggle.isOn = Screen.fullScreen;
        toggle.onValueChanged.AddListener(delegate { OnChange(); });
    }

    void OnChange()
    {
        if (toggle.isOn)
        {
            Screen.SetResolution(ScreenResolutions.MaxResolution.width, ScreenResolutions.MaxResolution.height, FullScreenMode.FullScreenWindow);
        }
        else
        {
            Screen.fullScreen = false;
        }
    }

}
