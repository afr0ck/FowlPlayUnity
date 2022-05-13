using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    // Public variables
    public int maxHealth;
    public int health;
    public float iFrame;
    public float iFlash;
    public float respawnTimer;
    public float fadeSpeed;
    public float fadeWait;
    
    public PlayerController player;
    public Renderer playerRender;
    public GameObject deathEffect;
    public Image fadeToBlack;

    // Private variables
    private float iCount;
    private float iFlashCount;
    private Color colorStore;
    private bool isRespawn;
    private Vector3 respawnPoint;
    private bool isFadedIn;
    private bool isFadedOut;

    // Start is called before the first frame update
    void Start()
    {
        colorStore = playerRender.material.color;
        health = maxHealth;

        respawnPoint = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (iCount > 0)
        {
            iCount -= Time.deltaTime;

            iFlashCount -= Time.deltaTime;
            if (iFlashCount <= 0)
            {
                playerRender.enabled = !playerRender.enabled;
                iFlashCount = iFlash;
            }

            if (iCount <= 0)
            {
                playerRender.enabled = true;
                playerRender.material.color = colorStore;
            }
        }

        if (isFadedOut)
        {
            fadeToBlack.color = new Color(fadeToBlack.color.r, fadeToBlack.color.b, fadeToBlack.color.b, Mathf.MoveTowards(fadeToBlack.color.a, 1f, fadeSpeed * Time.deltaTime));

            if (fadeToBlack.color.a == 1f)
            {
                isFadedOut = false;
            }
        }

        if (!isFadedIn)
        {
            fadeToBlack.color = new Color(fadeToBlack.color.r, fadeToBlack.color.b, fadeToBlack.color.b, Mathf.MoveTowards(fadeToBlack.color.a, 0f, fadeSpeed * Time.deltaTime));

            if (fadeToBlack.color.a == 0f)
            {
                isFadedIn = true;
            }
        }
    }

    public void damage(int damage, Vector3 direction)
    {
        if (iCount <= 0)
        {
            health -= damage;

            if (health <= 0)
            {
                respawn();
            } else
            {
                player.KnockBack(direction);

                iCount = iFrame;
                iFlashCount = iFlash;

                playerRender.enabled = false;
                playerRender.material.color = Color.red;
            }
        }
    }

    public void heal(int healAmt)
    {
        health += healAmt;

        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    public void addMaxHealth(int addHP) => maxHealth += addHP;

    public void respawn()
    {
        if (!isRespawn)
        {
            StartCoroutine("RespawnCo");
        }
    }

    public IEnumerator RespawnCo()
    {
        isRespawn = true;
        player.gameObject.SetActive(false);
        Instantiate(deathEffect, player.transform.position, player.transform.rotation);

        yield return new WaitForSeconds(respawnTimer);

        isFadedOut = true;

        yield return new WaitForSeconds(fadeWait);

        isFadedIn = false;

        isRespawn = false;

        player.transform.position = respawnPoint;
        health = maxHealth;
        player.gameObject.SetActive(true);

        iCount = iFrame;
        iFlashCount = iFlash;

        playerRender.enabled = false;
    }

    public void saveCheckPoint(Vector3 checkPoint) => respawnPoint = checkPoint;
}
