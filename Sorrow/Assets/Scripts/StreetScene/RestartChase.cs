using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using Cinemachine;
using UnityEngine.PlayerLoop;

public class RestartChase : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera monsterCamera;
    [SerializeField] float darknessSpeed;
    [SerializeField] float darknessDelay = 2f;
    [SerializeField] Image blackScreen;
    bool isRestarting = false;
    float timer = 0f;

    void OnTriggerEnter(Collider col)
    {
        if (!col.CompareTag("Player"))
            return;

        BreakingBlock.RefreshPlayer();

        ChaseController chaseController = GetComponent<ChaseController>();
        if (chaseController)
            chaseController.enabled = false;

        // Disable player movement
        InputManager inputManager = col.GetComponent<InputManager>();
        if (inputManager)
            inputManager.enabled = false;

        // Change camera
        if (monsterCamera)
            CinematicManager.instance.CameraChange(monsterCamera);

        // Fade to black
        blackScreen.gameObject.SetActive(true);
        isRestarting = true;
    }

    void Update()
    {
        if (!isRestarting)
            return;

        timer += Time.deltaTime * darknessSpeed;
        blackScreen.color = new(0f, 0f, 0f, Mathf.Clamp01(timer));

        if (timer < 1f)
            return;

        isRestarting = false;
        Invoke(nameof(ReloadScene), darknessDelay);
    }

    void ReloadScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}
