using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class abrirPuerta : MonoBehaviour
{
    public float velocidad = 2.0f;
    public float angulo = 90.0f;
   

    bool abierta;
    bool entrar;
    bool cerrada = false;

    Vector3 puertaCerrada;
    Vector3 puertaAbierta;

    void Start()
    {
        puertaCerrada = transform.eulerAngles;
        puertaAbierta = new Vector3(puertaCerrada.x, puertaCerrada.y + angulo, puertaCerrada.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (abierta)
        {
            // Abrimos la puerta
            transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, puertaAbierta, Time.deltaTime * velocidad);
        }
        else
        {
            // Cerramos la puerta
            transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, puertaCerrada, Time.deltaTime * velocidad);
        }

        if (Input.GetKeyDown("f") && entrar && Score.scoreCurrentValue >= 50)
        {
            Score.scoreCurrentValue = Score.scoreCurrentValue - 50;
            if (!cerrada)
            {
                cerrada = true;
            }
            else
            {
                cerrada = false;
            }
            
            abierta = !abierta;
        }
    }

    void OnGUI()
    {
        

        if (entrar && !cerrada)
        {
            GUI.Label(new Rect(Screen.width / 2 - 75, Screen.height - 100, 150, 30), "Pulsa F para abrir");

        }

        if (cerrada)
        {
            GUI.Label(new Rect(Screen.width / 2 - 75, Screen.height - 100, 150, 30), "Pulsa F para cerrar");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            entrar = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            entrar = false;
        }
    }
}
