using UnityEngine;

public abstract class Estado : MonoBehaviour
{
    [SerializeField] NombreEstado nombre;

    [SerializeField]
    bool inicial = false;

    protected Maquina maquina;

    public string Nombre
    {
        get
        {
            if (!nombre) return "";
            return nombre.Value;
        }
    }

    public bool Inicial { get => inicial; set => inicial = value; }

    void Awake()
    {
        maquina = GetComponent<Maquina>();
        OnAwake();
    }

    protected virtual void OnAwake()
    {
        if (maquina == null)
        {
            Debug.LogError("Maquina no asignada en el estado " + this.name);
            return;
        }
    }


    // Métodos públicos para invocar OnEnter, OnUpdate, OnExit
    public void InvocarOnEnter()
    {
        OnEnter();
    }

    public void InvocarOnUpdate()
    {
        OnUpdate();
    }

    public void InvocarOnExit()
    {
        OnExit();
    }
    protected virtual void OnEnter() { }
    protected virtual void OnUpdate() { }
    protected virtual void OnExit() { }




}
