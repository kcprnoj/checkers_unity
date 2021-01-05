using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public InputField nameInput;

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
