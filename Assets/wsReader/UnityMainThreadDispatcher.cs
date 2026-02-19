using System;
using System.Collections.Concurrent;
using UnityEngine;

/// <summary>
/// Диспетчер для выполнения действий в главном потоке Unity
/// Размещайте на любом активном GameObject в сцене
/// </summary>
public class UnityMainThreadDispatcher : MonoBehaviour
{
    private static UnityMainThreadDispatcher _instance;
    private readonly ConcurrentQueue<Action> _executionQueue = new();

    public static UnityMainThreadDispatcher Instance()
    {
        if (_instance == null)
        {
            GameObject go = new GameObject(nameof(UnityMainThreadDispatcher));
            DontDestroyOnLoad(go);
            _instance = go.AddComponent<UnityMainThreadDispatcher>();
        }
        return _instance;
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Update()
    {
        // Выполняем все отложенные действия
        while (_executionQueue.TryDequeue(out var action))
        {
            action?.Invoke();
        }
    }

    /// <summary>
    /// Добавить действие в очередь выполнения в главном потоке
    /// </summary>
    public void Enqueue(Action action)
    {
        if (action == null) return;
        _executionQueue.Enqueue(action);
    }

    private void OnDestroy()
    {
        if (_instance == this) _instance = null;
    }
}