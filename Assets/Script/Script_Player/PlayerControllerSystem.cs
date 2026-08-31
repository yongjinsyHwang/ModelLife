using UnityEngine;

public class PlayerControllerSystem : MonoBehaviour
{
    // ==============================
    // Interaction
    // ==============================

    // 현재 Interaction 상태
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

    // Interaction 중일 때 색상
    [SerializeField] private Color interactingColor = Color.cyan;

    // Interaction이 아닐 때 색상
    [SerializeField] private Color normalColor = Color.white;


    // ==============================
    // Renderer
    // ==============================

    // Player 자신과 자식의 모든 Renderer
    private Renderer[] playerRenderers;


    // ==============================
    // Awake
    // ==============================

    private void Awake()
    {
        // Player Renderer 가져오기
        playerRenderers =
            GetComponentsInChildren<Renderer>();


        // 시작 시 기본 색상
        SetPlayerColor(normalColor);


        Debug.Log(
            "PlayerController 생성 : " +
            gameObject.name
        );
    }


    // ==============================
    // Interaction 입력
    // ==============================

    public void OnInteraction()
    {
        // GameManager가 없으면 입력 처리하지 않음
        if (gameManager == null)
        {
            Debug.LogWarning(
                "PlayerControllerSystem : GameManager가 연결되지 않음"
            );

            return;
        }


        // Game Over / Game Clear 상태에서는 입력하지 않음
        if (gameManager.IsGameOver() ||
            gameManager.IsGameClear())
        {
            return;
        }


        // 현재 상태에 따라 시작 / 종료
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
        // --------------------------------
        // 1. 게임 로직 상태 변경
        // --------------------------------

        isInteracting = true;


        // --------------------------------
        // 2. Player 색상 변경
        // --------------------------------

        SetPlayerColor(
            interactingColor
        );


        // --------------------------------
        // 3. Animator 상태 변경
        // 반드시 Bool을 먼저 True로 만든다.
        // --------------------------------

        if (animationSystem != null)
        {
            animationSystem.SetInteractionState(
                true
            );


            // Bool이 True가 된 뒤
            // Joke 애니메이션 재생
            animationSystem.PlayJoke();
        }


        Debug.Log(
            "Player 상호작용 상태 : True"
        );


        // --------------------------------
        // 4. GameManager
        // --------------------------------

        gameManager.StartInteraction();
    }


    // ==============================
    // Interaction 종료
    // ==============================

    public void EndInteraction()
    {
        // 이미 false라면 중복 처리하지 않음
        if (!isInteracting)
        {
            return;
        }


        // --------------------------------
        // 1. 게임 로직 상태를 먼저 False
        // --------------------------------

        isInteracting = false;


        // --------------------------------
        // 2. 색상 복구
        // --------------------------------

        SetPlayerColor(
            normalColor
        );


        // --------------------------------
        // 3. Animator Bool False
        // --------------------------------

        if (animationSystem != null)
        {
            animationSystem.SetInteractionState(
                false
            );
        }


        Debug.Log(
            "Player 상호작용 상태 : False"
        );


        // --------------------------------
        // 4. GameManager 보상 종료
        // --------------------------------

        if (gameManager != null)
        {
            gameManager.EndInteraction();
        }


        // 여기서 PlayIdle()은 호출하지 않는다.
        //
        // Animator의
        //
        // Joke1 → Idle1
        // Joke2 → Idle1
        //
        // Transition이
        //
        // IsInteracting == false
        //
        // 를 보고 Idle로 전환한다.
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
            if (playerRenderer == null)
            {
                continue;
            }


            foreach (Material material in playerRenderer.materials)
            {
                if (material == null)
                {
                    continue;
                }


                // URP / 일반 Shader
                if (material.HasProperty("_BaseColor"))
                {
                    material.SetColor(
                        "_BaseColor",
                        color
                    );
                }
                // 기존 Standard Shader
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