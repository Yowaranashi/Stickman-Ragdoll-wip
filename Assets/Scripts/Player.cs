using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Player : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _followCamera, _finishCamera;
    public void Finish()
    {
        _followCamera.gameObject.SetActive(false);
        _finishCamera.gameObject.SetActive(true);
        //StopFollowCamera();
    }
    public void StopFollowCamera()
    {
        _finishCamera.Follow = null;
        _finishCamera.LookAt = null;
        _followCamera.Follow = null;
        _followCamera.LookAt = null;
    }
}
