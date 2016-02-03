using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy
{
    public class InputController : MonoBehaviour
    {
        private static InputController _i;
        public static InputController I
        {
            get
            {
                if (!_i)
                {
                    _i = new GameObject("InputController").AddComponent<InputController>();
                }

                return _i;
            }
        }

        private Camera currCamera;
        private Figure selectedFigure;
        private Player activePlayer;

        private InputController()
        {
            currCamera = Camera.main;
        }

        private void Update()
        {
            if (activePlayer == null) return;

            if (Input.GetKeyDown(KeyCode.Z))
            {
                TurnController.OnPlayerFinishedTurn();
            }

            if (Input.GetMouseButtonDown(0))
            {
                var ray = currCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 1000, 1 << 9))
                {
                    selectedFigure = hit.collider.GetComponent<Figure>();
                    StatsPanel.SetTarget((Figure)selectedFigure);
                }
            }

            //if (Input.GetMouseButtonDown(1))
            //{
            //    var ray = currCamera.ScreenPointToRay(Input.mousePosition);
            //    RaycastHit hit;
            //    if (Physics.Raycast(ray, out hit, 1000, 1 << 8))
            //    {
            //        GameMap.I.ResetFlags(0);
            //        hit.collider.GetComponent<Hex>().PaintNeighbors(Color.green);
            //    }
            //}


            if (Input.GetMouseButtonDown(1) && selectedFigure && selectedFigure.Owner == activePlayer)
            {
                var ray = currCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 1000, 1 << 8))
                    selectedFigure.MoveTo(hit.collider.GetComponent<Hex>(), Input.GetKey(KeyCode.Q));
            }

        }

        public void ActivatePlayer(Player player)
        {
            activePlayer = player;
        }

        public void DeactivatePlayer(Player player)
        {
            if (activePlayer == player) player = null;
        }
    }
}
