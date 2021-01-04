using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Server : MonoBehaviour
{
    public int port = 6321;

    private List<ServerClient> serverClients;
    private List<ServerClient> disconnectClients;

    private TcpListener server;
    private bool started;

    public bool Init()
    {
        DontDestroyOnLoad(gameObject);
        serverClients = new List<ServerClient>();
        disconnectClients = new List<ServerClient>();

        try
        {
            server = new TcpListener(IPAddress.Any, port);
            server.Start();
            StartListening();
            started = true;
        }
        catch (Exception e)
        {
            Debug.Log("Socekt error : " + e.Message);
            return false;
        }

        return true;
    }

    private void Update()
    {
        if (!started)
            return;
        foreach (ServerClient serverClient in serverClients)
        {
            if (!IsConnected(serverClient.Client))
            {
                serverClient.Client.Close();
                disconnectClients.Add(serverClient);
                continue;
            }
            else
            {
                NetworkStream stream = serverClient.Client.GetStream();
                if (stream.DataAvailable)
                {
                    StreamReader reader = new StreamReader(stream);
                    string data = reader.ReadLine();

                    if (data != null)
                        OnIncomingData(serverClient, data);
                }
            }
        }

        for (int i = disconnectClients.Count - 1; i >= 0; i--)
        {
            serverClients.Remove(disconnectClients[i]);
            disconnectClients.RemoveAt(i);
        }
    }

    private void StartListening()
    {
        server.BeginAcceptTcpClient(AcceptTcpClient, server);
    }

    private void AcceptTcpClient(IAsyncResult result)
    {
        TcpListener listener = (TcpListener)result.AsyncState;

        ServerClient client = new ServerClient(listener.EndAcceptTcpClient(result));
        serverClients.Add(client);

        StartListening();

        Debug.Log("Connected");
    }

    private bool IsConnected(TcpClient client)
    {
        try
        {
            if (client != null && client.Client != null && client.Client.Connected)
            {
                if (client.Client.Poll(0, SelectMode.SelectRead))
                    return !(client.Client.Receive(new byte[1], SocketFlags.Peek) == 0);

                return true;
            }
            else
                return false;
        }
        catch
        {
            return false;
        }
    }

    private void Broacast(string data, List<ServerClient> clients)
    {
        foreach (ServerClient client in clients)
        {
            try
            {
                StreamWriter writer = new StreamWriter(client.Client.GetStream());
                writer.WriteLine(data);
                writer.Flush();
            }
            catch (Exception e)
            {
                Debug.Log("Write error : " + e.Message);
            }
        }
    }

    private void OnIncomingData(ServerClient c, string data)
    {
        Debug.Log(c.clientName + " : " + data);
    }
}

public class ServerClient
{
    public string clientName;
    public TcpClient Client;

    public ServerClient(TcpClient client)
    {
        Client = client;
    }
}
