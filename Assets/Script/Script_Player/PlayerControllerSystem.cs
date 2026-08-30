using UnityEngine;

public class PlayerControllerSystem : MonoBehaviour
{
    // ==============================
    // Interaction
    // ==============================

    private bool isInteracting = false;


    // ==============================
    // Game Manager
    // ==============================

    [SerializeField] private GameManager gameManager;


    // ==============================
    // Animation System
    // ==============================

    [SerializeField] private PlayerAnimationSystem animationSystem;


    // ==============================
    // Player 색상
    // ==============================

    [SerializeField] private Color interactingColor = Color.cyan;

    [SerializeField] private Color normalColor = Color.white;


    // Player의 모든 Renderer
    private Renderer[] playerRenderers;


    // ==============================
    // Awake
    // ==============================

    private void Awake()
    {
        // Player 자신과 자식 오브젝트의 Renderer를 모두 가져온다.
        playerRenderers =
            GetComponentsInChildren<Renderer>();


        // 시작 시 기본 색상
        SetPlayerColor(normalColor);
    }


    // ==============================
    // Interaction 입력
    // ==============================

    public void OnInteraction()
    {
        // GameManager가 없으면 종료
        if (gameManager == null)
        {
            return;
        }


        // Game Over / Clear 상태에서는 입력하지 않는다.
        if (gameManager.IsGameOver() ||
            gameManager.IsGameClear())
        {
            return;
        }


        // 상태에 따라 시작 / 종료
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


        // Player 색상 변경
        SetPlayerColor(interactingColor);


        Debug.Log(
            "Player 상호작용 상태 : True"
        );


        // Joke 애니메이션 재생
        if (animationSystem != null)
        {
            animationSystem.PlayJoke();
        }


        // GameManager에 시작 전달
        gameManager.StartInteraction();
    }


    // ==============================
    // Interaction 종료
    // ==============================

    public void EndInteraction()
    {
        // 이미 종료되어 있다면 실행하지 않는다.
        if (!isInteracting)
        {
            return;
        }


        // 가장 먼저 상태를 false로 변경
        isInteracting = false;


        // 색상 원복
        SetPlayerColor(normalColor);


        Debug.Log(
            "Player 상호작용 상태 : False"
        );


        // GameManager에 종료 전달
        if (gameManager != null)
        {
            gameManager.EndInteraction();
        }
    }


    // ==============================
    // Player 색상 변경
    // ==============================

    private void SetPlayerColor(Color color)
    {
        if (playerRenderers == null)
        {
            return;
        }


        foreach (Renderer playerRenderer in playerRenderers)
        {
            foreach (Material material in playerRenderer.materials)
            {
                if (material.HasProperty("_BaseColor"))
                {
                    material.SetColor(
                        "_BaseColor",
                        color
                    );
                }
                else if (material.HasProperty("_Color"))
                {
                    material.SetColor(
                        "_Color",
                        color
                    );
                }
            }
        }
    }


    // ==============================
    // Interaction 상태 반환
    // ==============================

    public bool IsInteracting()
    {
        return isInteracting;
    }
}