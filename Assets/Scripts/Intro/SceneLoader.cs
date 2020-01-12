using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public AK.Wwise.Event stopMusicMenu;
    public AK.Wwise.Event startMusicGame;

    public GameObject wwiseObj;

    

    public void LoadIntro1Scene()
    {
        SceneManager.LoadScene("Intro1");
    }
    public void LoadIntro2Scene()
    {
        SceneManager.LoadScene("Intro2");
    }
    public void LoadIntro3Scene()
    {
        SceneManager.LoadScene("Intro3");
    }
        public void LoadIntro4Scene()
    {
        SceneManager.LoadScene("Intro4");
    }
    public void LoadMainSlowScene()
    {
        stopMusicMenu.Post(wwiseObj);
        startMusicGame.Post(wwiseObj);
        SceneManager.LoadScene("FINAL ASSEMBLY SCENE");
    }
    public void LoadMainSpringyScene() 
    {
        stopMusicMenu.Post(wwiseObj);
        startMusicGame.Post(wwiseObj);
        SceneManager.LoadScene("FINAL Free Touch 0");
    }
    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void LoadHowToPlay()
    {
        SceneManager.LoadScene("HowToPlay");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void LoadCredits()
    {
        SceneManager.LoadScene("Credits");
    }
}
