using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QualityDropdown : MonoBehaviour
{
    public Dropdown dropdown;

    private string[] levels;

    void Start()
    {
        if (dropdown == null) dropdown = GetComponent<Dropdown>();
        levels = QualitySettings.names;
        dropdown.AddOptions(new List<string>(levels));
        dropdown.value = QualitySettings.GetQualityLevel();
        dropdown.onValueChanged.AddListener(delegate { ValueChanged(); });
    }

    private void ValueChanged()
    {
        QualitySettings.SetQualityLevel(dropdown.value);
    }

}
