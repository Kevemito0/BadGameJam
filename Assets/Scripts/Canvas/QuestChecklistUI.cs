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
    private readonly string complete   = "<color=#4CAF50>☑  </color>";

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
        weaponText.text = (questManager.hasWeapon    ? complete : incomplete) + "Silahı bul";
        queueText.text  = (questManager.queueCleared ? complete : incomplete) + "Sırayı geç";
        cardText.text   = (questManager.cardLoaded   ? complete : incomplete) + "Kartı yükle";

        if (allDonePanel != null)
            allDonePanel.SetActive(questManager.AllQuestsComplete);
    }
}