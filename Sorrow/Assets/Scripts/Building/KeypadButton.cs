using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Podría hacer un shader para el sprite del círculo que se achica y desde un Update cambiar la variable del shader que controla el tamaño del círculo
public class KeypadButton : MonoBehaviour
{
    [HideInInspector] public KeyPadInteractable keyPadInteractable;

    bool waitingForBeat;
    bool canHitBeat;

    [SerializeField] float outerRingScale;
    [SerializeField] float clickSpareRange;

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

        waitingForBeat = false;
    }

    public IEnumerator WaitForBeat(float beatDuration)
    {
        waitingForBeat = true;

        ringSR.enabled = true;
        outerRingSR.enabled = true;
        outerRingSR.transform.localScale = new Vector3(outerRingScale, outerRingScale, outerRingScale);

        while(beatDuration > 0)
        {
            yield return new WaitForEndOfFrame();
            
            outerRingSR.transform.localScale -= new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime) * 1 / beatDuration;
            beatDuration -= Time.deltaTime;

            if (outerRingSR.transform.localScale.magnitude <= transform.localScale.magnitude * clickSpareRange && !canHitBeat)
                canHitBeat = true;

        }
    }
}
