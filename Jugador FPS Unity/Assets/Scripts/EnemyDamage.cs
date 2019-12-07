using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour

    /*
     * Script guarro tambien para que el jugador reciba daño. A mi me gusta mucho esta forma de hacerlo.
     * El script se pone en los 2 brazos o en el medio de los y asi solo es 1 collider.
     * Una vez el collider entre en contacto con el collider del Jugador este recibirá daño.
     * 
     * 
     * */
{
    public GameObject player; //Ponemos al GameObject del jugador en el Inspector de Unity.

    public void OnTriggerEnter(Collider collider)
    {
        collider.tag.Equals("Player"); //Solo si el collider es el del Jugador
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>(); //Hacemos referencia sobre el script PlayerHealth sobre el gameobject player.

        playerHealth.currentLife -= 34f; //Resta la vida que tiene en ese momento. Es 34 para que al golpear 3 veces muera el jugador.
        Debug.Log("Activado");
    }
}
