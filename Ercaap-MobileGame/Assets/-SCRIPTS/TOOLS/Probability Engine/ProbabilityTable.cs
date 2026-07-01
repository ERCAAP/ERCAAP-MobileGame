using System.Collections.Generic;
using UnityEngine;

namespace LunaGames.Extentions.Probability
{
    [System.Serializable]
    public class ProbabilityTable<T>
    {
        public float totalProbability
        {
            get
            {
                float t = 0;
                for (int i = 0; i < ObjectList.Count; i++)
                {
                    t += ObjectList[i].probability;
                }
                return t;
            }
        }
        [System.Serializable]
        public class ProbabilityItem
        {
            public ProbabilityItem(float probability = 1f, T Value = default(T))
            {
                this.probability = probability;
                this.Value = Value;
            }
            public float probability = 1f;
            public T Value = default;
        }
        public List<ProbabilityItem> ObjectList = new List<ProbabilityItem>();

        public void Add(T item, float probability = 1f)
        {
            ObjectList.Add(new ProbabilityItem(probability, item));
        }
        public void Remove(int index)
        {
            ObjectList.RemoveAt(index);
        }
        public void AddRange(T[] items)
        {
            foreach (T item in items)
            {
                ObjectList.Add(new ProbabilityItem(1, item));
            }
        }
        public T GetValue()
        {
            float rand = (float)Random.value * totalProbability;
            float currentProb = 0;
            foreach (var item in ObjectList)
            {
                currentProb += item.probability;
                if (rand <= currentProb) return item.Value;
            }
            return default;
        }
    }
}