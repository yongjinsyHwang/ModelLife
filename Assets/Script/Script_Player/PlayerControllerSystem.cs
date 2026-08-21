using UnityEngine;
using System.Collections;

public class PlayerControllerSystem : MonoBehaviour
{
    // 현재 상호작용 중인지 확인
    private bool isInteracting = false;

    // 점수를 획득하는 간격
    [SerializeField] private float scoreInterval = 1f;


    private void Start()
    {
        // 점수 획득 반복을 시작한다.
        StartCoroutine(ScoreRoutine());
    }


    // Input System에서 Interaction 입력이 들어왔을 때 호출
    public void OnInteraction()
    {
        // 현재 상호작용 상태를 반전시킨다.
        isInteracting = !isInteracting;

        Debug.Log("Player 상호작용 상태 : " + isInteracting);
    }


    // 상호작용 중일 때 일정 시간마다 점수를 획득하는 Coroutine
    private IEnumerator ScoreRoutine()
    {
        while (true)
        {
            // 점수 획득 간격만큼 기다린다.
            yield return new WaitForSeconds(scoreInterval);

            // 현재 상호작용 중이라면 점수 획득
            if (isInteracting)
            {
                Debug.Log("점수 획득");
            }
        }
    }
}