using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static CargadorEscena;

// Ejercicio 10: Cambiar los sprites para personalizar el juego

// Se encarga de gestionar las reglas del juego, como los turnos
public class GameManager : MonoBehaviour
{
    // Contador de turnos
    public int turno = 1;

    // Personaje que representa al heroe
    public Personaje heroe;

    // Personaje que representa al villano
    public Personaje villano;

    // Componente que controla las acciones del villano
    public Controlador controladorVillano;

    // Referencia al texto central
    public Text textoJuego;

    // Referencia al texto del panel 
    public Text textoRestart;

    // Referencia al texto del heroe
    public Text textoHeroe;

    // Referencia al texto del villano
    public Text textoVillano;

    // Referencia al panel de restart
    public GameObject panelRestart;

    // Interfaz del jugador
    public GameObject interfazJugador;

    // Interfaz para los turnos
    public Text textoTurnos;

    //Booleano para reiniciar el juego
    private bool restart = false;

    public GameObject iconoBoost;

    private SpriteRenderer Background;

    // Posibles estados del juego
    public enum GameState
    {
        INTRODUCCION,
        HEROE,
        VILLANO,
        PERDER,
        GANAR
    }

    // Estado actual del juego
    public GameState estadoJuego;


    public DIFICULTAD dificultadNivel;

    // Al comenzar el juego
    private void Start()
    {
        // Empezamos la introduccion

        estadoJuego = GameState.INTRODUCCION;
        StartCoroutine(Introduccion());

        dificultadNivel = (DIFICULTAD)PlayerPrefs.GetInt("dificultad");



        Debug.Log(dificultadNivel);
        Background = GameObject.FindGameObjectWithTag("Background").GetComponent<SpriteRenderer>();

        if (dificultadNivel == DIFICULTAD.FACIL)
        {
            heroe.saludInicial = 150;
            heroe.saludActual = 150;
            heroe.ataque = 40;
            heroe.defensa = 10;
            heroe.curacionesRest = 7;
            heroe.curaRecibida = 30;

            villano.nombre = "Porki";
            villano.spritePersonaje.sprite = Resources.Load<Sprite>("RPG/monster/boar");
            villano.saludInicial = 100;
            villano.ataque = 15;
            villano.defensa = 5;

            Background.sprite = Resources.Load<Sprite>("RPG/backgrounds/17");
        }
        else if (dificultadNivel == DIFICULTAD.NORMAL)
        {
            heroe.saludInicial = 100;
            heroe.ataque = 30;
            heroe.curacionesRest = 5;
            heroe.curaRecibida = 25;
            heroe.defensa = 7;

            villano.nombre = "Octi";
            villano.spritePersonaje.sprite = Resources.Load<Sprite>("RPG/monster/octopus");
            villano.saludInicial = 120;
            villano.ataque = 25;
            villano.defensa = 7;
        }
        else if (dificultadNivel == DIFICULTAD.DIFICIL)
        {
            heroe.saludInicial = 75;
            heroe.saludActual = 75;
            heroe.ataque = 20;
            heroe.defensa = 5;
            heroe.curacionesRest = 2;
            heroe.curaRecibida = 15;

            villano.nombre = "Dr. Destroyer";
            villano.spritePersonaje.sprite = Resources.Load<Sprite>("RPG/monster/giant");
            villano.saludInicial = 150;
            villano.ataque = 30;
            villano.defensa = 10;

            Background.sprite = Resources.Load<Sprite>("RPG/backgrounds/18");
        }

    }

    private void Update()
    {
        
       if (Input.GetKey(KeyCode.Space) && restart)
       {
        restart = false;
        SceneManager.LoadScene(0);
       }


        textoHeroe.text = "Nombre:" + heroe.nombre + "\nVida:" + heroe.saludActual +
            "\nAtaque:" + heroe.ataque + "\nDefensa:" + heroe.defensa + "\nCuraciones:" + heroe.curacionesRest;

        textoVillano.text = "Nombre:" + villano.nombre + "\nVida:" + villano.saludActual + "\nAtaque:" +
            villano.ataque + "\nDefensa:" + villano.defensa;

        if (turno >= 10 && turno <= 15)
        {
            villano.ataque = 50;
            iconoBoost.SetActive(true);

        }
        else if (villano.ataque == 50)
        {
            iconoBoost.SetActive(false);
            if (dificultadNivel == DIFICULTAD.FACIL)
            {
                villano.ataque = 15;
            }
            else if (dificultadNivel == DIFICULTAD.NORMAL)
            {
                villano.ataque = 25;
            }
            else if (dificultadNivel == DIFICULTAD.DIFICIL)
            {
                villano.ataque = 30;
            }
        }

        if (turno == 30)
        {
            villano.saludActual = villano.saludInicial;
            villano.ActualizarBarraSalud();
            textoJuego.text = "Dr. Zoiberg ha curado al esbirro!";
        }
        else if (turno == 31)
        {
            textoJuego.text = "";
        }


    }

    // Se pasa al siguiente turno
    public void SiguienteTurno()
    {
        // Ejercicio 4: A?adir las condiciones de victoria y perder para los estados
        if (heroe.SigueVivo())
        {
            estadoJuego = GameState.PERDER;
        }
        else if (villano.SigueVivo())
        {
            estadoJuego = GameState.GANAR;
        }
        // Si se cumple la condicion de perder, ponemos este estado
        
        // Si se cumple la condicion de ganar, ponemos este estado
        //

        // Segun el estado de juego
        switch (estadoJuego)
        {
            // Ejercicio 8: A?adir que sea aleatorio quien empieza el combate

            // Si es la introduccion, pasamos al turno del heroe
            case GameState.INTRODUCCION:
                // Ejercicio 1: Mostrar el nombre del heroe
                textoJuego.text = "Turno de " + heroe.nombre; 
                estadoJuego = GameState.HEROE;
                interfazJugador.SetActive(true);
                break;
            // Si es el turno del heroe pasa a ser el del villano
            case GameState.HEROE:
                // Ejercicio 1: Mostrar el nombre del villano
                textoJuego.text = "Turno del " + villano.nombre; 
                turno = turno + 1;
                estadoJuego = GameState.VILLANO;
                // El jugador no puede hacer nada
                interfazJugador.SetActive(false);
                // le indicamos que tiene que hacer algo
                controladorVillano.EscogerAccion();
                break;
            // Si es el turno del villano pasa a ser el del heroe
            case GameState.VILLANO:
                // Ejercicio 1: Mostrar el nombre del heroe
                textoJuego.text = "Turno de " + heroe.nombre;
                turno = turno + 1;
                estadoJuego = GameState.HEROE;
                interfazJugador.SetActive(true);
                break;
            // Si el heroe ha ganado                
            case GameState.GANAR:
                textoJuego.text = "Has ganado!";
                panelRestart.SetActive(true);
                textoRestart.text = "Presiona espacio para volver al menu!";
                restart = true;
                interfazJugador.SetActive(false);
                break;
            // Si el villano ha ganado
            case GameState.PERDER:
                textoJuego.text = "Has perdido...";
                panelRestart.SetActive(true);
                textoRestart.text = "Presiona espacio para volver al menu!";
                restart = true;
                interfazJugador.SetActive(false);
                break;
        }

        // Ejercicio 9: A?adir una caja de texto en la interfaz para mostrar el turno actual
        string turnoTexto = turno.ToString();
        textoTurnos.text = turnoTexto;
    }

    // Introduccion que dura un tiempo
    IEnumerator Introduccion()
    {
        // Ejercicio 6: A?adir una introduccion de varias lineas de textos que pasen con tiempo
        interfazJugador.gameObject.SetActive(false);
        textoJuego.text = "Bienvenidos a la batalla";
        yield return new WaitForSeconds(3);
        textoJuego.text = "Preparar vuestras armas!";
        yield return new WaitForSeconds(3);
        SiguienteTurno();
    }
}
