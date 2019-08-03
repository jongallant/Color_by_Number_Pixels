using TMPro;
using UnityEngine;

public class Pixel : MonoBehaviour
{
    public int ID { get; private set; }

    TextMeshPro Text;
    Color PixelColor;

    SpriteRenderer Background;
    SpriteRenderer Border;
    public bool IsFilledIn
    {
        get
        {
            if (Background.color == PixelColor)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    void Awake()
    {
        Border = transform.Find("Border").GetComponent<SpriteRenderer>();
        Background = transform.Find("Background").GetComponent<SpriteRenderer>();
        Text = transform.Find("Text").GetComponent<TextMeshPro>();
    }

    public void SetData(Color color, int colorID)
    {
        ID = colorID;
        PixelColor = color;
        Border.color = new Color(0.95f, 0.95f, 0.95f, 1);
        Text.text = colorID.ToString();

        Background.color = Color.Lerp(new Color(PixelColor.grayscale, PixelColor.grayscale, PixelColor.grayscale), Color.white, 0.85f);
    }

    public void SetSelected(bool selected)
    {
        if (selected)
        {
            if (!IsFilledIn)
            {
                Background.color = new Color(0.5f, 0.5f, 0.5f, 1);
            }
        }
        else
        {
            if (!IsFilledIn)
            {
                Background.color = Color.Lerp(new Color(PixelColor.grayscale, PixelColor.grayscale, PixelColor.grayscale), Color.white, 0.85f);// new Color(1, 1, 1, 1);
            }
        }
    }

    public void Fill()
    {
        if (!IsFilledIn)
        {
            Border.color = PixelColor;
            Background.color = PixelColor;
            Text.text = "";
        }
    }

    public void FillWrong()
    {
        if (!IsFilledIn)
        {
            Background.color = new Color(1, 170/255f, 170/255f, 1);
        }
    }
}

