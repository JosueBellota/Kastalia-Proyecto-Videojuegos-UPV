using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class TutorialManager : MonoBehaviour
{
    [SerializeField] Button resume;
    // [SerializeField] Button quit;
    // [SerializeField] Button backToMenu;
   

    void Start()
    {
       
        resume.onClick.AddListener(() => Continue());
        // quit.onClick.AddListener(() => CloseGame());
        // backToMenu.onClick.AddListener(() => BackToMenu());
    }

    private void Continue()
    {
        GameManager.instance.SkipTutorial("Tutorial1");

    }

    // private void CloseGame(){
    //     Application.Quit();

    //     #if UNITY_EDITOR
    //     UnityEditor.EditorApplication.isPlaying = false;
    //     #endif
    // }
}
