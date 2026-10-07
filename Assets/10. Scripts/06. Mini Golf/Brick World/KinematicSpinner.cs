using UnityEngine;

namespace Study3D.MiniGolf
{
    [RequireComponent(typeof(Rigidbody))]
    public class KinematicSpinner : MonoBehaviour
    {
        public Vector3 axis = Vector3.up;
        public float degreesPerSecond = 35f;

        private Rigidbody body;
        private Quaternion originRotation;
        private float angle;

        private void Awake()
        {
            body = GetComponent<Rigidbody>();
            body.isKinematic = true;
            originRotation = body.rotation;
        }

        private void FixedUpdate()
        {
            angle = Mathf.Repeat(angle + degreesPerSecond * Time.fixedDeltaTime, 360f);
            body.MoveRotation(originRotation * Quaternion.AngleAxis(angle, axis));
        }
    }
}
