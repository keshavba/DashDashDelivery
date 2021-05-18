using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

[Serializable]
public class ColorEvent : UnityEvent<Color> { }

public class ColorPicker : MonoBehaviour
{
    public ColorEvent CurrentColor;
    public ColorEvent OnColorClick;
    private RectTransform rect;
    private Texture2D colorTexture;

    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();
        colorTexture = GetComponent<Image>().mainTexture as Texture2D;
    }

    // Update is called once per frame
    void Update()
    {
        if(RectTransformUtility.RectangleContainsScreenPoint(rect, Input.mousePosition))
        {
            Vector2 delta;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, Input.mousePosition, null, out delta);

            float width = rect.rect.width;
            float height = rect.rect.height;
            delta += new Vector2(width * 0.5f, height *.5f);

            float x = Mathf.Clamp(delta.x / width, 0f, 1f);
            float y = Mathf.Clamp(delta.y / height, 0f, 1f);

            int textX = Mathf.RoundToInt(x * colorTexture.width);
            int textY = Mathf.RoundToInt(y * colorTexture.height);

            Color pickedColor = colorTexture.GetPixel(textX, textY);

            if(Input.GetMouseButtonDown(0))
            {
                CurrentColor?.Invoke(pickedColor);
                OnColorClick?.Invoke(pickedColor);
            }
        }
    }
}
