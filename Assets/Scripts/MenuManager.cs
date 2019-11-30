using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void ClickedNewGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ClickedExit()
    {
        Application.Quit();
    }
}
