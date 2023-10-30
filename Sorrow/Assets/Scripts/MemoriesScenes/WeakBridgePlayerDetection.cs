using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.Playables;

public class WeakBridgePlayerDetection : MonoBehaviour
{
    [SerializeField] Image darknessImage;
    [SerializeField] float darknessDelay;
    [SerializeField] CinemachineVirtualCamera lookAtMonsterCamera;
    [SerializeField] PlayableDirector lookAtMonsterTimeline;//remove controls activate monster and blend between cameras

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(transform.root.gameObject);//PLACEHOLDER debería haber una animacion del puente rompiendose
            LookAtMonster();
            StartCoroutine(FallToDarkness());
        }
    }

    void LookAtMonster()
    {
        lookAtMonsterCamera.transform.position = CinematicManager.instance.playerCamera.transform.position;
        lookAtMonsterCamera.transform.SetParent(CinematicManager.instance.player.transform);
        lookAtMonsterTimeline.Play();
    }

    IEnumerator FallToDarkness()
    {
        float timer = 1;
        Material darknessMaterial = new Material(darknessImage.material.shader);
        darknessMaterial.CopyMatchingPropertiesFromMaterial(darknessImage.material);
        darknessImage.material = darknessMaterial;
        while (timer > 0)
        {
            yield return new WaitForEndOfFrame();
            timer -= Time.deltaTime * 1 / darknessDelay;
            timer = Mathf.Clamp01(timer);
            darknessMaterial.SetFloat("_DarknessScale", timer);
        }
        SceneManager.LoadScene("DarkRoomScene");
    }
}
