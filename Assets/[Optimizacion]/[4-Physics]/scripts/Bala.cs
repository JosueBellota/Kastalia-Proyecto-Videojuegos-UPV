using System.Collections;
using UnityEngine;

namespace Optimicacion
{
    public class Bala : MonoBehaviour
    {

        public float lifeTime = 3f;

        void Start()
        {
            StartCoroutine(Life());
        }

        IEnumerator Life()
        {
            yield return new WaitForSeconds(lifeTime);
            Destroy(gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            Destroy(gameObject);
        }
    }
}
