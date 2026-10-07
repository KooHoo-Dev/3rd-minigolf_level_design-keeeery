using UnityEngine;

namespace Study3D.MiniGolf
{
    [RequireComponent(typeof(BrickJumpPad))]
    public class BrickJumpPadFeedback : MonoBehaviour
    {
        public Renderer[] rings;
        [Min(0.01f)] public float idleCycleTime = 1.4f;
        [Min(0.01f)] public float launchDuration = 0.45f;
        public Color idleColor = new Color(1f, 0.96f, 0.8f);
        public Color launchColor = new Color(1f, 1f, 0.65f);

        private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
        private BrickJumpPad pad;
        private Transform[] ringTransforms;
        private MaterialPropertyBlock[] properties;
        private float idlePhase;
        private float launchAge = float.PositiveInfinity;

        private void Awake()
        {
            pad = GetComponent<BrickJumpPad>();
            ringTransforms = new Transform[rings.Length];
            properties = new MaterialPropertyBlock[rings.Length];
            for (int i = 0; i < rings.Length; i++)
            {
                ringTransforms[i] = rings[i].transform;
                properties[i] = new MaterialPropertyBlock();
                rings[i].GetPropertyBlock(properties[i]);
            }
        }

        private void OnEnable()
        {
            pad.Launched += OnLaunched;
        }

        private void OnDisable()
        {
            pad.Launched -= OnLaunched;
            launchAge = float.PositiveInfinity;
        }

        private void OnLaunched()
        {
            launchAge = 0f;
        }

        private void Update()
        {
            idlePhase = Mathf.Repeat(idlePhase + Time.deltaTime / Mathf.Max(idleCycleTime, 0.01f), 1f);

            if (launchAge < launchDuration) launchAge += Time.deltaTime;

            bool launching = launchAge < launchDuration;

            for (int i = 0; i < rings.Length; i++)
            {
                float phase = launching
                    ? Mathf.Clamp01(launchAge / launchDuration)
                    : Mathf.Repeat(idlePhase + (float)i / rings.Length, 1f);
                float size = launching
                    ? Mathf.Lerp(0.45f, 1.2f, phase)
                    : Mathf.Lerp(0.5f, 1f, phase);

                ringTransforms[i].localScale = new Vector3(size * 1.1f, 1f, size * 0.75f);
                ringTransforms[i].localPosition = Vector3.up * (0.03f + (launching ? phase * 0.16f + i * 0.04f : i * 0.003f));
                Color color = launching ? launchColor : idleColor;
                color.a = (1f - phase) * (launching ? 0.9f : 0.32f);
                properties[i].SetColor(BaseColor, color);
                rings[i].SetPropertyBlock(properties[i]);
            }
        }
    }
}
