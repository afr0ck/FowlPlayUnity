using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string levelOne;
    public GameObject howToScreen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startGame()
    {
        SceneManager.LoadScene(levelOne);
    }

    public void openHowTo()
    {
        howToScreen.SetActive(true);
    }

    public void closeHowTo()
    {
        howToScreen.SetActive(false);
    }

    public void exitGame()
    {
        Application.Quit();
    }
}
