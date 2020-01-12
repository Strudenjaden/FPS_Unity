using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    public static bool GamePaused = false;
    public GameObject pauseMenuUI;
    public GameObject mainCamera;

    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera"); //Busca el GameObject con el tag MainCamera (la camara). Añadir el tag MainCamera si no lo está.
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GamePaused)
            {
                Resume();
            } else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GamePaused = false;
        mainCamera.GetComponent<PlayerCamera>().enabled = true; //Cogemos el script PlayerCamera y lo habilitamos para que no se mueva la camara.
        //Las armas las activamos desde sus propios scripts con !PauseMenu.GamePaused ya que comprueba que no esté en pausa a la hora de utilizarlas.
    }

    public void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GamePaused = true;
        mainCamera.GetComponent<PlayerCamera>().enabled = false; //Cogemos el script PlayerCamera y lo deshabilitamos para que no se mueva la camara.
        //Las armas las desactivamos desde sus propios scripts con !PauseMenu.GamePaused ya que comprueba que si está pausado el juego no se puedan utilizar.
    }
}
