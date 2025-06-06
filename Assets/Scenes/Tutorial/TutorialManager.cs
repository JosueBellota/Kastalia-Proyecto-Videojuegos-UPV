using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;


public class TutorialManager : MonoBehaviour
{

    [SerializeField] private string thisSceneName;
    [SerializeField] private Button resume;
    [SerializeField] private Button nextBtn;

    [SerializeField] private List<string> tutorialScenes = new List<string> { "Tutorial1", "Tutorial2"};

    private int currentIndex;
    void Start()
    {
        resume.onClick.AddListener(skip);
        nextBtn.onClick.AddListener(next);
        currentIndex = tutorialScenes.IndexOf(thisSceneName);

         // Optional: Warn if the scene name isn't found
        if (currentIndex == -1)
        {
            Debug.LogWarning($"'{thisSceneName}' not found in tutorialScenes list.");
        }
    }

    private void skip()
    {
        // Unload all tutorial scenes
        foreach (string scene in tutorialScenes)
        {
            if (SceneManager.GetSceneByName(scene).isLoaded)
            {
                GameManager.instance.SkipTutorial(scene);
            }
        }

        // Optionally resume game
        // GameManager.instance.ResumeGame();
    }

    private void next()
    {
        if (currentIndex >= tutorialScenes.Count)
        {
            skip();
            return;
        }

        string currentScene = tutorialScenes[currentIndex];

        // Unload current tutorial scene
        if (SceneManager.GetSceneByName(currentScene).isLoaded)
        {
            GameManager.instance.SkipTutorial(currentScene);
        }

        currentIndex++;

        if (currentIndex < tutorialScenes.Count)
        {
            string nextScene = tutorialScenes[currentIndex];
            GameManager.instance.LoadTutorialScene(nextScene, true);
        }
        else
        {
            skip();
        }
    }
}
