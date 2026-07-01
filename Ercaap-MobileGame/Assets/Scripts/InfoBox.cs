using System;
using UnityEngine;

// InfoBox özelliği - Unity Inspector'da açıklama göstermek için kullanılır
[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
public class InfoBox : PropertyAttribute
{
    public string text;
    public InfoBoxType type;
    public bool showInPlayMode = true;
    public bool showInEditMode = true;

    // Standart bilgi kutusu
    public InfoBox(string text)
    {
        this.text = text;
        this.type = InfoBoxType.Normal;
    }

    // Bilgi kutusu tipini belirterek
    public InfoBox(string text, InfoBoxType type)
    {
        this.text = text;
        this.type = type;
    }

    // Gösterim modunu belirterek
    public InfoBox(string text, InfoBoxType type, bool showInPlayMode, bool showInEditMode)
    {
        this.text = text;
        this.type = type;
        this.showInPlayMode = showInPlayMode;
        this.showInEditMode = showInEditMode;
    }
}

// Bilgi kutusu tipleri
public enum InfoBoxType
{
    Normal,
    Warning,
    Error
} 