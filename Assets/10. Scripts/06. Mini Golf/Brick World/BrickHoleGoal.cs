using Study.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Study3D.MiniGolf
{
    public class BrickHoleGoal : MonoBehaviour
    {
        public GolfPlayer player;
        public GolfHud hud;
        public string nextSceneName;

        public bool IsCompleted { get; private set; }
        private bool canAdvance;
        private bool isLoading;

        private void Update()
        {
            if (IsCompleted == false || isLoading) return;
            if (SimpleInput.GetKey(Key.Space) == false) canAdvance = true;
            if (canAdvance == false ||
                SimpleInput.GetKeyDown(Key.Space) == false ||
                string.IsNullOrEmpty(nextSceneName))
                return;

            isLoading = true;
            SceneManager.LoadScene(nextSceneName);
        }

        private void OnTriggerEnter(Collider other)
        {
            Rigidbody body = other.attachedRigidbody;

            if (IsCompleted || body == null || body.TryGetComponent(out GolfBall ball) == false)
                return;

            IsCompleted = true;
            canAdvance = SimpleInput.GetKey(Key.Space) == false;
            body.linearVelocity = Vector3.zero;

            body.angularVelocity = Vector3.zero;

            if (ball.TryGetComponent(out GolfBallTester tester)) tester.enabled = false;

            player.enabled = false;
            player.aimIndicator.Hide();

            hud.ShowResult();
        }
    }
}
