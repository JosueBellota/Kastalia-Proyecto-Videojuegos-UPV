using UnityEngine;

public class Cronometro : MonoBehaviour
{
    public static Cronometro instance;

    private float tiempo = 0f;
    private bool contando = true;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (contando)
        {
            tiempo += Time.deltaTime;
            
        }
    }

    public void Detener()
    {
        contando = false;

        string tiempoFinal = ObtenerTiempoFormateado();
        

        PlayerPrefs.SetString("DuracionPartida", tiempoFinal);
        PlayerPrefs.Save();
        

        MostrarDuracion textoDuracion = FindAnyObjectByType<MostrarDuracion>();
        if (textoDuracion != null)
        {
            textoDuracion.ActualizarTexto();
            
        }
    }

    public void ReiniciarCronometro()
    {
        tiempo = 0f;
        contando = true;
        
    }

    public string ObtenerTiempoFormateado()
    {
        int segundosTotales = Mathf.RoundToInt(tiempo);
        int minutos = segundosTotales / 60;
        int segundos = segundosTotales % 60;
        return string.Format("{0:00}:{1:00}", minutos, segundos);
    }

    public float ObtenerTiempoRaw()
    {
        return tiempo;
    }
}