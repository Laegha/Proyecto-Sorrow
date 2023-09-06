using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PantallaOpciones : MonoBehaviour
{
    [SerializeField] GameObject pantallaInicio;

    //private void Awake()
    //{
    //    pantallaInicio = GameObject.Find("PantallaInicio");
    //}
    public void BackButton()
    {
        gameObject.SetActive(false);
        pantallaInicio.SetActive(true);
    }
}
