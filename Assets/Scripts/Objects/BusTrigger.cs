// Assets/Scripts/Objects/BusTrigger.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class BusTrigger : MonoBehaviour, IInteractable
{
    [SerializeField] private PlayerQuestManager questManager;
    [SerializeField] private string nextSceneName = "BusScene";

    [SerializeField] private string lockedTxt = "Otobüs kilitli";
    [SerializeField] private string readyTxt  = "Otobüse bin [E]";

    public string InteractionText => questManager.AllQuestsComplete ? readyTxt : lockedTxt;

    public void Interact()
    {
        if (!questManager.AllQuestsComplete)
        {
            Debug.Log("[Bus] Henüz hazır değil.");
            return;
        }
        SceneManager.LoadScene(nextSceneName);
    }
}