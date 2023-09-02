using UnityEngine;
using Cinemachine;

public class StartCinematic : MonoBehaviour
{
    public CinemachineVirtualCameraBase botoneraCamera;

    bool isCinematicPlaying = false;
    [SerializeField] KeyCode exitButton = KeyCode.Escape;
    BoxCollider botoneraTriggerCollider;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isCinematicPlaying)
        {
            isCinematicPlaying = true;
            botoneraCamera.Priority = 10; // Asegurarte de que la cámara de la cinemática tenga la prioridad más alta

            // Iniciar la cinemática aquí si es necesario
            // Por ejemplo, podrías hacer que las cámaras empiecen a seguir rutas definidas
        }

        if (isCinematicPlaying && Input.GetKeyDown(exitButton))
        {
            isCinematicPlaying = false;
            botoneraCamera.Priority = 1;
            botoneraTriggerCollider.enabled = false;  
        }
    }
}

