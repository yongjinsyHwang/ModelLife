using UnityEngine;

public class EnemyAnimationSystem : MonoBehaviour
{
    // ========================================
    // Animator
    // ========================================

    [SerializeField] private Animator animator;


    // ========================================
    // Animation Clip
    // ========================================

    // Turn 애니메이션 길이 계산용
    [SerializeField] private AnimationClip turnAnimation;

    // Return 애니메이션 길이 계산용
    [SerializeField] private AnimationClip returnAnimation;


    // ========================================
    // Trigger
    // ========================================

    [SerializeField] private string glanceTriggerName = "Glance";

    [SerializeField] private string turnTriggerName = "Turn";

    [SerializeField] private string returnTriggerName = "Return";


    // ========================================
    // Glance
    // ========================================

    public void PlayGlance()
    {
        if (animator == null)
        {
            return;
        }


        ResetAllTriggers();


        animator.SetTrigger(
            glanceTriggerName
        );
    }


    // ========================================
    // Turn
    // ========================================

    public void PlayTurn(float turnDuration)
    {
        if (animator == null)
        {
            return;
        }


        if (turnAnimation == null)
        {
            return;
        }


        if (turnDuration <= 0f)
        {
            return;
        }


        float animationSpeed =
            turnAnimation.length /
            turnDuration;


        animator.speed =
            animationSpeed;


        ResetAllTriggers();


        animator.SetTrigger(
            turnTriggerName
        );
    }


    // ========================================
    // Return
    // ========================================

    public void PlayReturn(float returnDuration)
    {
        if (animator == null)
        {
            return;
        }


        if (returnAnimation == null)
        {
            return;
        }


        if (returnDuration <= 0f)
        {
            return;
        }


        float animationSpeed =
            returnAnimation.length /
            returnDuration;


        animator.speed =
            animationSpeed;


        ResetAllTriggers();


        animator.SetTrigger(
            returnTriggerName
        );
    }


    // ========================================
    // Animator Speed Reset
    // ========================================

    public void ResetAnimationSpeed()
    {
        if (animator == null)
        {
            return;
        }


        animator.speed = 1f;
    }


    // ========================================
    // Trigger Reset
    // ========================================

    private void ResetAllTriggers()
    {
        animator.ResetTrigger(
            glanceTriggerName
        );

        animator.ResetTrigger(
            turnTriggerName
        );

        animator.ResetTrigger(
            returnTriggerName
        );
    }
}