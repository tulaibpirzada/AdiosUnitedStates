using UnityEngine;
using System.Collections;

namespace Com.KhuongDuy.MachoDash
{
    public class BackGroundMove : MonoBehaviour
    {
        public MeshRenderer renderer;

        public float speedMove = 1.0f;

        private float position = 0.0f;

        // Constructor
        protected BackGroundMove() { }

        // Behaviour messages
        void Update()
        {
            position += speedMove;
            if (position > 1.0f)
            {
                position -= 1.0f;
            }

            renderer.material.mainTextureOffset = new Vector2(position, 0.0f);
        }
    }
}
