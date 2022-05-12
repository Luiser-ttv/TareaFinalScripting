using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CargadorEscena : MonoBehaviour
{
    public enum DIFICULTAD
    {
        FACIL,
        NORMAL,
        DIFICIL
    }
    public void CargarEscena(int numeroEscena)
    {
        //DontDestroyOnLoad(this.gameObject);
        SceneManager.LoadScene(numeroEscena);
    }

    public void SeleccionarDificultad(int dificultad)
    {
        SeleccionarDificultad((DIFICULTAD)dificultad);
    }

    public void SeleccionarDificultad(DIFICULTAD dificultad)
    {
        PlayerPrefs.SetInt("dificultad", (int)dificultad);
    }
}
