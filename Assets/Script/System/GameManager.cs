using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int PlayerHP = 3;

    [Header("Color")]
    public Gradient fireBall;
    public Gradient waterBall;
    public Gradient plantBall;
    public Gradient purpleBall;

    [Header("Skill Slot")]
    public GameObject fireSlot;
    public GameObject waterSlot;
    public GameObject naturalSlot;

    [Header("Unlock Skill")]
    public bool unlockFire;
    public bool unlockWater;
    public bool unlockNatural;

    [Header("Script")]
    public TextEvent textEvent;
    public TextEvent textEventTop;
    public ImageEvent imageEvent;

    [Header("CheckPoint")]
    public CheckPoint checkPoint;

    [Header("Panel")]
    public Image bossSceneEffect;
    public GameObject fadedPanel;
    public GameObject pauseMenu;
    public GameObject winPanel;

    [Header("Component")]
    public Animator anim;


    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        anim = fadedPanel.GetComponent<Animator>();
        if(unlockFire && unlockNatural && unlockWater)
        {
            fireSlot.SetActive(true);
            waterSlot.SetActive(true);
            naturalSlot.SetActive(true);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1)
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                pauseMenu.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }

    public void unlockFireBall()
    {
        unlockFire = true;
        fireSlot.SetActive(true);
        imageEvent.SetImageEvent(Element.fire);
    }
    public void unlockWaterBall()
    {
        unlockWater = true;
        waterSlot.SetActive(true);
        imageEvent.SetImageEvent(Element.water);
    }
    public void unlockNaturalBall()
    {
        unlockNatural = true;
        naturalSlot.SetActive(true);
        imageEvent.SetImageEvent(Element.plant);
    }

    public void SetCheckPoint(CheckPoint point)
    {
        textEvent.SetTextEvent("Checkpoint");
        if(checkPoint != null)
            checkPoint.CompleteCheckpoint();

        checkPoint = point;

        Player player = GameObject.Find("Player").GetComponent<Player>();
        player.ResetHP();
    }
    public void SetTextEvent(string value, bool up = false)
    {
        if (up == false)
            textEvent.SetTextEvent(value);
        else
            textEventTop.SetTextEvent(value);
    }

    public void SetSceneColor(Element element)
    {
        if(element == Element.fire)
        {
            bossSceneEffect.color = new Color(1, 0, 0, 0.1f);
        }
        if(element == Element.water)
        {
            bossSceneEffect.color = new Color(0, 1, 1, 0.1f);
        }
        if(element == Element.plant)
        {
            bossSceneEffect.color = new Color(0, 1, 0, 0.1f);
        }
    }

    public void WinTheGame()
    {
        StartCoroutine(WinTheGameCo());
    }
    private IEnumerator WinTheGameCo()
    {
        yield return new WaitForSeconds(1f);
        fadedPanel.SetActive(true);
        yield return new WaitForSeconds(0.75f);
        winPanel.SetActive(true);
    }

    //Player
    public void PlayerIsDeath(GameObject player)
    {
        StartCoroutine(PlayerIsDeathCO(player));
    }
    public IEnumerator PlayerIsDeathCO(GameObject player)
    {
        fadedPanel.SetActive(true);
        player.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        DestroyAllBullet();
        anim.SetTrigger("fade out");
        yield return new WaitForSeconds(0.25f);
        checkPoint.ResetEnemy();
        player.transform.position = checkPoint.transform.position;
        player.SetActive(true);
        player.GetComponent<Player>().ResetHP();
        yield return new WaitForSeconds(0.25f);
        fadedPanel.SetActive(false);
    }
    private void DestroyAllBullet()
    {
        GameObject[] bullet = GameObject.FindGameObjectsWithTag("Enemy Bullet");

        for (int i = 0; i < bullet.Length; i++)
        {
            Destroy(bullet[i]);
        }

        GameObject[] bullet1 = GameObject.FindGameObjectsWithTag("Player Bullet");

        for (int i = 0; i < bullet1.Length; i++)
        {
            Destroy(bullet1[i]);
        }
    }

    //Button
    public void PauseButton()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }
}
