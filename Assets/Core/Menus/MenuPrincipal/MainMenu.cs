using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{
    [SerializeField] Button BotonJugar;
    [SerializeField] Button BotonSalir;
    void Start()
    {
        BotonJugar.onClick.AddListener(() => Jugar());
        BotonSalir.onClick.AddListener(() => Salir());
    }

    private void Jugar(){
        GameManager.instance.StartMainGameLoop();
    }

    private void Salir()
    {
        Application.Quit();
    }
}
