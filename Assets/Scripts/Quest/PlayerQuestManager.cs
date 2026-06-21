// Assets/Scripts/Quest/PlayerQuestManager.cs
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Game/PlayerQuestManager", fileName = "PlayerQuestManager")]
public class PlayerQuestManager : ScriptableObject
{
    [Header("Quest State")]
    public bool hasIstanbulCard = false;  // YENİ — ilk zorunlu adım
    public bool hasWeapon       = false;
    public bool queueCleared    = false;
    public bool cardLoaded      = false;

    // UI'ya bildir
    public UnityAction OnQuestStateChanged;

    public bool AllQuestsComplete => hasIstanbulCard && hasWeapon && queueCleared && cardLoaded;

    // ── İstanbulKart ──────────────────────────────────────────────
    public void PickupIstanbulCard()
    {
        if (hasIstanbulCard) return;
        hasIstanbulCard = true;
        OnQuestStateChanged?.Invoke();
        Debug.Log("[Quest] İstanbulKart alındı.");
    }

    // ── Silah ─────────────────────────────────────────────────────
    public void PickupWeapon()
    {
        if (!hasIstanbulCard) { Debug.Log("[Quest] Önce İstanbulKart'ı bul!"); return; }
        if (hasWeapon) return;
        hasWeapon = true;
        OnQuestStateChanged?.Invoke();
        Debug.Log("[Quest] Silah alındı.");
    }

    // ── Sıra ──────────────────────────────────────────────────────
    public void ClearQueue()
    {
        if (!hasWeapon) { Debug.Log("[Quest] Önce silahı bul!"); return; }
        if (queueCleared) return;
        queueCleared = true;
        OnQuestStateChanged?.Invoke();
        Debug.Log("[Quest] Sıra temizlendi.");
    }

    // ── Kart Yükleme ──────────────────────────────────────────────
    public void LoadCard()
    {
        if (!hasIstanbulCard) { Debug.Log("[Quest] Önce İstanbulKart'ı bul!"); return; }
        if (!queueCleared)    { Debug.Log("[Quest] Önce sırayı geç!"); return; }
        if (cardLoaded) return;
        cardLoaded = true;
        OnQuestStateChanged?.Invoke();
        Debug.Log("[Quest] Kart yüklendi! Tüm görevler tamam.");
    }

    // ── Reset ──────────────────────────────────────────────────────
    public void ResetAll()
    {
        hasIstanbulCard = false;
        hasWeapon       = false;
        queueCleared    = false;
        cardLoaded      = false;
        OnQuestStateChanged?.Invoke();
    }
}