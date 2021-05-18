using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Customization
{
    [Header("Model Data")]
    public string modelName;
    [Tooltip("Automatically set the name of the model/texture")] public bool autoNaming;
    public GameObject model;

    public Material material;
    public bool isPurchased;
    public List<TextCustomization> baseTextures = new List<TextCustomization>();
    public List<TextCustomization> patterns = new List<TextCustomization>();
    public List<TextCustomization> logos = new List<TextCustomization>();

    [Header("For shoe customization!")]
    public GameObject additionalModel;

}

[System.Serializable]
public class TextCustomization
{
    public string textureName;
    [Tooltip("Automatically set the name of the model/texture")] public bool autoNaming;
    public Texture texture;
    public bool isPurchased;
}
