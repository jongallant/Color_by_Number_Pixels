using TMPro;
using UnityEngine;

public class ColorSwatch : MonoBehaviour
{
    public int ID { get; private set; }

    bool Completed;
    bool Selected;
    TextMeshPro Text;
    SpriteRenderer Background;
    SpriteRenderer Border;

    void Awake()
    {
        Border = transform.Find("Border").GetComponent<SpriteRenderer>();
        Background = transform.Find("Background").GetComponent<SpriteRenderer>();
        Text = transform.Find("Text").GetComponent<TextMeshPro>();
    }

    public void SetData(int id, Color color)
    {
        ID = id;
        Text.text = id.ToString();
        Background.color = color;
    }

    public void SetCompleted()
    {
        Completed = true;
        Text.text = "";
    }

    public void SetSelected(bool selected)
    {
        if (!Completed)
        {
            Selected = selected;
            if (Selected)
            {
                Border.color = Color.yellow;
            }
            else
            {
                Border.color = Color.black;
            }
        }
    }


}
