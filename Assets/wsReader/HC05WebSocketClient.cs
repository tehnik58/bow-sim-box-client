// Альтернативный скрипт с NativeWebSocket
using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using NativeWebSocket;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

public class HC05WebSocketClient : MonoBehaviour
{
    [Header("Настройки подключения")]
    [SerializeField] private string serverUrl = "ws://localhost:80/ws";
    [SerializeField] private bool autoConnectOnStart = true;

    [Header("События")]
    public UnityEvent<int> OnPosMessageReceived;
    public UnityEvent<float> OnShootMessageReceived;
    public UnityEvent<bool> OnConnectionStatusChanged;
    public UnityEvent<string> OnError;

    private WebSocket _ws;
    private bool _isConnected;

    public bool IsConnected => _isConnected;

    private void Start()
    {
        if (autoConnectOnStart)
            Connect();
    }

    private void Update()
    {
        #if !UNITY_WEBGL || UNITY_EDITOR
        if (_ws != null)
            _ws.DispatchMessageQueue();
        #endif
    }

    public async void Connect()
    {
        try
        {
            _ws = new WebSocket(serverUrl);

            _ws.OnMessage += (bytes) =>
            {
                string message = Encoding.UTF8.GetString(bytes);

                try
                {
                    // Парсинг pos
                    if (message.StartsWith("pos:") && message.EndsWith(";"))
                    {
                        string value = message.Substring(4, message.Length - 5); // Убираем "pos:" и ";"
                        
                        if (short.TryParse(value, out short pos))
                        {
                            OnPosMessageReceived?.Invoke(pos);
                            Debug.Log($"✅ Pos получен: {pos}");
                        }
                        else
                        {
                            Debug.LogWarning($"⚠️ Не удалось распарсить pos из: '{value}'");
                        }
                    }
                    // Парсинг shot
                    else if (message.StartsWith("shot:") && message.EndsWith(";"))
                    {
                        string value = message.Substring(5, message.Length - 6).Replace(".", ",");
                        
                        if (float.TryParse(value, out float shot))
                        {
                            OnShootMessageReceived?.Invoke(shot);
                            Debug.Log($"✅ Shot получен: {shot}");
                        }
                        else
                        {
                            Debug.LogWarning($"⚠️ Не удалось распарсить shot из: '{value}'");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"❌ Ошибка парсинга '{message}': {ex.Message}");
                }
                print(message);
            };

            _ws.OnOpen += () =>
            {
                _isConnected = true;
                Debug.Log("✅ Подключено");
                OnConnectionStatusChanged?.Invoke(true);
            };

            _ws.OnError += (e) =>
            {
                Debug.LogError($"❌ Ошибка: {e}");
                OnError?.Invoke(e);
            };

            _ws.OnClose += (e) =>
            {
                _isConnected = false;
                Debug.Log("🔌 Отключено");
                OnConnectionStatusChanged?.Invoke(false);
            };

            await _ws.Connect();
        }
        catch (Exception ex)
        {
            Debug.LogError($"Ошибка подключения: {ex.Message}");
            OnError?.Invoke(ex.Message);
        }
    }

    public void Disconnect()
    {
        if (_ws != null)
        {
            _ws.Close();
        }
    }

    private void OnDestroy()
    {
        Disconnect();
    }
}