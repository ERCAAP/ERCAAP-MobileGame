using UnityEngine;
using TMPro;
using LunaGames.Extentions;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LunaGames.Tools
{
    public class FPSDisplay : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI fpsText;
        float deltaTime = 0.0f;
        private void Awake()
        {
            Application.targetFrameRate = 300;
        }
        void Update()
        {
            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

            float msec = deltaTime * 1000.0f;
            float fps = 1.0f / deltaTime;
            fpsText.text = $"{msec.ToString("0")}ms\n{fps.ToString("0.0")} FPS";

            float lerp = (fps - 30) / 30;
            #if UNITY_EDITOR
            fpsText.text = $"{msec.ToString("0")}ms\n{fps.ToString("0.0").Material("Outline").Color(Color.Lerp(Color.red, Color.green, lerp)).Size(1.5f)} FPS\nBatches: {UnityStats.batches}";
            #endif

        }
    }
}