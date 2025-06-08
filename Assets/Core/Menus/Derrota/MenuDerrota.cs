using UnityEngine;
using UnityEngine.UI;

public class MenuDerrota : MonoBehaviour
{
    [SerializeField] private Button reintentarButton;
    [SerializeField] private Button irAlMenuPrincipalButton;
    [SerializeField] private Button salirDelJuegoButton;

    void Start()
    {
        reintentarButton.onClick.AddListener(Reintentar);
        irAlMenuPrincipalButton.onClick.AddListener(IrAlMenuPrincipal);
        salirDelJuegoButton.onClick.AddListener(SalirDelJuego);
    }

    private void Reintentar()
    {
        Time.timeScale = 1f;
        GameManager.instance.StartSeleccionMenu();
    }

    private void IrAlMenuPrincipal()
    {
        Time.timeScale = 1f;
        GameManager.instance.StartMainMenu();
    }

    private void SalirDelJuego()
    {
        GameManager.instance.QuitGame();
    }
}
