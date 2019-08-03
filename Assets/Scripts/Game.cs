using UnityEngine;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
    public Texture2D Texture;
    Pixel[,] Pixels;    
    Camera Camera;

    int ID = 1;
    Dictionary<Color, int> Colors = new Dictionary<Color, int>();
    List<ColorSwatch> ColorSwatches = new List<ColorSwatch>();

    Dictionary<int, List<Pixel>> PixelGroups = new Dictionary<int, List<Pixel>>();

    RaycastHit2D[] Hits = new RaycastHit2D[1];
    ColorSwatch SelectedColorSwatch;

    void Awake()
    {
        Camera = Camera.main;

        CreatePixelMap();
        CreateColorSwatches();
    }

    void CreatePixelMap()
    {
        Color[] colors = Texture.GetPixels();
        Pixels = new Pixel[Texture.width, Texture.height];

        for (int x = 0; x < Texture.width; x++)
        {
            for (int y = 0; y < Texture.height; y++)
            {
                if (colors[x + y * Texture.width].a != 0)
                {
                    GameObject go = GameObject.Instantiate(Resources.Load("Pixel") as GameObject);
                    go.transform.position = new Vector3(x, y);

                    int id = ID;
                    if (Colors.ContainsKey(colors[x + y * Texture.width]))
                    {
                        id = Colors[colors[x + y * Texture.width]];
                    }
                    else
                    {
                        Colors.Add(colors[x + y * Texture.width], ID);
                        ID++;
                    }

                    Pixels[x, y] = go.GetComponent<Pixel>();
                    Pixels[x, y].SetData(colors[x + y * Texture.width], id);

                    if (!PixelGroups.ContainsKey(id))
                    {
                        PixelGroups.Add(id, new List<Pixel>());                       
                    }
                    PixelGroups[id].Add(Pixels[x, y]);
                }
            }
        }
    }

    void CreateColorSwatches()
    {     
        foreach (KeyValuePair<Color, int> kvp in Colors)
        {
            GameObject go = GameObject.Instantiate(Resources.Load("ColorSwatch") as GameObject);

            float offset = 1.2f;
            go.transform.position = new Vector2(kvp.Value * 2 * offset, -3);
            ColorSwatch colorswatch = go.GetComponent<ColorSwatch>();
            colorswatch.SetData(kvp.Value, kvp.Key);

            ColorSwatches.Add(colorswatch);
        }
    }

    void DeselectAllColorSwatches()
    {
        for (int n = 0; n < ColorSwatches.Count; n++)
        {
            ColorSwatches[n].SetSelected(false);
        }
    }

    void Update()
    {
        Vector2 mousePos = Camera.ScreenToWorldPoint(Input.mousePosition);
        int x = Mathf.FloorToInt(mousePos.x);
        int y = Mathf.FloorToInt(mousePos.y);

        Pixel hoveredPixel = null;
        if (x >= 0 && x < Pixels.GetLength(0) && y >= 0 && y < Pixels.GetLength(1))
        {
            if (Pixels[x, y] != null)
            {
                hoveredPixel = Pixels[x, y];
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            // Check if we clicked on a color swatch
            int hitCount = Physics2D.RaycastNonAlloc(mousePos, Vector2.zero, Hits);
            for (int n = 0; n < hitCount; n++)
            {
                if (Hits[n].collider.CompareTag("ColorSwatch"))
                {
                    SelectColorSwatch(Hits[n].collider.GetComponent<ColorSwatch>());
                }
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (hoveredPixel != null && !hoveredPixel.IsFilledIn)
            {
                if (SelectedColorSwatch != null && SelectedColorSwatch.ID == hoveredPixel.ID)
                {
                    hoveredPixel.Fill();
                    if (CheckIfSelectedComplete())
                    {
                        SelectedColorSwatch.SetCompleted();
                    }
                }
                else
                {
                    hoveredPixel.FillWrong();
                }
            }
        }
    }
    
    void SelectColorSwatch(ColorSwatch swatch)
    {
        if (SelectedColorSwatch != null)
        {
            for (int n = 0; n < PixelGroups[SelectedColorSwatch.ID].Count; n++)
            {
                PixelGroups[SelectedColorSwatch.ID][n].SetSelected(false);
            }

            SelectedColorSwatch.SetSelected(false);
        }

        SelectedColorSwatch = swatch;
        SelectedColorSwatch.SetSelected(true);

        for (int n = 0; n < PixelGroups[SelectedColorSwatch.ID].Count; n++)
        {
            PixelGroups[SelectedColorSwatch.ID][n].SetSelected(true);
        }
    }

    bool CheckIfSelectedComplete()
    {
        if (SelectedColorSwatch != null)
        {
            for (int n = 0; n < PixelGroups[SelectedColorSwatch.ID].Count; n++)
            {
                if (PixelGroups[SelectedColorSwatch.ID][n].IsFilledIn == false)
                    return false;
            }
        }

        return true;
    }

}
