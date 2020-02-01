using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncenderLuz : MonoBehaviour
{
    GameObject Luz;

    bool entra;

    void Awake()
    {
        Luz = GameObject.FindGameObjectWithTag("Luz");
        Luz.SetActive(false);
    }

    void Update()
    {
        if (entra)
        {
            Luz.SetActive(true);
        }
        else
        {
            Luz.SetActive(false);
        }
            
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            entra = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            entra = false;
        }
    }
}
