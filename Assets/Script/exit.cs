using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class exit : MonoBehaviour {

	public void exitApp()
    {
        Application.Quit();
        Debug.Log("OK");
    }

    public void playgame()
    {
        SceneManager.LoadScene("main");
    }
    public void mainmenu()
    {
        SceneManager.LoadScene("menu");
    }
}
