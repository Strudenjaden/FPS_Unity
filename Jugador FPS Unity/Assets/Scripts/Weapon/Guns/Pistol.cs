using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    //Variables de munición del arma
    public float maxClip = 7; //Maxima munición que entra en el cargador.
    public float currentClip; //Munición que hay en el cargador.

    public float backupAmmo = 70; //Munición máxima que podemos llevar
    public float currentBackupAmmo; //Munición máxima que poseemos.

    //Variables booleanas para comprobar acciones o cantidades.
    public bool haveAmmo = true; //¿Hay balas?
    public bool isReloading = false; //¿Estoy recargando?
    public bool isShooting = false; //¿Estoy disparando?

    //Variabes para las animaciones.
    private float reloadTime = 1f; //Tiempo que tarda en recargar el jugador.
    public Animator animator; //Animación de recarga.

    public AudioSource shoot;
    public AudioSource reload;

    void Start()
    {
        currentClip = maxClip;
        currentBackupAmmo = backupAmmo;
        
    }

    void Update()
    {
        GameObject pistol = GameObject.FindGameObjectWithTag("Pistol"); //Busca el GameObject con el tag Rifle (el arma Rifle). Añadir el tag Rifle si no lo está.
        WeaponManager pistolParameters = pistol.GetComponent<WeaponManager>(); //Cogemos el script "Weapon Manager" en el GameObject pistol anteriormente creado.

        if (pistol.activeSelf) //Comprueba que la pistola esté activada. (Sea el arma seleccionada)
        {
            pistolParameters.damage = 25f;
            pistolParameters.range = 30f;
            pistolParameters.impactForce = 120f;

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
        GameObject pistol = GameObject.FindGameObjectWithTag("Pistol"); //Busca el GameObject con el tag Rifle (el arma Rifle). Añadir el tag Rifle si no lo está.
        WeaponManager pistolParameters = pistol.GetComponent<WeaponManager>(); //Cogemos el script "Weapon Manager" en el GameObject pistol anteriormente creado.
        shoot.Play();
        pistolParameters.Shoot();
        pistolParameters.EnableParticles();

        yield return new WaitForSeconds(0.1f);
        pistolParameters.DisableParticles();
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
            animator.SetBool("Reloading", false); //Indicamos al animator que ponga la variable "Reloading" en false.

            yield return new WaitForSeconds(0.8f); //Tiempo de espera para recolocar el arma durante la animación de recarga.
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
