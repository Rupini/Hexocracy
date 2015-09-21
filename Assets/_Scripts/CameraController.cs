using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy
{
    public class CameraController : MonoBehaviour
    {
        private Transform cameraT;
        public float speed;
        public float boundary;

        public bool keyboardControl;

        private void Awake()
        {
            cameraT = Camera.main.transform;
        }

        private void Update()
        {
            float dx = 0;
            float dz = 0;
            float dt = Time.deltaTime;

            if (keyboardControl)
            {
                dx = -speed * Input.GetAxis("Horizontal") * dt;
                dz = -speed * Input.GetAxis("Vertical") * dt;
            }
            else
            {
                var mouse = Input.mousePosition;
                if (mouse.x < boundary)
                    dx = speed * dt;
                else if (mouse.x > Screen.width - boundary)
                    dx = -speed * dt;

                if (mouse.y < boundary)
                    dz = speed * dt;
                else if (mouse.y > Screen.height - boundary)
                    dz = -speed * dt;
            }

            cameraT.position += new Vector3(dx, 0, dz);
        }
    }
}
