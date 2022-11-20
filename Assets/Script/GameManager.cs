using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] PlayerController player;
    [SerializeField] TMP_Text gameOverText;
    [SerializeField] Hole hole;
    private void Start()
    {
        gameOverPanel.SetActive(false);
    }
    private void Update()
    {
        if (hole.Entered && gameOverPanel.activeInHierarchy == false)
        {
            gameOverPanel.SetActive(true);
            gameOverText.text = "Masoook! Hanya " + player.ShootCount + " Tembakan";
        }
    }

    public void BackToMainMenu()
    {
        SceneLoader.Load("Main Menu 1");
    }

    public void Replay()
    {
        SceneLoader.ReloadLevel();
    }

    public void PlayNext()
    {
        SceneLoader.LoadNextLevel();
    }

}
