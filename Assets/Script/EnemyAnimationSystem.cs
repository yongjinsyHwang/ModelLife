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

    [SerializeField] private AnimationClip glanceAnimation;
    [SerializeField] private AnimationClip turnAnimation;
    [SerializeField] private AnimationClip returnAnimation;


    // ========================================
    // Trigger 이름
    // ========================================

    [SerializeField] private string glanceTriggerName = "Glance";
    [SerializeField] private string turnTriggerName = "Turn";
    [SerializeField] private string returnTriggerName = "Return";


    // ========================================
    // Idle
    // ========================================

    public void PlayIdle()
    {
        if (animator == null)
        {
            return;
        }

        animator.SetTrigger("Idle");
    }


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

        animator.SetTrigger(glanceTriggerName);
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


        // 애니메이션 원본 길이에 맞춰 속도 계산
        float animationSpeed =
            turnAnimation.length / turnDuration;


        animator.speed = animationSpeed;


        ResetAllTriggers();

        animator.SetTrigger(turnTriggerName);
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
            returnAnimation.length / returnDuration;


        animator.speed = animationSpeed;


        ResetAllTriggers();

        animator.SetTrigger(returnTriggerName);
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
    // Trigger 초기화
    // ========================================

    private void ResetAllTriggers()
    {
        animator.ResetTrigger(glanceTriggerName);
        animator.ResetTrigger(turnTriggerName);
        animator.ResetTrigger(returnTriggerName);
    }
}