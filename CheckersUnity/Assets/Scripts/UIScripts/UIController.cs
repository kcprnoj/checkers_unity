using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class UIController : MonoBehaviour
{
    public InputField nameInput;

    public GameObject ServerPrefab;
    public GameObject ClientPrefab;

    public void PlaySingleplayerGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        UIData.GameMode = "single";
        UIData.StartTime = UIData.GetUnixTime();
    }

    public void ChooseBlackSide()
    {
        UIData.Color = "black";
    }

    public void ChooseWhiteSide()
    {
        UIData.Color = "white";
    }

    public void ConfirmNameEnter()
    {
        UIData.Name = nameInput.text.ToString();
    }

    public void ConnectToHost()
    {
        string hostAddress = GameObject.Find("HostInput").GetComponent<InputField>().text;
        string clientName = GameObject.Find("InputNameClient").GetComponent<InputField>().text;

        if (hostAddress == "")
            hostAddress = "127.0.0.1";

        if (clientName == "")
            clientName = "Guest";

        UIData.Name = clientName;

        try
        {
            Client client = Instantiate(ClientPrefab).GetComponent<Client>();
            client.ClientName = clientName;
            client.ConnectToServer(hostAddress, 6321);
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
            return;
        }
    }

    public void CreateMultiPlayerGame()
    {
        string hostName = GameObject.Find("InputNameHost").GetComponent<InputField>().text;

        if (hostName == "")
            hostName = "Host";

        UIData.Name = hostName;

        try
        {
            Server server = Instantiate(ServerPrefab).GetComponent<Server>();
            server.Init();
            Client client = Instantiate(ClientPrefab).GetComponent<Client>();
            client.ClientName = hostName;
            client.ConnectToServer("127.0.0.1", 6321);
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
            return;
        }
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }

    private void GoToMultiplayerGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        UIData.GameMode = "multi";
        UIData.StartTime = UIData.GetUnixTime();
    }

    public void BackMultiplayerButton()
    {
        Server server = FindObjectOfType<Server>();
        if (server != null)
            Destroy(server.gameObject);

        Client client = FindObjectOfType<Client>();
        if (client != null)
            Destroy(client.gameObject);
    }
}
