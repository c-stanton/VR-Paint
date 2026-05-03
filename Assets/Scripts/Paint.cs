using System.Collections.Generic;
using UnityEngine;

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
        tip.parent.GetComponent<Renderer>().material.color = currentColor;
    }

    void Update()
    {
        if (aController != null && drawColor)
        {
            if (aController.GetGripValue() == 1 && aController.GetTriggerValue() == 1)
            {
                currentBrushStroke = Instantiate(brushStroke, tip.position, tip.rotation, tip);
                currentBrushStroke.material.color = currentColor;
                previousStroke.Push(currentBrushStroke);
            }
            
            drawCount = 1;
            drawColor = false;
        }

        if (!drawColor)
        {
            if (aController != null && drawCount ==  1)
            {
                if (aController.GetGripValue() == 1 && aController.GetTriggerValue() != 1)
                {
                    if (currentBrushStroke != null)
                    {
                        if (currentBrushStroke.transform.parent != null)
                        {
                            currentBrushStroke.transform.parent = null;
                            drawCount = 0;
                        }
                    }
                }
            }
        }

        if (!drawColor)
        {
            if (aController != null && drawCount == 0)
            {
                if (aController.GetGripValue() == 1 && aController.GetTriggerValue() == 1)
                {
                    currentBrushStroke = Instantiate(brushStroke, tip.position, tip.rotation, tip);
                    currentBrushStroke.material.color = currentColor;
                    previousStroke.Push(currentBrushStroke);
                    drawCount = 1;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("VRHand"))
        {
            return;
        }

        AnimateHandController aHandController = other.gameObject.GetComponent<AnimateHandController>();
        
        if (aController != null)
        {
            aController = aHandController;
            drawColor = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("VRHand")) 
        { 
            return; 
        }

        if (currentBrushStroke != null)
        {
            if (currentBrushStroke.transform.parent != null)
            {
                currentBrushStroke.transform.parent = null;
            }

            if (aController != null)
            {
                aController = null;
            }

            drawCount = 0;
        }
    }
}
