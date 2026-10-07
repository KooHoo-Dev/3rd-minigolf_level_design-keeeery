using UnityEngine;

namespace Study3D.MiniGolf
{
    public class BrickFallReturn : MonoBehaviour
    {
        public GolfBall ball;
        public float returnBelowY = -5f;

        private void FixedUpdate()
        {
            if (ball?.transform.position.y < returnBelowY)
                ball.ReturnToLasShotPosition();
        }
    }
}
