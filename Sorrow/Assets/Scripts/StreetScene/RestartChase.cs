using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class RestartChase : MonoBehaviour
{
    [SerializeField] PlayableDirector restartTimeline;
    [SerializeField] Renderer darknessSphere;
    [SerializeField] float darknessDelay;

    void OnTriggerEnter(Collider col)
    {
        if (!col.CompareTag("Player"))
            return;

        BreakingBlock.RefreshPlayer();

        ChaseController chaseController = GetComponent<ChaseController>();
        if (chaseController)
            chaseController.isMoving = false;

        // Start timeline clip
        if (restartTimeline != null)
            restartTimeline.Play();

        StartCoroutine(RestartScene()); // DEBUG
    }

    IEnumerator RestartScene()
    {
        float timer = 1;
        darknessSphere.transform.rotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        Material darknessMaterial = new Material(darknessSphere.material.shader);
        darknessMaterial.CopyMatchingPropertiesFromMaterial(darknessSphere.material);
        darknessSphere.material = darknessMaterial;
        while (timer > 0)
        {
            yield return new WaitForEndOfFrame();
            timer -= Time.deltaTime * 1/darknessDelay;
            timer = Mathf.Clamp01(timer);
            darknessMaterial.SetFloat("_DarknessScale", timer);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
