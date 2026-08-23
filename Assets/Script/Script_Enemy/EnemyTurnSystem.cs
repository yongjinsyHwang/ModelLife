using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class EnemyTurnSystem : MonoBehaviour
{
    // ==============================
    // 회전 대기 시간
    // ==============================

    // 다음 회전까지 기다리는 최소 시간
    [SerializeField] private float minTurnTime = 5f;

    // 다음 회전까지 기다리는 최대 시간
    [SerializeField] private float maxTurnTime = 10f;


    // ==============================
    // 회전 시간
    // ==============================

    // 180도 회전하는 데 걸리는 최소 시간
    [SerializeField] private float minTurnDuration = 1f;

    // 180도 회전하는 데 걸리는 최대 시간
    [SerializeField] private float maxTurnDuration = 3f;


    // ==============================
    // 복귀 회전 시간
    // ==============================

    // 복귀 회전에 걸리는 최소 시간
    [SerializeField] private float minReturnDuration = 1f;

    // 복귀 회전에 걸리는 최대 시간
    [SerializeField] private float maxReturnDuration = 3f;


    // ==============================
    // 뒤를 보는 시간
    // ==============================

    // 뒤를 바라보고 있는 최소 시간
    [SerializeField] private float minLookBackTime = 2f;

    // 뒤를 바라보고 있는 최대 시간
    [SerializeField] private float maxLookBackTime = 5f;


    // ==============================
    // Spot Light
    // ==============================

    // Enemy에 붙어 있는 Spot Light
    [SerializeField] private Light spotLight;

    // 정면을 바라볼 때의 색상
    [SerializeField] private Color normalLightColor = Color.green;

    // 뒤돌기 시작했을 때의 색상
    [SerializeField] private Color turningLightColor = Color.yellow;

    // 90도 이상 회전했을 때의 색상
    [SerializeField] private Color dangerLightColor = Color.red;


    // ==============================
    // Developer UI
    // ==============================

    // 개발자용 UI 전체 오브젝트
    [SerializeField] private GameObject timeStatusUI;

    // 다음 회전까지 대기 시간 Text
    [SerializeField] private Text detectionWaitingTimeText;

    // 앞 → 뒤 회전 시간 Text
    [SerializeField] private Text detectionTurnTimeText;

    // 뒤 → 앞 복귀 회전 시간 Text
    [SerializeField] private Text detectionReturnTimeText;


    // ==============================
    // 실제 현재 설정된 시간
    // ==============================

    // 현재 사이클의 대기 시간
    private float currentWaitingTime;

    // 현재 사이클의 회전 시간
    private float currentTurnTime;

    // 현재 사이클의 복귀 회전 시간
    private float currentReturnTime;


    // ==============================
    // 현재 진행 시간
    // ==============================

    // 현재 대기 단계에서 지난 시간
    private float currentWaitingElapsedTime;

    // 현재 회전 단계에서 지난 시간
    private float currentTurnElapsedTime;

    // 현재 복귀 단계에서 지난 시간
    private float currentReturnElapsedTime;


    // ==============================
    // Coroutine
    // ==============================

    // 현재 실행 중인 회전 Coroutine
    private Coroutine turnRoutine;


    private void Start()
    {
        // 시작 시 Spot Light는 초록색
        SetSpotLightColor(normalLightColor);


        // 개발자 UI는 기본적으로 비활성화
        if (timeStatusUI != null)
        {
            timeStatusUI.SetActive(false);
        }


        // 회전 시작
        StartTurn();
    }


    private void Update()
    {
        // Tab 키로 개발자 UI ON/OFF
        if (Keyboard.current != null &&
            Keyboard.current.tabKey.wasPressedThisFrame)
        {
            ToggleDeveloperUI();
        }


        // Developer UI가 활성화되어 있다면 실시간 UI 갱신
        if (timeStatusUI != null &&
            timeStatusUI.activeSelf)
        {
            UpdateDeveloperUI();
        }
    }


    // ==============================
    // Developer UI
    // ==============================

    // 개발자 UI를 켜고 끈다.
    private void ToggleDeveloperUI()
    {
        if (timeStatusUI == null)
        {
            return;
        }

        bool isActive = !timeStatusUI.activeSelf;

        timeStatusUI.SetActive(isActive);
    }


    // 개발자 UI의 시간을 갱신한다.
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


    // ==============================
    // 회전 시작
    // ==============================

    public void StartTurn()
    {
        // 이미 실행 중이라면 중복 실행하지 않는다.
        if (turnRoutine != null)
        {
            return;
        }


        turnRoutine = StartCoroutine(TurnRoutine());
    }


    // ==============================
    // 전체 회전 루틴
    // ==============================

    private IEnumerator TurnRoutine()
    {
        while (true)
        {
            // ==============================
            // 대기 시간 결정
            // ==============================

            currentWaitingTime = Random.Range(
                minTurnTime,
                maxTurnTime
            );


            // 대기 시간 초기화
            currentWaitingElapsedTime = 0f;


            // 대기
            while (currentWaitingElapsedTime < currentWaitingTime)
            {
                currentWaitingElapsedTime += Time.deltaTime;

                yield return null;
            }


            // ==============================
            // 앞 → 뒤 회전 시간 결정
            // ==============================

            currentTurnTime = Random.Range(
                minTurnDuration,
                maxTurnDuration
            );


            // 회전 시간 초기화
            currentTurnElapsedTime = 0f;


            // 앞 → 뒤 회전
            yield return TurnAround(
                currentTurnTime,
                false
            );


            // ==============================
            // 뒤를 바라보는 시간
            // ==============================

            float lookBackTime = Random.Range(
                minLookBackTime,
                maxLookBackTime
            );


            yield return new WaitForSeconds(
                lookBackTime
            );


            // ==============================
            // 복귀 회전 시간 결정
            // ==============================

            currentReturnTime = Random.Range(
                minReturnDuration,
                maxReturnDuration
            );


            // 복귀 시간 초기화
            currentReturnElapsedTime = 0f;


            // 뒤 → 앞 복귀
            yield return TurnAround(
                currentReturnTime,
                true
            );
        }
    }


    // ==============================
    // 180도 회전
    // ==============================

    private IEnumerator TurnAround(
        float turnDuration,
        bool isReturning
    )
    {
        // 회전 시작 방향
        Quaternion startRotation = transform.rotation;


        // 현재 방향에서 180도 회전한 방향
        Quaternion targetRotation =
            startRotation *
            Quaternion.Euler(
                0f,
                180f,
                0f
            );


        // ==============================
        // 회전 시작 색상
        // ==============================

        if (isReturning)
        {
            // 뒤 → 앞 복귀
            // 시작부터 초록색
            SetSpotLightColor(
                normalLightColor
            );
        }
        else
        {
            // 앞 → 뒤
            // 시작부터 노란색
            SetSpotLightColor(
                turningLightColor
            );
        }


        // ==============================
        // 회전 진행
        // ==============================

        while (true)
        {
            if (isReturning)
            {
                currentReturnElapsedTime += Time.deltaTime;
            }
            else
            {
                currentTurnElapsedTime += Time.deltaTime;
            }


            float progress =
                Mathf.Clamp01(
                    (isReturning
                        ? currentReturnElapsedTime
                        : currentTurnElapsedTime)
                    / turnDuration
                );


            // 회전
            transform.rotation =
                Quaternion.Slerp(
                    startRotation,
                    targetRotation,
                    progress
                );


            // ==============================
            // 앞 → 뒤 회전의 색상
            // ==============================

            if (!isReturning)
            {
                float currentAngle =
                    Quaternion.Angle(
                        startRotation,
                        transform.rotation
                    );


                // 90도 이전 → 노랑
                // 90도 이후 → 빨강
                if (currentAngle >= 90f)
                {
                    SetSpotLightColor(
                        dangerLightColor
                    );
                }
            }


            // 회전 완료
            if (progress >= 1f)
            {
                break;
            }


            yield return null;
        }


        // 최종 회전 방향 확정
        transform.rotation = targetRotation;


        // ==============================
        // 회전 종료 후 색상
        // ==============================

        if (isReturning)
        {
            // 다시 정면을 바라보므로 초록색
            SetSpotLightColor(
                normalLightColor
            );
        }
        else
        {
            // 뒤를 바라보므로 빨간색
            SetSpotLightColor(
                dangerLightColor
            );
        }
    }


    // ==============================
    // Spot Light 색상
    // ==============================

    private void SetSpotLightColor(Color color)
    {
        if (spotLight == null)
        {
            return;
        }


        spotLight.color = color;
    }
}