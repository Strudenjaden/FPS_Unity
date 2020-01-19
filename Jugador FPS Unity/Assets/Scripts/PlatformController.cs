using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    
    public Rigidbody plataformaRB;
    public Transform[] plataformaPosicion;
    public float plataformaVelocidad;
    public bool dentro;
    private int actualPosicion = 0;
    private int siguientePosicion = 1;
    Enemy enemy;


    void Update()
    {
        
        MoverPlataforma();

    }

    void MoverPlataforma()
    {
        
            plataformaRB.MovePosition(Vector3.MoveTowards(plataformaRB.position, plataformaPosicion[siguientePosicion].position, plataformaVelocidad * Time.deltaTime));


            if (Vector3.Distance(plataformaRB.position, plataformaPosicion[siguientePosicion].position) <= 0)
            {
                actualPosicion = siguientePosicion;
                siguientePosicion++;

                if (siguientePosicion > plataformaPosicion.Length - 1)
                {
                    siguientePosicion = 0;
                }
            }

        
        
    }

    public void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Enemy"))
        {
            enemy = other.GetComponent<Enemy>();
            enemy.TakeDamage(100f);

            Debug.Log("Estoy dentro");
            dentro = true;
        }

    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            dentro = false;
        }
    }
    

}
