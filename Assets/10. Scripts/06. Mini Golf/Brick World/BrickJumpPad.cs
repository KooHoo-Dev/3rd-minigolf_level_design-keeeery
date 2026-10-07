using System;
using UnityEngine;

namespace Study3D.MiniGolf
{
    public class BrickJumpPad : MonoBehaviour
    {
        public Vector3 velocityChange = new Vector3(0f, 2.8f, 1f);
        public event Action Launched;

        private void OnTriggerEnter(Collider other)
        {
            Rigidbody body = other.attachedRigidbody;

            if (body == null || body.TryGetComponent(out GolfBall ball) == false)
                return;

            body.AddForce(transform.TransformDirection(velocityChange), ForceMode.VelocityChange);
            Launched?.Invoke();
        }
    }
}
