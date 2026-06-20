using UnityEngine;

public class LoopBreakerObject : MonoBehaviour, IInteractable, ILoopBreaker
{
    [Header("Interaction")]
    [SerializeField] private string interactionTxt = "Inspect";

    [Header("Time Loop")]
    [Tooltip("Break the loop?")]
    [SerializeField] private bool breaksLoop = true;

    [Header("Optional")]
    [SerializeField] private GameObject activateOnBreak;   
    [SerializeField] private GameObject deactivateOnBreak; 

    public string InteractionText => interactionTxt;

    public void Interact()
    {
        if (CanBreakLoop)
        {
            TimeLoopManager.Instance?.BreakLoop();

            if (activateOnBreak != null)
                activateOnBreak.SetActive(true);

            if (deactivateOnBreak != null)
                deactivateOnBreak.SetActive(false);
        }
    }

    public bool CanBreakLoop => breaksLoop && !TimeLoopManager.Instance.LoopBroken;
}