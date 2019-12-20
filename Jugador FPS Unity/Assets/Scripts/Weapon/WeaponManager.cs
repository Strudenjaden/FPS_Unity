using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour

{
    /* Variables del daño, rango, velocidad de disparo y potencia de la bala
     * Estas variables las inicializamos aquí, ya que es donde manejamos todas las cosas que podemos
     * hacer con el arma.
     * Los valores de cada variable se los pasamos por el script asignado a cada arma (nombreArma.cs)
     */
    public float damage; //Declaramos la variable de daño que tendrá el arma activa.
    public float range; //Declaramos la variable de rango que tendrá el arma activa.
    public float fireRate; //Declaramos la variable de velocidad de disparo que tendrá el arma activa.
    public float impactForce; //Declaramos la variable de fuerza de impacto que tendrá el arma activa.
    public float nextTimeToFire = 0f; //Tiempo para ejecutar el siguiente disparo. (Por si queremos que espere cierto tiempo)

    /*Variables para el impacto y visualización de las balas*/
    public Camera camera; //Referencia a la camara. Esto se usrá para indicar que el disparo se efectue al lugar donde estamos mirando.
    public ParticleSystem muzzleFlash; //Sistema de particulas del fogonazo del arma.
    public ParticleSystem bulletTrail; //Sistema de particulas del trazado de la bala.
    public GameObject impactEffect; // Gameobject que se inicia cuando la bala impacta contra un objeto.
    public GameObject impactEffectEnemy; // Gameobject que se inicia cuando la bala impacta contra un enemigo.
    
    
    /*
     * Método con el que dispara nuestro jugador.
     * A través de un Raycast le decimos el lugar y la distancia que queremos que tenga el disparo.
     * Si encuentra un enemigo durante el recorrido le hará el daño de la variable "damage" que se la pasa como parámetro al Script "Enemy"
     * Si no moverá un objeto si tiene fisica y se puede mover.
     * Por último inicia un efecto de impacto, lo elimina pasados 1s.
     */
    public void Shoot()
    {

        RaycastHit hit; //Creamos un raycast para representar el disparo.

        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name); //Muestra por consola el nombre del GameObject donde ha impactado el rayo "disparo".

           Enemy enemy = hit.transform.GetComponent<Enemy>(); //Llamamos al script del Enemigo y lo asignamos con el rayo.


            if (enemy != null) //Comprueba si el impacto colisiona contra un enemigo. (Lo detecta mediante si el GameObject tiene el script Enemigo)
            {
                enemy.TakeDamage(damage); //Daño que recibe el enemigo. El valor damage se le pasa al Script "Enemy" como parametro "amount".
                GameObject impactGoEnemy = Instantiate(impactEffectEnemy, hit.point, Quaternion.LookRotation(hit.normal)); //Instancia un GameObject (prefab fuera del Hierarchy) 
                                                                                                                           //en el lugar donde haya impactado el rayo. 
                                                                                                                           //En este caso el efecto de impacto hacia un enemigo.
                Destroy(impactGoEnemy, 1f); //Destruye el impacto pasados 1 segundo.
            }
            else
            {
                GameObject impactGo = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal)); //Instancia un GameObject (prefab fuera del Hierarchy) 
                                                                                                                 //en el lugar donde haya impactado el rayo.
                                                                                                                 //En este caso el efecto de impacto hacia algo que no es enemigo.
                Destroy(impactGo, 1f); //Destruye el impacto pasados 1 segundo.
            }

            if (hit.rigidbody != null) //Comprueba si el impacto colisiona contra un rigidbody.
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce); //Aplica una fuerza en la dirección en la que golpee el impacto para mover el objeto.
            }

            
        }
    }

    /*
    * Activa los sistemas de particulas de las armas.
    */
    public void EnableParticles()
    {

        muzzleFlash.Play(); //Activamos el sistema de particulas del fogonazo.
        bulletTrail.Play(); //Activamos el sistema de particulas del trazado de la bala.
    }

    /*
    * Desactiva los sistema de particulas de las armas. 
    */
    public void DisableParticles()
    {

         muzzleFlash.Stop(); //Desactivamos el sistema de particulas del fogonazo.
         bulletTrail.Stop(); //Desactivamos el sistema de particulas del trazado de la bala

    }

}
