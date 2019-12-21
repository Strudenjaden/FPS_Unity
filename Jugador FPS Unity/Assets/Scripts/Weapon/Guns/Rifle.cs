using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : MonoBehaviour
{
    public Camera camera; //Referencia a la camara. Esto se usrá para indicar que el disparo se efectue al lugar donde estamos mirando.

    //Busqueda del objeto rifle y del controlador de las armas
    GameObject rifle; //Variable para guardar el objeto Rifle.
    //WeaponManager rifleParameters; //Variable para guardar la referencia del Script Weapon Manager.

    //Variables del daño, rango, velocidad de disparo y potencia de la bala
    public static float damage = 10f; //Declaramos la variable de daño que tendrá el arma activa.
    public float range = 50f; //Declaramos la variable de rango que tendrá el arma activa.
    public float fireRate = 7f; //Declaramos la variable de velocidad de disparo que tendrá el arma activa.
    public float impactForce = 35f; //Declaramos la variable de fuerza de impacto que tendrá el arma activa.
    public float nextTimeToFire = 0f; //Tiempo para ejecutar el siguiente disparo. (Por si queremos que espere cierto tiempo)

    //Variables de munición del arma
    public float maxClip = 30; //Maxima munición que entra en el cargador.
    public float currentClip; //Munición que hay en el cargador.

    public static float backupAmmo = 300; //Munición máxima que podemos llevar
    public static float currentBackupAmmo; //Munición máxima que poseemos.

    //Variables booleanas para comprobar acciones o cantidades.
    public bool haveAmmo = true; //¿Hay balas?
    public bool isReloading = false; //¿Estoy recargando?
    public bool isShooting = false; //¿Estoy disparando?

    //Variabes para las animaciones.
    private float reloadTime = 2f; //Tiempo que tarda en recargar el jugador.
    public Animator animator; //Animación de recarga.

    //Sonidos
    public AudioSource shoot; //Sonido de disparo
    public AudioSource reload; //Sonido de Recarga

    /*Variables para el impacto y visualización de las balas*/
    public ParticleSystem muzzleFlash; //Sistema de particulas del fogonazo del arma.
    public ParticleSystem bulletTrail; //Sistema de particulas del trazado de la bala.
    public GameObject impactEffect; // Gameobject que se inicia cuando la bala impacta contra un objeto.
    public GameObject impactEffectEnemy; // Gameobject que se inicia cuando la bala impacta contra un enemigo.

    void Start()
    {

        rifle = GameObject.FindGameObjectWithTag("Rifle"); //Busca el GameObject con el tag Rifle (el arma Rifle). Añadir el tag Rifle si no lo está.
        //rifleParameters = rifle.GetComponent<WeaponManager>(); //Cogemos el script WeaponManager en el GameObject rifle anteriormente creado.

        currentClip = maxClip;
        currentBackupAmmo = backupAmmo;

    }

    void OnEnable()
    {
        isReloading = false;
        animator.SetBool("Reloading", false);
        reload.Stop();
    }

    void Update()
    {
        if (rifle.activeSelf) //Comprueba que el rifle esté activado. (Sea el arma seleccionada)
        {

            //Disparamos
            if (Input.GetKey(KeyCode.Mouse0) && Time.time >= nextTimeToFire && haveAmmo && !isReloading)
            {
                shoot.Play();
                nextTimeToFire = Time.time + 1f / fireRate; //Aumentamos la velocidad de disparo.
                Shoot();
                EnableParticles();
                currentClip--; //Restamos balas del cargador.
                isShooting = true; //El jugador está disparando.

                if (currentClip == 0) //Si nos quedamos sin balas indicamos que no tenemos munición.
                {
                    haveAmmo = false; //No le quedan balas al cargador.
                    isShooting = false; //El jugador ya no esta disparando.
                    DisableParticles();
                    Debug.Log("Sin balas en el cargador Rifle");
                }

            }

            //Ya no disparamos. Desactivamos todo lo que tenga que ver en relación al arma.
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                DisableParticles();
                isShooting = false; //El jugador ya no esta disparando.
            }

            //Recarga si pulsa R y muestra mensaje.
            if (Input.GetKeyDown(KeyCode.R) && !isShooting && currentClip != maxClip)
            {

                StartCoroutine(Reload());
                //return;
                Debug.Log("Recargando...");
                //Cambiar el debug.log por un CANVAS.
            }

            //Mostramos mensaje si no le quedan balas para que recargue.
            if (currentClip == 0)
            {
                Debug.Log("Recarga, no tienes munición.");
                //Cambiar el debug.log por un CANVAS.
            }



        }
    }

    IEnumerator Reload()
    {
        //Comprobamos que tengamos balas de repuesto.
        if (currentBackupAmmo > 0)
        {
            animator.SetBool("Reloading", true); //Indicamos al animator que ponga la variable "Reloading" en true.
            isReloading = true; // Jugador recargando.
            yield return new WaitForSeconds(reloadTime); //Tiempo de espera mientras se efectua la animación de recarga.
            reload.Play(); //Ejecutamos el sonido de recarga porque ya hemos acoplado el cargador.
            float ammoToReload; //Munición que necesitamos recargar.
            ammoToReload = maxClip - currentClip; //La munición que necesitamos recargar es la munición del cargador lleno menos la de nuestro cargador.

            if (ammoToReload < currentBackupAmmo) //Si la munición que tenemos que cargar es menor que la munición de repuesto.
            {
                currentClip += ammoToReload; //El cargador se recarga con las balas que necesitemos recargar.
                currentBackupAmmo -= ammoToReload; //Se resta las balas recargadas a la munición de repuesto.
            }
            else //Si la munición que tenemos que recargar es mayor que la que tenemos de repuesto.
            {
                currentClip += currentBackupAmmo; //El cargador suma la munición que tengamos de repuesto.
                currentBackupAmmo = 0; //Ponemos el repuesto a 0.
            }

            yield return new WaitForSeconds(0.8f); //Tiempo de espera para recolocar el arma durante la animación de recarga.
            animator.SetBool("Reloading", false); //Indicamos al animator que ponga la variable "Reloading" en false.
            isReloading = false; //Jugador ya no esta recargando
            haveAmmo = true; //Jugador tiene balas en el arma.
        }
        else //Mostramos al jugador que no puede recargar porque no tiene balas de repuesto.
        {
            Debug.Log("No puedes recargar, no tienes balas de repuesto.");
            isReloading = false; //El jugador ya no esta recargando.
        }
    }

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
                Score.scoreValue += 10;
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
