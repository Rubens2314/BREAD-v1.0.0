using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
public class SwitchVcam : MonoBehaviour
{
    public PlayerInput playerInput;
    private InputAction aimAction;
    private CinemachineVirtualCamera virtualCamera;
    public int priorityBoostAmount=10;

    public Canvas thirdPersonCanvas;
    public Canvas aimCanvas;
    private void Start()
    {

        aimCanvas.enabled = false;
    }
    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        aimAction = playerInput.actions["Aim"];
    }

    private void OnEnable()
    {
        aimAction.performed += _ => StartAim();
        aimAction.canceled += _ => CancelAim();
    }
    private void OnDisable()
    {
        aimAction.performed -= _ => StartAim();
        aimAction.canceled -= _ => CancelAim();
    }

    private void StartAim()
    {
        virtualCamera.Priority += priorityBoostAmount;
        aimCanvas.enabled = true;
        thirdPersonCanvas.enabled = false;
    }
    private void CancelAim()
    {
        virtualCamera.Priority -= priorityBoostAmount;
         aimCanvas.enabled = false;
        thirdPersonCanvas.enabled = true;
    }
}
