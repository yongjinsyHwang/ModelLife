using UnityEngine;

public class PlayerAnimationSystem : MonoBehaviour
{
    // ========================================
    // Animator
    // ========================================

    [SerializeField] private Animator animator;


    // ========================================
    // Joke Animation Group
    // ========================================

    // Joke 애니메이션 State 이름들
    [SerializeField] private string[] jokeStateNames;


    // ========================================
    // HP Decrease Animation Group
    // ========================================

    // HP 감소 애니메이션 State 이름들
    [SerializeField] private string[] hpDecreaseStateNames;


    // ========================================
    // Game Over Animation
    // ========================================

    [SerializeField] private string gameOverStateName = "GameOver";


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


        animator.Play(
            selectedState,
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


        animator.Play(
            selectedState,
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


        animator.Play(
            gameOverStateName,
            0,
            0f
        );
    }


    // ========================================
    // Game Over Animation Event
    // ========================================

    // Game Over 애니메이션 마지막 프레임에서 호출
    public void OnGameOverAnimationFinished()
    {
        GameManager gameManager =
            FindFirstObjectByType<GameManager>();


        if (gameManager == null)
        {
            return;
        }


        gameManager.ShowGameOverUI();
    }
}