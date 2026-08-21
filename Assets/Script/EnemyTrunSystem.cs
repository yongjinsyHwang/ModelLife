using UnityEngine;
using System.Collections;

public class EnemyTurnSystem : MonoBehaviour
{
    // 다음 회전까지 기다리는 최소 시간
    [SerializeField] private float minTurnTime = 5f;

    // 다음 회전까지 기다리는 최대 시간
    [SerializeField] private float maxTurnTime = 10f;

    // 180도 회전하는 데 걸리는 최소 시간
    [SerializeField] private float minTurnDuration = 1f;

    // 180도 회전하는 데 걸리는 최대 시간
    [SerializeField] private float maxTurnDuration = 3f;

    // 뒤를 바라보고 있는 최소 시간
    [SerializeField] private float minLookBackTime = 2f;

    // 뒤를 바라보고 있는 최대 시간
    [SerializeField] private float maxLookBackTime = 5f;


    // Enemy의 회전 행동을 시작한다.
    public void StartTurn()
    {
        StartCoroutine(TurnRoutine());
    }


    // Enemy의 회전 행동을 계속 반복한다.
    private IEnumerator TurnRoutine()
    {
        while (true)
        {
            // 다음 회전까지 기다릴 시간을 랜덤하게 결정
            float waitTime = Random.Range(
                minTurnTime,
                maxTurnTime
            );

            // 결정된 시간만큼 대기
            yield return new WaitForSeconds(waitTime);


            // 뒤돌아보는 데 걸리는 시간을 랜덤하게 결정
            float turnDuration = Random.Range(
                minTurnDuration,
                maxTurnDuration
            );

            // 뒤돌아봄
            yield return Turn(turnDuration);


            // 뒤를 바라보고 있을 시간을 랜덤하게 결정
            float lookBackTime = Random.Range(
                minLookBackTime,
                maxLookBackTime
            );

            // 결정된 시간 동안 뒤를 바라봄
            yield return new WaitForSeconds(lookBackTime);


            // 다시 앞을 바라봄
            float returnDuration = Random.Range(
                minTurnDuration,
                maxTurnDuration
            );

            yield return Turn(returnDuration);
        }
    }


    // 지정된 시간 동안 180도 회전한다.
    private IEnumerator Turn(float turnDuration)
    {
        // 회전 시작 방향
        Quaternion startRotation = transform.rotation;

        // 현재 방향에서 180도 회전한 목표 방향
        Quaternion targetRotation =
            startRotation * Quaternion.Euler(0f, 180f, 0f);

        // 회전에 걸린 시간
        float elapsedTime = 0f;


        // 회전이 끝날 때까지 반복
        while (elapsedTime < turnDuration)
        {
            // 지난 시간을 누적
            elapsedTime += Time.deltaTime;

            // 회전 진행도 계산
            float progress = elapsedTime / turnDuration;

            // 시작 방향에서 목표 방향으로 회전
            transform.rotation = Quaternion.Slerp(
                startRotation,
                targetRotation,
                progress
            );

            // 다음 프레임까지 대기
            yield return null;
        }

        // 최종적으로 정확한 목표 방향 적용
        transform.rotation = targetRotation;
    }
}