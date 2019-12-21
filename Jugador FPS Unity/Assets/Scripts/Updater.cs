using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Updater : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Animation>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
