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

    [SerializeField] private AnimationClip turnAnimation;

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
        if (animator == null ||
            turnAnimation == null ||
            turnDuration <= 0f)
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
        if (animator == null ||
            returnAnimation == null ||
            returnDuration <= 0f)
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
    // Animation Speed Reset
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