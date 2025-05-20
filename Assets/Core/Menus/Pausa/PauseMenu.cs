using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] Button resume;
    [SerializeField] Button quit;
    [SerializeField] Button backToMenu;
    [SerializeField] Button retryButton;

    void Start()
    {
        resume.onClick.AddListener(() => GameManager.instance.ResumeGame());
        quit.onClick.AddListener(() => CloseGame());
        backToMenu.onClick.AddListener(() => BackToMenu());
        retryButton.onClick.AddListener(() => RetryGame());
    }

    private void BackToMenu()
    {
        Time.timeScale = 1f;
        GameManager.instance.StartMainMenu();
    }

    private void RetryGame()
    {
        Time.timeScale = 1f; // Reanuda el tiempo
        GameManager.instance.ResumeGame(); // Cierra el menú de pausa
        GameManager.instance.ReiniciarPartidaConPersonajeAnterior(); // ✅ nuevo método limpio
    }

    private void CloseGame()
    {
        Application.Quit();

        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
