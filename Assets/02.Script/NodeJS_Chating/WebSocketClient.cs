using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
using WebSocket = WebSocketSharp.WebSocket;

public class WebSocketClient : MonoBehaviour
{
    [Header("Websocket")]
    public WebSocket ws;
    [SerializeField] private InputField InputField_Text;

    [Header("MalPongsun")]
    public GameObject chatContent; // Content ������Ʈ
    public GameObject chatMessagePrefab; // ä�� �޽��� ������

    private Queue<Action> mainThreadQueue = new Queue<Action>();

    void Start()
    {
        // WebSocket ������ ����
        ws = new WebSocket("ws://3.38.178.218:8080");

        // �����κ��� �޽����� ���� �� ȣ��Ǵ� �̺�Ʈ �ڵ鷯 ����
        ws.OnMessage += (sender, e) =>
        {
            if (e.IsBinary)
            {
                // ����Ʈ �����͸� UTF-8 ���ڿ��� ��ȯ
                string message = System.Text.Encoding.UTF8.GetString(e.RawData);
                Debug.Log("Received from server (binary): " + message);
                //AddChatMessage(message);

                // AddChatMessage ȣ���� ���� �����忡�� �����ϵ��� ť�� �߰�
                mainThreadQueue.Enqueue(() => AddChatMessage(message));
            }
            else if (e.IsText)
            {
                // �ؽ�Ʈ �޽����� ��� (���� Data�� �����)
                Debug.Log("Received from server (text): " + e.Data);
            }
            else
            {
                Debug.Log("Received from server: Unknown data format.");
            }
        };

        // ������ ����
        ws.Connect();

        // ���� ���� ���
        Debug.Log("WebSocket State: " + ws.ReadyState.ToString());

        // �׽�Ʈ �޽��� ����
        ws.Send("Hello, Server!");
    }

    void LateUpdate()
    {
        // ť�� ����� �۾��� ���� �����忡�� ó��
        while (mainThreadQueue.Count > 0)
        {
            var action = mainThreadQueue.Dequeue();
            action?.Invoke();
        }
    }

    void OnApplicationQuit()
    {
        // ���� ���α׷� ���� �� WebSocket ���� �ݱ�
        if (ws != null)
        {
            ws.Close();
        }
    }

    public void SendMessageToServer(string message)
    {
        // ������ �޽��� ����
        if (ws != null && ws.ReadyState == WebSocketSharp.WebSocketState.Open)
        {
            ws.Send(message);
        }
    }



    public void SendMessage()
    {
        string mess = InputField_Text.text;

        if (false == mess.IsNullOrEmpty())
        {
            SendMessageToServer(mess);
            //Debug.Log(mess);
            InputField_Text.text = "";
        }
        
    }

    public void AddChatMessage(string message)
    {
        // ������ �ν��Ͻ� ����
        GameObject newMessage = Instantiate(chatMessagePrefab, chatContent.transform);

        // �޽��� �ؽ�Ʈ ����
        Text messageText = newMessage.GetComponentInChildren<Text>();
        messageText.text = message;

        // �޽��� �߰� �� ��ũ���� �Ʒ��� �̵�
        Canvas.ForceUpdateCanvases();
        ScrollRect scrollRect = chatContent.GetComponentInParent<ScrollRect>();
        scrollRect.verticalNormalizedPosition = 0f;
    }

}
