using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void CountinueButton()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

    public void BackToMenuButton()
    {
        SceneManager.LoadScene("Start Scene");
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }
}
