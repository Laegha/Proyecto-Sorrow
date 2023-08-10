using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    InputManager inputManager;
    [SerializeField] float playerSpeed;
    void Start() => inputManager = FindObjectOfType<InputManager>();
    void Update()
    {
        Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"),0 , Input.GetAxisRaw("Vertical"));
        if (movement != Vector3.zero)
            transform.Translate(movement.normalized * playerSpeed * Time.deltaTime);
    }
}
