using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A?adir al GameObject del heroe y crear el boton en la interfaz
public class AtaqueMagico : MonoBehaviour
{
    // Datos para el ataque magico

    // Con cuantos puntos de magia vamos a empezar
    public int puntosMagiaIniciales = 50;

    // Cuantos puntos de magia tenemos, tiene que empezar con la misma cantidad que los iniciales
    public int puntosMagia = 50;

    // Cuanto da?o hace este ataque
    public int ataque = 50;

    // Cuanto cuesta realizar el ataque magico
    public int coste = 10;

    // Referencia al transform de la barra de magia
    // Esta barra hay que crearla en la escena e indicarla como referencia igual que la de salud
    public RectTransform barraMagia;

    // Referencia al GameManager para informar sobre fin de turno
    public GameManager gameManager;

    public GameObject iconoRecarga;

    // Cuando empieza el juego
    private void Awake()
    {
        puntosMagia = puntosMagiaIniciales;
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
    }

    private void Update()
    {
        if (gameManager.turno >= 20 && puntosMagia < 50)
        {
            iconoRecarga.SetActive(true);
            puntosMagia += Random.Range(1, 5);
            if (puntosMagia > 50)
            {
                puntosMagia = 50;
            }
            ActualizarBarraMagia();
        }
    }

    // Comprobamos el requisito para realizar un ataque magico y si es asi, iniciar la corutina
    // Necesitamos recibir como parametro al personaje que estamos atacando
    // En el caso del heroe controlador por el jugador, se llamara desde un boton en la interfaz
    // Si fuera el enemigo, el componente controlador elegiria realizar este ataque
    public void Atacar(Personaje aQuienAtaco)
    {
        if (puntosMagia >= coste)
        {            
            Debug.Log("Tengo suficientes puntos de magia");
            // Restamos el coste del ataque magico
            puntosMagia = puntosMagia - coste;
            // Actualizamos la interfaz ya que ha cambiado
            ActualizarBarraMagia();
            // Si tenemos puntos de magia realizamos el ataque
            StartCoroutine(Ataque(aQuienAtaco));
            GetComponent<SpriteRenderer>().color = Color.blue;
        }
        else
        {
            // Si no tenemos puntos de magia pasamos al siguiente turno
            Debug.Log("No tengo suficientes puntos de magia");
            gameManager.SiguienteTurno();
        }
    }

    // Corutina que representa el ataque magico
    IEnumerator Ataque(Personaje aQuienAtaco)
    {
        aQuienAtaco.Recibir(ataque);
        yield return new WaitForSeconds(2);
        GetComponent<SpriteRenderer>().color = Color.white;
        gameManager.SiguienteTurno();
    }

    // Actualiza la barra de magia segun el porcentaje que tiene
    public void ActualizarBarraMagia()
    {
        // Calculamos el nuevo tama?o de la barra de magia
        float xNewHealthScale = ((float)puntosMagia / puntosMagiaIniciales);
        barraMagia.localScale = new Vector2(xNewHealthScale, barraMagia.localScale.y);
    }
}
