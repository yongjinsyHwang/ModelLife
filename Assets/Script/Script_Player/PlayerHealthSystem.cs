using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealthSystem : MonoBehaviour
{
    // 최대 체력
    [SerializeField] private int maxHealth = 100;

    // 현재 체력
    [SerializeField] private int currentHealth;

    // 일정 시간마다 감소할 체력
    [SerializeField] private int healthDecreaseAmount = 10;

    // 체력이 감소하는 시간 간격
    [SerializeField] private float healthDecreaseInterval = 1f;

    // 현재 체력 UI
    [SerializeField] private Image currentHealthImage;


    private void Start()
    {
        // 게임 시작 시 최대 체력으로 설정
        currentHealth = maxHealth;

        // 체력 UI 갱신
        UpdateHealthUI();

        // 일정 시간마다 체력을 감소시키는 Coroutine 시작
        StartCoroutine(HealthDecreaseRoutine());
    }


    // 일정 시간마다 체력을 감소시킨다.
    private IEnumerator HealthDecreaseRoutine()
    {
        while (true)
        {
            // 설정한 시간만큼 대기
            yield return new WaitForSeconds(healthDecreaseInterval);

            // 체력 감소
            DecreaseHealth(healthDecreaseAmount);
        }
    }


    // 체력을 감소시킨다.
    public void DecreaseHealth(int amount)
    {
        currentHealth -= amount;

        // 체력이 0보다 작아지지 않도록 한다.
        currentHealth = Mathf.Max(currentHealth, 0);

        // 체력 UI 갱신
        UpdateHealthUI();

        Debug.Log("Player Health : " + currentHealth);
    }


    // 체력을 회복시킨다.
    public void IncreaseHealth(int amount)
    {
        currentHealth += amount;

        // 최대 체력을 넘지 않도록 한다.
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        // 체력 UI 갱신
        UpdateHealthUI();

        Debug.Log("Player Health : " + currentHealth);
    }


    // 현재 체력이 0인지 확인한다.
    public bool IsDead()
    {
        return currentHealth <= 0;
    }


    // 현재 체력을 반환한다.
    public int GetCurrentHealth()
    {
        return currentHealth;
    }


    // 체력 UI를 갱신한다.
    private void UpdateHealthUI()
    {
        if (currentHealthImage == null)
        {
            return;
        }

        // 현재 체력의 비율을 계산한다.
        float healthPercent = (float)currentHealth / maxHealth;

        // 체력바를 현재 체력 비율만큼 표시한다.
        currentHealthImage.fillAmount = healthPercent;
    }
}