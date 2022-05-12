using System.Collections;
using UnityEngine;

// Componente que se encarga de escoger las acciones del villano
public class Controlador : MonoBehaviour
{
    // A quien va a dirigir sus ataques
    public Personaje personajeEnemigo;

    // El personaje que estoy controlando, que sera el villano
    public Personaje miPersonaje;

    // Componente que gestiona el juego
    public GameManager gameManager;

    public AtaqueCargado ataqueCargado;

    int rndNum;

    // Al comenzar el juego
    private void Awake()
    {
        // Cojo la referencia a mi componente de Personaje
        miPersonaje = GetComponent<Personaje>();
        // Cojo la referencia al GameManager
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        ataqueCargado = GameObject.FindGameObjectWithTag("Villain").GetComponent<AtaqueCargado>();
    }

    // Metodo que escoge y realiza una accion
    public void EscogerAccion()
    {
        // Ejercicio 2: A?adir un sistema de eleccion Random
        rndNum = Random.Range(1, 4);
        if (ataqueCargado.cargado == true)
        {
            rndNum = 3;
        }
        if (personajeEnemigo.saludActual < 20 || miPersonaje.saludActual < 20)
        {
            rndNum = Random.Range(1, 2);
        }
        if (personajeEnemigo.defendiendo)
        {
            rndNum = 1;
        }
        switch (rndNum)
        {
            case 1:
                miPersonaje.Atacar(personajeEnemigo);
                break;
            case 2:
                miPersonaje.Defender();
                break;
            case 3:
                miPersonaje.GetComponent<AtaqueCargado>().Atacar(personajeEnemigo, miPersonaje);
                break;
            case 4:
                miPersonaje.GetComponent<Curacion>().Curarse();
                break;
            default:
                break;
        }
       
    }
}
