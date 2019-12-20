using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    [SerializeField]
    private float instaKill = 9999f; //Declaramos el daño para asegurarnos que mataremos de un golpe al enemigo.
    private float instaKillTime = 3f; //Tiempo que durará el insta kill a los enemigos.

    //Resets del daño.
    private float resetToNormalDamageRifle; //Guarda el daño normal del arma Rifle para cuando acabe el Insta Kill este se resetee.
    private float resetToNormalDamagePistol; //Guarda el daño normal del arma Pistola para cuando acabe el Insta Kill este se resetee.
    

    //Resets
    void Start()
    {
        resetToNormalDamageRifle = Rifle.damage; //Le asignamos el daño inicial del arma Rifle.
        resetToNormalDamagePistol = Pistol.damage;  //Le asiganmos el daño inicial del arma Pistola.
    }


    public void OnTriggerEnter(Collider other)
    {
        //Esto solo se va a ejecutar cuando el power up sea InstaKill y solo cuando o recoja el Player.
        if (this.gameObject.CompareTag("InstaKill") && other.gameObject.CompareTag("Player"))
        {

            StartCoroutine(InstaKill());

            transform.position = new Vector3(transform.position.x, 3f, transform.position.z); //Esto es para hacer que el jugador no pueda volver a coger el objeto, 
                                                                                              //ya que si lo eleminamos o lo desactivamos no se ejecuta la coroutine. 
                                                                                              //Si alguien sabe como solucionar esto para que no haya mas gameobjects 
                                                                                              //en escena de los que necesitamosque lo modifique. 
                                                                                              //El 3f es visual cambiadlo por un -999f cuando termines el juego para 
                                                                                              //que se vaya al suelo y así no cogerlo nunca.

            Destroy(this.gameObject, instaKillTime + 1f); //Eliminamos el objeto después del tiempo de InstaKill y le añadimos 1s 
                                                          //para asegurarnos que cambia el valor antes de ser eliminado
        }
    }

    IEnumerator InstaKill()
    {
        Rifle.damage = instaKill; //Le asiganmos el daño de InstaKill al arma Rifle.
        Pistol.damage = instaKill; //Le asiganmos el daño de InstaKill al arma Rifle.
        yield return new WaitForSeconds(instaKillTime); //Tiempo que queremos que esté el power up Insta Kill.
        Rifle.damage = resetToNormalDamageRifle; //Reseteamos el daño del arma Rifle con el inicial anteriormente guardado.
        Pistol.damage = resetToNormalDamagePistol; //Reseteamos el daño del arma Pistol con el inicial anteriormente guardado.
    }
}
