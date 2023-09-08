using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Podr�a hacer un shader para el sprite del c�rculo que se achica y desde un Update cambiar la variable del shader que controla el tama�o del c�rculo
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
        if (!waitingForBeat)
            return;

        if (canHitBeat)
            BeatSucceed();
        else
            BeatFailed();

    }
    void BeatFailed()
    {
        //print("Fallaste el beat pa");
        //El anillo exterior se pone en rojo
        //El bot�n queda presionado
    }

    void BeatSucceed()
    {
        //print("Godeano");
        //El anillo exterior se pone en verde y se detiene el decrecimiento
    }

    IEnumerator RingColorChange(Color newColor)
    {
        outerRingSR.material.SetColor("_RingColor", newColor);
        yield return new WaitForSeconds(2f);

        WaitingForBeat = false;

    }
    public IEnumerator WaitForBeat(float beatDuration)
    {
        WaitingForBeat = true;

        outerRingSR.transform.localScale = new Vector3(outerRingScale, outerRingScale, outerRingScale);
        outerRingSR.material.SetColor("_RingColor", unhighlightedRingColor);
        canHitBeat = false;

        float timer = 0;

        while (timer < beatDuration)
        {
            yield return new WaitForEndOfFrame();

            float currOuterRingScale = outerRingScale / (beatDuration * beatDuration) * ((timer - beatDuration) * (timer - beatDuration));
            outerRingSR.transform.localScale = new Vector3(currOuterRingScale, currOuterRingScale, currOuterRingScale);
            timer += Time.deltaTime;

            if (canHitBeat)
                continue;

            if (outerRingSR.transform.localScale.magnitude <= transform.localScale.magnitude * clickSpareRange)
            {
                outerRingSR.material.SetColor("_RingColor", highlightedRingColor);
                canHitBeat = true;
            }
        }
        if (waitingForBeat)
        {
            BeatFailed();
            WaitingForBeat = false;

        }
    }
}
