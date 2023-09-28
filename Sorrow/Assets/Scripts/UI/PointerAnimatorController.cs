using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerAnimatorController : MonoBehaviour
{
    Animator animator;

    void Awake() => animator = GetComponent<Animator>();

    public void SetPointerState(bool state) => animator.SetBool("Show", state);
}
