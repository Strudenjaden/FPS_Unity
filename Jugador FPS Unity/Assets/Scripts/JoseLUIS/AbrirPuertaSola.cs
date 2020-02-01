using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class abrirPuertaSola : MonoBehaviour
{
    public float velocidad = 2.0f;
    public float angulo = 90.0f;


   
    bool entrar;
   

    Vector3 puertaCerrada;
    Vector3 puertaAbierta;

    void Start()
    {
        puertaCerrada = transform.eulerAngles;
        puertaAbierta = new Vector3(puertaCerrada.x, puertaCerrada.y + angulo, puertaCerrada.z);
    }

   
    void Update()
    {
        if (entrar)
        {
            transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, puertaAbierta, Time.deltaTime * velocidad);
        }

    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            entrar = true;
        }
    }

    
}
