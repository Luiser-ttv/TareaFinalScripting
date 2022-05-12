using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curacion : MonoBehaviour
{
    public Personaje personaje;

    public GameManager gameManager;

    public int curacionesRest = 5;

    public int curaRecibida = 25;


    private void Awake()
    {
       
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        personaje = GetComponent<Personaje>();
    }

    public void Curarse()
    {

        if (personaje.saludActual == personaje.saludInicial)
        {
            gameManager.SiguienteTurno();
        }
        else if (curacionesRest > 0)
        {
            Debug.Log("ENTRA");
            personaje.saludActual += curaRecibida;
            if (personaje.saludActual > 100)
            {
                personaje.saludActual = 100;
            }
            curacionesRest -= 1;
            StartCoroutine(Curaciones());
            StartCoroutine(MostrarCura(curaRecibida));
            personaje.ActualizarBarraSalud();
        }
        else if (curacionesRest <= 0)
        {
            gameManager.SiguienteTurno();
        }


    }

    IEnumerator MostrarCura(int num)
    {
        personaje.textoGolpeRecibido.text = "+" + num.ToString();
        yield return new WaitForSeconds(2);
        personaje.textoGolpeRecibido.text = "";
    }

    IEnumerator Curaciones()
    {
        GetComponent<SpriteRenderer>().color = Color.green;
        yield return new WaitForSeconds(2);
        gameManager.SiguienteTurno();
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
