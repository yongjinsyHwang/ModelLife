using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    // ========================================
    // Cameras
    // ========================================

    [SerializeField] private Camera mainCamera;

    [SerializeField] private Camera playerCamera;

    [SerializeField] private Camera enemyCamera;

    [SerializeField] private Camera fpsCamera;


    // ========================================
    // Start
    // ========================================

    private void Start()
    {
        // 시작 시 Main Camera
        SwitchCamera(mainCamera);
    }


    // ========================================
    // Update
    // ========================================

    private void Update()
    {
        // 1 → Main Camera
        if (Keyboard.current != null &&
            Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            SwitchCamera(mainCamera);
        }


        // 2 → Player Camera
        if (Keyboard.current != null &&
            Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            SwitchCamera(playerCamera);
        }


        // 3 → Enemy Camera
        if (Keyboard.current != null &&
            Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            SwitchCamera(enemyCamera);
        }


        // 4 → FPS Camera
        if (Keyboard.current != null &&
            Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            SwitchCamera(fpsCamera);
        }
    }


    // ========================================
    // Camera Switch
    // ========================================

    private void SwitchCamera(Camera targetCamera)
    {
        if (targetCamera == null)
        {
            return;
        }


        // 모든 카메라 OFF
        SetCameraEnabled(
            mainCamera,
            false
        );

        SetCameraEnabled(
            playerCamera,
            false
        );

        SetCameraEnabled(
            enemyCamera,
            false
        );

        SetCameraEnabled(
            fpsCamera,
            false
        );


        // 선택한 카메라 ON
        SetCameraEnabled(
            targetCamera,
            true
        );
    }


    // ========================================
    // Camera Enabled
    // ========================================

    private void SetCameraEnabled(
        Camera targetCamera,
        bool enabled
    )
    {
        if (targetCamera == null)
        {
            return;
        }


        targetCamera.enabled = enabled;
    }
}