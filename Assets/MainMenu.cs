using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // handle difficulty buttons
    public void EasyMode()
    {
       SceneManager.LoadScene("Scenes/Easy_Mode"); 
    }

    public void MediumMode()
    {
       SceneManager.LoadScene("Scenes/Medium_Mode"); 
    }

    public void HardMode()
    {
       SceneManager.LoadScene("Scenes/Hard_Mode"); 
    }

    public void ExitGame()
    {
      Debug.Log("Game is exiting");
      Application.Quit();
    }
}
