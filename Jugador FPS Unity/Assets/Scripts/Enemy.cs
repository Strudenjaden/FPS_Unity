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

    public float health = 100f;
    public bool isAttacking; 
    public GameObject player;
    public Animator animator;

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
        Destroy(gameObject);
    }

    public void OnTriggerEnter(Collider collider)
    {
        collider.tag.Equals("Player");
        isAttacking = true;
        Debug.Log("Activo la animación");

        isAttacking = true;
        StartCoroutine(EnemyAttack());
        
        
    }

    IEnumerator EnemyAttack()
    {
        animator.SetBool("Attack", true);

        yield return new WaitForSeconds(3f);
        animator.SetBool("Attack", false);
        isAttacking = false;
    }
}
