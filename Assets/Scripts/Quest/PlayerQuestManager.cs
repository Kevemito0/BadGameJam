// Assets/Scripts/Quest/PlayerQuestManager.cs
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Game/PlayerQuestManager", fileName = "PlayerQuestManager")]
public class PlayerQuestManager : ScriptableObject
{
    [Header("Quest State")]
    public bool hasIstanbulCard = false;  
    public bool hasWeapon       = false;
    public bool queueCleared    = false;
    public bool cardLoaded      = false;

    public UnityAction OnQuestStateChanged;

    public bool AllQuestsComplete => hasIstanbulCard && hasWeapon && queueCleared && cardLoaded;

    public void PickupIstanbulCard()
    {
        if (hasIstanbulCard) return;
        hasIstanbulCard = true;
        OnQuestStateChanged?.Invoke();
    }

    public void PickupWeapon()
    {
        if (!hasIstanbulCard) {  return; }
        if (hasWeapon) return;
        hasWeapon = true;
        OnQuestStateChanged?.Invoke();
    }

    public void ClearQueue()
    {
        if (!hasWeapon) {  return; }
        if (queueCleared) return;
        queueCleared = true;
        OnQuestStateChanged?.Invoke();
    }

    public void LoadCard()
    {
        if (!hasIstanbulCard) {  return; }
        if (!queueCleared)    {  return; }
        if (cardLoaded) return;
        cardLoaded = true;
        OnQuestStateChanged?.Invoke();
    }

    public void ResetAll()
    {
        hasIstanbulCard = false;
        hasWeapon       = false;
        queueCleared    = false;
        cardLoaded      = false;
        OnQuestStateChanged?.Invoke();
    }
}