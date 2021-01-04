using System;
using System.IO;
using System.Net.Sockets;
using UnityEngine;

public class Client : MonoBehaviour
{
    private bool socketReady;
    private TcpClient socket;
    private NetworkStream stream;
    private StreamWriter writer;
    private StreamReader reader;

    private void Update()
    {
        if (socketReady && stream.DataAvailable)
        {
            string data = reader.ReadLine();
            if (data != null)
                OnIncomingData(data);
        }
    }

    public bool ConnectToServer(string host, int port)
    {
        if (socketReady)
            return false;

        try
        {
            socket = new TcpClient(host, port);
            stream = socket.GetStream();
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream);

            socketReady = true;
        }
        catch (Exception e)
        {
            Debug.Log("Socket error " + e.Message);
            return false;
        }

        return socketReady;
    }

    public void Send(string data)
    {
        if (!socketReady)
            return;

        writer.WriteLine(data);
        writer.Flush();
    }

    private void OnIncomingData(string data)
    {
        Debug.Log(data);
    }

    private void OnApplicationQuit()
    {
        CloseSocket();
    }

    private void OnDisable()
    {
        CloseSocket();
    }

    private void CloseSocket()
    {
        if (socketReady)
            return;

        writer.Close();
        reader.Close();
        socket.Close();
        socketReady = false;
    }
}

public class GameClient
{
    public string name;
    public bool isHost;
}
