using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawMeshUI : MonoBehaviour {

    private void Awake() {
        SetThickness(0.1f);
        SetColor(Color.red);
        transform.Find("Thickness1Btn").GetComponent<Button>().onClick.AddListener(() => { SetThickness(0.1f); });
        transform.Find("Thickness2Btn").GetComponent<Button>().onClick.AddListener(() => { SetThickness(0.2f); });
        transform.Find("Thickness3Btn").GetComponent<Button>().onClick.AddListener(() => { SetThickness(0.3f); });

        //transform.Find("Color1Btn").GetComponent<Button>().onClick.AddListener(() => { SetColor(GetColorFromString("000000")); });
        //transform.Find("Color2Btn").GetComponent<Button>().onClick.AddListener(() => { SetColor(GetColorFromString("FFFFFF")); });
        //transform.Find("Color3Btn").GetComponent<Button>().onClick.AddListener(() => { SetColor(GetColorFromString("22FF00")); });
        //transform.Find("Color4Btn").GetComponent<Button>().onClick.AddListener(() => { SetColor(GetColorFromString("0077FF")); });
    }

    private void SetThickness(float thickness) {
        DrawMeshFull.Instance.SetThickness(thickness);
    }

    private void SetColor(Color color) {
        DrawMeshFull.Instance.SetColor(color);
    }

    [SerializeField]
    private Material lineRenderMat;

    public void SetColorCustom(string hex)
    {
        Color color = GetColorFromString(hex);
        lineRenderMat.color = color;
    }

    public static Color GetColorFromString(string hex)
    {
        if (string.IsNullOrEmpty(hex))
            return Color.white;

        // Ensure it starts with '#'
        if (!hex.StartsWith("#"))
            hex = "#" + hex;

        Color color;
        if (ColorUtility.TryParseHtmlString(hex, out color))
            return color;

        Debug.LogWarning($"Invalid hex color string: {hex}");
        return Color.magenta; // fallback color
    }
}