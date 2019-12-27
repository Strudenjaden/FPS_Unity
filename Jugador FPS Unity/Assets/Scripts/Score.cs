using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public static int scorePoints = 10; //Valor que queremos que se sume a nuestros puntos actuales.
    public static int scoreCurrentValue = 0; //Valor que mostraremos por pantalla. (Puntuación)
    public Text score; //Variable para guardar la referencia al texto.

    void Start()
    {
        score = GetComponent<Text>(); //Referencia al elemento text.
        
    }


    void Update()
    {
        score.text = "Score: " + scoreCurrentValue; //Ponemos el texto que queremos que aparezca
    }
}
