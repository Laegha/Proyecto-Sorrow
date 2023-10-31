using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.Playables;

public class WeakBridgePlayerDetection : MonoBehaviour
{
    [SerializeField] Renderer darknessSphere;
    [SerializeField] float darknessDelay;
    [SerializeField] CinemachineVirtualCamera lookAtMonsterCamera;
    [SerializeField] CinemachineVirtualCamera playerCameraSustitute;
    //[SerializeField] CinemachineBlenderSettings customBlends;
    [SerializeField] PlayableDirector fallTimeline;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject bridge = transform.root.gameObject;
            transform.SetParent(null);
            Destroy(bridge);//PLACEHOLDER debería haber una animacion del puente rompiendose
            LookAtMonster();
            StartCoroutine(FallToDarkness());
        }
    }
    
    void LookAtMonster()
    {
        //InputManager.instance.RemRegControl(false);
        lookAtMonsterCamera.transform.position = CinematicManager.instance.playerCamera.transform.position;
        lookAtMonsterCamera.transform.SetParent(CinematicManager.instance.player.transform);

        print(CinematicManager.instance.playerCamera);
        playerCameraSustitute.transform.position = CinematicManager.instance.playerCamera.transform.position;
        playerCameraSustitute.transform.rotation = CinematicManager.instance.playerCamera.transform.rotation;
        CinematicManager.instance.CameraChange(playerCameraSustitute);

        fallTimeline.Play();
    }

    IEnumerator FallToDarkness()
    {
        float timer = 1;
        Material darknessMaterial = new Material(darknessSphere.material.shader);
        darknessMaterial.CopyMatchingPropertiesFromMaterial(darknessSphere.material);
        darknessSphere.material = darknessMaterial;
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
