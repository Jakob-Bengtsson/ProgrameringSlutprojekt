using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Text levelText; //Text fär levelnumret.
    public Text levelText2; //Bakgrundstext fär levelnumret.
    public Text levelNameText; //Text fär levelnamnet.
    public Text levelNameText2; //Bakgrundstext fär levelnamnet.
    public Text highscoreText; //Highscorelistans text.

    public Transform backgroundParent; //"Föräldern" till alla bagrunder.
    public Transform starParent; //"Föräldern" till alla stjärnorna i levelselect menyn (med mera).

    //Namnen på alla banor i spelet.
    private string[] levelNames =
    {
        "Grasslands",
        "Rocky Hills",
        "Stone Hedge",
        "Tricky Tower",
        "Hell cave"
    };

    //Antalet stjärnor på varje bana som visar svårighetsgraden.
    private int[] levelStars =
    {
        1, 1, 2, 4, 5
    };

    //Denna metoden körs varje gång när du kommer in startmenyn eller levelselect.
    void Start()
    {
        if (GameController.Startup) //Om du startar spelet för första gången så dyker startmenyn upp.
        {
            GameController.Startup = false;
            ActivateMenu("MainMenu");
        }
        else //Annnars så dyker levelselect upp.
        {
            SetupLevelSelect();
        }
    }

    //Aktiverar korrekt antal stjärnor i levelselect beroende på vilken bana det är.
    private void ActivateStars()
    {
        //Går igenom alla GameObject som är "barn" till levelselect-menyn.
        for (int i = 0; i < starParent.childCount; i++)
        {
            GameObject child = starParent.GetChild(i).gameObject;

            if (child.name.StartsWith("Star")) //Om namnet på objektet börjar med "Star" aktivera eller avaktivera det beroende på hur svår banan är.
            {
                if (int.Parse(child.name.Substring(4)) < levelStars[GameController.LevelId]) //Checkar om bana är tillräckligt svår för att visa denna stjärnan.
                {
                    child.SetActive(true);
                }
                else
                {
                    child.SetActive(false);
                }
            }
        }
    }

    //Används för att aktivera korrekt meny.
    public void ActivateMenu(string name)
    {
        //Går igenom alla GameObject som är menyerna.
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;

            if (child.name == name) //Akteverar menyn beserat på om den har ett visst namn.
            {
                child.SetActive(true);
            }
            else
            {
                child.SetActive(false);
            }
        }
    }

    //Använd för att aktivera rätt menybakgrund.
    public void ActivateBackground(string name)
    {
        //Går igenom alla GameObject som är menyernas bakgrunder.
        for (int i = 0; i < backgroundParent.childCount; i++)
        {
            GameObject child = backgroundParent.GetChild(i).gameObject;

            if (child.name == name) //Akteverar bakgrunden beserat på om den har ett visst namn.
            {
                child.SetActive(true);
            }
            else
            {
                child.SetActive(false);
            }
        }
    }

    //Visar namnet och numret på banan som är vald.
    private void SetLevelText()
    {
        //Sätter levelnumret.
        levelText.text = "Level " + (GameController.LevelId + 1);
        levelText2.text = levelText.text;

        //Sätter levelnamnet.
        levelNameText.text = levelNames[GameController.LevelId];
        levelNameText2.text = levelNameText.text;
    }

    //Byter mellan banorna, används av fram och tillbaka knapparna i levelselectmenyn.
    public void ChangeLevel(bool next)
    {
        int temp = GameController.LevelId;

        if (next)
        {
            temp = GameController.LevelId + 1;
        }
        else
        {
            temp = GameController.LevelId - 1;
        }

        //Checkar så inte det blir ett negativt nummer eller större en 4 eftersom vi bara har 5 banor.
        if (temp < 0)
        {
            temp = 4;
        }
        else if (temp > 4)
        {
            temp = 0;
        }

        GameController.LevelId = temp;
    }

    //Uppdaterar alla element i levelselect är med korrekta värden/text.
    public void SetupLevelSelect()
    {
        ActivateBackground("Level" + (GameController.LevelId + 1));
        ActivateStars();
        SetLevelText();
    }

    //Metod för att starta den banan som spelaren har valt.
    public void StartLevel()
    {
        SceneManager.LoadScene("Level" + (GameController.LevelId + 1));
    }

    //Metod som stänger av spelet.
    public void ExitGame()
    {
        Application.Quit();
    }

    //Metod för att uppdaterar highscorlistan så att den visar listan för rätt bana.
    public void UpdateHighscoreText()
    {
        highscoreText.text = GameController.GetHighscoreList(GameController.LevelId);
    }
}
