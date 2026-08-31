using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class EnemyTurnSystem : MonoBehaviour
{
    // ========================================
    // 회전 전 대기 시간
    // ========================================

    [SerializeField] private float minTurnTime = 5f;
    [SerializeField] private float maxTurnTime = 10f;


    // ========================================
    // Glance 시간
    // ========================================

    [SerializeField] private float glanceDuration = 0.5f;


    // ========================================
    // Turn 시간
    // ========================================

    [SerializeField] private float minTurnDuration = 1f;
    [SerializeField] private float maxTurnDuration = 3f;


    // ========================================
    // Return 시간
    // ========================================

    [SerializeField] private float minReturnDuration = 1f;
    [SerializeField] private float maxReturnDuration = 3f;


    // ========================================
    // 뒤를 보는 체류 시간
    // ========================================

    [SerializeField] private float minLookBackTime = 2f;
    [SerializeField] private float maxLookBackTime = 5f;


    // ========================================
    // Spot Light
    // ========================================

    [SerializeField] private Light frontSpotLight;
    [SerializeField] private Light left90SpotLight;
    [SerializeField] private Light redSpotLight;


    // ========================================
    // Spot Light 색상
    // ========================================

    [SerializeField] private Color normalLightColor = Color.green;
    [SerializeField] private Color warningLightColor = Color.yellow;
    [SerializeField] private Color dangerLightColor = Color.red;


    // ========================================
    // Animation System
    // ========================================

    [SerializeField] private EnemyAnimationSystem animationSystem;


    // ========================================
    // Sound Manager
    // ========================================

    [SerializeField] private SoundManager soundManager;


    // ========================================
    // Developer UI
    // ========================================

    [SerializeField] private GameObject timeStatusUI;

    [SerializeField] private Text detectionWaitingTimeText;
    [SerializeField] private Text detectionTurnTimeText;
    [SerializeField] private Text detectionReturnTimeText;


    // ========================================
    // 현재 설정된 시간
    // ========================================

    private float currentWaitingTime;
    private float currentTurnTime;
    private float currentReturnTime;


    // ========================================
    // 현재 진행 시간
    // ========================================

    private float currentWaitingElapsedTime;
    private float currentTurnElapsedTime;
    private float currentReturnElapsedTime;


    // ========================================
    // Coroutine
    // ========================================

    private Coroutine turnRoutine;


    // ========================================
    // Start
    // ========================================

    private void Start()
    {
        SetFrontSpotLight();


        if (timeStatusUI != null)
        {
            timeStatusUI.SetActive(false);
        }


        StartTurn();
    }


    // ========================================
    // Update
    // ========================================

    private void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.tabKey.wasPressedThisFrame)
        {
            ToggleDeveloperUI();
        }


        if (timeStatusUI != null &&
            timeStatusUI.activeSelf)
        {
            UpdateDeveloperUI();
        }
    }


    // ========================================
    // Developer UI
    // ========================================

    private void ToggleDeveloperUI()
    {
        if (timeStatusUI == null)
        {
            return;
        }

        timeStatusUI.SetActive(
            !timeStatusUI.activeSelf
        );
    }


    private void UpdateDeveloperUI()
    {
        if (detectionWaitingTimeText != null)
        {
            detectionWaitingTimeText.text =
                "Waiting Time: " +
                currentWaitingElapsedTime.ToString("F1") +
                " / " +
                currentWaitingTime.ToString("F1");
        }


        if (detectionTurnTimeText != null)
        {
            detectionTurnTimeText.text =
                "Turn Time: " +
                currentTurnElapsedTime.ToString("F1") +
                " / " +
                currentTurnTime.ToString("F1");
        }


        if (detectionReturnTimeText != null)
        {
            detectionReturnTimeText.text =
                "Return Time: " +
                currentReturnElapsedTime.ToString("F1") +
                " / " +
                currentReturnTime.ToString("F1");
        }
    }


    // ========================================
    // 회전 루틴 시작
    // ========================================

    public void StartTurn()
    {
        if (turnRoutine != null)
        {
            return;
        }


        turnRoutine =
            StartCoroutine(
                TurnRoutine()
            );
    }


    // ========================================
    // 전체 행동 루틴
    // ========================================

    private IEnumerator TurnRoutine()
    {
        while (true)
        {
            // ========================================
            // 1. 대기
            // ========================================

            currentWaitingTime =
                Random.Range(
                    minTurnTime,
                    maxTurnTime
                );

            currentWaitingElapsedTime = 0f;


            SetFrontSpotLight();


            while (currentWaitingElapsedTime <
                   currentWaitingTime)
            {
                currentWaitingElapsedTime +=
                    Time.deltaTime;

                yield return null;
            }


            // ========================================
            // 2. Glance
            // ========================================

            if (animationSystem != null)
            {
                animationSystem.PlayGlance();
            }


            SetLeft90SpotLight();


            yield return new WaitForSeconds(
                glanceDuration
            );


            // ========================================
            // 3. Turn
            // ========================================

            currentTurnTime =
                Random.Range(
                    minTurnDuration,
                    maxTurnDuration
                );

            currentTurnElapsedTime = 0f;


            if (animationSystem != null)
            {
                animationSystem.PlayTurn(
                    currentTurnTime
                );
            }


            // 회전 시작음
            if (soundManager != null)
            {
                soundManager.PlayTurnSound();
            }


            // ========================================
            // 4. 실제 180° 회전
            // ========================================

            yield return RotateEnemy(
                currentTurnTime,
                false
            );


            // ========================================
            // 5. 뒤를 보는 체류
            // ========================================

            float lookBackTime =
                Random.Range(
                    minLookBackTime,
                    maxLookBackTime
                );


            SetRedSpotLight();


            yield return new WaitForSeconds(
                lookBackTime
            );


            // ========================================
            // 6. Return
            // ========================================

            currentReturnTime =
                Random.Range(
                    minReturnDuration,
                    maxReturnDuration
                );

            currentReturnElapsedTime = 0f;


            if (animationSystem != null)
            {
                animationSystem.PlayReturn(
                    currentReturnTime
                );
            }


            // Return 시작음
            if (soundManager != null)
            {
                soundManager.PlayReturnSound();
            }


            // Return 시작 = 정면 초록
            SetFrontSpotLight();


            // ========================================
            // 7. 실제 180° 복귀
            // ========================================

            yield return RotateEnemy(
                currentReturnTime,
                true
            );


            // ========================================
            // 8. 기본 상태
            // ========================================

            SetFrontSpotLight();


            if (animationSystem != null)
            {
                animationSystem.ResetAnimationSpeed();
            }
        }
    }


    // ========================================
    // 실제 Enemy 회전
    // ========================================

    private IEnumerator RotateEnemy(
        float duration,
        bool isReturning
    )
    {
        Quaternion startRotation =
            transform.rotation;


        Quaternion targetRotation =
            startRotation *
            Quaternion.Euler(
                0f,
                180f,
                0f
            );


        while (true)
        {
            if (isReturning)
            {
                currentReturnElapsedTime +=
                    Time.deltaTime;
            }
            else
            {
                currentTurnElapsedTime +=
                    Time.deltaTime;
            }


            float elapsedTime =
                isReturning
                ? currentReturnElapsedTime
                : currentTurnElapsedTime;


            float progress =
                Mathf.Clamp01(
                    elapsedTime / duration
                );


            transform.rotation =
                Quaternion.Slerp(
                    startRotation,
                    targetRotation,
                    progress
                );


            // ========================================
            // Turn
            // ========================================

            if (!isReturning)
            {
                if (progress < 0.5f)
                {
                    // 0° ~ 90°
                    SetLeft90SpotLight();
                }
                else
                {
                    // 90° ~ 180°
                    SetRedSpotLight();
                }
            }
            else
            {
                // Return 중 정면 초록
                SetFrontSpotLight();
            }


            if (progress >= 1f)
            {
                break;
            }


            yield return null;
        }


        transform.rotation =
            targetRotation;


        if (isReturning)
        {
            SetFrontSpotLight();
        }
        else
        {
            SetRedSpotLight();
        }
    }


    // ========================================
    // 정면 Spot Light
    // ========================================

    private void SetFrontSpotLight()
    {
        if (frontSpotLight != null)
        {
            frontSpotLight.enabled = true;
            frontSpotLight.color =
                normalLightColor;
        }


        if (left90SpotLight != null)
        {
            left90SpotLight.enabled = false;
        }


        if (redSpotLight != null)
        {
            redSpotLight.enabled = false;
        }
    }


    // ========================================
    // 왼쪽 90° Spot Light
    // ========================================

    private void SetLeft90SpotLight()
    {
        if (frontSpotLight != null)
        {
            frontSpotLight.enabled = false;
        }


        if (left90SpotLight != null)
        {
            left90SpotLight.enabled = true;
            left90SpotLight.color =
                warningLightColor;
        }


        if (redSpotLight != null)
        {
            redSpotLight.enabled = false;
        }
    }


    // ========================================
    // 빨간 Spot Light
    // ========================================

    private void SetRedSpotLight()
    {
        if (frontSpotLight != null)
        {
            frontSpotLight.enabled = false;
        }


        if (left90SpotLight != null)
        {
            left90SpotLight.enabled = false;
        }


        if (redSpotLight != null)
        {
            redSpotLight.enabled = true;
            redSpotLight.color =
                dangerLightColor;
        }
    }
}