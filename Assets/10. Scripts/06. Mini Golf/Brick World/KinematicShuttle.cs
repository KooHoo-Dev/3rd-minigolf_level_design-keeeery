using UnityEngine;

namespace Study3D.MiniGolf
{
    [RequireComponent(typeof(Rigidbody))]
    public class KinematicShuttle : MonoBehaviour
    {
        public Vector3 offset = new Vector3(1.6f, 0f, 0f);
        public float cycleTime = 3f;

        private Rigidbody body;
        private Vector3 originPosition;
        private Vector3 worldOffset;
        private float elapsedTime;

        private void Awake()
        {
            body = GetComponent<Rigidbody>();
            body.isKinematic = true;
            originPosition = body.position;
            worldOffset = transform.TransformVector(offset);
        }

        private void FixedUpdate()
        {
            float duration = Mathf.Max(0.01f, cycleTime);
            elapsedTime = Mathf.Repeat(elapsedTime + Time.fixedDeltaTime, duration);
            float progress = (1f - Mathf.Cos(elapsedTime / duration * Mathf.PI * 2f)) * 0.5f;
            body.MovePosition(originPosition + worldOffset * progress);
        }
    }
}
