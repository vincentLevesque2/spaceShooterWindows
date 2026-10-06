using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using NUnit.Framework;

public class gameManager : MonoBehaviour
{


    //gameOverUI
    [SerializeField] TextMeshProUGUI gameOverScoreUI;
    [SerializeField] TextMeshProUGUI highScoreUI;
    [SerializeField] TextMeshProUGUI gameOverTextUI;
    [SerializeField] Canvas canvas;

    //gameUI
    [SerializeField] TextMeshProUGUI scoreUI;
    int score = 0;

    //boss et core
    GameObject boss;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boss = GameObject.FindGameObjectWithTag("Boss");
        //set la healthBar à son max
        UpdateHealthBar(1);
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //update le score de la partie en augmentant selon la valeur passé en param
    public void UpdateScore(int scoreAugmente)
    {
        score += scoreAugmente;
        scoreUI.SetText(score+"");

    }

    //update le slider de vie du 
    public void UpdateHealthBar(float currHp)
    {
        Slider healthBar = GetComponentInChildren<Slider>();
        healthBar.value = currHp;
    }

    //réduit le nombre de core de 1 et fair spawner le boss quand il reste 0 core
    public void killCore()
    {
        GameObject[] lesCores = GameObject.FindGameObjectsWithTag("Cores");
        
        //enable le colider le sprite renderer du boss et met sa valeur de spawn a true
        if(lesCores!= null && lesCores.Length<=1)
        {
            boss.GetComponent<SpriteRenderer>().enabled = true;
            boss.GetComponent<BoxCollider2D>().enabled = true;
            boss.GetComponent<Boss_script>().apparaitreBoss = true;
        }
    }

    //permet de déclancher le gameOver et d'afficher si le joueur a gagné ou persu selon le bool passé en param
    public void gameOver(bool playerWon)
    {
        Cursor.lockState = CursorLockMode.Confined;


        //si joueur a gagné affiche you won
        if (playerWon)
            gameOverTextUI.SetText("You won!");

        //si joueur a perdu affiche game over
        else
            gameOverTextUI.SetText("Game over");

        gameOverScoreUI.SetText("Score: " + score);

        //set le high score si le le score est plus ggrand que le high score
        if (PlayerPrefs.HasKey("highScore"))
        {
            if (score > PlayerPrefs.GetInt("highScore"))
                PlayerPrefs.SetInt("highScore", score);
        }

        //set le high score si aucun high score existe
        else
        {
            PlayerPrefs.SetInt("highScore", score);
        }
        PlayerPrefs.Save();

        highScoreUI.SetText("HighScore : " + PlayerPrefs.GetInt("highScore"));
        canvas.enabled = true; //affiche le ui gameOver

    }
}
