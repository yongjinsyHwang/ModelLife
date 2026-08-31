using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    // ========================================
    // Cameras
    // ========================================

    [SerializeField] private Camera cctv1Camera;
    [SerializeField] private Camera cctv2Camera;
    [SerializeField] private Camera enemyCamera;
    [SerializeField] private Camera playerACamera;
    [SerializeField] private Camera playerBCamera;
    [SerializeField] private Camera playerCCamera;


    // ========================================
    // Game Clear Camera Lock
    // ========================================

    private bool isGameClearCameraLocked = false;


    // ========================================
    // Game Clear 연출
    // ========================================

    [SerializeField] private GameObject effectItem;

    [SerializeField] private Transform effectSpawnPoint;

    [SerializeField] private Transform effectTargetPoint;

    [SerializeField] private float effectFallSpeed = 2f;


    private bool hasPlayedGameClearEffect = false;


    // ========================================
    // Start
    // ========================================

    private void Start()
    {
        SwitchCamera(
            cctv1Camera
        );
    }


    // ========================================
    // Update
    // ========================================

    private void Update()
    {
        if (Keyboard.current == null)
        {
            return;
        }


        if (isGameClearCameraLocked)
        {
            return;
        }


        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            SwitchCamera(
                cctv1Camera
            );
        }


        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            SwitchCamera(
                cctv2Camera
            );
        }


        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            SwitchCamera(
                enemyCamera
            );
        }


        if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            SwitchCamera(
                playerACamera
            );
        }


        if (Keyboard.current.digit5Key.wasPressedThisFrame)
        {
            SwitchCamera(
                playerBCamera
            );
        }


        if (Keyboard.current.digit6Key.wasPressedThisFrame)
        {
            SwitchCamera(
                playerCCamera
            );
        }
    }


    // ========================================
    // Game Clear Camera Lock
    // ========================================

    public void LockGameClearCamera()
    {
        if (isGameClearCameraLocked)
        {
            return;
        }


        isGameClearCameraLocked = true;


        SwitchCamera(
            playerCCamera
        );


        Debug.Log(
            "Game Clear : PlayerC 카메라 고정"
        );
    }


    // ========================================
    // Game Clear 연출
    // ========================================

    public void PlayGameClearEffect()
    {
        if (hasPlayedGameClearEffect)
        {
            return;
        }


        hasPlayedGameClearEffect = true;


        if (effectItem == null ||
            effectSpawnPoint == null ||
            effectTargetPoint == null)
        {
            Debug.LogWarning(
                "CameraManager : " +
                "Game Clear 연출 설정을 확인하세요."
            );

            return;
        }


        GameObject spawnedItem =
            Instantiate(
                effectItem,
                effectSpawnPoint.position,
                effectSpawnPoint.rotation
            );


        StartCoroutine(
            MoveGameClearEffect(
                spawnedItem
            )
        );
    }


    // ========================================
    // Game Clear 아이템 이동
    // ========================================

    private IEnumerator MoveGameClearEffect(
        GameObject spawnedItem
    )
    {
        if (spawnedItem == null)
        {
            yield break;
        }


        float speed =
            Mathf.Max(
                effectFallSpeed,
                0.01f
            );


        while (spawnedItem != null)
        {
            spawnedItem.transform.position =
                Vector3.MoveTowards(
                    spawnedItem.transform.position,
                    effectTargetPoint.position,
                    speed * Time.deltaTime
                );


            float distance =
                Vector3.Distance(
                    spawnedItem.transform.position,
                    effectTargetPoint.position
                );


            if (distance <= 0.01f)
            {
                break;
            }


            yield return null;
        }


        if (spawnedItem != null)
        {
            spawnedItem.transform.position =
                effectTargetPoint.position;
        }
    }


    // ========================================
    // Camera Switch
    // ========================================

    private void SwitchCamera(
        Camera targetCamera
    )
    {
        if (targetCamera == null)
        {
            Debug.LogWarning(
                "CameraManager : " +
                "전환할 카메라가 연결되지 않았습니다."
            );

            return;
        }


        SetCameraEnabled(
            cctv1Camera,
            false
        );

        SetCameraEnabled(
            cctv2Camera,
            false
        );

        SetCameraEnabled(
            enemyCamera,
            false
        );

        SetCameraEnabled(
            playerACamera,
            false
        );

        SetCameraEnabled(
            playerBCamera,
            false
        );

        SetCameraEnabled(
            playerCCamera,
            false
        );


        SetCameraEnabled(
            targetCamera,
            true
        );


        Debug.Log(
            "Camera 변경 : " +
            targetCamera.gameObject.name
        );
    }


    // ========================================
    // Camera Enabled
    // ========================================

    private void SetCameraEnabled(
        Camera targetCamera,
        bool state
    )
    {
        if (targetCamera == null)
        {
            return;
        }


        targetCamera.enabled =
            state;
    }
}