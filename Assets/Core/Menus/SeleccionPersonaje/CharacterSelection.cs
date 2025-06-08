using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    public Button Lyx;
    public Button Dreven;
    public Button Confirm;

    private int selectedCharacter = -1;

    void Start()
    {
        Lyx.onClick.AddListener(() => selectCharacter(1));
        Dreven.onClick.AddListener(() => selectCharacter(2));
        Confirm.onClick.AddListener(confirmSelection);
    }

    private void selectCharacter(int value)
    {
        selectedCharacter = value;
    }

    void confirmSelection()
    {
        if (selectedCharacter != -1)
        {
            GameManager.instance.characterIndex = selectedCharacter;

            GameManager.instance.personajeSeleccionado = selectedCharacter == 1
                ? GameManager.instance.Lyx
                : GameManager.instance.Dreven;

            GameManager.instance.playerSpawned = false;

             // 🆕 NUEVO: Instanciar arma para el personaje seleccionado
            GameManager.instance.InstanciarArmaParaPersonaje();

            if (Cronometro.instance != null)
            {
                Cronometro.instance.ReiniciarCronometro();
            }

            if (SceneManager.GetSceneByName("CharacterSelection").isLoaded)
            {
                SceneManager.UnloadSceneAsync("CharacterSelection");
            }
            GameManager.instance.isPaused = false;
        }
    }
}