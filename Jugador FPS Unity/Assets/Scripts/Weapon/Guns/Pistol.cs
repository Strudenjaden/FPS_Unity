using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour
{

    public Camera camera; //Referencia a la camara. Esto se usrá para indicar que el disparo se efectue al lugar donde estamos mirando.

    //Busqueda del objeto Pistola y del controlador de las armas
    GameObject pistol; //Variable para guardar el objeto Pistola.
    WeaponManager pistolParameters; //Variable para guardar la referencia del Script Weapon Manager.

    //Variables del daño, rango, velocidad de disparo y potencia de la bala
    public static float damage = 25f; //Declaramos la variable de daño que tendrá el arma activa.
    public float range = 30f; //Declaramos la variable de rango que tendrá el arma activa.
    public float impactForce = 120f; //Declaramos la variable de fuerza de impacto que tendrá el arma activa.

    //Variables de munición del arma
    public float maxClip = 7; //Maxima munición que entra en el cargador.
    public float currentClip; //Munición que hay en el cargador.

    public static float backupAmmo = 70; //Munición máxima que podemos llevar
    public static float currentBackupAmmo; //Munición máxima que poseemos.
    public float viewCurrentBackupAmmo;

    //Variables booleanas para comprobar acciones o cantidades.
    public bool haveAmmo = true; //¿Hay balas?
    public bool isReloading = false; //¿Estoy recargando?
    public bool isShooting = false; //¿Estoy disparando?

    //Variabes para las animaciones.
    private float reloadTime = 1f; //Tiempo que tarda en recargar el jugador.
    public Animator animator; //Animación de recarga.

    //Variables de audio
    public AudioSource shoot;
    public AudioSource reload;

    /*Variables para el impacto y visualización de las balas*/
    public ParticleSystem muzzleFlash; //Sistema de particulas del fogonazo del arma.
    public ParticleSystem bulletTrail; //Sistema de particulas del trazado de la bala.
    public GameObject impactEffect; // Gameobject que se inicia cuando la bala impacta contra un objeto.
    public GameObject impactEffectEnemy; // Gameobject que se inicia cuando la bala impacta contra un enemigo.

    void Start()
    {
        pistol = GameObject.FindGameObjectWithTag("Pistol"); //Busca el GameObject con el tag Rifle (el arma Rifle). Añadir el tag Rifle si no lo está.
        pistolParameters = pistol.GetComponent<WeaponManager>(); //Cogemos el script "Weapon Manager" en el GameObject pistol anteriormente creado.

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
        
        if (pistol.activeSelf) //Comprueba que la pistola esté activada. (Sea el arma seleccionada)
        {
            //Disparamos
            if (Input.GetKeyDown(KeyCode.Mouse0) && haveAmmo && !isReloading)
            {
                if (currentClip > 0)
                {
                    StartCoroutine(ShootEffect());
                    currentClip--; //Restamos las balas del cargador
                    isShooting = true; //El jugador esta disparando.
                }
                
                else//Si nos quedamos sin balas indicamos que no tenemos munición
                {
                    haveAmmo = false; //No le quedan balas al cargador.
                    isShooting = false; //El jugador ya no está disparando.
                    Debug.Log("Sin balas en el cargador");
                }
            }

            //Recarga si pulsa R y muestra mensaje.
            if (Input.GetKeyDown(KeyCode.R) && !isShooting && currentClip != maxClip)
            {
                
                StartCoroutine(Reload());
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

    IEnumerator ShootEffect()
    {
        shoot.Play();
        Shoot();
        EnableParticles();
        yield return new WaitForSeconds(0.1f);
        DisableParticles();
        isShooting = false;
    }

    IEnumerator Reload()
    {
        //Comprobamos que tengamos balas de repuesto.
        if (currentBackupAmmo > 0)
        {
            animator.SetBool("Reloading", true); //Indicamos al animator que ponga la variable "Reloading" en true.
            isReloading = true; // Jugador recargando.
            yield return new WaitForSeconds(reloadTime); //Tiempo de espera mientras se efectua la animación de recarga.
            reload.Play();
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
            
            yield return new WaitForSeconds(.8f); //Tiempo de espera para recolocar el arma durante la animación de recarga.
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
