using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeguirJugador : MonoBehaviour
{
    //Sirve para tomar la posicion del jugador
    public Transform jugador;
    UnityEngine.AI.NavMeshAgent enemigo;
    private bool dentro = false;


    // Start is called before the first frame update
    void Start()
    {
        enemigo = GetComponent<UnityEngine.AI.NavMeshAgent>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            dentro = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            dentro = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!dentro)
        {
            
            enemigo.destination = jugador.position;
        }
        if (dentro)
        {
            enemigo.destination = this.transform.position;
        }

    }
}
