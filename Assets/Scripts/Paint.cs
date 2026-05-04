using UnityEngine;
using System.Collections.Generic;

public class Paint : MonoBehaviour
{
    public Transform tip;
    public TrailRenderer brushStroke;
    private TrailRenderer currentBrushStroke;
    private Stack<TrailRenderer> previousStroke = new Stack<TrailRenderer>();
    private AnimateHandController aController = null;
    private Color32 currentColor = Color.red;
    int drawCount = 0;
    bool drawColor = true;

    void Start()
    {
        if(tip != null && tip.parent != null)
            tip.parent.GetComponent<Renderer>().material.color = currentColor;
    }

    void Update()
    {
        if (aController == null) return;

        bool isGripPressed = aController.GetGripValue() > 0.8f;
        bool isTriggerPressed = aController.GetTriggerValue() > 0.8f;

        if (drawColor)
        {
            if (isGripPressed && isTriggerPressed)
            {
                currentBrushStroke = Instantiate(brushStroke, tip.position, tip.rotation, tip);
                currentBrushStroke.material.color = currentColor;
                previousStroke.Push(currentBrushStroke);
                
                drawCount = 1;
                drawColor = false;
                Debug.Log("First Stroke Started");
            }
        }
        else
        {
            if (drawCount == 1)
            {
                if (!isTriggerPressed)
                {
                    if (currentBrushStroke != null)
                    {
                        currentBrushStroke.transform.parent = null;
                        drawCount = 0;
                        Debug.Log("Stroke Finished");
                    }
                }
            }
            else if (drawCount == 0)
            {
                if (isGripPressed && isTriggerPressed)
                {
                    currentBrushStroke = Instantiate(brushStroke, tip.position, tip.rotation, tip);
                    currentBrushStroke.material.color = currentColor;
                    previousStroke.Push(currentBrushStroke);
                    drawCount = 1;
                    Debug.Log("Subsequent Stroke Started");
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("VRHand") || (other.transform.parent != null && other.transform.parent.CompareTag("VRHand")))
        {
            AnimateHandController aHandController = other.gameObject.GetComponentInParent<AnimateHandController>();
            
            if (aHandController != null)
            {
                aController = aHandController;
                drawColor = true;
                Debug.Log("Controller Linked Successfully!");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("VRHand") || (other.transform.parent != null && other.transform.parent.CompareTag("VRHand")))
        {
            if (currentBrushStroke != null && currentBrushStroke.transform.parent != null)
            {
                currentBrushStroke.transform.parent = null;
            }
            aController = null;
            drawCount = 0;
            Debug.Log("Controller Unlinked.");
        }
    }

    public void ChangeBrushColor(Color32 newColor)
    {
        currentColor = newColor;

        if (tip != null && tip.parent != null)
        {
            tip.parent.GetComponent<Renderer>().material.color = currentColor;
        }
    }

    public void UndoLastStroke()
    {
        if (previousStroke.Count > 0)
        {
            TrailRenderer strokeToRemove = previousStroke.Pop();
            
            if (strokeToRemove != null)
            {
                Destroy(strokeToRemove.gameObject);
                Debug.Log("Last stroke erased. Strokes remaining: " + previousStroke.Count);
            }
        }
    }
}