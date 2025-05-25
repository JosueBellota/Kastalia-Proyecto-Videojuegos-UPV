using UnityEngine;
using UnityEngine.SceneManagement;

public class PasarEscenaScript : MonoBehaviour
{
    public void CambiarEscena(string nombreEscena)
    {
        SceneManager.LoadScene(nombreEscena);
    }
}
