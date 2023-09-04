using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Podría hacer un shader para el sprite del círculo que se achica y desde un Update cambiar la variable del shader que controla el tamaño del círculo
public class KeypadButton : MonoBehaviour
{
    [HideInInspector] public KeyPadInteractable keyPadInteractable;

    bool waitingForBeat;

    private void OnMouseDown()
    {
        print("Clickeaste " + name);
        if (!waitingForBeat)
            return;
        //Dar puntos si acerto el beat
        waitingForBeat = false;
    }

    public void WaitForBeat(float beatDuration)
    {
        waitingForBeat = true;

        //Generar el aro negro

    }
}
