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
    // Sound Manager
    // ==============================

    [SerializeField] private SoundManager soundManager;


    // ==============================
    // Player 색상
    // ==============================

    [SerializeField] private Color interactingColor = Color.cyan;

    [SerializeField] private Color normalColor = Color.white;


    // ==============================
    // Renderer
    // ==============================

    private Renderer[] playerRenderers;


    // ==============================
    // Awake
    // ==============================

    private void Awake()
    {
        playerRenderers =
            GetComponentsInChildren<Renderer>();


        SetPlayerColor(
            normalColor
        );
    }


    // ==============================
    // Interaction 입력
    // ==============================

    public void OnInteraction()
    {
        if (gameManager == null)
        {
            return;
        }


        if (gameManager.IsGameOver() ||
            gameManager.IsGameClear())
        {
            return;
        }


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


        SetPlayerColor(
            interactingColor
        );


        // Animator Bool True
        if (animationSystem != null)
        {
            animationSystem.SetInteractionState(
                true
            );

            animationSystem.PlayJoke();
        }


        // Interaction 사운드
        if (soundManager != null)
        {
            soundManager.PlayInteractionSound();
        }


        Debug.Log(
            "Player 상호작용 상태 : True"
        );


        gameManager.StartInteraction();
    }


    // ==============================
    // Interaction 종료
    // ==============================

    public void EndInteraction()
    {
        if (!isInteracting)
        {
            return;
        }


        // 게임 상태
        isInteracting = false;


        SetPlayerColor(
            normalColor
        );


        // Animator Bool False
        if (animationSystem != null)
        {
            animationSystem.SetInteractionState(
                false
            );
        }


        Debug.Log(
            "Player 상호작용 상태 : False"
        );


        if (gameManager != null)
        {
            gameManager.EndInteraction();
        }


        // Idle을 직접 호출하지 않는다.
        // Animator의 Joke → Idle Transition이 처리한다.
    }


    // ==============================
    // Player 색상
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
    // Interaction 상태
    // ==============================

    public bool IsInteracting()
    {
        return isInteracting;
    }
}