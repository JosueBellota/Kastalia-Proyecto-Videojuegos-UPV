using UnityEngine;
using TMPro;

public class MostrarDuracion : MonoBehaviour
{
    private TextMeshProUGUI texto;

    void Awake()
    {
        texto = GetComponent<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        ActualizarTexto();
    }

    public void ActualizarTexto()
    {
        if (PlayerPrefs.HasKey("DuracionPartida"))
        {
            string valor = PlayerPrefs.GetString("DuracionPartida");
            texto.text = "Duración: " + valor;
        }
        else
        {
            texto.text = "Duración: --:--";
        }
    }
}
