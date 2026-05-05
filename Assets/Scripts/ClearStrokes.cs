using UnityEngine;

public class ClearStrokes : MonoBehaviour
{
    public Transform visibleButton;
    public Transform pressedPosition;
    public Paint paintBrush;
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = visibleButton.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("VRHand")) return;
        visibleButton.position = pressedPosition.position;
        paintBrush.UndoLastStroke(); 
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("VRHand")) return;
        visibleButton.position = initialPosition;
    }
}