using Hexocracy.Core;
using Hexocracy.View;
using UnityEngine;

namespace Hexocracy.Systems
{
    [RawPrototype]
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
                GS.Get<TurnController>().OnPlayerFinishedTurn();
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
            int createBombFlag = 0;

            if (Input.GetKey(KeyCode.B) && selectedFigure && selectedFigure.Active)
            {
                createBombFlag = selectedFigure.bombCurrCD == 0 ? 1 : -1;
            }

            if (Input.GetMouseButtonDown(1) && selectedFigure && selectedFigure.Active && selectedFigure.Owner == activePlayer)
            {
                if (createBombFlag != -1)
                {
                    var ray = currCamera.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, 1000, 1 << 8))
                    {
                        var result = selectedFigure.MoveTo(hit.collider.GetComponent<Hex>(), Input.GetKey(KeyCode.Q));
                        if (result == MoveResult.Ok && createBombFlag == 1)
                        {
                            selectedFigure.CreateBomb();
                        }
                    }
                }
                else
                {
                    Instantiate(Resources.Load<DummyMsgSpawner>("Prefabs/Play/Msg")).Initialize(25, 1, 2, 2, 
                        "Bomb kaking in CD! Sry Bitch! Wait " + selectedFigure.bombCurrCD + " nah!", Color.red);
                }
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
