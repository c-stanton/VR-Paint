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
        ChangeBrushColor(currentColor);
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
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("VRHand"))
        {
            AnimateHandController aHandController = other.gameObject.GetComponentInParent<AnimateHandController>();
            
            if (aHandController != null)
            {
                aController = aHandController;
                drawColor = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("VRHand"))
        {
            if (currentBrushStroke != null && currentBrushStroke.transform.parent != null)
            {
                currentBrushStroke.transform.parent = null;
            }
            aController = null;
            drawCount = 0;
        }
    }

    public void ChangeBrushColor(Color32 newColor)
    {
        currentColor = newColor;

        if (tip != null && tip.parent != null)
        {
            tip.parent.GetComponent<Renderer>().material.color = currentColor;
        }

        Gradient newGradient = new Gradient();

        GradientColorKey[] colorKeys = new GradientColorKey[2];
        colorKeys[0] = new GradientColorKey(currentColor, 0.0f);
        colorKeys[1] = new GradientColorKey(currentColor, 1.0f);

        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
        alphaKeys[0] = new GradientAlphaKey(currentColor.a / 255f, 0.0f);
        alphaKeys[1] = new GradientAlphaKey(currentColor.a / 255f, 1.0f);

        newGradient.SetKeys(colorKeys, alphaKeys);

        if (brushStroke != null)
        {
            brushStroke.colorGradient = newGradient;
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
            }
        }
    }
}