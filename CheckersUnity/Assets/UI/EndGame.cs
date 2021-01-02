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

    void Start()
    {
        winner = UIData.Winner;
        color = UIData.Color;
    }
    private void Update()
    {
        if (color == "black")
        {
            if (winner == "black")
            {
                winMenu.SetActive(true);
                blackWinBackground.SetActive(true);
            }
            else
            {
                loseMenu.SetActive(true);
                blackLoseBackground.SetActive(true);
            }
        }
        else if (color == "white")
        {
            if (winner == "white")
            {
                winMenu.SetActive(true);
                whiteWinBackground.SetActive(true);
            }
            else
            {
                loseMenu.SetActive(true);
                whiteLoseBackground.SetActive(true);
            }
        }
        else
            return;
    }
        

    public void NewGameButton()
    {
        SceneManager.LoadScene(1);
    }

    public void EndGameButton()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene(0);
    }
}
