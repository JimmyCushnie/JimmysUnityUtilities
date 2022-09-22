using UnityEngine;
using UnityEngine.UI;

namespace JimmysUnityUtilities
{
    [RequireComponent(typeof(Graphic))]
    public class SimpleGraphicFader : MonoBehaviour
    {
        public float PeriodSeconds = 1;

        [Range(0f, 1f)]
        public float MinAlpha = 0;

        [Range(0f, 1f)]
        public float MaxAlpha = 1;


        private void Awake()
        {
            Graphic = GetComponent<Graphic>();
        }
        private Graphic Graphic;

        private void Update()
        {
            const float tau = Mathf.PI / 2f;
            float coefficient = tau / PeriodSeconds;

            float alpha = MinAlpha + Mathf.Sin(Time.time * coefficient) * (MaxAlpha - MinAlpha);
            Graphic.SetAlpha(alpha);
        }
    }
}