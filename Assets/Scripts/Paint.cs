using UnityEngine;

public class Paint : MonoBehaviour
{
    public Transform tip;
    public TrailRenderer brushStroke;
    private AnimateHandController aController = null;
    private TrailRenderer currentStroke;

    void Start()
    {
        if (tip != null)
            tip.parent.GetComponent<Renderer>().material.color = Color.red;
    }

    void Update()
    {
        if (aController == null) return;

        float triggerValue = aController.GetTriggerValue();

        if (triggerValue > 0.1f && currentStroke == null)
        {
            currentStroke = Instantiate(brushStroke, tip.position, tip.rotation, tip);
        }
        
        if (triggerValue <= 0.1f && currentStroke != null)
        {
            currentStroke.transform.parent = null;
            currentStroke = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("VRHand")) return;

        AnimateHandController aHandController = other.gameObject.GetComponent<AnimateHandController>();
        
        if (aHandController != null)
        {
            aController = aHandController;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("VRHand")) return;

        if (currentStroke != null)
        {
            currentStroke.transform.parent = null;
            currentStroke = null;
        }
        aController = null;
    }
}