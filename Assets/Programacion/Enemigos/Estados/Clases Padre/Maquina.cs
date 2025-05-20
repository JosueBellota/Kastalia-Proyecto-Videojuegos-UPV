using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(-100)]
public abstract class Maquina : MonoBehaviour
{
    public UnityEvent<string> OnStateChanged;

    Estado[] estados;
    Estado _estado;

    [SerializeField] private GameObject agroPopupPrefab;

    public Estado Estado
    {
        get => _estado;
        set
        {
            if (_estado == value) return;

            if (_estado != null)
                _estado.InvocarOnExit(); // Llamamos a InvocarOnExit en el estado anterior

            _estado = value;

            foreach (Estado estado in estados)
                estado.enabled = (estado == _estado);

            if (_estado != null)
                _estado.InvocarOnEnter(); // Llamamos a InvocarOnEnter en el nuevo estado

            OnStateChanged?.Invoke(_estado.Nombre);
        }
    }

    void Awake()
    {
        estados = GetComponents<Estado>();
        foreach (Estado estado in estados)
        {
            if (estado.Inicial)
            {
                Estado = estado;
          
            }
        }

        if (Estado == null && estados.Length > 0)
        {
            Estado = estados[0];
        }

        OnAwake();
    }

    protected virtual void OnAwake() { }

    void Update()
    {
        if (_estado != null)
        {
            _estado.InvocarOnUpdate(); // Llamamos a InvocarOnUpdate del estado actual
        }
    }

    public void SetEstado(string nombre)
    {
        foreach (Estado estado in estados)
        {
            if (estado.Nombre == nombre)
            {
                Estado = estado;
                return;
            }
        }

        Debug.LogError($"No hay un estado {nombre}");
    }

    public void DisplayAgroPopup(){
        if(agroPopupPrefab) Instantiate(agroPopupPrefab, transform.position, Quaternion.identity, transform);
    }
}

public enum EnemyType {
    CaballeroNormal,
    CaballeroReal,
    Ballestero,
    Bombardero,
    Kaspar
}
