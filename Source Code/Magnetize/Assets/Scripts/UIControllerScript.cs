using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIControllerScript : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject resumeBtn;
    public GameObject levelClearTxt;
    public GameObject nextlv;
    private Scene currActiveScene;

    // Start is called before the first frame update
    void Start()
    {
        currActiveScene = SceneManager.GetActiveScene();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseGame();
        }
    }

    public void pauseGame()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }

    public void resumeGame()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }

    public void restartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(currActiveScene.name);
    }

    public void endGame()
    {
        pausePanel.SetActive(true);
        resumeBtn.SetActive(false);
        nextlv.SetActive(true);
        levelClearTxt.SetActive(true);
        Time.timeScale = 0;
    }
    public void LoadScene(string name)
    {
        //Debug.Log(string.IsNullOrEmpty(name));
        //Melakukan pengecekan jika name tidak null atau empty
        if (!string.IsNullOrEmpty(name))
        {
            //membuka scene dengan nama sesuai dengan variable name
            SceneManager.LoadScene(name);
            Time.timeScale = 1;
        }
    }
}
