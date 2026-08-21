using UnityEngine;
using System.Collections;

public class PlayerControllerSystem : MonoBehaviour
{
    // 현재 상호작용 중인지 확인
    private bool isInteracting = false;

    // 점수 시스템
    [SerializeField] private ScoreSystem scoreSystem;


    // Interaction 입력이 들어왔을 때 호출
    public void OnInteraction()
    {
        // 상호작용 상태를 반전시킨다.
        isInteracting = !isInteracting;

        Debug.Log("Player 상호작용 상태 : " + isInteracting);


        // 상호작용이 시작되었을 때
        if (isInteracting)
        {
            // Interaction 점수 획득 시작
            StartCoroutine(InteractionScoreRoutine());
        }
    }


    // Interaction 중 일정 시간마다 점수를 획득한다.
    private IEnumerator InteractionScoreRoutine()
    {
        while (isInteracting)
        {
            // ScoreSystem에 설정된 시간이 아니라
            // PlayerControllerSystem에서 직접 시간을 정하지 않고,
            // ScoreSystem의 설정값을 사용한다.
            yield return new WaitForSeconds(
                scoreSystem.GetInteractionScoreInterval()
            );

            // 기다리는 동안 Interaction이 종료되지 않았다면 점수 획득
            if (isInteracting)
            {
                scoreSystem.AddInteractionScore();
            }
        }
    }


    // 현재 상호작용 중인지 반환한다.
    public bool IsInteracting()
    {
        return isInteracting;
    }
}