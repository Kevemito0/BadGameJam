// Assets/Scripts/Canvas/QuestChecklistUI.cs
using UnityEngine;
using TMPro;

public class QuestChecklistUI : MonoBehaviour
{
    [SerializeField] private PlayerQuestManager questManager;

    [Header("Checklist Items (TextMeshPro)")]
    [SerializeField] private TextMeshProUGUI weaponText;
    [SerializeField] private TextMeshProUGUI queueText;
    [SerializeField] private TextMeshProUGUI cardText;

    [Header("Optional: All done panel")]
    [SerializeField] private GameObject allDonePanel;

    private readonly string incomplete = "☐  ";

    private void OnEnable()
    {
        questManager.OnQuestStateChanged += Refresh;
        Refresh();
    }

    private void OnDisable()
    {
        questManager.OnQuestStateChanged -= Refresh;
    }

    private void Refresh()
    {
        if (allDonePanel != null)
            allDonePanel.SetActive(questManager.AllQuestsComplete);
    }
}