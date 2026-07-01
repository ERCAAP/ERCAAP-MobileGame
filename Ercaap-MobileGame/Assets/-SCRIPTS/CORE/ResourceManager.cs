#region Libraries
using System;
using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
using AssetKits.ParticleImage;
using System.Collections.Generic;
using LunaGames.Extentions;
using TMPro;
#endregion

namespace LunaGames.Main
{
    public class ResourceManager : SerializedMonoBehaviour
    {
        #region Properties
        public Dictionary<string, Resource> ResourceList = new Dictionary<string, Resource>();

        [Serializable]
        public class Resource
        {
            //Name hem PlayerPrefde hem de TextMeshPro emojisinde kullaniliyor
            public string ID;
            public float Amount;
            public ParticleImage Particle;
            public TextMeshProUGUI UI;
            public void Increase(float earn, float bonusMultiplier = 1f)
            {
                Amount += earn * bonusMultiplier;
                UpdateUI();
            }
            public void Decrease(float spend)
            {
                Amount -= spend;
                UpdateUI();
            }
            public void Save() => PlayerPrefs.SetFloat(ID, Amount);
            public void Load()
            {
                Amount = PlayerPrefs.GetFloat(ID, 0);
                UpdateUI();
            }
            public void UpdateUI()
            {
                UI.text = /*RichText.Emote("UI.Resources." + ID).Size(1.2f) + " " + */Amount.ToString("0");
            }
        }
        #endregion

        private void Awake()
        {
            CORE.RESOURCES = this;
            foreach (Resource resource in ResourceList.Values)
            {
                resource.Particle.enabled = true;
            }
        }
        private void OnEnable() => LoadData();
        private void OnDisable() => SaveData();
        private void SaveData()
        {
            foreach (Resource resource in ResourceList.Values)
            {
                resource.Save();
            }
        }
        private void LoadData()
        {
            foreach (Resource resource in ResourceList.Values)
            {
                resource.Load();
            }
        }

        #region CollectWithAnimations
        
        /// <summary>
        /// Fiziksel olarak kaynak toplamak için kullanılır. Kaynağın toplandığı kordinattan UI alanında ilgili kaynak için bir icon çıkar
        /// ve kaynağa ait olarak belirlenmiş UI bölgesine doğru tweenlenir. İşlem bittiğinde ilgili kaynak miktarı bir birim artar.
        /// </summary>
        /// <param name="resourceID"></param>
        /// <param name="amount"></param>
        /// <param name="worldPosition"></param>
        [Button] public void Collect(string resourceID, int amount, Vector3 worldPosition)
        {
            StartCoroutine(Collect_CR(resourceID, amount, worldPosition));
        }
        private IEnumerator Collect_CR(string resourceName, int amount, Vector3 worldPosition)
        {
            ResourceList[resourceName].Particle.rectTransform.position = Camera.main.WorldToScreenPoint(worldPosition);
            ResourceList[resourceName].Particle.AddBurst(0, amount);
            yield return null;
            ResourceList[resourceName].Particle.RemoveBurst(0);
        }
        /// <summary>
        /// Bu fonksiyon MMF_Player tarafından event olarak çağırılıyor
        /// </summary>
        /// <param name="resourceName"></param>
        public void IncrementResource(string resourceName)
        {
            ResourceList[resourceName].Increase(1);
        }
        #endregion
        /// <summary>
        /// Harcamak istenilen kaynaktan yeterince var mı diye kontrol eder.
        /// </summary>
        /// <param name="resourceName">İstenilen kaynak ID'si</param>
        /// <param name="neededValue">istenilen miktar</param>
        /// <returns></returns>
        public bool DoIHaveEnough(string resourceName, int neededValue)
        {
            return ResourceList[resourceName].Amount >= neededValue;
        }
        #region InstantEditAmount
        /// <summary>
        /// Hedef kaynak miktarı anında değiştirilir ve UI güncellenir.
        /// </summary>
        /// <param name="resourceName"></param>
        /// <param name="newValue"></param>
        public void EditAmount(string resourceName, int newValue)
        {
            ResourceList[resourceName].Amount = newValue;
            ResourceList[resourceName].UpdateUI();
        }
        /// <summary>
        /// Hedef kaynak miktarı anında istenilen miktar kadar arttırılır ve UI güncellenir.
        /// </summary>
        /// <param name="resourceName"></param>
        /// <param name="newValue"></param>
        public void IncreaseAmount(string resourceName, int earn)
        {
            ResourceList[resourceName].Increase(earn);
        }
        /// <summary>
        /// Hedef kaynak miktarı anında istenilen miktar kadar azaltır ve UI güncellenir.
        /// </summary>
        /// <param name="resourceName"></param>
        /// <param name="newValue"></param>
        public void DecreaseAmount(string resourceName, int spend)
        {
            ResourceList[resourceName].Decrease(spend);
        }
        #endregion

    }
}
