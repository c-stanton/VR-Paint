using UnityEngine;

public class ClearStrokes : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Paint brush = other.GetComponentInParent<Paint>();

        if (brush != null)
        {
            brush.UndoLastStroke();
        }
    }
}
