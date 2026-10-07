using UnityEngine;
using UnityEngine.Serialization;

namespace Study3D.MiniGolf
{
    public class GolfAimIndicator : MonoBehaviour
    {
        [Header("Settings")] 
        [FormerlySerializedAs("maxLength")]
        public float length = 2.5f;

        private LineRenderer line;

        private void Awake()
        {
            line = GetComponent<LineRenderer>();
            line.useWorldSpace = true; // 월드 공간 좌표계를 사용한다고 설정합니다.
            line.positionCount = 2; // 점 두개의 위치로 선을 그린다고 설정합니다.
            Hide();
        }

        public void Show(Vector3 origin, Vector3 direction)
        {
            line.SetPosition(0, origin);
            line.SetPosition(1, origin + direction * length);
            line.enabled = true;
        }

        public void Hide()
        {
            line.enabled = false;
        }
    }
}
