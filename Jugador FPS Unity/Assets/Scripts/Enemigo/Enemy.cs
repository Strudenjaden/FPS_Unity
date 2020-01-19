using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Esto es una guarrada que he hecho hasta que Pedro termine
 * o empieze a hacer su script de Enemigos.
 * No tiene ningun sentido que lo explique ya que no va a ser el definitivo, aun así
 * creo que es bastante intuitivo lo que hace el script.
 * */

public class Enemy : MonoBehaviour
{

    public float health = 100f; //Vida enemigo
    public bool isAttacking; 
    public GameObject player;
    public Animator animator;
    public Transform enemyPos;

    //Prefabs de los Power Ups
    public GameObject instaKill;
    public GameObject doublePoints;
    public GameObject maxAmmo;

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        SpawnBonus();
        Destroy(gameObject);
    }

    public void OnTriggerEnter(Collider collider)
    {
        collider.tag.Equals("Player");
        Debug.Log("Activo la animación");
        StartCoroutine(EnemyAttack());
        
    }

    IEnumerator EnemyAttack()
    {
        animator.SetBool("Attack", true);
        isAttacking = true;
        yield return new WaitForSeconds(3f);
        animator.SetBool("Attack", false);
        isAttacking = false;
    }

    /*
     * Método que genera un número aleatorio en base a un porcentaje.
     * Si el número es menor a ese número (menor que 100) genera un objecto PowerUp con el que el jugador adquiere mejoras.
     */
    public void SpawnBonus()
    {
        int percentage = 100; //Numero del porcentaje.
        int randomValue = Random.Range(1, 100); //Numero random entre 1 y 100
        Debug.Log("El número aletorio es: " + randomValue);

        if (randomValue < percentage) //Si el numero es menor que el porcentaje ejecuta el siguiente código.
        { 
            InstantiateBonus(); 
        }
       
    }

    /*
     * Instancia un objeto en la posición en la que ha muerto el enemigo. 
     * Según el número random que obtengamos se generará un PowerUp u otro.
     */
    public void InstantiateBonus()
    {
        int selectedBonus = Random.Range(0, 3) + 1; //Numero aleatorio entre 1 y 3 para decidir que PowerUp se instanciará.
        Debug.Log("El número del PowerUp es: " + selectedBonus);

        if (selectedBonus == 1) //Al ser 1 se instanciará el PowerUp de InstaKill.
        {
            Instantiate(instaKill, enemyPos.position, enemyPos.rotation);
        }

        if (selectedBonus == 2) //Al ser 2 se instanciará el PowerUp de Double Points.
        {
            Instantiate(doublePoints, enemyPos.position, enemyPos.rotation);
        }

        if (selectedBonus == 3) //Al ser 3 se instanciará el PowerUp de Max Ammo.
        {
            Instantiate(maxAmmo, enemyPos.position, enemyPos.rotation);
        }

    }
}
