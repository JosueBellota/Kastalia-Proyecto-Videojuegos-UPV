using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionDropdown : MonoBehaviour
{
    public Dropdown dropdown;

    private WidthHeight[] resolutions;

    private List<string> labels = new List<string>();

    void Start()
    {
        if (dropdown == null) dropdown = GetComponent<Dropdown>();
        HashSet<WidthHeight> set = new HashSet<WidthHeight>();
        for (int i = 0; i < ScreenResolutions.Resolutions.Length; i++)
        {
            set.Add(new WidthHeight(ScreenResolutions.Resolutions[i].width, ScreenResolutions.Resolutions[i].height));
        }
        resolutions = new WidthHeight[set.Count];
        set.CopyTo(resolutions);
        int v = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            //labels.Add($"{resolutions[i].width}x{resolutions[i].height}@{resolutions[i].refreshRateRatio}");
            labels.Add(resolutions[i].ToString());
            if (resolutions[i].Compare(Screen.currentResolution))
            {
                v = i;
            }
        }
        dropdown.AddOptions(labels);
        dropdown.value = v;
        dropdown.onValueChanged.AddListener(delegate { ValueChanged(); });
    }

    private void ValueChanged()
    {
        WidthHeight r = resolutions[dropdown.value];
        Screen.SetResolution(r.width, r.height, Screen.fullScreen);
    }

}

struct WidthHeight
{
    public int width;
    public int height;
    public WidthHeight(int width, int height)
    {
        this.width = width;
        this.height = height;
    }

    public override string ToString()
    {
        return $"{width} - {height}";
    }

    public bool Compare(Resolution resolution)
    {
        return width == resolution.width && height == resolution.height;
    }
}

public static class ScreenResolutions
{
    static private Resolution[] resolutions;

    public static Resolution[] Resolutions
    {
        get
        {
            if (resolutions == null)
            {
                resolutions = Screen.resolutions;
                Array.Reverse(resolutions);
            }
            return resolutions;
        }
    }

    public static Resolution MaxResolution
    {
        get
        {
            return Resolutions[0];
        }
    }
}