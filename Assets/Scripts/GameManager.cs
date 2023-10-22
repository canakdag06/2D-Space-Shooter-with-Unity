using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool _isGameOver;
    [SerializeField] private GameObject pauseMenu;

    private void Start()
    {
        Time.timeScale = 1f;
    }
    private void Update()
    {
        if (_isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.R) && _isGameOver)
            {
                SceneManager.LoadScene(1); // Scene: Game
            }
        }
        else
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
            }
        }


    }

    public void GameOver()
    {
        _isGameOver = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Main_Menu");
    }

}
