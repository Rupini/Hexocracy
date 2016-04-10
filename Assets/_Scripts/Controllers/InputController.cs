using Hexocracy.Core;
using Hexocracy.View;
using UnityEngine;

namespace Hexocracy.Systems
{
    [RawPrototype]
    [GameService(GameServiceType.Contoller)]
    public class InputController : MonoBehaviour
    {
        private const int HEX_LAYER = 1 << 8;
        private const int FIGURE_LAYER = 1 << 9;
        private const float CAST_DISTANCE = 100;

        private Figure selectedFigure;
        private Player activePlayer;

        private static InputController r_mono_ctor()
        {
            return new GameObject("InputController").AddComponent<InputController>();
        }

        private void Update()
        {
            if (activePlayer != null)
            {
                Process();
            }
        }

        private void Process()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                GS.Get<TurnController>().OnPlayerFinishedTurn();
            }

            if (Input.GetMouseButtonDown(0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, CAST_DISTANCE, FIGURE_LAYER))
                {
                    selectedFigure = hit.collider.GetComponent<Figure>();
                    StatsPanel.SetTarget((Figure)selectedFigure);
                }
            }

            if (Input.GetMouseButtonDown(1) && selectedFigure && selectedFigure.Active && selectedFigure.Owner == activePlayer)
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, CAST_DISTANCE, HEX_LAYER))
                {
                    selectedFigure.MoveTo(hit.collider.GetComponent<Hex>(), Input.GetKey(KeyCode.Q));
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
