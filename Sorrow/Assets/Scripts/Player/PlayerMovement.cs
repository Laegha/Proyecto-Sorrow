using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    InputManager inputManager;
    [SerializeField] float playerSpeed;
    // Start is called before the first frame update
    void Start()
    {
        inputManager = FindObjectOfType<InputManager>();
    }
    void Update()
    {
        Vector3 movement = new Vector3(Input.GetAxis(inputManager.xMovementAxis),0 , Input.GetAxis(inputManager.zMovementAxis));
        if (movement != Vector3.zero)
            transform.Translate(movement);
    }
}
