using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    [SerializeField] Button resume;
    [SerializeField] Button quit;
    [SerializeField] Button backToMenu;
   

    void Start()
    {
       
        resume.onClick.AddListener(() => GameManager.instance.ResumeGame());
        quit.onClick.AddListener(() => CloseGame());
        backToMenu.onClick.AddListener(() => BackToMenu());
    }

    private void BackToMenu()
    {


        Time.timeScale = 1f;
        GameManager.instance.StartMainMenu();
    }

    private void CloseGame(){
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
    
}

