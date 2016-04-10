using Hexocracy.Systems;
using Hexocracy.HUD;
using UnityEngine;
using Hexocracy.Core;

namespace Hexocracy.Controllers
{
    [GameService(GameServiceType.Contoller)]
    public class HUDController
    {
        private static readonly Vector3 DEFAULT_OFFSET = new Vector3(0, 20, 20);

        private StatsPanel statsPanel;

        public Canvas Canvas { get; private set; }

        private HUDController()
        {
            Canvas = RM.InstantiatePrefab<Canvas>("Canvas");

            if (!GameObject.FindObjectOfType<Camera>())
            {
                RM.InstantiatePrefab<Camera>("MainCamera");
            }
        }

        [RawPrototype]
        public void FocusCameraOnPoint(Vector3 point)
        {
            Camera.main.transform.position = new Vector3(point.x, DEFAULT_OFFSET.y, point.z + DEFAULT_OFFSET.z);
        }

        [RawPrototype]
        public void CreateDummyMSG(int frontSize, float startScale, float endScale, float time, string msg, Color color)
        {
            var dummy = RM.InstantiatePrefab<DummyMsg>("Msg");

            dummy.Initialize(frontSize, startScale, endScale, time, msg, color);

            dummy.GetComponent<RectTransform>().SetParent(Canvas.GetComponent<RectTransform>());

            dummy.GetComponent<RectTransform>().localPosition = new Vector3();
        }

        [RawPrototype]
        public void ShowStatsPanel(Figure target)
        {
            if (!statsPanel)
            {
                statsPanel = RM.InstantiatePrefab<StatsPanel>("StatsPanel");
                statsPanel.GetComponent<RectTransform>().SetParent(Canvas.GetComponent<RectTransform>());
            }

            statsPanel.gameObject.SetActive(true);
            statsPanel.SetTarget(target);
        }
    }
}
