using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KeypadButton : MonoBehaviour
{
    [HideInInspector] public KeyPadInteractable keyPadInteractable;

    [SerializeField] SpriteRenderer outerRingSR;
    SpriteRenderer ringSR => GetComponent<SpriteRenderer>();
    [HideInInspector]public bool waitingForBeat;
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

    [SerializeField] int materialIndex;
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
        keyPadInteractable.buttons.Remove(this);

        keyPadInteractable.GetComponent<Renderer>().materials[materialIndex].SetColor("_Color", Color.red);
        if (keyPadInteractable.buttons.Count == 0)
            keyPadInteractable.RestartMinigame();
            //reiniciar el minijuego

            yield return new WaitForSeconds(ringDissapearTime);
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

        while (timer < beatDuration - beatDuration * 0.37)
        {
            yield return new WaitForEndOfFrame();

            if (!waitingForBeat)
                break;

            float currOuterRingScale = outerRingScale / (beatDuration * beatDuration) * ((timer - beatDuration) * (timer - beatDuration));
            outerRingSR.transform.localScale = new Vector3(currOuterRingScale, currOuterRingScale, currOuterRingScale);
            timer += Time.deltaTime;

            if (canHitBeat)
                continue;

            if (new Vector2(outerRingSR.transform.localScale.x, outerRingSR.transform.localScale.y).magnitude <= 2 * clickSpareRange)
            {
                outerRingSR.material.SetColor("_RingColor", highlightedRingColor);
                canHitBeat = true;
            }
        }
        if (waitingForBeat)
        {
            StartCoroutine(BeatFailed());
            WaitingForBeat = false;

        }
    }
}
