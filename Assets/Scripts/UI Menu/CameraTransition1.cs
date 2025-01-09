using UnityEngine;
using Cinemachine;

public class CameraTransition : MonoBehaviour
{
    public CinemachineVirtualCamera startCamera;
    public CinemachineVirtualCamera endCamera;
    public CinemachineFreeLook playerCam;

    public void SwitchCamera()
    {
        startCamera.Priority = 0;

        if (playerCam != null )
        {
            playerCam.Priority = 100;
        }else
        {
            endCamera.Priority = 100;
        }
    }
}
