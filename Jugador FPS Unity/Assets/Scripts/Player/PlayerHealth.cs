using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour

    /*
     * Este script comprende todas las funcionalidades relacionadas con la vida del jugador.
     * 
     * En un futuro se añadirá las variables de animaciones, textos y particulas.
     * 
     */
{
    public float maxLife = 100f; //Maxima vida del jugador.

    public float currentLife; //Vida actual del jugador.

    void Start()
    {
        currentLife = maxLife; //Indicamos que al iniciar el juego el jugador obtenga la máxima vida.
    }


}
