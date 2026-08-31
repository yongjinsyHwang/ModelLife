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
    // Glance 체류 전환용 시간
    // ========================================

    [SerializeField] private float glanceDuration = 0.5f;


    // ========================================
    // 정면 → 뒤 회전 시간
    // ========================================

    [SerializeField] private float minTurnDuration = 1f;
    [SerializeField] private float maxTurnDuration = 3f;


    // ========================================
    // 뒤 → 정면 복귀 시간
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

    // Enemy 정면을 비추는 Spot Light
    [SerializeField] private Light frontSpotLight;

    // Enemy 기준 왼쪽 90°를 비추는 Spot Light
    [SerializeField] private Light left90SpotLight;

    // Enemy 기준 뒤쪽 / 빨간 Raycast 방향을 비추는 Spot Light
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
        // 시작 상태 = 정면 초록
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
        // Tab → Developer UI
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


            // 대기 중에는 정면 초록
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


            // Glance 중에는 왼쪽 90° 노랑
            SetLeft90SpotLight();


            yield return new WaitForSeconds(
                glanceDuration
            );


            // ========================================
            // 3. Turn 시간 결정
            // ========================================

            currentTurnTime =
                Random.Range(
                    minTurnDuration,
                    maxTurnDuration
                );

            currentTurnElapsedTime = 0f;


            // Turn Animation
            if (animationSystem != null)
            {
                animationSystem.PlayTurn(
                    currentTurnTime
                );
            }


            // ========================================
            // 4. 실제 180° 회전
            // ========================================

            yield return RotateEnemy(
                currentTurnTime,
                false
            );


            // ========================================
            // 5. 뒤를 바라보는 체류
            // ========================================

            float lookBackTime =
                Random.Range(
                    minLookBackTime,
                    maxLookBackTime
                );


            // 회전 완료 후 빨간 Spot Light 유지
            SetRedSpotLight();


            yield return new WaitForSeconds(
                lookBackTime
            );


            // ========================================
            // 6. Return 시간 결정
            // ========================================

            currentReturnTime =
                Random.Range(
                    minReturnDuration,
                    maxReturnDuration
                );

            currentReturnElapsedTime = 0f;


            // Return Animation
            if (animationSystem != null)
            {
                animationSystem.PlayReturn(
                    currentReturnTime
                );
            }


            // Return 시작과 동시에
            // 정면 초록 Spot Light ON
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
            // ----------------------------------------
            // 진행 시간
            // ----------------------------------------

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


            // ----------------------------------------
            // 진행률
            // ----------------------------------------

            float progress =
                Mathf.Clamp01(
                    elapsedTime / duration
                );


            // ----------------------------------------
            // 실제 Enemy 회전
            // ----------------------------------------

            transform.rotation =
                Quaternion.Slerp(
                    startRotation,
                    targetRotation,
                    progress
                );


            // ----------------------------------------
            // Turn 중 Spot Light
            // ----------------------------------------

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
                // Return 중에는 정면 초록 유지
                SetFrontSpotLight();
            }


            // ----------------------------------------
            // 회전 완료
            // ----------------------------------------

            if (progress >= 1f)
            {
                break;
            }


            yield return null;
        }


        // 정확한 최종 방향
        transform.rotation =
            targetRotation;


        // ----------------------------------------
        // 회전 완료 후 Spot Light
        // ----------------------------------------

        if (isReturning)
        {
            // 복귀 완료 → 정면 초록
            SetFrontSpotLight();
        }
        else
        {
            // 180° 회전 완료 → 빨강
            SetRedSpotLight();
        }
    }


    // ========================================
    // 정면 Spot Light
    // ========================================

    private void SetFrontSpotLight()
    {
        // 정면 초록 ON
        if (frontSpotLight != null)
        {
            frontSpotLight.enabled = true;
            frontSpotLight.color =
                normalLightColor;
        }


        // 나머지 OFF
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
        // 정면 OFF
        if (frontSpotLight != null)
        {
            frontSpotLight.enabled = false;
        }


        // 왼쪽 90° 노랑 ON
        if (left90SpotLight != null)
        {
            left90SpotLight.enabled = true;
            left90SpotLight.color =
                warningLightColor;
        }


        // 빨강 OFF
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
        // 정면 OFF
        if (frontSpotLight != null)
        {
            frontSpotLight.enabled = false;
        }


        // 왼쪽 90° OFF
        if (left90SpotLight != null)
        {
            left90SpotLight.enabled = false;
        }


        // 빨강 ON
        if (redSpotLight != null)
        {
            redSpotLight.enabled = true;
            redSpotLight.color =
                dangerLightColor;
        }
    }
}