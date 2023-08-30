using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

public class RestartChase : MonoBehaviour
{
    [SerializeField] TimelineAsset restartTimeline;

    void OnTriggerEnter(Collider col)
    {
        if (!col.CompareTag("Player"))
            return;

        // Start timeline clip
        Debug.Log("Restarting chase");
        EndResult(); // DEBUG
    }

    void EndResult() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}
