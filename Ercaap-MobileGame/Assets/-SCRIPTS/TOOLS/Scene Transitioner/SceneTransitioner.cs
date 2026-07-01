using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using NUnit.Framework;
using System.Collections.Generic;

namespace LunaGames.Tools
{
    public class SceneTransitioner : MonoBehaviour
    {
        public static SceneTransitioner Instance;
        [SerializeField] float transitionSpeed = 0.5f;
        [SerializeField] Image curtain;
        private bool InTransition;
        void Awake()
        {
            if (Instance != null) Destroy(gameObject);
            else Instance = this;

            DontDestroyOnLoad(gameObject);
        }

        public void LoadScene() {
            if (InTransition) return;
            InTransition = true;
            //int currentScene = SceneManager.GetActiveScene().buildIndex;
            //if (currentScene == sceneIndex) LoadSceneInstant(sceneIndex);
            //else StartCoroutine(TransitionToScene(sceneIndex));
            StartCoroutine(TransitionToScene());
        }
        private void LoadSceneInstant(int sceneIndex) {
            SceneManager.LoadScene(sceneIndex);
            InTransition = false;
        }
        private IEnumerator TransitionToScene()
        {
            float lerp = 0;
            SetRopeSpeed(10);
            while (curtain.color.a < 1)
            {
                lerp += Time.fixedDeltaTime / transitionSpeed;
                curtain.color = Color.Lerp(Color.clear, Color.black, lerp);
                yield return null;
            }
            lerp = 0;

            PlayerSM.PlayerSM.instance.transform.position = Vector3.down * 0.075f + Vector3.back;
            yield return new WaitForSeconds(0.3f);

            while (curtain.color.a > 0)
            {
                lerp += Time.fixedDeltaTime / transitionSpeed;
                curtain.color = Color.Lerp(Color.black, Color.clear, lerp);
                yield return null;
            }
            SetRopeSpeed(1);
            InTransition = false;
        }
        private void SetRopeSpeed(float speed)
        {
            List<RopeController> ropelist = new List<RopeController>();

            foreach (var item in PlayerSM.PlayerSM.instance.ropeList)
            {
                ropelist.Add(item.rope);
            }
            foreach (var item in ropelist)
            {
                item.ropeSpeed = speed;
            }
        }
    }
}
