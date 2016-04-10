using Hexocracy.Controllers;
using Hexocracy.Core;
using Hexocracy.HUD;
using UnityEngine;

namespace Hexocracy.Controller
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

        private HUDController hud;

        private TurnController turnController;

        private static InputController r_mono_ctor()
        {
            return new GameObject("InputController").AddComponent<InputController>();
        }

        private void r_post_ctor()
        {
            hud = GS.Get<HUDController>();
            turnController = GS.Get<TurnController>();
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
                turnController.OnPlayerFinishedTurn();
            }

            if (Input.GetMouseButtonDown(0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, CAST_DISTANCE, FIGURE_LAYER))
                {
                    selectedFigure = hit.collider.GetComponent<Figure>();
                    GS.Get<HUDController>().ShowStatsPanel(selectedFigure);
                }
            }

            if (Input.GetMouseButtonDown(1) && selectedFigure && selectedFigure.GetComponent<Actor>().Active && selectedFigure.Owner == activePlayer)
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, CAST_DISTANCE, HEX_LAYER))
                {
                    selectedFigure.MoveTo(hit.collider.GetComponent<Hex>(), Input.GetKey(KeyCode.Q));
                }

            }
        }

        public bool ActivateActor(IActor actor)
        {
            if (actor.Activate())
            {
                activePlayer = actor.Owner;

                hud.FocusCameraOnPoint(actor.Position);

                hud.ShowStatsPanel(actor.Figure);

                return true;
            }

            return true;
        }

        public bool DeactivateActor(IActor actor)
        {
            if (actor.Deactivate())
            {
                activePlayer = null;

                return true;
            }

            return false;
        }
    }
}
