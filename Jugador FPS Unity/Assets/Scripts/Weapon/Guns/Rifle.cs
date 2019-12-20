using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : MonoBehaviour
{

    //Variables de munición del arma
    public float maxClip = 30; //Maxima munición que entra en el cargador.
    public float currentClip; //Munición que hay en el cargador.
    
    public float backupAmmo = 300; //Munición máxima que podemos llevar
    public float currentBackupAmmo; //Munición máxima que poseemos.
    
    //Variables booleanas para comprobar acciones o cantidades.
    public bool haveAmmo = true; //¿Hay balas?
    public bool isReloading = false; //¿Estoy recargando?
    public bool isShooting = false; //¿Estoy disparando?

    //Variabes para las animaciones.
    private float reloadTime = 2f; //Tiempo que tarda en recargar el jugador.
    public Animator animator; //Animación de recarga.

    //Sonidos
    public AudioSource shoot;
    public AudioSource reload;

    void Start()
    {
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
        GameObject rifle = GameObject.FindGameObjectWithTag("Rifle"); //Busca el GameObject con el tag Rifle (el arma Rifle). Añadir el tag Rifle si no lo está.
        WeaponManager rifleParameters = rifle.GetComponent<WeaponManager>(); //Cogemos el script WeaponManager en el GameObject rifle anteriormente creado.
        if (rifle.activeSelf) //Comprueba que el rifle esté activado. (Sea el arma seleccionada)
        {
            //Recogemos la variables del Script "WeaponManager" y les asignamos un valor
            rifleParameters.damage = 10f;
            rifleParameters.fireRate = 7f;
            rifleParameters.range = 50f;
            rifleParameters.impactForce = 35f;


            //Disparamos
            if (Input.GetKey(KeyCode.Mouse0) && Time.time >= rifleParameters.nextTimeToFire && haveAmmo && !isReloading)
            {
                shoot.Play();
                rifleParameters.nextTimeToFire = Time.time + 1f / rifleParameters.fireRate; //Aumentamos la velocidad de disparo.
                rifleParameters.Shoot();
                rifleParameters.EnableParticles();
                currentClip--; //Restamos balas del cargador.
                isShooting = true; //El jugador está disparando.
                
                if (currentClip == 0) //Si nos quedamos sin balas indicamos que no tenemos munición.
                {
                    haveAmmo = false; //No le quedan balas al cargador.
                    isShooting = false; //El jugador ya no esta disparando.
                    rifleParameters.DisableParticles();
                    Debug.Log("Sin balas en el cargador Rifle");
                }

            }

            //Ya no disparamos. Desactivamos todo lo que tenga que ver en relación al arma.
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                rifleParameters.DisableParticles();
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
}
