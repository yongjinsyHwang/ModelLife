using UnityEngine;

public class PlayerControllerSystem : MonoBehaviour
{
    // 현재 상호작용 중인지 확인
    private bool isInteracting = false;


    // GameManager
    [SerializeField] private GameManager gameManager;


    // ==============================
    // Interaction 입력
    // ==============================

    // Input System의 Interaction 입력이 들어왔을 때 호출
    public void OnInteraction()
    {
        // 게임 오버 상태에서는 입력을 무시
        if (gameManager.IsGameOver())
        {
            return;
        }


        // 현재 상호작용 중이라면 종료
        if (isInteracting)
        {
            EndInteraction();
        }
        else
        {
            StartInteraction();
        }
    }


    // ==============================
    // Interaction 시작
    // ==============================

    private void StartInteraction()
    {
        isInteracting = true;

        Debug.Log(
            "Player 상호작용 상태 : True"
        );


        // GameManager에 시작 전달
        gameManager.StartInteraction();
    }


    // ==============================
    // Interaction 종료
    // ==============================

    public void EndInteraction()
    {
        // 이미 종료 상태라면 실행하지 않는다.
        if (!isInteracting)
        {
            return;
        }


        isInteracting = false;

        Debug.Log(
            "Player 상호작용 상태 : False"
        );


        // GameManager에 종료 전달
        gameManager.EndInteraction();
    }


    // ==============================
    // 상태 확인
    // ==============================

    public bool IsInteracting()
    {
        return isInteracting;
    }
}