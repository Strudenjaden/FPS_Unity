using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{

    public static int scoreValue = 0; //Valor que mostraremos por pantalla. (Puntuación)
    Text score; //Variable para guardar la referencia al texto.
    // Start is called before the first frame update
    void Start()
    {
        score = GetComponent<Text>(); //Referencia al elemento text.
    }

    // Update is called once per frame
    void Update()
    {
        score.text = "Score: " + scoreValue; //Ponemos el texto que queremos que aparezca
    }
}
