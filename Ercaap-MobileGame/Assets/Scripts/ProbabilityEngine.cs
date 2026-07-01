using System;
using System.Collections.Generic;
using UnityEngine;

// Olasılık motoru sınıfı - Editor tarafından kullanılacak
[Serializable]
public class ProbabilityEngine : MonoBehaviour
{
    // Temel olasılık motoru özellikleri
    [SerializeField] 
    private List<ProbabilityTable> tables = new List<ProbabilityTable>();

    public List<ProbabilityTable> Tables => tables;

    // Olasılık hesaplama fonksiyonu
    public T GetRandomItem<T>(string tableName) where T : class
    {
        foreach (var table in tables)
        {
            if (table.name == tableName)
            {
                return table.GetRandomItem() as T;
            }
        }
        
        Debug.LogWarning($"Olasılık tablosu bulunamadı: {tableName}");
        return null;
    }
}

// Olasılık tablosu sınıfı
[Serializable]
public class ProbabilityTable
{
    public string name;
    [SerializeField] 
    private List<ProbabilityItem> items = new List<ProbabilityItem>();

    public List<ProbabilityItem> Items => items;

    // Rastgele öğe seçme
    public object GetRandomItem()
    {
        float totalWeight = 0f;
        
        foreach (var item in items)
        {
            totalWeight += item.weight;
        }
        
        float randomValue = UnityEngine.Random.Range(0, totalWeight);
        float currentWeight = 0f;
        
        foreach (var item in items)
        {
            currentWeight += item.weight;
            if (randomValue <= currentWeight)
            {
                return item.item;
            }
        }
        
        return null;
    }
}

// Olasılık öğesi sınıfı
[Serializable]
public class ProbabilityItem
{
    public UnityEngine.Object item;
    public float weight = 1f;
}

// Jenerik ProbabilityTable
[Serializable]
public class ProbabilityTable<T> where T : UnityEngine.Object
{
    public string name;
    [SerializeField] 
    private List<ProbabilityItem<T>> items = new List<ProbabilityItem<T>>();

    public List<ProbabilityItem<T>> Items => items;

    // Rastgele öğe seçme
    public T GetRandomItem()
    {
        float totalWeight = 0f;
        
        foreach (var item in items)
        {
            totalWeight += item.weight;
        }
        
        float randomValue = UnityEngine.Random.Range(0, totalWeight);
        float currentWeight = 0f;
        
        foreach (var item in items)
        {
            currentWeight += item.weight;
            if (randomValue <= currentWeight)
            {
                return item.item;
            }
        }
        
        return null;
    }
}

// Jenerik ProbabilityItem
[Serializable]
public class ProbabilityItem<T> where T : UnityEngine.Object
{
    public T item;
    public float weight = 1f;
} 