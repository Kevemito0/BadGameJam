using UnityEngine;
using UnityEngine.Events;


public class GameEventListener : MonoBehaviour
{
    [Tooltip("Dinlenecek event asset'i")]
    [SerializeField] private GameEvent gameEvent;

    [Tooltip("Event gelince çalışacak fonksiyonları buraya bağla")]
    [SerializeField] private UnityEvent response;

    private void OnEnable()
    {
        gameEvent?.RegisterListener(this);
    }

    private void OnDisable()
    {
        gameEvent?.UnregisterListener(this);
    }

    public void OnEventRaised()
    {
        response?.Invoke();
    }
}