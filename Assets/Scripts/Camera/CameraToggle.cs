using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraToggle : MonoBehaviour
{

    [SerializeField] private CinemachineVirtualCamera fullscreenCamera;
    [SerializeField] private CinemachineVirtualCamera playerCamera;
    [SerializeField] private GameObject minimap;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            ToggleCameras();

    }
    private void ToggleCameras()
    {
        int temp = playerCamera.Priority;
        playerCamera.Priority = fullscreenCamera.Priority;
        fullscreenCamera.Priority = temp;

        minimap.SetActive(getCurrentCamera() == playerCamera);
    }

    public CinemachineVirtualCamera getCurrentCamera()
    {
        return playerCamera.Priority > fullscreenCamera.Priority ? playerCamera : fullscreenCamera;
    }
}
