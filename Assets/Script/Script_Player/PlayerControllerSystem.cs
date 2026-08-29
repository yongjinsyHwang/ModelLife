using UnityEngine;

public class PlayerControllerSystem : MonoBehaviour
{
    // ==============================
    // Interaction
    // ==============================

    // 현재 상호작용 중인지 확인
    private bool isInteracting = false;

    // GameManager
    [SerializeField] private GameManager gameManager;


    // ==============================
    // Player 색상
    // ==============================

    // Interaction이 true일 때 사용할 색상
    [SerializeField] private Color interactingColor = Color.cyan;


    // Interaction이 false일 때 사용할 원래 색상
    [SerializeField] private Color normalColor = Color.white;


    // Player의 모든 Renderer
    private Renderer[] playerRenderers;


    private void Awake()
    {
        // Player 자신과 자식 오브젝트의 Renderer를 모두 가져온다.
        playerRenderers = GetComponentsInChildren<Renderer>();

        // 시작 시 기본 색상 적용
        SetPlayerColor(normalColor);
    }


    // ==============================
    // Interaction 입력
    // ==============================

    // Input System의 Interaction 입력이 들어왔을 때 호출
    public void OnInteraction()
    {
        // Game Over 또는 Game Clear 상태라면 입력하지 않는다.
        if (gameManager.IsGameOver() ||
            gameManager.IsGameClear())
        {
            return;
        }


        // 현재 상태에 따라 Interaction 시작 또는 종료
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

        // Player 색상을 청록색으로 변경
        SetPlayerColor(interactingColor);

        Debug.Log("Player 상호작용 상태 : True");

        // GameManager에 Interaction 시작 전달
        gameManager.StartInteraction();
    }


    // ==============================
    // Interaction 종료
    // ==============================

    public void EndInteraction()
    {
        // 이미 종료된 상태라면 실행하지 않는다.
        if (!isInteracting)
        {
            return;
        }

        isInteracting = false;

        // Player 색상을 원래 색상으로 복구
        SetPlayerColor(normalColor);

        Debug.Log("Player 상호작용 상태 : False");

        // GameManager에 Interaction 종료 전달
        gameManager.EndInteraction();
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
            // Renderer의 모든 Material 색상을 변경
            foreach (Material material in playerRenderer.materials)
            {
                // 일반적인 Standard 계열 Shader
                if (material.HasProperty("_BaseColor"))
                {
                    material.SetColor("_BaseColor", color);
                }
                // 구형 Standard Shader 대응
                else if (material.HasProperty("_Color"))
                {
                    material.SetColor("_Color", color);
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