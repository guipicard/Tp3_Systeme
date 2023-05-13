using System.Collections;
using System.Collections.Generic;
using TP3.Script;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControls : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void PlayGame()
    {
        LevelManager.Save = null;
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LeaveGame()
    {
        LevelManager.UnsubsribeAll();
        SceneManager.LoadScene("MainMenu");
    }

    public void ChargeGame()
    {
        LevelManager.LoadGame();
        SceneManager.LoadScene("Game");
    }

    public void ChargeSave()
    {
        LevelManager.LoadGame();
        LevelManager.ChargeGame();
    }
}