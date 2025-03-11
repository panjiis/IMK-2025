using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{
  public void playGame()
    {
        SceneManager.LoadSceneAsync(0);
    }

  public void ExitGame()
    {
        Application.Quit();
    }
}
