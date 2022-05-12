using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Personaje : MonoBehaviour
{
    // Nombre para identificar al personaje
    public string nombre;

    // Necesitamos una variable para guardar la vida maxima
    public int saludInicial;

    // Necesitamos una variable para guardar la vida actual
    public int saludActual;

    // Variable para el ataque
    public int ataque;

    // Variable para la defensa
    public int defensa;

    // Variable para indicar si se defiende
    public bool defendiendo = false;

    // Referencia al transform de la barra de salud
    public RectTransform barraSalud;

    // Referencia al GameManager
    public GameManager gameManager;

    // Referencia al texto donde se muestra el da?o
    public Text textoGolpeRecibido;

    // Referenci para el color de la muerte
    public Color MuerteColor;

    public int danoCalculado;

    public int curacionesRest = 5;

    public int curaRecibida = 25;

    public AudioSource sonidoAtaque;

    bool estaMuerto = false;

    public SpriteRenderer spritePersonaje;

    // Cuando empieza el juego
    private void Awake()
    {
        saludActual = saludInicial;
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        spritePersonaje = gameObject.GetComponent<SpriteRenderer>();
    }

    // Iniciamos un ataque
    public void Atacar(Personaje oponente)
    {
        StartCoroutine(Ataque(oponente));
    }

    // Corutina que representa un ataque
    IEnumerator Ataque(Personaje oponente)
    {
        oponente.Recibir(ataque);
        yield return new WaitForSeconds(2);
        gameManager.SiguienteTurno();
    }

    // Cuando recibimos un golpe hay que actualizar la interfaz
    public void Recibir(int ataqueRecibido)
    {
        int rndCrit = Random.Range(1, 5);
        int rndRest = Random.Range(1, 5);
        // Calculamos el da?o
        danoCalculado = ataqueRecibido - defensa + rndCrit - rndRest;
        if (defendiendo)
        {
            danoCalculado = danoCalculado - defensa;
            // Si recibimos un ataque, nos dejamos de defender para el siguiente
            DejarDefender();
        }
        // Nos restamos la vida si nos hace da?o
        if(danoCalculado > 0)
            saludActual -= danoCalculado;
        // Comprobamos que no baje de 0
        if (saludActual < 0)
            saludActual = 0;
        // Actualizamos la barra de salud
        ActualizarBarraSalud();

        // Ejercicio 5: Cambiar el color del sprite a negro si un personaje muerte
        if (this.SigueVivo())
        {
            SpriteRenderer personajeColor = GetComponent<SpriteRenderer>();
            personajeColor.color = MuerteColor;
        }

        // Mostramos el da?o recibido
        StartCoroutine(MostrarGolpe(danoCalculado));
    }

    // Mostramos un numero encima de la barra
    IEnumerator MostrarGolpe(int num)
    {
        textoGolpeRecibido.text = num.ToString();
        yield return new WaitForSeconds(2);
        textoGolpeRecibido.text = "";
    }


    // Nos defendemos
    public void Defender()
    {
        // Si no nos estabamos defendiendo
        if(!defendiendo)
        {
            defendiendo = true;
            StartCoroutine(Defensa());
        }
        // Si ya nos estabamos defendiendo
        else
        {
            gameManager.SiguienteTurno();
        }
    }

    // Mostramos la defensa como un cambio de color
    IEnumerator Defensa()
    {
        GetComponent<SpriteRenderer>().color = Color.grey;
        yield return new WaitForSeconds(2);
        gameManager.SiguienteTurno();
    }

    // Al dejar de defender tenemos que restaurar el estado anterior
    public void DejarDefender()
    {
        if (defendiendo)
        {
            defendiendo = false;
            GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    // Ejercicio 3: A?adir un metodo que devuelva si un personaje esta muerto o no
    // Pista: public bool ...
    public bool SigueVivo()
    {
        if (saludActual <= 0)
        {
            estaMuerto = true;
        }
        return estaMuerto;
    }

    public void EjecutarSonidoAtaque()
    {
        sonidoAtaque.PlayOneShot(sonidoAtaque.clip);
    }

    // Actualiza la barra de salud segun el porcentaje que tiene
    public void ActualizarBarraSalud()
    {
        // Calculamos el nuevo tama?o de la barra de salud
        float xNewHealthScale = ((float)saludActual / saludInicial);
        barraSalud.localScale = new Vector2(xNewHealthScale, barraSalud.localScale.y);
    }

}
