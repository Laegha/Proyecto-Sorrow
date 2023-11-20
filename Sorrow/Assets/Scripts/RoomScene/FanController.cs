using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class FanController : MonoBehaviour
{
    [SerializeField] float acceleration;
    //[SerializeField] float spinsToThrowMonster;
    [SerializeField] PlayableDirector timeline;
    [SerializeField] Animator armShrinkAnimator;
    [HideInInspector] public bool lapDone = false;

    public void StartSpinning() => StartCoroutine(Spin());

    IEnumerator Spin()
    {
        timeline.Pause();
        float spinCheckpoint = transform.rotation.eulerAngles.y;
        //int spinsDone = 0;

        Animator animator = GetComponent<Animator>();
        animator.Play("Spin");
        float speed = 0;

        armShrinkAnimator.Play("Shrink");
        while(speed < 1)
        {
            speed += Time.deltaTime * acceleration;
            animator.SetFloat("SpinSpeed", speed);

            yield return new WaitForEndOfFrame();
        }
        
        float prevRotation = transform.rotation.eulerAngles.y;
        while(!lapDone)
        {
            yield return new WaitForEndOfFrame();
        }
        timeline.Resume();

        //while (spinsDone > spinsToThrowMonster)
        //{
        //    if (prevRotation < spinCheckpoint && transform.rotation.y > spinCheckpoint || prevRotation > spinCheckpoint && transform.rotation.y < spinCheckpoint)
        //        spinsDone++;

        //    prevRotation = transform.rotation.y;
        //    yield return new WaitForEndOfFrame();
        //}
    }
}
