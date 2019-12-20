using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Este script nos concede la opción de tener varias armas en nuestro jugador y cambiarlas entre sí.
 * Con la rueda del raton o los botones del teclado le indicamos que arma será la que seleccionaremos.
 * Mediante un GameObject lo recorremos y según su posición se muestra un arma u otra.
 * 
 *      **********************************************
 *      **********Autor: Dejan Polit Andrés***********
 *      **********************************************
 */
public class WeaponSwitcher : MonoBehaviour
{
    //Iniciamos la variable del arma seleccionada en la posición 0.
    public int selectedWeapon = 0;

    // Start is called before the first frame update
    void Start()
    { 
        SelectWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        int previousSelectedWeapon = selectedWeapon;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (selectedWeapon >= transform.childCount - 1)
            {
                selectedWeapon = 0;

            }
            else
            {
                selectedWeapon++;
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (selectedWeapon <= 0)
            {
                selectedWeapon = transform.childCount - 1;
            }
            else
            {
                selectedWeapon--;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //Rifle
            selectedWeapon = 0;
        }

        /*if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2)
        {
            //Opcional
            selectedWeapon = 1;
        }*/

        if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2)
        {
            //Pistola. Siempre al final. Así se sacará moviendo la rueda del ratón hacia abajo.
            selectedWeapon = 1;
        }

        if (previousSelectedWeapon != selectedWeapon)
        {
            SelectWeapon();
        }
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }

            i++;
        }
    }
}
