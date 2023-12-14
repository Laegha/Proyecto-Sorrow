using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KeypadButton : ActionInteractable
{
    [SerializeField] float beatAnticipation, clickSpareTime, ringDisappearTime;
    [SerializeField] Color unhighlightedRingColor, highlightedRingColor, pushedInColor;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip errorClip;
    [HideInInspector] public bool waitingForBeat;
    SpriteRenderer outerRingSR, innerRingSR;
    Material ringMaterial;
    Color oldRingColor;
    float timer, speedMultiplier;
    Vector3 outerRingInit;
    public static event System.EventHandler OnMiss;
    bool IsInTime => Mathf.Abs(timer - beatAnticipation) < clickSpareTime;


    protected override void Awake()
    {
        base.Awake();
        var renderer = GetComponent<Renderer>();
        ringMaterial = renderer.materials[1];
        outerRingSR = transform.Find("outerRing").GetComponent<SpriteRenderer>();
        innerRingSR = transform.Find("innerRing").GetComponent<SpriteRenderer>();
        outerRingInit = outerRingSR.transform.localScale;
        var delta = outerRingInit.x - 1f;
        var deltaPercent = delta / outerRingInit.x;
        speedMultiplier = deltaPercent / beatAnticipation;
    }

    void OnEnable() => ringMaterial.SetColor("_Color", oldRingColor);

    public override void Interaction()
    {
        base.Interaction();

        if (waitingForBeat && IsInTime)
            Success();
        else
            Fail();

        waitingForBeat = false;
    }

    void Update()
    {
        if (!waitingForBeat)
            return;

        timer += Time.deltaTime * speedMultiplier;
        outerRingSR.transform.localScale = Vector3.Lerp(outerRingInit, Vector3.zero, timer);
        outerRingSR.material.SetColor("_RingColor", IsInTime ? highlightedRingColor : unhighlightedRingColor);

        if (timer < 1f)
            return;

        waitingForBeat = false;
        Fail();
    }

    public void WaitForBeat()
    {
        timer = 0f;
        waitingForBeat = innerRingSR.enabled = outerRingSR.enabled = true;
        outerRingSR.transform.localScale = outerRingInit;
    }

    void Success()
    {
        outerRingSR.material.SetColor("_RingColor", oldRingColor);
        Invoke(nameof(DisableRings), ringDisappearTime);
    }

    void Fail()
    {
        OnMiss?.Invoke(this, System.EventArgs.Empty);
        outerRingSR.material.SetColor("_RingColor", pushedInColor);
        ringMaterial.SetColor("_Color", pushedInColor);
        audioSource.PlayOneShot(errorClip);
        Invoke(nameof(DisableRings), ringDisappearTime);
        enabled = false;
    }

    void DisableRings() => innerRingSR.enabled = outerRingSR.enabled = false;

    public void RestoreColor() => ringMaterial.SetColor("_Color", oldRingColor);
}
