using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void Load()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void CreditScene()
    {
        SceneManager.LoadScene("CreditScene");

    }

    public void RulesScene()
    {
        SceneManager.LoadScene("RulesScene");

    }
    public void IntroScene()
    {
        SceneManager.LoadScene("IntroScene");

    }
    public void Play()
    {
        SceneManager.LoadScene("GameScene");

    }
    public void WinScene()
    {
        SceneManager.LoadScene("WinScene");

    }
    public void LoseScene()
    {
        SceneManager.LoadScene("LoseScene");

    }
}
