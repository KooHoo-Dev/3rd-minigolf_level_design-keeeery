using UnityEngine;

namespace Study3D.MiniGolf
{
    [RequireComponent(typeof(GolfBall))]
    public class GolfBallMotionColor : MonoBehaviour
    {
        public Color movingColor = new Color(1f, 0.12f, 0.12f);

        private GolfBall ball;
        private Renderer ballRenderer;
        private MaterialPropertyBlock originalAppearance;
        private MaterialPropertyBlock movingAppearance;
        private bool wasMoving;

        private void Awake()
        {
            ball = GetComponent<GolfBall>();
            ballRenderer = GetComponentInChildren<Renderer>();
            originalAppearance = new MaterialPropertyBlock();
            movingAppearance = new MaterialPropertyBlock();
            ballRenderer.GetPropertyBlock(originalAppearance);
            ballRenderer.GetPropertyBlock(movingAppearance);

            int colorProperty = Shader.PropertyToID(ballRenderer.sharedMaterial.HasProperty("_BaseColor") ? "_BaseColor" : "_Color");

            movingAppearance.SetColor(colorProperty, movingColor);
        }

        private void Update()
        {
            bool isMoving = ball.IsStopped == false;

            if (isMoving == wasMoving) return;

            ballRenderer.SetPropertyBlock(isMoving ? movingAppearance : originalAppearance);
            wasMoving = isMoving;
        }

        private void OnDisable()
        {
            ballRenderer.SetPropertyBlock(originalAppearance);
            wasMoving = false;
        }
    }
}
