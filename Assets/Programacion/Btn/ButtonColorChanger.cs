using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonColorChanger : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    // public Color selectedColor = new Color(0.729f, 0.549f, 0.118f, 0.141f); 

    public Color selectedColor = new Color(1f, 1f, 1f, 0f); 
    public Color hoverColor = new Color(0.729f, 0.549f, 0.118f, 0.141f);
    public Color defaultColor = new Color(1f, 1f, 1f, 0f); 

    private Image image;
    private static ButtonColorChanger currentlySelected;

    void Awake()
    {
        image = GetComponent<Image>();
        ResetColor();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentlySelected != null)
        {
            currentlySelected.ResetColor();
        }

        SetSelected();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (this != currentlySelected)
        {
            image.color = hoverColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (this != currentlySelected)
        {
            ResetColor();
        }
    }

    private void SetSelected()
    {
        image.color = selectedColor;
        currentlySelected = this;
    }

    private void ResetColor()
    {
        image.color = defaultColor;
    }
}
