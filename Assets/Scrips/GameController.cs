using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public Player player;
    public GameUI inGameUI;

    //Property som håller reda på om spelet nyss har startats.
    public static bool Startup { get; set; } = true;

    //Property som används för att pausa spelet. Sätter timescale till 0 vid pause vilket betyder att den inte ska uppdatera spelfysiken när det är pausat.
    public static bool Paused
    {
        get
        {
            return paused;
        }
        set
        {
            if (value)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }

            paused = value;
        }
    }

    //Håller i ID:t på den aktiva banan. Kan bara sättas till ett värde mellan 0 och 4.
    public static int LevelId
    {
        get
        {
            return levelId;
        }
        set
        {
            if (value >= 0 && value < 5)
            {
                levelId = value;
            }
        }
    }

    //Property som sparar spelarens senaste namn de skrivit på en highscore lista.
    public static string PlayerName
    {
        get
        {
            return playerName;
        }
        set
        {
            if (value != null && value.Length > 10)
            {
                playerName = value;
            }
        }
    }

    //Privata variabler som håller värdena för några av de properties åvan.
    private static bool paused = false;
    private static int levelId = 0;
    private static string playerName = "";

    //Listor med alla namn och poäng för varje highscore lista. Sparas ej till fil.
    private static List<float>[] highscoreTimeLists = new List<float>[] { new List<float>(), new List<float>(), new List<float>(), new List<float>(), new List<float>() };
    private static List<string>[] highscoreNameLists = new List<string>[] { new List<string>(), new List<string>(), new List<string>(), new List<string>(), new List<string>() };

    //Sätter paused till false varje gång en bana startar.
    void Start()
    {
        Paused = false;
    }

    //Checkar spelarens status.
    void Update()
    {
        if (!Paused)
        {
            if (player.IsDead()) //Om spelaren är död starta om banan.
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else if (player.HasWon()) //Om spelaren har kommit i mål visa highscore.
            {
                Paused = true;
                inGameUI.HandleWon();
            }
            else
            {
                GameObject bonus = player.CheckBonusCollision(); //Checkar om spelaren har kolliderat med en bonus.

                if (bonus != null) //Om spelaren kolliderat ta bort 10 sekunder från timern och radera bonusen från banan.
                {
                    inGameUI.ChangeTime(-10.0F);
                    Destroy(bonus);
                }
            }
        }
    }

    //Den gör om sekunder (timern) till minuter och sekunder i textform.
    public static string GetTimeText(float timer)
    {
        int time = (int)timer;

        int seconds = time % 60; //Plockar ut alla sekunder med hjälp av rest.
        int minutes = time / 60; //Plockar ut alla minter med hjälp av int division (tar bort resten och ger bara heltal).

        string text = minutes + ":";

        if (seconds < 10) //Den lägger till en nolla om sekunderna är mindre än 10 så att timern aldrig får formatet 0:1 utan 0:01.
        {
            text += "0";
        }

        text += seconds;
        return text;
    }

    //Kollar om man har det som krävs för att komma in på highscorelistan.
    public static bool IsOnHighscoreTopFive(int levelId, float time)
    {
        if (highscoreTimeLists[levelId].Count < 5) //Om det finns mindre än 5 spelare i listan kommar man alltid med i top 5.
        {
            return true;
        }

        //Hittar platsen man kommer på i listan.
        for (int i = 0; i < highscoreTimeLists[levelId].Count; i++)
        {
            if (time < highscoreTimeLists[levelId][i]) //Sant om man är bättre än spelaren på plats i i listan.
            {
                if (i < 5) //Man kan bara komma in på highscorelistan om man är i top 5.
                {
                    return true;
                }
            }
        }

        return false;
    }

    //Lägger till spelare på korrekt plats i highscorelistan så att den alltid är sorterad.
    public static void AddHighscore(int levelId, float time, string name)
    {
        for (int i = 0; i < highscoreTimeLists[levelId].Count; i++)
        {
            if (time < highscoreTimeLists[levelId][i])
            {
                //Lägger in spelaren på korrekt plats.
                highscoreTimeLists[levelId].Insert(i, time);
                highscoreNameLists[levelId].Insert(i, name);
                return;
            }
        }

        //Om man kommer sist i listan körs dessa två rader.
        highscoreTimeLists[levelId].Add(time);
        highscoreNameLists[levelId].Add(name);
    }

    //Ger formaterad text på higscorelistan från en specifik bana.
    public static string GetHighscoreList(int levelId)
    {
        List<float> times = highscoreTimeLists[levelId];
        List<string> names = highscoreNameLists[levelId];
        string text = "";

        for (int i = 0; i < times.Count; i++)
        {
            if (i > 4)
            {
                break; //Skriv ut bara de 5 bästa tiderna/spelarna.
            }

            text += (i + 1) + " | " + names[i] + " - " + GetTimeText(times[i]); //Formaterar varje rad i listan enligt: "PLATS | NAMN - TID". Ex: "1 | Jakob - 0:14".

            if (i < times.Count - 1)
            {
                text += "\n"; //Lägger till en newline för alla rader förutom den sista.
            }
        }

        return text;
    }
}
