using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
public class PauseMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static bool gamePaused = false;
    public GameObject PauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (gamePaused) {
                Resume();
            }
            else {
                Pause();
            }
            
        }
        
    }


    // back to game
    public void Resume() {

        PauseMenuUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
        gamePaused = false;

    }

    // pause game
    void Pause() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        PauseMenuUI.SetActive(true);

        Time.timeScale = 0f;
        gamePaused = true; 
    
    }

    public void LoadMenu() {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("TitleScreen");
    
    }

   
}
