using System.Collections;
using UnityEngine;

public class EnemyTurnSystem : MonoBehaviour
{
    // ==============================
    // 다음 회전까지 대기
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


    // 현재 실행 중인 회전 Coroutine
    private Coroutine turnRoutine;


    private void Start()
    {
        // 게임 시작 시 정면 상태이므로 초록색
        SetSpotLightColor(normalLightColor);

        // 회전 시작
        StartTurn();
    }


    // ==============================
    // 회전 시작
    // ==============================

    public void StartTurn()
    {
        // 이미 회전 루틴이 실행 중이라면
        // 중복으로 실행하지 않는다.
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
            // 다음 회전까지 랜덤하게 대기
            float waitTime = Random.Range(
                minTurnTime,
                maxTurnTime
            );

            yield return new WaitForSeconds(waitTime);


            // ==============================
            // 앞 → 뒤
            // ==============================

            float turnDuration = Random.Range(
                minTurnDuration,
                maxTurnDuration
            );

            yield return TurnAround(
                turnDuration,
                false
            );


            // ==============================
            // 뒤를 보는 시간
            // ==============================

            float lookBackTime = Random.Range(
                minLookBackTime,
                maxLookBackTime
            );

            yield return new WaitForSeconds(
                lookBackTime
            );


            // ==============================
            // 뒤 → 앞
            // ==============================

            float returnDuration = Random.Range(
                minTurnDuration,
                maxTurnDuration
            );

            yield return TurnAround(
                returnDuration,
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

        // 현재 방향에서 정확히 180도 회전한 방향
        Quaternion targetRotation =
            startRotation * Quaternion.Euler(
                0f,
                180f,
                0f
            );


        // 회전 시작
        float elapsedTime = 0f;


        // ==============================
        // 회전 시작 시 색상 설정
        // ==============================

        if (isReturning)
        {
            // 뒤 → 앞
            // 회전 시작부터 초록색
            SetSpotLightColor(
                normalLightColor
            );
        }
        else
        {
            // 앞 → 뒤
            // 회전 시작부터 노란색
            SetSpotLightColor(
                turningLightColor
            );
        }


        // ==============================
        // 회전 진행
        // ==============================

        while (elapsedTime < turnDuration)
        {
            elapsedTime += Time.deltaTime;

            // 0 ~ 1 사이의 회전 진행도
            float progress =
                Mathf.Clamp01(
                    elapsedTime / turnDuration
                );


            // 회전 적용
            transform.rotation =
                Quaternion.Slerp(
                    startRotation,
                    targetRotation,
                    progress
                );


            // ==============================
            // 앞 → 뒤 회전일 때
            // 90도에서 빨간색
            // ==============================

            if (!isReturning)
            {
                float currentAngle =
                    Quaternion.Angle(
                        startRotation,
                        transform.rotation
                    );


                if (currentAngle >= 90f)
                {
                    SetSpotLightColor(
                        dangerLightColor
                    );
                }
            }


            // 다음 프레임까지 대기
            yield return null;
        }


        // 회전 종료 시 정확한 방향 적용
        transform.rotation = targetRotation;


        // ==============================
        // 회전 종료 후 색상
        // ==============================

        if (isReturning)
        {
            // 앞을 다시 바라봤으므로 초록색
            SetSpotLightColor(
                normalLightColor
            );
        }
        else
        {
            // 뒤를 바라보고 있으므로 빨간색
            SetSpotLightColor(
                dangerLightColor
            );
        }
    }


    // ==============================
    // Spot Light 색상 변경
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