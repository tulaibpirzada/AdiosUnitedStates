using UnityEngine;
using System.Collections;

namespace Com.KhuongDuy.MachoDash
{
    /// <summary>
    /// Handle bullet is fired
    /// </summary>
    /// 
    public class Bullet : MonoBehaviour
    {
        private GameObject[] bullets;   // this variable only use for 'Hiding bullet'

        private Rigidbody2D m_rg2D;

        public float bulletSpeed = 450.0f;

        // Constructor
        private Bullet() { }

        // Behaviour messages
        void Awake()
        {
            m_rg2D = GetComponent<Rigidbody2D>();
        }

        // Behaviour messages
        void Start()
        {
            SpriteRenderer[] bulletChilds = transform.parent.gameObject.GetComponentsInChildren<SpriteRenderer>();
            bullets = new GameObject[bulletChilds.Length];

            for (int i = bulletChilds.Length - 1; i >= 0; i--)
            {
                bullets[i] = bulletChilds[i].gameObject;
            }
        }

        // Behaviour messages
        void OnEnable()
        {
            if (this.name == "Hiding Bullet")
            {
                if (bullets != null)
                {
                    for (int i = bullets.Length - 1; i >= 0; i--)
                    {
                        bullets[i].SetActive(true);
                    }
                }
            }

            transform.localPosition = Vector3.zero;

            m_rg2D.AddRelativeForce(transform.right * bulletSpeed);
        }

        // Behaviour messages
        void Update()
        {
            if (transform.position.x >= 7.6f)
            {
                if (transform.parent.parent.name != "Gun 2" && transform.parent.parent.name != "Gun 3")
                {
                    this.gameObject.SetActive(false);
                }
                else
                {
                    transform.parent.gameObject.SetActive(false);
                }
            }
        }
    }
}
