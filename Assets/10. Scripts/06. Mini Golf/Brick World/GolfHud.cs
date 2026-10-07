using Study.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Serialization;

namespace Study3D.MiniGolf
{
    public class GolfHud : MonoBehaviour
    {
        [Min(1)] public int par = 4;
        [Min(1)] public int holeNumber = 1;
        [Min(1)] public int nextHoleNumber = 2;
        public GolfPlayer player;
        [FormerlySerializedAs("remainingShotsText")]
        public Text currentShotsText;
        public Image powerFill;
        public GameObject powerGauge;
        public Text holeInfoText;
        public GameObject resultPanel;
        public Text resultNameText;
        public Text resultDetailsText;
        public Text resultHeadingText;
        public Text resultHintText;

        [Header("Power Colors")]
        public Color lowPowerColor = new Color(0.18f, 0.8f, 0.47f);
        public Color mediumPowerColor = new Color(1f, 0.78f, 0.14f);
        public Color highPowerColor = new Color(0.96f, 0.16f, 0.19f);

        public int ShotsTaken { get; private set; }
        private bool isCompleted;

        private void Awake()
        {
            ResetShots();
            powerGauge.SetActive(false);
        }

        private void OnEnable()
        {
            player.ShotFired += OnShotFired;
        }

        private void OnDisable()
        {
            player.ShotFired -= OnShotFired;
        }

        private void Update()
        {
            // 기존 시작 위치 복귀 키와 함께 현재 타수도 초기화
            if (isCompleted == false && SimpleInput.GetKeyDown(Key.Digit2)) ResetShots();
        }

        private void LateUpdate()
        {
            bool showPower = player.enabled && player.IsCharging;
            if (powerGauge.activeSelf != showPower) powerGauge.SetActive(showPower);
            if (showPower)
            {
                float power = player.Power01;
                powerFill.fillAmount = power;
                powerFill.color = power > 0.8f ? highPowerColor : power > 0.5f ? mediumPowerColor : lowPowerColor;
            }
        }

        public void ResetShots()
        {
            ShotsTaken = 0;
            UpdateShotsText();
            holeInfoText.text = "홀 " + holeNumber + " · 목표 " + par + "타";
            HideResult();
        }

        private void OnShotFired()
        {
            ShotsTaken++;
            UpdateShotsText();
        }

        public void ShowResult()
        {
            isCompleted = true;

            resultHeadingText.text = "홀 " + holeNumber + " 완료";
            resultHintText.text = "스페이스바를 눌러 홀 " + nextHoleNumber + "로 이동";

            int difference = ShotsTaken - par;
            resultNameText.text = ScoreName(ShotsTaken, par);

            string relativeScore = difference == 0 ? "기준 타수" : difference > 0 ? difference + "오버파" : -difference + "언더파";
            resultDetailsText.text = ShotsTaken <= 0 ? "기록된 타격이 없습니다" : ShotsTaken + "타 · 목표 " + par + "타 · " + relativeScore;
            resultPanel.SetActive(true);
            powerGauge.SetActive(false);
        }

        public void HideResult()
        {
            isCompleted = false;
            resultPanel.SetActive(false);
        }

        private static string ScoreName(int strokes, int par)
        {
            if (strokes <= 0) return "타수 기록 없음";
            if (strokes == 1) return "홀인원";

            int difference = strokes - par;

            switch (difference)
            {
                case -3: return "알바트로스";
                case -2: return "이글";
                case -1: return "버디";
                case 0: return "파";
                case 1: return "보기";
                case 2: return "더블 보기";
                case 3: return "트리플 보기";
                default: return difference > 0 ? difference + "오버파" : -difference + "언더파";
            }
        }

        private void UpdateShotsText()
        {
            currentShotsText.text = ShotsTaken + "타";
        }
    }
}
