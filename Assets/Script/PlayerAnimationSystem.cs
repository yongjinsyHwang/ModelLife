using UnityEngine;

public class PlayerAnimationSystem : MonoBehaviour
{
    // ========================================
    // Animator
    // ========================================

    [SerializeField] private Animator animator;


    // ========================================
    // Game Manager
    // ========================================

    [SerializeField] private GameManager gameManager;


    // ========================================
    // Interaction Bool
    // ========================================

    [SerializeField]
    private string interactionBoolName =
        "IsInteracting";


    // ========================================
    // Joke
    // ========================================

    [SerializeField] private string[] jokeStateNames;


    // ========================================
    // HP Decrease
    // ========================================

    [SerializeField] private string[] hpDecreaseStateNames;


    // ========================================
    // Game Over
    // ========================================

    [SerializeField]
    private string gameOverStateName =
        "GameOver";


    [SerializeField] private AnimationClip gameOverAnimation;


    // ========================================
    // Interaction 상태
    // ========================================

    public void SetInteractionState(bool state)
    {
        if (animator == null)
        {
            return;
        }


        animator.SetBool(
            interactionBoolName,
            state
        );
    }


    // ========================================
    // Joke
    // ========================================

    public void PlayJoke()
    {
        if (animator == null)
        {
            return;
        }


        if (jokeStateNames == null ||
            jokeStateNames.Length == 0)
        {
            return;
        }


        string selectedState =
            jokeStateNames[
                Random.Range(
                    0,
                    jokeStateNames.Length
                )
            ];


        animator.CrossFadeInFixedTime(
            selectedState,
            0.05f,
            0,
            0f
        );
    }


    // ========================================
    // HP Decrease
    // ========================================

    public void PlayHpDecrease()
    {
        if (animator == null)
        {
            return;
        }


        if (hpDecreaseStateNames == null ||
            hpDecreaseStateNames.Length == 0)
        {
            return;
        }


        string selectedState =
            hpDecreaseStateNames[
                Random.Range(
                    0,
                    hpDecreaseStateNames.Length
                )
            ];


        animator.CrossFadeInFixedTime(
            selectedState,
            0.05f,
            0,
            0f
        );
    }


    // ========================================
    // Game Over
    // ========================================

    public void PlayGameOver()
    {
        if (animator == null)
        {
            return;
        }


        animator.CrossFadeInFixedTime(
            gameOverStateName,
            0.05f,
            0,
            0f
        );
    }


    // ========================================
    // Game Over Animation 길이
    // ========================================

    public float GetGameOverAnimationLength()
    {
        if (gameOverAnimation == null)
        {
            return 0f;
        }


        return gameOverAnimation.length;
    }
}