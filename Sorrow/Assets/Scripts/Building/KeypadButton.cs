using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//Podría hacer un shader para el sprite del círculo que se achica y desde un Update cambiar la variable del shader que controla el tamaño del círculo
public class KeypadButton : MonoBehaviour
{
    [HideInInspector] public KeyPadInteractable keyPadInteractable;

    [SerializeField] SpriteRenderer outerRingSR;
    SpriteRenderer ringSR => GetComponent<SpriteRenderer>();
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

    [SerializeField] float ringDissapearTime;

    ParticleSystem particleSystem => GetComponent<ParticleSystem>();
    private void OnMouseDown()
    {
        if (!waitingForBeat)
            return;

        if (canHitBeat)
            StartCoroutine(BeatSucceed());
        else
            StartCoroutine(BeatFailed());

    }
    IEnumerator BeatFailed()
    {
        //El botón queda presionado
        outerRingSR.material.SetColor("_RingColor", Color.red);
        waitingForBeat = false;
        yield return new WaitForSeconds(ringDissapearTime);

        keyPadInteractable.buttons.Remove(this);
        //keypadInteractable.GetComponent<Renderer>().materials.First(m => m.name == "").SetVectorArray
        if(keyPadInteractable.buttons.Count == 0)
            //reiniciar el minijuego
        WaitingForBeat = false;
    }

    IEnumerator BeatSucceed()
    {
        outerRingSR.material.SetColor("_RingColor", Color.green);
        //particleSystem.Play();
        waitingForBeat = false;
        yield return new WaitForSeconds(ringDissapearTime);
     
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
