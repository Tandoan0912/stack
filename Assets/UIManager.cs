using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Start Game Menu")]
    public GameObject startMenu;
    [Header("Ingame")]
    public GameObject ingameUI;
    public TMP_Text playerScore;
    public TMP_Text record;
    [Header("End Game Menu")]
    public GameObject endgameMenu;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void StartGame()
    {
        startMenu.SetActive(false);
        ingameUI.SetActive(true);
        int temp = PlayerPrefs.GetInt("record", 0);
        record.text = "Record: " + temp.ToString();
        Plank.instance.end = false;
    }
    public void EndGame()
    {
        ingameUI.SetActive(false);
        endgameMenu.SetActive(true);
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ChangeScore(int value)
    {
        playerScore.SetText(value +"");
    }

    public void HighestScore(int score)
    {
        if(score > PlayerPrefs.GetInt("record", 0))
        {
            PlayerPrefs.SetInt("record", score);
            record.text = "Record: " + score.ToString();
        }
    }
}
