using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Cinemachine;
using System.Diagnostics;

public class ActiveCamera : MonoBehaviour
{
    public GameObject playerVirtualCamera;
    private GameObject currentCamera;
    private int camTest = 0;

    public void OnTriggerEnter2D(Collider2D other)
    {
        //UnityEngine.Debug.Log("Trigger Enter Test");

        // Swap Camera at transition
        if (other.gameObject.tag == "CameraBoundary" && camTest == 1)
        {
            // Set new player camera to the new one
            playerVirtualCamera = other.GetComponent<CameraBoundaryChange>().virtualCamera;
            // Set old camera priority to 10
            currentCamera.GetComponent<CinemachineVirtualCamera>().m_Priority = 10;
            // Set new camera priority to 11
            playerVirtualCamera.GetComponent<CinemachineVirtualCamera>().m_Priority = 11;
            // Cache new camera under 'Current Camera'
            currentCamera = playerVirtualCamera;

            //UnityEngine.Debug.Log("Swap Test");

        }

        // Set Camera to 11 at Spawn
        if (other.gameObject.tag == "CameraBoundary" && camTest == 0)
        {
            playerVirtualCamera = other.GetComponent<CameraBoundaryChange>().virtualCamera;
            currentCamera = playerVirtualCamera;
            currentCamera.GetComponent<CinemachineVirtualCamera>().m_Priority = 11;
            camTest = 1;

            //UnityEngine.Debug.Log("Spawn Test");
        }
    }
}
