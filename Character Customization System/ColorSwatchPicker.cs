using UnityEngine;
using UnityEngine.UI;

public class ColorSwatchPicker : MonoBehaviour
{
    public Image currentColor;
    private BodyCustomizer customizer;

    void Start()
    {
        customizer = transform.GetComponentInParent<Transform>().GetComponentInParent<BodyCustomizer>();
    }

    public void OnColorClick()
    {
        currentColor.color = GetComponent<Image>().color;
        customizer.SetSkinColor(GetComponent<Image>().color);
    }
}
