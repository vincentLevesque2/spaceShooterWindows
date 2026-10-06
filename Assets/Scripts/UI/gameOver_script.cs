using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameOver_script : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //appeler par un bouton pour reload la scene
    public void restartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
