using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    public Personaje personaje;

    public GameManager gameManager;

    int esquivarRnd;

    private void Awake()
    {

        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        personaje = GetComponent<Personaje>();

    }

    public void Esquivar()
    {
        esquivarRnd = Random.Range(1, 4);

        if (esquivarRnd == 2)
        {
            personaje.saludActual += personaje.danoCalculado;
        }
        else
        {
            gameManager.SiguienteTurno();
        }
    }
}
