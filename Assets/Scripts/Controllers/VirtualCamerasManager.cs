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
        TavernEventsManager.NightStarts += SwitchCams;
    }
    private void OnDisable()
    {
        TavernEventsManager.NightStarts -= SwitchCams;
    }

    public void SwitchCams()
    {
        followPlayerCam.enabled = !followPlayerCam.enabled;
    }

}
