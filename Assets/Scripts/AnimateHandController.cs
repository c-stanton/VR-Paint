using UnityEngine;
using UnityEngine.InputSystem;

public class AnimateHandController : MonoBehaviour
{
    public InputActionProperty gripInputActionValue;
    public InputActionProperty triggerInputActionValue;
    private Animator handAnimator;
    private float gripValue;
    private float triggerValue;

    void Start()
    {
        handAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        GripAnimate();
        TriggerAnimate();
    }

    private void GripAnimate()
    {
        gripValue = gripInputActionValue.action.ReadValue<float>();
        handAnimator.SetFloat("Grip", gripValue);
    }

    private void TriggerAnimate()
    {
        triggerValue = triggerInputActionValue.action.ReadValue<float>();
        handAnimator.SetFloat("Trigger", triggerValue);
    }

    public float GetGripValue()
    {
        return gripValue;
    }

    public float GetTriggerValue()
    {
        return triggerValue;
    }
}
