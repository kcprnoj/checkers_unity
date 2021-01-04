using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public void PlaySingleplayerGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ChooseBlackSide()
    {
        UIData.Color = "black";
    }

    public void ChooseWhiteSide()
    {
        UIData.Color = "white";
    }

    public void ConnectToHost()
    {

    }

    public void CreateMultiPlayerGame()
    {

    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
