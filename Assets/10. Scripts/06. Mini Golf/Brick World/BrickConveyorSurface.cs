using UnityEngine;

namespace Study3D.MiniGolf
{
    [RequireComponent(typeof(BrickConveyor))]
    public class BrickConveyorSurface : MonoBehaviour
    {
        public Renderer beltRenderer;
        [Min(0f)] public float scrollSpeed = 0.8f;
        [Min(0.01f)] public float beltLength = 2.16f;

        private static readonly int BaseMapTransform = Shader.PropertyToID("_BaseMap_ST");
        private MaterialPropertyBlock properties;
        private Vector4 textureTransform;
        private float phase;

        private void Awake()
        {
            properties = new MaterialPropertyBlock();
            beltRenderer.GetPropertyBlock(properties);
            Material material = beltRenderer.sharedMaterial;
            Vector2 scale = material.GetTextureScale("_BaseMap");
            Vector2 offset = material.GetTextureOffset("_BaseMap");
            textureTransform = new Vector4(scale.x, scale.y, offset.x, offset.y);
            phase = textureTransform.w;
        }

        private void Update()
        {
            // UV를 음의 방향으로 이동하면 표면 무늬는 컨베이어의 앞쪽으로 흐름
            phase = Mathf.Repeat(phase - scrollSpeed * Time.deltaTime / beltLength, 1f);
            textureTransform.w = phase;
            properties.SetVector(BaseMapTransform, textureTransform);
            beltRenderer.SetPropertyBlock(properties);
        }
    }
}
