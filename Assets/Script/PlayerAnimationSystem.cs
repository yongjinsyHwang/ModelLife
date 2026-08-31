using UnityEngine;

public class PlayerAnimationSystem : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] private string[] jokeStateNames;

    [SerializeField] private string[] hpDecreaseStateNames;

    [SerializeField] private string gameOverStateName = "GameOver";

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
            "IsInteracting",
            state
        );

        Debug.Log(
            "Animator IsInteracting = " + state
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

        Debug.Log(
            "Joke 재생 : " + selectedState
        );

        animator.CrossFadeInFixedTime(
            selectedState,
            0.05f,
            0,
            0f
        );
    }


    // ========================================
    // HP 감소
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
    // Game Over Animation Length
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