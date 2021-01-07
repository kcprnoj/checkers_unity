using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    public GameObject winMenu;
    public GameObject loseMenu;
    public GameObject whiteWinBackground;
    public GameObject blackWinBackground;
    public GameObject whiteLoseBackground;
    public GameObject blackLoseBackground;

    private string winner;
    private string color;
    private string gameMode;
    private string playerName;
    private int startTime;
    private int endTime;
    private string status;

    void Start()
    {
        this.endTime = UIData.GetUnixTime();
        this.winner = UIData.Winner;
        this.color = UIData.Color;
        this.gameMode = UIData.GameMode;
        this.playerName = UIData.Name;
        this.startTime = UIData.StartTime;
        CheckWinorDefeat();
        if(this.gameMode == "single")
        {
            SaveToDatabase();
        }
    }
    private void Update()
    {
        CheckWinorDefeat();

        if (gameMode == "multi")
        {
            if (UIData.NewGameEnemy && UIData.NewGame)
            {
                SceneManager.LoadScene(1);
            }
        }
    }

    public void NewGameButton()
    {
        if (gameMode == "single")
            SceneManager.LoadScene(1);
        else
        {
            Client client = FindObjectOfType<Client>();
            client.Send("CNEW:" + color);
            UIData.NewGame = true;
        }

    }

    public void EndGameButton()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }

    public void MainMenuButton()
    {
        Server server = FindObjectOfType<Server>();
        if (server != null)
            Destroy(server.gameObject);

        Client client = FindObjectOfType<Client>();
        if (client != null)
            Destroy(client.gameObject);
        SceneManager.LoadScene(0);
    }

    private void SaveToDatabase()
    {
        DatabaseManager dbManager = new DatabaseManager();
        dbManager.InsertScore(this.playerName, this.color, this.status, this.endTime - this.startTime);
    }

    private void CheckWinorDefeat()
    {
        if (this.color == "black")
        {
            if (this.winner == "black")
            {
                winMenu.SetActive(true);
                blackWinBackground.SetActive(true);
                this.status = "Win";
            }
            else
            {
                loseMenu.SetActive(true);
                blackLoseBackground.SetActive(true);
                this.status = "Defeat";
            }
        }
        else if (this.color == "white")
        {
            if (this.winner == "white")
            {
                winMenu.SetActive(true);
                whiteWinBackground.SetActive(true);
                this.status = "Win";
            }
            else
            {
                loseMenu.SetActive(true);
                whiteLoseBackground.SetActive(true);
                this.status = "Defeat";
            }
        }
        else
            return;
    }
}
