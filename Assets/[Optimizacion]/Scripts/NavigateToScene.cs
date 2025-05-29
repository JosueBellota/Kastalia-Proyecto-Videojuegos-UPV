using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigateToScene : MonoBehaviour
{
    [SerializeField] public string Scene;

    public void NavigateTo()
    {
        SceneManager.LoadScene(Scene);
    }
}
