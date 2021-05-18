using UnityEngine;
using UnityEngine.UI;

public class ColorSwatches : MonoBehaviour
{
    [SerializeField] private Color32[] colors = {
        new Color32(45, 34, 30, 255),
        new Color32(75, 57, 50, 255),
        new Color32(105, 80, 70, 255), 
        new Color32(135, 103, 90, 255),
        new Color32(165, 126, 110, 255),
        new Color32(195, 149, 130, 255),
        new Color32(225, 172, 150, 255),
        new Color32(255, 195, 170, 255)
        };

    void Start()
    {
        for(int i = 0; i < colors.Length; i++)
        {
            transform.GetChild(i).GetComponent<Image>().color = colors[i];
        }
    }
}
