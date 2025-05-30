using UnityEngine;

namespace Dapasa
{

    /// <summary>
    /// Devuelve un objeto a su posición original si cae por debajo de un límite.
    /// </summary>
    public class RestartPosition : MonoBehaviour
    {
        public float limitY = 0f;

        private Vector3 startPos;

        // Use this for initialization
        void Start()
        {
            SetStartPos();
        }

        // Update is called once per frame
        void LateUpdate()
        {
            if (transform.position.y < limitY)
            {
                transform.position = startPos;
                //Debug.Log("Restart to " + transform.position);
            }
        }

        public void SetStartPos()
        {
            startPos = transform.position;
        }
    }
}
