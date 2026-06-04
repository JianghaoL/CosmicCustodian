using System;
using DG.Tweening;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Transform camTransform;
    
    private void Awake()
    {
        camTransform = transform.GetChild(0).GetComponent<Transform>();
        
        GameEventsManager.OnCameraCentered.AddListener(RotateCamera);
        GameEventsManager.OnGameWin.AddListener(OnGameWinAnim);
    }

    private void OnDestroy()
    {
        GameEventsManager.OnCameraCentered.RemoveListener(RotateCamera);
        GameEventsManager.OnGameWin.RemoveListener(OnGameWinAnim);
    }

    private void RotateCamera()
    {
        Debug.Log("RotateCamera");
        var rotation = GameDataManager.Instance.GetConfig().cameraRotation;
        
        camTransform.rotation = Quaternion.Euler(Vector3.zero);
        camTransform.DORotateQuaternion(Quaternion.Euler(rotation), GameDataManager.Instance.GetConfig().cameraRotationDuration);
    }

    private void OnGameWinAnim()
    {
        var position = GameDataManager.Instance.GetConfig().onGameWinCameraPosition;
        position = transform.position + position;
        
        transform.DOMove(position, GameDataManager.Instance.GetConfig().onGameWinCamMoveDuration).SetEase(Ease.OutSine);
    }
}
