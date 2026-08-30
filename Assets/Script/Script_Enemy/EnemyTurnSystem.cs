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

    // 힐끔 본 후 실제 Turn까지 기다리는 시간
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

    [SerializeField] private Light spotLight;

    [SerializeField] private Color normalLightColor = Color.green;

    [SerializeField] private Color turningLightColor = Color.yellow;

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
        SetSpotLightColor(normalLightColor);


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
            StartCoroutine(TurnRoutine());
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


            SetSpotLightColor(
                normalLightColor
            );


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


            // Glance 단계에서는
            // Spot Light를 노란색으로 변경
            SetSpotLightColor(
                turningLightColor
            );


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

            SetSpotLightColor(
                normalLightColor
            );


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
            // 0 ~ 1 진행률
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
            // Spot Light
            // ----------------------------------------

            if (isReturning)
            {
                SetSpotLightColor(
                    normalLightColor
                );
            }
            else
            {
                if (progress < 0.5f)
                {
                    SetSpotLightColor(
                        turningLightColor
                    );
                }
                else
                {
                    SetSpotLightColor(
                        dangerLightColor
                    );
                }
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


        // 최종 방향 보정
        transform.rotation =
            targetRotation;


        // 최종 Light
        if (isReturning)
        {
            SetSpotLightColor(
                normalLightColor
            );
        }
        else
        {
            SetSpotLightColor(
                dangerLightColor
            );
        }
    }


    // ========================================
    // Spot Light
    // ========================================

    private void SetSpotLightColor(
        Color color
    )
    {
        if (spotLight == null)
        {
            return;
        }

        spotLight.color = color;
    }
}