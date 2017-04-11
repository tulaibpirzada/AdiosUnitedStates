using UnityEngine;
using System.Collections;

namespace Com.KhuongDuy.MachoDash
{
    public class Move : MonoBehaviour
    {
        public float speed;

        // Constructor
        protected Move() { }

        // Behaviour messages
        void OnEnable()
        {
            transform.localPosition = new Vector3(6.5f, Random.Range(-0.75f, 1.75f), 0.0f);
        }

        // Behaviour messages
        protected virtual void Update()
        {
            transform.position -= new Vector3(Time.deltaTime * speed, 0.0f, 0.0f);

            if (transform.localPosition.x <= -6.5f)
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}
