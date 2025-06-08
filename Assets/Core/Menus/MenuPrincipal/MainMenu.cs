using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button BotonJugar;
    [SerializeField] private Button BotonSalir;

    void Start()
    {
        BotonJugar.onClick.AddListener(Jugar);
        BotonSalir.onClick.AddListener(Salir);
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
}
