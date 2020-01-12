using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
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
        SceneManager.LoadScene("FINAL ASSEMBLY SCENE");
    }
    public void LoadMainSpringyScene()
    {
        SceneManager.LoadScene("FINAL Free Touch 0");
    }

}
