using UnityEngine;
using Cinemachine;

public class StartCinematic : MonoBehaviour
{
    public CinemachineVirtualCameraBase botoneraCamera;

    private bool isCinematicPlaying = false;
    [SerializeField] KeyCode exitButton=KeyCode.Escape;
    BoxCollider botoneraTriggerCollider;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isCinematicPlaying)
        {
            isCinematicPlaying = true;
            botoneraCamera.Priority = 10; // Asegurarte de que la c�mara de la cinem�tica tenga la prioridad m�s alta

            // Iniciar la cinem�tica aqu� si es necesario
            // Por ejemplo, podr�as hacer que las c�maras empiecen a seguir rutas definidas
        }

        if (isCinematicPlaying && Input.GetKeyDown(exitButton))
        {
            isCinematicPlaying = false;
            botoneraCamera.Priority = 1;
            botoneraTriggerCollider.enabled = false;
                    
        }
    }
}

