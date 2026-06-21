// Assets/Scripts/Manager/PlayerQuestManager.cs
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Game/PlayerQuestManager", fileName = "PlayerQuestManager")]
public class PlayerQuestManager : ScriptableObject
{
    [Header("Quest State")]
    public bool istCardFound   = false;
    public bool hasWeapon      = false;
    public bool queueCleared   = false;
    public bool cardLoaded     = false;

    // UI'ya bildir
    public UnityAction OnQuestStateChanged;

    public bool AllQuestsComplete => hasWeapon && queueCleared && cardLoaded;


    public void IstCardFound()
    {
        if(istCardFound) return;
        istCardFound = true;
        OnQuestStateChanged?.Invoke();
        Debug.Log("[Quest] ist card found.");
    }
    
    public void PickupWeapon()
    {
        if (hasWeapon) return;
        hasWeapon = true;
        OnQuestStateChanged?.Invoke();
        Debug.Log("[Quest] Silah alındı.");
    }

    public void ClearQueue()
    {
        if (!hasWeapon) { Debug.Log("[Quest] Önce silahı bul!"); return; }
        if (queueCleared) return;
        queueCleared = true;
        OnQuestStateChanged?.Invoke();
        Debug.Log("[Quest] Sıra temizlendi.");
    }

    public void LoadCard()
    {
        if (!queueCleared) { Debug.Log("[Quest] Önce sırayı geç!"); return; }
        if (cardLoaded) return;
        cardLoaded = true;
        OnQuestStateChanged?.Invoke();
        Debug.Log("[Quest] Kart yüklendi! Tüm görevler tamam.");
    }

    public void ResetAll()
    {
        hasWeapon    = false;
        queueCleared = false;
        cardLoaded   = false;
        OnQuestStateChanged?.Invoke();
    }
}