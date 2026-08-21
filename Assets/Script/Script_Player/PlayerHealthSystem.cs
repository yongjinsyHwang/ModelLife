using UnityEngine;

public class PlayerHealthSystem : MonoBehaviour
{
    // Player의 최대 체력
    [SerializeField] private int maxHealth = 3;

    // Player의 현재 체력
    [SerializeField] private int currentHealth;


    private void Start()
    {
        // 게임 시작 시 최대 체력으로 설정
        currentHealth = maxHealth;
    }


    // Player의 체력을 감소
    public void DecreaseHealth(int amount)
    {
        currentHealth -= amount;

        // 체력이 0 이하로 감소 안되도록 설정
        currentHealth = Mathf.Max(currentHealth, 0);

        Debug.Log("Player Health : " + currentHealth);


        // 체력이 0이 되면 사망 판정
        if (currentHealth <= 0)
        {
            Die();
        }
    }


    // Player가 사망했을 때 실행
    private void Die()
    {
        Debug.Log("Player Game Over");
    }
    public void IncreaseHealth(int amount)
    {
        currentHealth += amount;

        // 최대 체력을 넘지 않도록 한다.
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        Debug.Log("Player Health : " + currentHealth);
    }
}