using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthSystem : MonoBehaviour
{
    // ==============================
    // Health
    // ==============================

    [SerializeField] private int maxHealth = 100;

    [SerializeField] private int currentHealth;


    // ==============================
    // Health UI
    // ==============================

    [SerializeField] private Image currentHealthImage;


    // ==============================
    // Game Manager
    // ==============================

    [SerializeField] private GameManager gameManager;


    // ==============================
    // Animation System
    // ==============================

    [SerializeField] private PlayerAnimationSystem animationSystem;


    // ==============================
    // Start
    // ==============================

    private void Start()
    {
        // 시작 시 최대 체력
        currentHealth = maxHealth;


        // UI 갱신
        UpdateHealthUI();
    }


    // ==============================
    // Health 감소
    // ==============================

    public void DecreaseHealth(int amount)
    {
        // 이미 죽었으면 더 이상 감소시키지 않는다.
        if (currentHealth <= 0)
        {
            return;
        }


        // HP 감소
        currentHealth -= amount;


        // 0 이하 방지
        currentHealth =
            Mathf.Max(
                currentHealth,
                0
            );


        // UI 갱신
        UpdateHealthUI();


        Debug.Log(
            "Player Health : " +
            currentHealth
        );


        // HP 감소 애니메이션
        if (animationSystem != null)
        {
            animationSystem.PlayHpDecrease();
        }


        // Health 0
        if (currentHealth <= 0)
        {
            if (gameManager != null)
            {
                gameManager.OnPlayerHealthDepleted();
            }
        }
    }


    // ==============================
    // Health 회복
    // ==============================

    public void IncreaseHealth(int amount)
    {
        // 사망 상태라면 회복하지 않는다.
        if (currentHealth <= 0)
        {
            return;
        }


        // HP 증가
        currentHealth += amount;


        // 최대 체력 제한
        currentHealth =
            Mathf.Min(
                currentHealth,
                maxHealth
            );


        // UI 갱신
        UpdateHealthUI();


        Debug.Log(
            "Player Health : " +
            currentHealth
        );
    }


    // ==============================
    // 상태 확인
    // ==============================

    public bool IsDead()
    {
        return currentHealth <= 0;
    }


    public int GetCurrentHealth()
    {
        return currentHealth;
    }


    // ==============================
    // Health UI
    // ==============================

    private void UpdateHealthUI()
    {
        if (currentHealthImage == null)
        {
            return;
        }


        float healthPercent =
            (float)currentHealth /
            maxHealth;


        currentHealthImage.fillAmount =
            healthPercent;
    }
}