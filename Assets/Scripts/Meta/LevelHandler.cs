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

    if (scene < SceneManager.sceneCount - 1) SceneManager.LoadScene(scene + 1);
    SceneManager.LoadScene(0);
  }

  public void QuitGame() { Application.Quit(); }
}
