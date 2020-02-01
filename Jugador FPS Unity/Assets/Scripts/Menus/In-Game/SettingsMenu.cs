using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    [Header("Audio Mixers")]
    public AudioMixer audioMixer; //Audio mixer para la música de fondo.
    
    //GameObjects de los Highlights
    [Header ("Highlights de Calidad Gráfica")]
    public GameObject highLightUltra;
    public GameObject highLightMed;
    public GameObject highLightLow;

    [Header("V-Sync")]
    public TMP_Text vsyncText; //Texto a mostrar del VSync.

    [Header("FullScreen")]
    public TMP_Text fullScreenText; //Texto a mostrar para la opción Pantalla.

    [Header ("¿Está el Menú de Opciones abierto?")]
    public static bool settingsEnabled; //Variable que comprueba si la ventana de Opciones está activada.

    void Start()
    {
        /**
         * Comprobamos el valor de la VSYNC para modificar el texto que aparece.
         * Esto lo hacemos por si el usuario ha cambiado el valor de la VSync en la escena de Menú.
         */
        if (QualitySettings.vSyncCount == 0)
        {
           vsyncText.text = "OFF";
        }
        else if (QualitySettings.vSyncCount == 1)
        {
            vsyncText.text = "ON";
        }

        /**
         * Comprobamos el valor de la CALIDAD GRAFICA para modificar el texto que aparece.
         * Esto lo hacemos por si el usuario ha cambiado el valor de la Calidad Gráfica en la escena de Menú.
         * Cogemos el valor del script del Menú de opciones de la escena Menú.
         */

        if (OptionsMenuNew.qualityValue == 0)
        {
            highLightUltra.SetActive(true); //Activo el Highlight de Ultra porque es el que ha elegido el jugador en la pantalla de menú.
        }

        if (OptionsMenuNew.qualityValue == 1) 
        {
            highLightMed.SetActive(true); //Activo el Highlight de Medio porque es el que ha elegido el jugador en la pantalla de menú.
        }

        if (OptionsMenuNew.qualityValue == 2) 
        {
            highLightLow.SetActive(true); //Activo el Highlight de Bajo porque es el que ha elegido el jugador en la pantalla de menú.
        }

        /**
         * Comprobamos si el jugador ha puesto PANTALLA COMPLETA o MODO VENTANA.
         * Esto lo hacemos por si el usuario ha cambiado el valor de la Pantalla en la escena de Menú.
         */
        if (Screen.fullScreen == true)
        {
            fullScreenText.text = "PANTALLA COMPLETA";
        }
        else if (Screen.fullScreen == false)
        {
            fullScreenText.text = "MODO VENTANA";
        }



       /* resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolution = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolution = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolution;
        resolutionDropdown.RefreshShownValue();*/

    }
    /*
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }*/

     /**
      * ´Método dinámico por el cual le pasamos el float volume dinamicamente según la posición del slider.
      * El nombre entre "" es el mismo que hemos llamado a ese Audio Mixer
      */
    public void VolumeManager(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume); //Cambia el valor del mixer MaterVolume en función de la posición en la que esté.
    }

    /**
     * Desde Unity en: Edit -> Project Setings... -> Graphics podemos tocar la configuración de los gráficos.
     * En nuestro juego, hay 3 tipos diferentes (Ultra 0, Medio 1 y Minimo 2) ya que es en orden descendente.
     * Según el número que le pasemos por qualityIndex se modificará.
     * qualityIndex es un parámetro que le pasamos al hacer clic en uno de los botones. Esto se puede ver en el Inspector de Unity.
     * Por defecto el Minimo tiene la VSync desactivada.
     */
    public void QualityManager(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex); //Fija el nivel que queremos que tengan nuestras Texturas.

        if(qualityIndex == 0) //Es la primera opción (Ultra) se pondrá la mejor calidad. Esto quiere decir que ha hecho clic en el botón Ultra del menú opciones.
                              //Por defecto la Vsync ya está activada.
        {
            highLightMed.SetActive(false); //Desactivamos el Highlight de Medio.
            highLightLow.SetActive(false); //Desactivamos el Highlight de Bajo.
            QualitySettings.vSyncCount = 1; //Activamos la Vsync.
            vsyncText.text = "ON"; //Cambiamos el texto de la Vsync a ON para indicar que está activada.
        }

        if (qualityIndex == 1) //Es la segunda opción (Medio) se pondrá la calidad media. Esto quiere decir que ha hecho clic en el botón Medio del menú opciones.
                               //Además le quito la Vsync manualmente.
        {
            highLightUltra.SetActive(false); //Desactivamos el Highlight de Ultra.
            highLightLow.SetActive(false); //Desactivamos el Highlight de Bajo.
            QualitySettings.vSyncCount = 0; //Desactivamos la Vsync.
            vsyncText.text = "OFF"; //Cambiamos el texto de la Vsync a OFF para indicar que está desactivada.

        }

        if (qualityIndex == 2) //Es la tercera opción (Bajo) se pondrá la peor calidad. Esto quiere decir que ha hecho clic en el botón Bajo del menú opciones.
                               //Por defecto la Vsync ya está desactivada.
        {
            highLightUltra.SetActive(false); //Desactivamos el Highlight de Ultra.
            highLightMed.SetActive(false); //Desactivamos el Highlight de Medio.
            QualitySettings.vSyncCount = 0; //Desactivamos la Vsync.
            vsyncText.text = "OFF"; //Cambiamos el texto de la Vsync a OFF para indicar que está desactivada.

        }
    }

    /**
     * Activa o desactiva el V-Sync. Tasa de refresco de nuestro Monitor. 
     * Esto evita el Tearing y que se vea bien a la hora de hacer giros rápidos.
     */
    public void VSync()
    {
        if (QualitySettings.vSyncCount == 0) //Si es 0 (está desactivada) se activa y se pone el texto en ON.
        {
            QualitySettings.vSyncCount = 1; //Activamos la Vsync.
            vsyncText.text = "ON"; //Cambiamos el texto de la Vsync a ON para indicar que está activada.

        }
        else if (QualitySettings.vSyncCount == 1) //Si es 1 (está activada) se desactiva y se pone el texto en OFF. 
        {
            QualitySettings.vSyncCount = 0; //Desactivamos la VSync.
            vsyncText.text = "OFF"; //Cambiamos el texto de la Vsync a OFF para indicar que está desactivada.
        }
    }

    /**
     * Hace que el juego se ponga en Pantalla completa o en Modo Ventana.
     * Además cambia el texto que aparece.
     * Por defecto el valor de fullScreen en Unity es true.
     */
    public void FullScreen ()
    {

        Screen.fullScreen = !Screen.fullScreen; //Intercambia el valor, si es True -> False y si es False -> True.

        if (Screen.fullScreen == true) //Al ser false es modo ventana, pero al ejecutar el método lo cambiamos a true.
        {
            fullScreenText.text = "MODO VENTANA";
        }
        else if (Screen.fullScreen == false) //Al ser True es pantalla completa, pero al ejecutar el método lo cambiamos a false.
        {
            fullScreenText.text = "PANTALLA COMPLETA";
        }

    }

    /**
     * Este método lo utilizamos para que al abrir el Menú de Opciones, no desaparezca el menú de pausa.
     * Al hacer click en él le decimos que el menú de opciones ya no está habilitado.
     * Esto lo podemos ver en el Inspector de Unity
     * */
    public void backButton(bool options)
    {
        settingsEnabled = options; //True o false, dependiendo de lo que le indiquemos en la variable.
    }
}
