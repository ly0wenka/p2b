using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilsClass : MonoBehaviour
{
    // public static TextMesh CreateWorldText(string text, Transform parent = null,
    //     Vector3 localPosition = default(Vector3), int fontSize = 40, Color color)
    // {
    //     return CreateWorldText(parent, text, localPosition, fontSize,
    //         (Color) color, textAnchor, textAlignment, sortingOrder);
    // }
    public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize,
        Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
    {
        var gameObject = new GameObject("World_Text", typeof(TextMesh));
        var transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        var textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }
}
