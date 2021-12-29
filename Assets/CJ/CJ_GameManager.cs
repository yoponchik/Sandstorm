using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CJ_GameManager : MonoBehaviour
{
    public static CJ_GameManager instance;
    private void Awake()
    {
        instance = this;
    }

    public GameObject GameOverUI;
    // Start is called before the first frame update
    void Start()
    {
        GameOverUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClickRestart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    public void OnClickQuit()
    {
        Application.Quit();
    }
}
