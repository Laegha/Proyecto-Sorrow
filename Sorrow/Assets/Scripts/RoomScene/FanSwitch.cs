using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class FanSwitch : HitableTarget
{
    [SerializeField] GameObject button;
    [SerializeField] UnityEvent endActions;
    MeshRenderer buttonMR;
    PlayableDirector playableDirector;
    AudioSource audioSource;

    void Awake()
    {
        buttonMR = button.GetComponent<MeshRenderer>();
        playableDirector = GetComponent<PlayableDirector>();
        audioSource = GetComponent<AudioSource>();
    }

    public override void Activate()
    {
        button.transform.rotation = Quaternion.Euler(0f, -button.transform.rotation.eulerAngles.y, 0f);
        buttonMR.material.DisableKeyword("_EMISSION");
        audioSource.Play();
        playableDirector.Play();
        playableDirector.stopped += End;
    }

    void End(PlayableDirector _)
    {
        endActions.Invoke();
        Destroy(this);
    }
}
