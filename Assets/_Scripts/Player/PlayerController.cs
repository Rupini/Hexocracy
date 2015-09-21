using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy
{
    public class PlayerController : MonoBehaviour
    {
        public static class Builder
        {
            public static List<PlayerController> Build(List<Player> players)
            {
                List<int> orders = new List<int>();
                List<PlayerController> controllers = new List<PlayerController>();
                for (int i = 0; i < players.Count; i++)
                    orders.Add(i);


                foreach (var player in players)
                {
                    var go = new GameObject("Player Controller - " + player.Name);
                    var controller = go.AddComponent<PlayerController>();

                    controllers.Add(controller);

                    controller.Owner = player;

                    var orderIndex = Random.Range(0, orders.Count);
                    controller.TurnOrder = orders[orderIndex];
                    orders.RemoveAt(orderIndex);
                }

                return controllers;
            }
        }

        private Camera currCamera;
        private bool activateInNextUpdate;
        private bool deactivateInNextUpdate;
        private bool active;

        public Player Owner { get; private set; }

        public int TurnOrder { get; private set; }

        private void Awake()
        {
            currCamera = Camera.main;
        }

        private Figure selectedFigure;

        private void Update()
        {
            if (active)
            {

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


                if (Input.GetMouseButtonDown(1) && selectedFigure && selectedFigure.Owner == Owner)
                {
                    var ray = currCamera.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, 1000, 1 << 8))
                        selectedFigure.MoveTo(hit.collider.GetComponent<Hex>(), Input.GetKey(KeyCode.Q));
                }
            }

            UpdateActiveState();
        }

        private void UpdateActiveState()
        {
            if (activateInNextUpdate)
            {
                activateInNextUpdate = false;
                active = true;
            }
            if (deactivateInNextUpdate)
            {
                deactivateInNextUpdate = false;
                active = false;
            }
        }

        public void Activate()
        {
            activateInNextUpdate = true;
            Debug.Log("Player " + Owner.Name + " Activated");
        }

        public void Deactivate()
        {
            deactivateInNextUpdate = true;
        }
    }
}
