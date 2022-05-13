using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI diamondText;
    public int curDiamonds;
    public Image fadeToBlack;
    public float fadeSpeed;
    public float fadeWait;
    public GameObject instructions;
    public GameObject pause;

    public TextMeshProUGUI instructions1;
    public TextMeshProUGUI instructions2;

    private readonly int maxDiamonds = 10;
    private bool gameStart;
    private bool isPaused;

    // Start is called before the first frame update
    void Start()
    {
        gameStart = true;
        instructions.SetActive(true);
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStart)
        {
            StartCoroutine("FadeInstructions");
        }

        if (curDiamonds == maxDiamonds)
        {
            fadeToBlack.color = new Color(fadeToBlack.color.r, fadeToBlack.color.g, fadeToBlack.color.b, Mathf.MoveTowards(fadeToBlack.color.a, 1f, fadeSpeed * Time.deltaTime));

            StartCoroutine("FadeToMenu");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu();
        }
    }

    public void addDiamond(int diamondToAdd)
    {
        curDiamonds += diamondToAdd;
        diamondText.text = "Diamonds: " + curDiamonds + " / " + maxDiamonds;
    }

    public IEnumerator FadeToMenu()
    {
        yield return new WaitForSeconds(fadeWait);

        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("EndScene");
    }

    public IEnumerator FadeInstructions()
    {
        instructions1.color = new Color(instructions1.color.r, instructions1.color.g, instructions1.color.b, Mathf.MoveTowards(instructions1.color.a, 0f, .1f * Time.deltaTime));
        instructions2.color = new Color(instructions2.color.r, instructions2.color.g, instructions2.color.b, Mathf.MoveTowards(instructions2.color.a, 0f, .1f * Time.deltaTime));

        yield return new WaitForSeconds(10);

        instructions.SetActive(false);
        gameStart = false;
    }

    public void pauseMenu()
    {
        if (isPaused)
        {
            pause.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            isPaused = false;
        } else
        {
            pause.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            isPaused = true;
        }
    }

    public void goMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void exitGame()
    {
        Application.Quit();
    }
}
