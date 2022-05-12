using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A?adir al GameObject del villano
public class AtaqueCargado : MonoBehaviour
{
    // Cuanto da?o hace este ataque
    public int ataque = 25;

    // Si esta ya o no cargado
    public bool cargado = false;

    // Referencia al GameManager para informar sobre fin de turno
    public GameManager gameManager;

    // Cuando empieza el juego
    private void Awake()
    {
        cargado = false;
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
    }

    // Comprobamos primero si esta cargado o no
    // Necesitamos recibir como parametro al personaje que estamos atacando
    // En el caso del heroe controlador por el jugador, se llamaria desde un boton en la interfaz
    // Si fuera el enemigo, el componente controlador elegiria realizar este ataque
    public void Atacar(Personaje aQuienAtaco, Personaje golpePropio)
    {
        if (!cargado)
        {
            Debug.Log("El ataque todavia no esta cargado");
            // Lo indicamos como cargado
            cargado = true;
            // Cambiamos el color para indicar que esta cargado
            GetComponent<SpriteRenderer>().color = Color.red;
            // Pasamos al siguiente turno
            gameManager.SiguienteTurno();
        }
        else
        {
            // Si ya esta cargado lo realizamos
            Debug.Log("Realizo el ataque cargado");
            cargado = false;
            StartCoroutine(Ataque(aQuienAtaco, golpePropio));
        }
    }

    // Corutina que representa el ataque cargado
    IEnumerator Ataque(Personaje aQuienAtaco, Personaje golpePropio)
    {
        aQuienAtaco.Recibir(ataque);
        golpePropio.Recibir(ataque);
        yield return new WaitForSeconds(2);
        // Cambiamos el color para indicar que ya no esta cargado
        GetComponent<SpriteRenderer>().color = Color.white;
        gameManager.SiguienteTurno();
    }
}
