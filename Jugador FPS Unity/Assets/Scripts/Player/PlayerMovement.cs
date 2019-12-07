using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Este Script comprende todas las acciones que puede hacer el personaje en nuestro videojuego.
 * Se ha hecho mediante métodos, los cuales son llamados en el Update() cuando sea necesaria su ejecución.
 * El jugador es capaz de: moverse, correr, saltar y agacharse.
 * 
 * **********************************************
 * **** Script creado por Dejan Polit Andrés ****
 * **********************************************
 */

public class PlayerMovement : MonoBehaviour
{
    CharacterController character; //Inicializamos nuestro personaje.
    CapsuleCollider charColision; //Inicializamos la colisión de nuestro personaje.

    //Variables de movimiento
    Vector3 movement = Vector3.zero; //Inicializamos el movimiento.
    float moveHorizontal; //Guarda el valor del movimiento Horizontal para que puedan acceder los demás metodos.
    float moveVertical; //Guarda el valor del movimiento Vertical para que puedan acceder los demás metodos.

    public float speed; //Velocidad del personaje.
    public float jumpPower = 8f; //Potencia ocn la que saltará el personaje.
    public float gravity = 25f; //Gravedad con la que queremos que el objeto caiga hacia el suelo.

    //Variables de acciones
    bool jump; //Comprueba si el personaje está saltando.
    bool sprint; //Comprueba si el personaje está corriendo.
    bool crouch; //Comprueba si el personaje está agachado.
    bool crouchArea; //Comprueba si el personaje esta en una zona donde tiene que estar agachado.

    //Altura del personaje
    float charHeight; //Guarda el valor de la altura original del personaje.
    float crouchHeight = 1.5f; //El valor que queremos que tenga la altura cuando el personaje este agachado.


    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>(); //Selecciona al personaje.
        charColision = GetComponent<CapsuleCollider>(); //Selecciona la colisión.
        charHeight = character.height; //Le damos el valor que tenemos asignado en Unity.
    }

    // Update is called once per frame
    void Update()
    {
        //Variables para el movimiento
        moveHorizontal = Input.GetAxis("Horizontal"); //Guardamos el movimiento horizontal (Derecha 1 | Izquierda -1).
        moveVertical = Input.GetAxis("Vertical"); //Guardamos el movimeinto vertical (Arriba 1 | Abajo -1).

        //Metodos
        Move();
        Crouch(); //Crouch es capaz de detectar por si solo si el personaje esta agachado o no.

        if (!sprint) //Ejecuta el método Sprint() cuando el personaje no está corriendo.
        {
            Sprint();
        }

        if (!jump) //Ejecuta el método Jump() cuando el personaje no está saltando. Con esto conseguimos que no hayan saltos infinitos.
         {
            Jump();
         }

    }

    /*
     * Este método sirve para obtener el movimiento del personaje
     * 
     * Le asignamos una velocidad con la variable "speed" para que después el personaje obtenga el valor y se mueva a esa velocidad.
     * Si el personaje está en el suelo creamos el movimiento a partir de un Vector llamado "movement". Las velocidades del Vector estan comprendidas entre -1 y 1. (Multiplicandolo por "speed" aumentamos la velocidad)
     * Creamos unos IF para que la variable "speed" sea distinta según si está corriendo o agachado.
     * Multiplicamos "movement" por la "speed" que obtenemos para hacer que el personaje se mueva mas rápido.
     * 
     * Le añadimos la gravedad.
     * Le indicamos que el movimiento del CharacterController sea el mismo que el Vector "movement" y lo multiplicamos por deltaTime para asegurarnos que va a ser la misma velocidad independientemente dle frame rate.
     */
    void Move()
    {
        speed = 4f; //Le asignamos la velocidad al andar.

        if (character.isGrounded)
        {
            movement = new Vector3(moveHorizontal, 0f, moveVertical); //Creamos el movimiento (La velocidad es -1 0 1).
            movement = transform.TransformDirection(movement); //Transformamos la dirección del movimiento hacia el lugar donde esté mirando el cursor.

            //Velocidades
            if (sprint)
            {
                speed *= 1.5f; //Multipla la velocidad del perosnaje si sprint es true.
            }

            if (crouch)
            {
                speed /= 5f; //Divide entre 5 la velocidad del personaje si crouch es true.
            }

            movement *= speed; //Le pasamos la velocidad al movimiento para que se mueva a la velocidad de "Speed".
        }

        //Gravedad
        movement.y -= gravity * Time.deltaTime; //Empuja al jugador hacia el suelo.

        character.Move(movement * Time.deltaTime); //Nos aseguramos que ande igual independientemente del frame rate.
    }

    /*
     * Este método sirve para que el personaje corra.
     * 
     * Comprobamos que el personaje al pulsar la tecla "Shift Izquierdo" no esté corriendo hacia atrás ni este agachado.
     * Le asignamos un valor a la variable "sprint" para que pueda entrar en los IF de los métodos Update() y Move().
     * Solo si el jugador está en el suelo le pasamos el método Move() para que pueda variar su velocidad.
     */
    void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift) && moveVertical != -1 && !crouch) //Solo correrá cuando el personaje no ande hacia atrás ni esté agachado.
        {
            sprint = true; //Le damos valor a "sprint". Lo necesitamos para que Move() aumente su velocidad.
            if (character.isGrounded)
            {
                Move(); //Llamamos al método Move() para entre en el IF para cambiar la velocidad.
            }  
        }
        sprint = false;
    }

    /*
     * Este método sirve para que el personaje salte.
     * 
     * Le asignamos un valor a la variable "jump" para que pueda entrar en el IF del Update().
     * Comprobamos que al pulsar la tecla "Espacio" el personaje no esté agachado.
     * Para evitar que el personaje salte infinitamente utilizamos el isGrounded.
     */
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !crouch) //Solo saltará cuando el personaje no esté agachado.
        {
            jump = true;
            if (character.isGrounded) //Con esto evitamos que haga saltos infinitos ya que solo podrá saltar cuando toque el suelo.
            {
            movement.y += jumpPower; //Saltará, se moverá en el eje Y la suma de la potencia que indiquemos para el salto.
            
            character.Move(movement * Time.deltaTime); //Se lo volvemos a pasar para recalcular la posición cuando salta y asegurarnos el frame rate.
            }
        }
        jump = false;
    }

    /*
     * Este método sirve para que el personaje se agachase.
     * 
     * Con esto el personaje cambiará su altura cuando mantengamos pulsado el botón "Ctrl Izquierdo" o la variable "crouchArea" sea true.
     * La variable "crouchArea" se activa con el método OnTriggerStay y se desactiva con el OnTriggerExit.
     * Cuando ninguna de estas 2 acciones es activada volverá (o se mantendrá) en su estado original.
     */
    void Crouch()
    {
        if (Input.GetKey(KeyCode.LeftControl) | crouchArea)
            {
            if (character.isGrounded)
            {
                crouch = true; //Le damos un valor. La necesitamos para que Move() baje la velocidad.
                character.height = Mathf.Lerp(character.height, crouchHeight, Time.deltaTime * 80f); //Reduce la altura de nuestro personaje de una forma suave.
                charColision.height = Mathf.Lerp(charColision.height, crouchHeight, Time.deltaTime * 80f); //Reduce la altura de colisión de nuestro personaje de una forma suave.

                //El Mathf.Lerp funciona de la siguiente forma (valor actual, valor que queremos, velocidad en la que se ejecuta).
            }
        }
        else
        {
            //Vuelve a su estado original
            crouch = false;
            character.height = Mathf.Lerp(character.height, charHeight, Time.deltaTime * 30f); //Aumenta la altura de nuestro personaje de una forma suave.
            charColision.height = Mathf.Lerp(charColision.height, charHeight, Time.deltaTime * 30f); //Aumenta la altura de colisión de nuestro personaje de una forma suave.

            //El Mathf.Lerp funciona de la siguiente forma (valor actual, valor que queremos, velocidad en la que se ejecuta).
        }
    }

    /*
     * Al entrar a una zona con el tag "crouchArea" el personaje se mantendrá agachado.
     * 
     * Le indicamos que la variable "crouchArea" es true porque ha entrado en la zona donde queremos que se mantenga agachado.
     * Le pasamos el método Crouch() ya que es el que hace que la altura del personaje cambie y, en este caso, no varie gracias al crouchArea = true.
     */
    private void OnTriggerStay(Collider obstacle)
    {
        if (obstacle.gameObject.CompareTag("crouchArea")) //Comprueba si el jugador entra en el trigger de la colisión "crouchArea"
        {
            crouchArea = true; //Le da el valor true para indicar que está dentro del area donde se debe mantener agachado el personaje.
            Crouch(); //Llama al método Crouch() para que compruebe la variable "crouchArea".
        } 
    }

    /*
     * Al salir de la zona con el tag "crouchArea" el personaje se levantará.
     * 
     * Le indicamos que la variable "crouchArea" es false porque ha salido de la zona donde queremos que se mantenga agachado.
     * Le pasamos el método Crouch() ya que es el que hace que la altura del personaje cambie y, en este caso, se levante gracias al crouchArea = false.
     */
    private void OnTriggerExit(Collider obstacle)
    {
        if (obstacle.gameObject.CompareTag("crouchArea"))
        {
            crouchArea = false; //Le da el valor false para indicar que el personaje ya no está dentro del area.
            Crouch(); //Llama al método Crouch() para que compruebe la variable "crouchArea".
        }
    }
}
