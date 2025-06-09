using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private Button botonControles;
    [SerializeField] private Button botonPantalla;
    [SerializeField] private Button botonVolver;

    [SerializeField] private Dropdown resolucionDropdown;

    private void Start()
    {
        // PersonalizarDropdown();
        botonControles.onClick.AddListener(IrAControles);
        botonPantalla.onClick.AddListener(MostrarResoluciones);
        botonVolver.onClick.AddListener(VolverAlMenuPrincipal);

        LlenarResoluciones();
        resolucionDropdown.gameObject.SetActive(false);
    }

    private void IrAControles()
    {
        GameManager.instance.StartControlsMenu();
    }

    private void MostrarResoluciones()
    {
        resolucionDropdown.gameObject.SetActive(!resolucionDropdown.gameObject.activeSelf);
    }

    private void VolverAlMenuPrincipal()
    {
        GameManager.instance.StartMainMenu();  
    }

    private void LlenarResoluciones()
    {
        resolucionDropdown.ClearOptions();
        Resolution[] resoluciones = Screen.resolutions;
        int resolucionActual = 0;

        var opciones = new System.Collections.Generic.List<string>();

        for (int i = 0; i < resoluciones.Length; i++)
        {
            string opcion = resoluciones[i].width + " x " + resoluciones[i].height;
            opciones.Add(opcion);

            if (resoluciones[i].width == Screen.currentResolution.width &&
                resoluciones[i].height == Screen.currentResolution.height)
            {
                resolucionActual = i;
            }
        }

        resolucionDropdown.AddOptions(opciones);
        resolucionDropdown.value = resolucionActual;
        resolucionDropdown.RefreshShownValue();

        resolucionDropdown.onValueChanged.AddListener(CambiarResolucion);
    }

    private void CambiarResolucion(int indice)
    {
        Resolution resolucion = Screen.resolutions[indice];
        Screen.SetResolution(resolucion.width, resolucion.height, Screen.fullScreen);
    }


    // void PersonalizarDropdown()
    // {
    //     if (resolucionDropdown == null)
    //     {
    //         Debug.LogError("resolucionDropdown no está asignado.");
    //         return;
    //     }

    //     // Cambiar el fondo del Dropdown principal
    //     Image background = resolucionDropdown.GetComponent<Image>();
    //     if (background != null)
    //     {
    //         background.color = Color.red;
    //     }

    //     // Cambiar el texto del label
    //     Text label = resolucionDropdown.transform.Find("Label")?.GetComponent<Text>();
    //     if (label != null)
    //     {
    //         label.fontSize = 24;
    //         label.color = new Color(0, 0, 0, 0.5f);
    //     }

    //     // 🔴 Activar temporalmente el template para acceder a sus hijos
    //     Transform template = resolucionDropdown.transform.Find("Template");
    //     if (template != null)
    //     {
    //         bool wasActive = template.gameObject.activeSelf;
    //         template.gameObject.SetActive(true);

    //         // Modificar item label
    //         Transform itemLabelTransform = template.Find("Viewport/Content/Item/Item Label");
    //         if (itemLabelTransform != null)
    //         {
    //             Text itemLabel = itemLabelTransform.GetComponent<Text>();
    //             if (itemLabel != null)
    //             {
    //                 itemLabel.fontSize = 24;
    //                 itemLabel.color = new Color(0, 0, 0, 0.5f);
    //             }
    //         }

    //         // Modificar fondo del item
    //         Transform itemTransform = template.Find("Viewport/Content/Item");
    //         if (itemTransform != null)
    //         {
    //             Image itemBg = itemTransform.GetComponent<Image>();
    //             if (itemBg != null)
    //             {
    //                 itemBg.color = Color.black;
    //             }
    //         }

    //         // Restaurar estado anterior del template
    //         template.gameObject.SetActive(wasActive);
    //     }
    // }



}
