using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bonus : MonoBehaviour
{
    [SerializeField]
    private float instaKillDamage = 9999f; //Declaramos el daño para asegurarnos que mataremos de un golpe al enemigo.
    private float instaKillTime = 3f; //Tiempo que durará el insta kill a los enemigos.
    private float doublePointsTime = 5f; //Tiempo que durarán los puntos dobles.

    //Resets.
    private float resetToNormalDamageRifle; //Guarda el daño normal del arma Rifle para cuando acabe el Insta Kill este se resetee.
    private float resetToNormalDamagePistol; //Guarda el daño normal del arma Pistola para cuando acabe el Insta Kill este se resetee.
    private int resetToNormalPoints;

    //Textos
    
    void Start()
    {
        resetToNormalDamageRifle = Rifle.damage; //Le asignamos el daño inicial del arma Rifle.
        resetToNormalDamagePistol = Pistol.damage;  //Le asiganmos el daño inicial del arma Pistola.
        resetToNormalPoints = Score.scorePoints;
        
    }

    public void OnTriggerEnter(Collider other)
    {
        //Comprueba que el objeto solo sea cogido por el Player.
        if (other.gameObject.CompareTag("Player"))
        {
            //Esto solo se va a ejecutar cuando el Power-Up sea InstaKill y solo cuando o recoja el Player.
            if (this.gameObject.CompareTag("InstaKill"))
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

            if (this.gameObject.CompareTag("MaxAmmo"))
            {
                Rifle.currentBackupAmmo = Rifle.backupAmmo;
                Pistol.currentBackupAmmo = Pistol.backupAmmo;

                Destroy(this.gameObject, 0.2f);
            }

            if (this.gameObject.CompareTag("DoublePoints"))
            {
                StartCoroutine(DoublePoints());

                transform.position = new Vector3(transform.position.x, 3f, transform.position.z); //Esto es para hacer que el jugador no pueda volver a coger el objeto, 
                                                                                                  //ya que si lo eleminamos o lo desactivamos no se ejecuta la coroutine. 
                                                                                                  //Si alguien sabe como solucionar esto para que no haya mas gameobjects 
                                                                                                  //en escena de los que necesitamosque lo modifique. 
                                                                                                  //El 3f es visual cambiadlo por un -999f cuando termines el juego para 
                                                                                                  //que se vaya al suelo y así no cogerlo nunca.

                Destroy(this.gameObject, doublePointsTime + 1f);//Eliminamos el objeto después del tiempo de DoublePoints y le añadimos 1s 
                                                                //para asegurarnos que cambia el valor antes de ser eliminado
            }
        }
    }

    IEnumerator InstaKill()
    {
        Rifle.damage = instaKillDamage; //Le asiganmos el daño de InstaKill al arma Rifle.
        Pistol.damage = instaKillDamage; //Le asiganmos el daño de InstaKill al arma Rifle.
        yield return new WaitForSecondsRealtime(instaKillTime); //Tiempo que queremos que esté el power up Insta Kill activo.
        Rifle.damage = resetToNormalDamageRifle; //Reseteamos el daño del arma Rifle con el inicial anteriormente guardado.
        Pistol.damage = resetToNormalDamagePistol; //Reseteamos el daño del arma Pistol con el inicial anteriormente guardado.
    }

    IEnumerator DoublePoints()
    {
        Score.scorePoints *= 2; //Multiplicamos por 2 (ya que son puntos dobles no creo la variable).
        yield return new WaitForSecondsRealtime(doublePointsTime); //Tiempo que queremos que esté el power up Double Points activo.
        Score.scorePoints = resetToNormalPoints; //Reseteamos el valor de los puntos.
    }

}
