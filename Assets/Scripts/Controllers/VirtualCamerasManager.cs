using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class VirtualCamerasManager : MonoBehaviour
{
    [SerializeField] CinemachineFreeLook followPlayerCam;
    [SerializeField] CinemachineFreeLook cardGameCam;

    private void OnEnable()
    {
        TavernEventsManager.CameraSwitchToCardGame += SwitchCameraToCardGame;
        TavernEventsManager.CameraSwitchToFollowPlayer += SwitchCameraToFollowPlayer;
    }
    private void OnDisable()
    {
        TavernEventsManager.CameraSwitchToCardGame -= SwitchCameraToCardGame;
        TavernEventsManager.CameraSwitchToFollowPlayer -= SwitchCameraToFollowPlayer;
    }

    private void SwitchCameraToCardGame()
    {
        followPlayerCam.enabled = false;

    }
    private void SwitchCameraToFollowPlayer()
    {
        followPlayerCam.enabled = true;
    }

}
