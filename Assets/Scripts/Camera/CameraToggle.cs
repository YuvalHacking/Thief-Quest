using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraToggle : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera fullscreenCamera;  // Reference to the fullscreen camera
    [SerializeField] private CinemachineVirtualCamera playerCamera;     // Reference to the player camera
    [SerializeField] private GameObject minimap;                         // Reference to the minimap GameObject

    private void Update()
    {
        // Check for user input to toggle cameras when the 'C' key is pressed
        if (Input.GetKeyDown(KeyCode.C))
            ToggleCameras();
    }

    // Function to toggle cameras and update the minimap visibility
    private void ToggleCameras()
    {
        // Swap camera priorities to switch between fullscreen and player cameras
        int temp = playerCamera.Priority;
        playerCamera.Priority = fullscreenCamera.Priority;
        fullscreenCamera.Priority = temp;

        // Set minimap's active status based on the currently active camera
        minimap.SetActive(getCurrentCamera() == playerCamera);
    }

    // Function to determine the currently active camera
    public CinemachineVirtualCamera getCurrentCamera()
    {
        // Return the camera with higher priority (active camera)
        return playerCamera.Priority > fullscreenCamera.Priority ? playerCamera : fullscreenCamera;
    }
}
