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
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"),0 , Input.GetAxis("Vertical"));
        if (movement != Vector3.zero)
            transform.Translate(movement * playerSpeed * Time.deltaTime);
    }
}
