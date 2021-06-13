using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelHandler : MonoBehaviour
{
  public void StartGame()
  {
    SceneManager.LoadScene(1);
  }

  public void RestartLevel()
  {
    int scene = SceneManager.GetActiveScene().buildIndex;
    SceneManager.LoadScene(scene);
  }

  public void NextLevel()
  {
    int scene = SceneManager.GetActiveScene().buildIndex;

    try
    {
      SceneManager.LoadScene(scene + 1);

    }
    catch (System.Exception)
    {
      SceneManager.LoadScene(0);
    }
  }
}
