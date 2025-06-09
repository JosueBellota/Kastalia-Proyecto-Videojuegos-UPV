using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControlsMenu : MonoBehaviour
{
    [SerializeField] private Button BotonVolver;
    void Start()
    {
        BotonVolver.onClick.AddListener(VolverAlMenuOpciones);

    }

    private void VolverAlMenuOpciones()
    {
        GameManager.instance.StartOptionsMenu();
    }

}
