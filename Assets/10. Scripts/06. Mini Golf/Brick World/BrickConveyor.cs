using UnityEngine;

namespace Study3D.MiniGolf
{
    public class BrickConveyor : MonoBehaviour
    {
        public Vector3 direction = Vector3.forward;
        public float acceleration = 2f;

        private void OnTriggerStay(Collider other)
        {
            Rigidbody body = other.attachedRigidbody;

            if (body == null || body.TryGetComponent(out GolfBall ball) == false)
                return;

            Vector3 worldDirection = transform.TransformDirection(direction.normalized);
            body.AddForce(worldDirection * acceleration, ForceMode.Acceleration);
        }
    }
}
