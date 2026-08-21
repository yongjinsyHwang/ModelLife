using UnityEngine;

public class PlayerControllerSystem : MonoBehaviour
{
    // 현재 상호작용 중인지 확인
    private bool isInteracting = false;


    // Interaction 입력이 들어왔을 때 호출
    public void OnInteraction()
    {
        isInteracting = !isInteracting;

        Debug.Log("Player 상호작용 상태 : " + isInteracting);
    }
}