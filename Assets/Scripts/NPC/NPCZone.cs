// Assets/Scripts/NPC/NPCZone.cs
using UnityEngine;

public class NPCZone : MonoBehaviour
{
    [SerializeField] private PlayerQuestManager questManager;

    private NPCHealth[] _npcs;

    private void Start()
    {
        _npcs = GetComponentsInChildren<NPCHealth>();
    }

    private void Update()
    {
        if (questManager.queueCleared) return;

        foreach (var npc in _npcs)
        {
            if (!npc.IsDead) return; // biri bile sağsa çık
        }

        questManager.ClearQueue();
    }
}