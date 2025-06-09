using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button BotonJugar;
    [SerializeField] private Button BotonSalir;
    [SerializeField] private Button BotonOpciones; 

    void Start()
    {
        BotonJugar.onClick.AddListener(Jugar);
        BotonSalir.onClick.AddListener(Salir);
        BotonOpciones.onClick.AddListener(Opciones); 
    }

    private void Jugar()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.StartSeleccionMenu();
        }
    }

    private void Salir()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.QuitGame();
        }
    }

    private void Opciones()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.StartOptionsMenu();
        }
    }
}
