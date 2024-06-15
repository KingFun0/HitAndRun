using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraSwitch : MonoBehaviour
{
    [SerializeField]
    CinemachineVirtualCamera MainCam;
    [SerializeField]
    CinemachineVirtualCamera BattleCam;
    private bool is_in_batlle;
    // Start is called before the first frame update
    void Start()
    {
        is_in_batlle = false;
        MainCam.Priority = 1;
        BattleCam.Priority = 0;
    }

    // Update is called once per frame
    public void SwitchPriority()
    {
        Debug.Log("Смена камер");
        if(is_in_batlle)
        {
            MainCam.Priority = 1;
            BattleCam.Priority = 0;
            
        }
        else
        {
            MainCam.Priority = 0;
            BattleCam.Priority = 1;
        }
        is_in_batlle = !is_in_batlle;
    }
}
