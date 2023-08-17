using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    RunningManager runningManager;

    [SerializeField] float playerSpeed;

    void Start() => runningManager = FindObjectOfType<RunningManager>();
    void Update()
    {
        float speed = runningManager.isRunning ? runningManager.runSpeed : playerSpeed;
        Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"),0 , Input.GetAxisRaw("Vertical"));
        if (movement != Vector3.zero)
            transform.Translate(movement.normalized * speed * Time.deltaTime);
    }
}
