using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public void PlaySingleplayerGameWhiteSide()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        UIData.Color = "white";
    }

    public void PlaySingleplayerGameBlackSide()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        UIData.Color = "black";
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
