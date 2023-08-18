using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningManager : MonoBehaviour
{
    /*
    [HideInInspector] public bool isRunning = false;
    bool canRun = true;
    [SerializeField] float maxRunSeconds;
    float currStamina;

    public float runSpeed;

    private void Start() => currStamina = maxRunSeconds;

    void Update()
    {
        if (isRunning)
        {
            currStamina -= Time.deltaTime;
            if (currStamina <= 0)
            {
                canRun = false;
                isRunning = false;
            }
            UIManager.instance.UpdateStaminaBar(currStamina);
        }
        else if(currStamina < maxRunSeconds)
        {
            currStamina += Time.deltaTime;
            UIManager.instance.UpdateStaminaBar(currStamina);
            if (currStamina >= maxRunSeconds)
                canRun = true;
        }

        if (Input.GetKeyDown(InputManager.instance.runKey) && canRun)
            isRunning = true;
        if (Input.GetKeyUp(InputManager.instance.runKey))
            isRunning = false;
    }
    */
}