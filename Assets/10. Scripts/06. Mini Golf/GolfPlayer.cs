using System;
using Study.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Study3D.MiniGolf
{
    public class GolfPlayer : MonoBehaviour
    {
        [Header("Settings")]
        public float minPower;
        public float maxPower;
        public float cycleTime;

        [Header("Ref")] 
        public GolfBall ball;
        public GolfAimIndicator aimIndicator;
        
        private float chargeTime;
        private Camera mainCam;
        private float power01; // 0~1 까지로 치환한 세기
        private bool chargeCancelled;

        public float Power01 => power01;
        public bool IsCharging { get; private set; }
        public event Action ShotFired;

        private void Awake()
        {
            mainCam = Camera.main;
            // 다음 홀로 이동할 때 누른 스페이스가 타격 입력으로 이어지지 않게 한다.
            chargeCancelled = true;
        }
        
        private void Update()
        {
            Vector3 aimDirection = mainCam.transform.forward;
            aimDirection.y = 0.0f;
            aimDirection.Normalize();

            bool canShoot = ball.IsStopped;
            bool isHoldingCharge = SimpleInput.GetKey(Key.Space);
            if (canShoot && isHoldingCharge && SimpleInput.GetMouseButtonDown(SimpleInput.MouseButton.Left))
            {
                chargeCancelled = true;
                chargeTime = 0f;
                power01 = 0f;
            }
            IsCharging = canShoot && isHoldingCharge && chargeCancelled == false;

            if (canShoot)
            {
                if (IsCharging) // power charge
                {
                    chargeTime += Time.deltaTime;
                    power01 = Mathf.Clamp01(chargeTime / Mathf.Max(cycleTime, 0.01f));
                }
                else if (SimpleInput.GetKeyUp(Key.Space) && !chargeCancelled) // 입력 전달
                {
                    float power = Mathf.Lerp(minPower, maxPower, power01);
                    ball.Shoot(aimDirection, power);
                    chargeTime = 0.0f;
                    power01 = 0.0f;
                    ShotFired?.Invoke();
                }
            }
            if (!isHoldingCharge) chargeCancelled = false;
            
            UpdateFeedback(aimDirection, canShoot);
        }

        // 보통 플레이어의 입력정보들에 관련된 UI나 피드백들은
        // 하나의 함수로 모아두는게 좋습니다. (찢지 마세여)
        // (생각외로 엄청 많이 바뀜)
        private void UpdateFeedback(Vector3 direction, bool canShoot)
        {
            if (canShoot && IsCharging)
                aimIndicator.Show(ball.transform.position, direction);
            else
                aimIndicator.Hide();
        }
    }
}
