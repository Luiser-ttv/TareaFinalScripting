using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recarga : MonoBehaviour
{
    public Personaje personaje;

    public AtaqueMagico ataqueMagico;

    public GameManager gameManager;

    public int recargasRest = 3;

    public int recargaRecibida = 10;


    private void Awake()
    {

        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        personaje = GetComponent<Personaje>();
        ataqueMagico = GetComponent<AtaqueMagico>();
    }

    public void Recargarse()
    {

        if (ataqueMagico.puntosMagia == ataqueMagico.puntosMagiaIniciales)
        {
            gameManager.SiguienteTurno();
        }
        else if (recargasRest > 0)
        {
            //Debug.Log("ENTRA");
            ataqueMagico.puntosMagia += recargaRecibida;
            if (ataqueMagico.puntosMagia > 50)
            {
                ataqueMagico.puntosMagia = 50;
            }
            recargasRest -= 1;
            StartCoroutine(Recargas());
            StartCoroutine(MostrarRecarga(recargaRecibida));
            ataqueMagico.ActualizarBarraMagia();
        }
        else if (recargaRecibida <= 0)
        {
            gameManager.SiguienteTurno();
        }


    }

    IEnumerator MostrarRecarga(int num)
    {
        personaje.textoGolpeRecibido.text = "+" + num.ToString();
        yield return new WaitForSeconds(2);
        personaje.textoGolpeRecibido.text = "";
    }

    IEnumerator Recargas()
    {
        GetComponent<SpriteRenderer>().color = Color.yellow;
        yield return new WaitForSeconds(2);
        gameManager.SiguienteTurno();
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}