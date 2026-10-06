using System.Collections;
using System.Data;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    // Audio
    [SerializeField] AudioClip error;
    [SerializeField] AudioClip byebye;
    AudioSource audioSource;

    [SerializeField] float cinematicTime = 5.0f;
    [SerializeField] float exitDuration = 2.0f;
    [SerializeField] float tempsEntre = 3.0f;
    [SerializeField] Image boss;
    // Animation
    Animator anim;
    // Values
    string levelToLoad = "SampleScene";
    bool play = true;
    bool exit = true;


    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
        anim = boss.GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Confined;
    }
    public void StartGame()
    {
        if (play)
        {
            play = false;
            StartCoroutine(CinematicTime());
            StartCoroutine(CinematicStart());

        }
    }
    public void QuitGame()
    {
        if (exit)
        {
            exit = false;
            StartCoroutine(Exit());
        }
    }
    IEnumerator CinematicStart()
    {
        audioSource.PlayOneShot(error);
        boss.enabled = true;
        yield return new WaitForSeconds(tempsEntre);
        anim.enabled = true;
    }

    IEnumerator CinematicTime()
    {
        PlayerPrefs.SetString("LevelToLoad", "SampleScene");
        PlayerPrefs.Save();
        yield return new WaitForSeconds(cinematicTime);
        SceneManager.LoadScene(levelToLoad);
    }
    IEnumerator Exit()
    {
        audioSource.PlayOneShot(byebye);
        yield return new WaitForSeconds(exitDuration);
        Application.Quit();
    }
}
