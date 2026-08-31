using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealthSystem : MonoBehaviour
{
    // ==============================
    // Health
    // ==============================

    [SerializeField] private int maxHealth = 100;

    [SerializeField] private int currentHealth;


    // ==============================
    // 기본 지속 체력 감소
    // ==============================

    [SerializeField] private int passiveDecreaseAmount = 10;

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
        currentHealth =
            maxHealth;


        UpdateHealthUI();


        passiveHealthDecreaseRoutine =
            StartCoroutine(
                PassiveHealthDecreaseRoutine()
            );
    }


    // ==============================
    // 지속 HP 감소
    // ==============================

    private IEnumerator PassiveHealthDecreaseRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(
                passiveDecreaseInterval
            );


            if (currentHealth <= 0)
            {
                yield break;
            }


            DecreaseHealth(
                passiveDecreaseAmount
            );


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


        currentHealth -=
            amount;


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


        currentHealth +=
            amount;


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
    // 상태
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
    // UI
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