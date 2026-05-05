using UnityEngine;

public class ColorPicker : MonoBehaviour
{
    public Color32 newColor; 

    private void OnTriggerEnter(Collider other)
    {
        Paint brush = other.GetComponentInParent<Paint>();

        if (brush != null)
        {
            brush.ChangeBrushColor(newColor);
        }
    }
}