using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;
using LunaGames.Extentions;
using MoreMountains.Feedbacks;
using Cinemachine;
using System.Collections.Generic;
using LunaGames.Tools.UpgradeSystem;
using DG.Tweening;
using System.Threading;
using System.Collections;

namespace LunaGames.Main
{
    [HideMonoScript]
    public class UIManager : MonoBehaviour
    {
        [BoxGroup("Assaignables")] public Transform PanelGame;
        [BoxGroup("Assaignables")] public Transform QuestPanel;
        [BoxGroup("Assaignables")] public TextMeshProUGUI questCounterText;
        [BoxGroup("Assaignables")] public TextMeshProUGUI xpGroupText;
        [BoxGroup("Assaignables")] public TextMeshProUGUI healthText;
        [BoxGroup("Assaignables")] public TextMeshProUGUI stackCapacityFullText;
        [BoxGroup("Assaignables")] public TextMeshProUGUI stackCounterText;
        [BoxGroup("Assaignables")] public GameObject feetPanel;
        [BoxGroup("Assaignables")] public RectTransform feetStickTransform;
        [BoxGroup("Assaignables")] public TextMeshProUGUI feetText;

        [BoxGroup("Assignable")] public GameObject targetEnemyFinder;
        [BoxGroup("Assignable")] public GameObject upgradePanel;
        [BoxGroup("Assignable")] public GameObject playerUpgradePanel;
        [BoxGroup("Assignable")] public GameObject playerUpgraNotification;
        [BoxGroup("Assignable")] public GameObject capacityPanelU;
        [BoxGroup("Assignable")] public GameObject fallDownButton;
        [BoxGroup("Assignable")] public UpgradeButton[] upgradeButtons;

        [BoxGroup("Assaignables")] public TextMeshProUGUI levelText;
        [BoxGroup("Assaignables")] public TextMeshProUGUI moneyText;
        [BoxGroup("Assaignables")] public TextMeshProUGUI diomandText;
        [BoxGroup("Assaignables")] public Image healthBarImage;
        [BoxGroup("Assaignables")] public Image xpBarImage;
        [BoxGroup("Assaignables")] public Image stackCapacityImage;
        [BoxGroup("Assaignables")] public GameObject stackCapacityFull;
        [BoxGroup("Assaignables"), SerializeField] GameObject fpsPanel;
       
   
        [BoxGroup("Toggles"), SerializeField] Toggle soundToggle;
        [BoxGroup("Toggles"), SerializeField] Toggle vibrationToggle;
        [BoxGroup("Toggles"), SerializeField] Toggle fpsToggle;
        [BoxGroup("Toggles"), SerializeField] Toggle HDToggle;


        [BoxGroup("Alerts"), SerializeField] public MMF_Player youCantClimbAlert;
        [BoxGroup("Alerts"), SerializeField] public ParticleSystem windParticle;
        [BoxGroup("Alerts"), SerializeField] public MMF_Player playerDamageVignette;
        [BoxGroup("Cameras"), SerializeField] public CinemachineVirtualCamera playerCam;
        [BoxGroup("Cameras"), SerializeField] public CinemachineVirtualCamera lockedAreaCam;
        [BoxGroup("Joystick"), SerializeField] public GameObject playerJoystick;
        [BoxGroup("Joystick"), SerializeField] public RectTransform playerJoystickBG;
        [BoxGroup("Joystick"), SerializeField] public RectTransform playerJoystickHandle;


        [BoxGroup("StairInfo"), SerializeField] public RectTransform stairInfoPanel;
        [BoxGroup("StairInfo"), SerializeField] public TextMeshProUGUI ladderCountText;
        [BoxGroup("StairInfo"), SerializeField] public TextMeshProUGUI ladderTypeText;
        
        [BoxGroup("PlayerSprites"), SerializeField] public List<Sprite> playerSpriteList;

        public GameObject targetColonyFinderButton;
        public GameObject lockedAreaContinueButton;
        Coroutine zoomOutCor;
        Coroutine zoomInCor;
        private bool isStackCapacityFullTextEnable;

        private void Awake()
        {
   
            CORE.UI = this;
        }
        private void Start()
        {
            soundToggle.isOn = CORE.DATA.Sound;
            vibrationToggle.isOn = CORE.DATA.Vibration;
            fpsToggle.isOn = CORE.DATA.FPS;
            HDToggle.isOn = CORE.DATA.HD;
            HDToggle.gameObject.SetActive(CORE.DATA.Debugger);
            fpsPanel.SetActive(CORE.DATA.FPS);
            moneyText.text = CORE.DATA.Gold.ToString();
            SetUpgradePanelNoticifation();
        }
        public void ResetGame()
        {
            SceneManager.LoadScene(1);
        }

        public void SetMoneyParticlePanelAndPlay(int particleCount)
        {
       
        }
        public void SetXpParticlePanel(int particleCount)
        {
           
        }
        public void SetUpgradePanelNoticifation()
        {
            bool isOpen = false;
            float money = CORE.RESOURCES.ResourceList["Gold"].Amount;
            foreach (Transform t in CORE.UI.playerUpgradePanel.transform)
            {
                if (t.TryGetComponent<UpgradeButton>(out UpgradeButton button))
                {
                    if (!(button.HaveIMaxedOut || !button.DoIHaveMoney))
                        isOpen = true;
                    break;
                }
            }
            playerUpgraNotification.SetActive(isOpen);
        }
        public void PanelQuest()
        {
            CORE.UI.capacityPanelU.SetActive(true);
            CORE.UI.upgradePanel.SetActive(true);
        }
      
        public void UpdateHealthUI(float currentHealth, float maxHealth)
        {
            healthText.text = $"{currentHealth:F0} / {maxHealth:F0}";
            healthBarImage.fillAmount = currentHealth / maxHealth;
        }

 
        public void UpdateXPUI(float currentXP, float targetXP)
        {
            xpGroupText.text = $"{currentXP:F0} / {targetXP:F0}";
            xpBarImage.fillAmount = currentXP / targetXP;
            stackCapacityImage.color = Color.green;
        }

        public void UpdateStackUI(int currentStack, int maxStack)
        {
            stackCapacityImage.transform.DOKill();
            stackCapacityImage.DOFillAmount((float)currentStack / (float)maxStack, 0.2f);
            
            if (currentStack >= maxStack)
            {
                stackCounterText.gameObject.SetActive(false);
                stackCapacityFull.SetActive(true);
            }
            else
            {
                stackCounterText.gameObject.SetActive(true);
                stackCapacityFull.SetActive(false);
                stackCounterText.text = $"CAPACITY ({currentStack}/{maxStack})";
            }
            //stackCapacityImage.fillAmount = currentStack / maxStack;
        }

        public void ShowStackCapacityFullText()
        {
            //stackCapacityImage.color = Color.red;
            if (isStackCapacityFullTextEnable)
                return;
            isStackCapacityFullTextEnable = true;
            GameObject go = Instantiate(stackCapacityFullText.gameObject,Vector3.zero, Quaternion.identity, PanelGame);
            go.transform.localPosition = Vector3.zero;
            go.transform.localPosition = Vector3.up * -600;
            go.SetActive(true);
            Invoke(nameof(HideStackCapacityFullText), 2f); // Hide the text after 2 seconds
        }

        private void HideStackCapacityFullText()
        {
            stackCapacityFullText.gameObject.SetActive(false);
            isStackCapacityFullTextEnable = false;
        }

        public void CamZoomOut()
        {
            if (zoomInCor != null) StopCoroutine(zoomInCor);
            if (zoomOutCor != null) StopCoroutine(zoomOutCor);
            zoomOutCor = StartCoroutine(CamZoomOutCor());
        }
        public void CamZoomIn()
        {
            if (zoomInCor != null) StopCoroutine(zoomInCor);
            if (zoomOutCor != null) StopCoroutine(zoomOutCor);
            zoomInCor = StartCoroutine(CamZoomInCor());
        }
        private IEnumerator CamZoomOutCor()
        {
            float timer = 0;
            float currentFOV = playerCam.m_Lens.FieldOfView;
            windParticle.gameObject.SetActive(true);
            while (timer <= 1f)
            {
                var main = windParticle.main;
                main.startColor = new Color(1f, 1f, 1f, timer);

                timer += Time.deltaTime * 2f;
                playerCam.m_Lens.FieldOfView = Mathf.Lerp(currentFOV, 70f, timer);
                yield return new WaitForEndOfFrame();
            }
        }
        private IEnumerator CamZoomInCor()
        {
            float timer = 0;
            float currentFOV = playerCam.m_Lens.FieldOfView;
            while (timer <= 1f)
            {
                var main = windParticle.main;
                main.startColor = new Color(1f, 1f, 1f, 1f - timer);

                timer += Time.deltaTime * 2f;
                playerCam.m_Lens.FieldOfView = Mathf.Lerp(currentFOV, 60f, timer);
                yield return new WaitForEndOfFrame();
            }
            windParticle.gameObject.SetActive(false);
        }
    }
}