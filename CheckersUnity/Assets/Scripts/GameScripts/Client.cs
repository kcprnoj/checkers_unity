using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Client : MonoBehaviour
{
    public string ClientName;
    public bool isWhite;
    public Vector2 EnemyMove = new Vector2(-1.0f, -1.0f);

    private bool socketReady;
    private TcpClient socket;
    private NetworkStream stream;
    private StreamWriter writer;
    private StreamReader reader;

    private List<GameClient> players = new List<GameClient>();

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        EnemyMove = new Vector2(-1.0f, -1.0f);
    }

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
        catch (SocketException e)
        {
            Debug.Log(e.Message);
            return false;
        }

        return socketReady;
    }

    public void Send(string data)
    {
        if (!socketReady)
            return;

        writer.WriteLine(data);
        try
        {
            writer.Flush();
        }
        catch (SocketException sc)
        {
            Debug.Log(sc.Message);
            SceneManager.LoadScene(0);
            Destroy(gameObject);
        }
    }

    private void OnIncomingData(string data)
    {
        string[] serverData = data.Split(':');
        Debug.Log("Client : " + data);

        switch (serverData[0])
        {
            case "SW":
                for (int i = 1; i < serverData.Length -1; i++)
                {
                    UserConnected(serverData[i], false);
                    if (serverData[i] != ClientName)
                        UIData.EnemyName = serverData[i];
                }
                Send("CW:" + ClientName + ":" + ((UIData.Color == "white")?1:0));
                break;

            case "SNC":
                UserConnected(serverData[1], false);
                break;

            case "SMOVE":
                string color = (serverData[3] == "0") ? "black" : "white";
                if (UIData.Color != color)
                    SetEnemyMove(Int32.Parse(serverData[1]), Int32.Parse(serverData[2]));
                break;

            case "SCC":
                if (serverData[1] == "black" && !UIData.IsHost)
                    UIData.Color = "white";
                else if (!UIData.IsHost)
                    UIData.Color = "black";
                break;

            case "SDEL":
                for (int i = 0; i < players.Count; i++)
                {
                    if (players[i].name == serverData[1])
                    {
                        players.RemoveAt(i);
                        CheckersBoard b = FindObjectOfType<CheckersBoard>();
                        if (UIData.Color == "black")
                            b.WhiteLeft = 0;
                        else
                            b.BlackLeft = 0;
                        b.CheckWinner();
                        break;
                    }
                }
                break;

            case "SNEW":
                if (serverData[1] != UIData.Color)
                    UIData.NewGameEnemy = true;
                break;
            case "SDOWN":
                SceneManager.LoadScene(0);
                Destroy(gameObject);
                break;
            case "SMSG":
                FindObjectOfType<CheckersBoard>().ChatMessage(serverData[1] + " : " + serverData[2]);
                break;
        }

    }

    private void SetEnemyMove(int x, int y)
    {
        Debug.Log("Enemy Move : " + EnemyMove);
        EnemyMove.x = (float)x;
        EnemyMove.y = (float)y;
    }

    private void UserConnected(string name, bool isWhite)
    {
        GameClient client = new GameClient();
        client.name = name;

        players.Add(client);
        Debug.Log("Connected " + client.name);

        if (players.Count == 2)
            StartGame();
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

    private void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        UIData.GameMode = "multi";
        UIData.StartTime = UIData.GetUnixTime();
    }
}

public class GameClient
{
    public string name;
    public bool isHost;
}
