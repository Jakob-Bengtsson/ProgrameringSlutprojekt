using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public Text timerText; //Texten för timern.
    public Text timerText2; //Bakgrundstexten för timern.
    public Text highscoreText; //Highscorelistans text.
    public InputField highscoreNameField; //InputField där man skriver i sitt namn för highscorelistan.

    //Alla GameObject för menyerna.
    public GameObject inGame;
    public GameObject pauseMenu;
    public GameObject highscoreList;
    public GameObject highscoreName;

    private float timer;

    //Sätter timern till noll vid start av banan.
    void Start()
    {
        timer = 0;
    }

    //Uppdaterar timern och sätter texten på timern i UI.
    void Update()
    {
        if (!GameController.Paused)
        {
            timer += Time.deltaTime;

            timerText.text = GameController.GetTimeText(timer);
            timerText2.text = timerText.text;
        }
    }

    //Går tillbaka till level select när man trycker på exit knappen i pausmenyn.
    public void ExitLevel()
    {
        SceneManager.LoadScene("MainMenu");
    }

    //Pausar eller opausar spelet.
    public void SetPaused(bool paused)
    {
        GameController.Paused = paused;
    }

    //Metod för att ta bort eller lägga till tid på timern.
    public void ChangeTime(float amount)
    {
        timer += amount;

        if (timer < 0) //Check så att timern inte blir negativ
        {
            timer = 0;
        }
    }

    //Visar highscorelistan efter en bana.
    public void ShowHighscore()
    {
        highscoreText.text = GameController.GetHighscoreList(GameController.LevelId); //Hämtar den formaterade texten till highscorelistan.

        //Aktiverar korrekt UI.
        inGame.SetActive(false);
        pauseMenu.SetActive(false);
        highscoreList.SetActive(true);
        highscoreName.SetActive(false);
    }

    //Kollar om man kommer med på topplistan och visar korrekt meny.
    public void HandleWon()
    {
        if (GameController.IsOnHighscoreTopFive(GameController.LevelId, timer)) //Är sant om du är med på topplistan.
        {
            ShowHighscoreName();
        }
        else
        {
            ShowHighscore();
        }
    }

    //Lägger till highscoret på listan och visar det sedan för spelaren
    public void AddHighscore()
    {
        if (highscoreNameField.text.Length > 0 && highscoreNameField.text.Length <= 10)
        {
            GameController.AddHighscore(GameController.LevelId, timer, highscoreNameField.text);
            GameController.PlayerName = highscoreNameField.text; //Håller reda på senaste namnet som var i skrivet så man slipper att fylla i det igen.

            ShowHighscore();
        }
    }

    //Visar menyn där du får skriva i ditt namn till highscorelistan.
    public void ShowHighscoreName()
    {
        if (GameController.PlayerName != null) //Om spelaren har fyllt i ett namn tidigare: Sätt texten i InputBox till namnet redan nu så att spelaren kanske inte behöver skriva det igen.
        {
            highscoreNameField.text = GameController.PlayerName;
        }

        //Aktiverar korrekt UI.
        inGame.SetActive(false);
        pauseMenu.SetActive(false);
        highscoreList.SetActive(false);
        highscoreName.SetActive(true);
    }
}
