using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Podría hacer un shader para el sprite del círculo que se achica y desde un Update cambiar la variable del shader que controla el tamaño del círculo
public class KeypadButton : MonoBehaviour
{
    [HideInInspector] public KeyPadInteractable keyPadInteractable;

    bool waitingForBeat;
    bool WaitingForBeat
    {
        set
        {
            waitingForBeat = value;

            ringSR.enabled = value;
            outerRingSR.enabled = value;
            if(value)
                outerRingSR.transform.localScale = new Vector3(outerRingScale, outerRingScale, outerRingScale);
        }
    }
    bool canHitBeat;

    [SerializeField] float outerRingScale;
    [SerializeField] float clickSpareRange;

    [SerializeField] Color unhighlightedRingColor;
    [SerializeField] Color highlightedRingColor;
    [SerializeField] SpriteRenderer outerRingSR;
    SpriteRenderer ringSR;

    private void Start() => ringSR = GetComponent<SpriteRenderer>();

    private void OnMouseDown()
    {
        print("Clickeaste " + name);
        if (!waitingForBeat)
            return;

        if (canHitBeat)
            print("Acertaste el beat");
        else
            print("Fallaste el beat");

        WaitingForBeat = false;
    }

    public IEnumerator WaitForBeat(float beatDuration)
    {
        WaitingForBeat = true;

        outerRingSR.material.SetColor("_RingColor", unhighlightedRingColor);
        canHitBeat = false;

        while (beatDuration > 0)
        {
            yield return new WaitForEndOfFrame();
            
            outerRingSR.transform.localScale -= new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime) * 1 / beatDuration;
            beatDuration -= Time.deltaTime;

            if (outerRingSR.transform.localScale.magnitude <= 0)
            {
                //Fallar
                yield return null;
            }
            
            if (outerRingSR.transform.localScale.magnitude <= transform.localScale.magnitude * clickSpareRange && !canHitBeat)
            {
                outerRingSR.material.SetColor("_RingColor", highlightedRingColor);
                canHitBeat = true;
            }
        }
        WaitingForBeat = false;
    }
}
