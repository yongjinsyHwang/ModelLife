using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealthSystem : MonoBehaviour
{
    // ==============================
    // Health 설정
    // ==============================

    // 최대 체력
    [SerializeField] private int maxHealth = 100;

    // 현재 체력
    [SerializeField] private int currentHealth;


    // ==============================
    // 지속 감소 설정
    // ==============================

    // 일정 시간마다 감소할 체력
    [SerializeField] private int passiveDecreaseAmount = 10;

    // 체력이 감소하는 시간 간격
    [SerializeField] private float passiveDecreaseInterval = 1f;


    // ==============================
    // UI
    // ==============================

    // 현재 체력 UI
    [SerializeField] private Image currentHealthImage;


    // ==============================
    // GameManager
    // ==============================

    // GameManager
    [SerializeField] private GameManager gameManager;


    private void Start()
    {
        // 게임 시작 시 최대 체력으로 설정
        currentHealth = maxHealth;

        // 체력 UI 갱신
        UpdateHealthUI();

        // 지속적인 체력 감소 시작
        StartCoroutine(PassiveHealthDecreaseRoutine());
    }


    // ==============================
    // 지속적인 Health 감소
    // ==============================

    private IEnumerator PassiveHealthDecreaseRoutine()
    {
        while (true)
        {
            // 설정된 시간만큼 대기
            yield return new WaitForSeconds(
                passiveDecreaseInterval
            );

            // 체력 감소
            DecreaseHealth(
                passiveDecreaseAmount
            );
        }
    }


    // ==============================
    // Health 감소
    // ==============================

    public void DecreaseHealth(int amount)
    {
        // 이미 0이라면 더 이상 감소시키지 않는다.
        if (currentHealth <= 0)
        {
            return;
        }


        currentHealth -= amount;


        // 체력이 0 아래로 내려가지 않도록 한다.
        currentHealth = Mathf.Max(
            currentHealth,
            0
        );


        // 체력 UI 갱신
        UpdateHealthUI();


        Debug.Log(
            "Player Health : " + currentHealth
        );


        // Health가 0이 되었다면 GameManager에 전달
        if (currentHealth <= 0)
        {
            gameManager.OnPlayerHealthDepleted();
        }
    }


    // ==============================
    // Health 회복
    // ==============================

    public void IncreaseHealth(int amount)
    {
        // Game Over 이후에는 회복하지 않는다.
        if (currentHealth <= 0)
        {
            return;
        }


        currentHealth += amount;


        // 최대 체력을 넘지 않도록 한다.
        currentHealth = Mathf.Min(
            currentHealth,
            maxHealth
        );


        // 체력 UI 갱신
        UpdateHealthUI();


        Debug.Log(
            "Player Health : " + currentHealth
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


        // 현재 체력 비율 계산
        float healthPercent =
            (float)currentHealth / maxHealth;


        // 현재 체력에 맞춰 체력바 표시
        currentHealthImage.fillAmount =
            healthPercent;
    }
}