using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public GameObject startMenu;
    public GameObject creditMenu;
    public GameObject fadedSceen;

    public void StartTheGame()
    {
        StartCoroutine(LoadSceneCo());
    }

    public void OpenCredit()
    {
        startMenu.SetActive(false);
        creditMenu.SetActive(true);
    }
    public void BackToStartMenu()
    {
        startMenu.SetActive(true);
        creditMenu.SetActive(false);
    }

    private IEnumerator LoadSceneCo()
    {
        fadedSceen.SetActive(true);
        yield return new WaitForSeconds(0.75f);
        SceneManager.LoadScene("Level 1");
    }
}
