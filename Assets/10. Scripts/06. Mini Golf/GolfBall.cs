using System;
using UnityEngine;

namespace Study3D.MiniGolf
{
    // 방향과 힘을 받아서 움직이고, 현재 공이 운동을 하는지 안하는지 알려주는 공
    
    public class GolfBall : MonoBehaviour
    {
        [Header("Settings")] 
        public float stopSpeed = 0.1f;
        public float stopDelay = 0.3f;
        [Range(0f, 1f)] public float movingSurfaceSpeedTolerance = 0.5f;
        // 공을 다이나믹으로 사용하기 때문에 게임적 표현을
        // 회전 스피드에 제한을 활용하여 간접적으로 제어 합니다.
        public float maxAngularSpeed = 150f;

        private Rigidbody rBody;
        private Vector3 lastPosition; // 유저가 마지막으로 공을 건드린 위치
        private Vector3 shotVelocity; // 공의 방향 * 세기가 연산되는 변수
        private bool shotRequested;
        private float slowTime;
        private Rigidbody supportingBody;
        private Vector3 supportPoint;

        public bool IsStopped => (shotRequested == false) && slowTime >= stopDelay;
        
        private void Awake()
        {
            rBody = GetComponent<Rigidbody>();
            rBody.maxAngularVelocity = maxAngularSpeed;
            lastPosition = transform.position;
        }

        // 공의 물리를 사용할 예정이라 Update가 아닌 FixedUpdate를 사용합니다.
        private void FixedUpdate()
        {
            Vector3 relativeVelocity = rBody.linearVelocity;
            float speedThreshold = stopSpeed;

            if (supportingBody != null)
            {
                Vector3 surfaceVelocity = supportingBody.GetPointVelocity(supportPoint);
                relativeVelocity -= surfaceVelocity;
                // 구형 공은 발판 위에서 조금씩 굴러 미끄러지므로 바닥 속도에 비례한 여유를 둔다.
                speedThreshold += surfaceVelocity.magnitude * movingSurfaceSpeedTolerance;
            }

            supportingBody = null;

            if (shotRequested)
            {
                shotRequested = false;
                rBody.AddForce(shotVelocity, ForceMode.VelocityChange);
                return;
            }

            if (relativeVelocity.magnitude < speedThreshold)
                slowTime += Time.fixedDeltaTime;
            else
                slowTime = 0;
        }

        private void OnCollisionEnter(Collision collision)
        {
            UpdateSupport(collision);
        }

        private void OnCollisionStay(Collision collision)
        {
            UpdateSupport(collision);
        }

        private void UpdateSupport(Collision collision)
        {
            Rigidbody otherBody = collision.rigidbody;

            if (otherBody == null || otherBody.isKinematic == false) return;

            for (int i = 0; i < collision.contactCount; i++)
            {
                ContactPoint contact = collision.GetContact(i);

                // 옆면 충돌은 바닥에 실려 이동하는 속도로 취급하지 않음
                if (contact.normal.y < 0.5f) continue;

                supportingBody = otherBody;
                supportPoint = contact.point;

                return;
            }
        }

        // 외부에서 Shoot함수를 호출하여 동작하도록 함수를 public으로 선언합니다
        public void Shoot(Vector3 direction, float power)
        {
            shotRequested = true;
            slowTime = 0f;
            shotVelocity = direction * power;
            lastPosition = transform.position;
        }

        // 공을 position의 위치로 옮기고
        // 다시 shoot을 할 수 있는 상태로 바꿔 줍니다.
        private void Teleport(Vector3 position)
        {
            shotRequested = false;
            slowTime = 0f;
            supportingBody = null;
            // 리지드 바디의 운동량(선 운동량, 각 운동량)을 0으로 바꿔줍니다.
            rBody.linearVelocity = Vector3.zero;
            rBody.angularVelocity = Vector3.zero;
            
            // 공을 물리를 처리하는 개체이므로
            // transform과 리지드 바디의 position을 모두 변경해줍니다.
            // (Interpolate를 사용하는 Rigidbody는 transform도 함께 옮겨야
            // 끌려오듯이 안보일 거입니다.)
            rBody.position = position;
            transform.position = position;
        }

        // public으로 선언해서 외부에서 호출 할 수 있도록 해줍니다.
        public void ReturnToLasShotPosition()
        {
            Teleport(lastPosition);
        }

        // 스테이지 시작시 공을 놓기 위한 함수
        public void PlaceAt(Vector3 position)
        {
            lastPosition = position;
            Teleport(position);
        }
    }
}
