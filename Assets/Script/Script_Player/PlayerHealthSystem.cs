using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealthSystem : MonoBehaviour
{
    // ==============================
    // Health
    // ==============================

    // 최대 체력
    [SerializeField] private int maxHealth = 100;

    // 현재 체력
    [SerializeField] private int currentHealth;


    // ==============================
    // 기본 지속 체력 감소
    // ==============================

    // 일정 시간마다 감소할 체력
    [SerializeField] private int passiveDecreaseAmount = 10;

    // 체력 감소 간격
    [SerializeField] private float passiveDecreaseInterval = 1f;


    // ==============================
    // Health UI
    // ==============================

    [SerializeField] private Image currentHealthImage;


    // ==============================
    // Game Manager
    // ==============================

    [SerializeField] private GameManager gameManager;


    // ==============================
    // Coroutine
    // ==============================

    private Coroutine passiveHealthDecreaseRoutine;


    // ==============================
    // Start
    // ==============================

    private void Start()
    {
        currentHealth = maxHealth;


        UpdateHealthUI();


        // 기본 지속 HP 감소 시작
        passiveHealthDecreaseRoutine =
            StartCoroutine(
                PassiveHealthDecreaseRoutine()
            );
    }


    // ==============================
    // 기본 지속 HP 감소
    // ==============================

    private IEnumerator PassiveHealthDecreaseRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(
                passiveDecreaseInterval
            );


            // 이미 죽었으면 종료
            if (currentHealth <= 0)
            {
                yield break;
            }


            // 기본 지속 HP 감소
            DecreaseHealth(
                passiveDecreaseAmount
            );


            // Game Over라면 종료
            if (gameManager != null &&
                gameManager.IsGameOver())
            {
                yield break;
            }
        }
    }


    // ==============================
    // Health 감소
    // ==============================

    public void DecreaseHealth(int amount)
    {
        if (currentHealth <= 0)
        {
            return;
        }


        currentHealth -= amount;


        currentHealth =
            Mathf.Max(
                currentHealth,
                0
            );


        UpdateHealthUI();


        Debug.Log(
            "Player Health : " +
            currentHealth
        );


        // ==================================
        // 여기서는 HP 감소 애니메이션을
        // 호출하지 않는다.
        //
        // Enemy에 의한 감소인지
        // 기본 지속 감소인지 구분해야 하기 때문.
        // ==================================


        // Health = 0
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
        if (currentHealth <= 0)
        {
            return;
        }


        currentHealth += amount;


        currentHealth =
            Mathf.Min(
                currentHealth,
                maxHealth
            );


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