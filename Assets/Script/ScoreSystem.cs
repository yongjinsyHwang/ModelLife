using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreSystem : MonoBehaviour
{
    // Interaction 점수 획득 주기를 반환한다.
    public float GetInteractionScoreInterval()
    {
        return interactionScoreInterval;
    }
    // ========================================
    // 기본 점수
    // ========================================

    // 일정 시간마다 증가하는 점수
    [SerializeField] private int passiveScoreAmount = 1;

    // 기본 점수를 획득하는 시간 간격
    [SerializeField] private float passiveScoreInterval = 1f;


    // ========================================
    // Interaction 점수
    // ========================================

    // Interaction 중 일정 시간마다 추가되는 점수
    [SerializeField] private int interactionScoreAmount = 10;

    // Interaction 점수를 획득하는 시간 간격
    [SerializeField] private float interactionScoreInterval = 1f;


    // ========================================
    // 점수 데이터
    // ========================================

    // 현재 점수
    [SerializeField] private int currentScore = 0;

    // 최대 기록
    [SerializeField] private int highScore = 0;


    // ========================================
    // UI
    // ========================================

    // 현재 점수를 표시할 UI
    [SerializeField] private Text currentScoreText;

    // 최대 기록을 표시할 UI
    [SerializeField] private Text highScoreText;


    private void Start()
    {
        // 기본 점수 증가 시작
        StartCoroutine(PassiveScoreRoutine());

        // UI 초기화
        UpdateScoreUI();
    }


    // ========================================
    // 기본 점수
    // ========================================

    // 일정 시간마다 기본 점수를 증가시킨다.
    private IEnumerator PassiveScoreRoutine()
    {
        while (true)
        {
            // 설정한 시간만큼 대기
            yield return new WaitForSeconds(passiveScoreInterval);

            // 기본 점수 획득
            AddScore(passiveScoreAmount);
        }
    }


    // ========================================
    // 점수 기능
    // ========================================

    // 점수를 증가시킨다.
    public void AddScore(int amount)
    {
        currentScore += amount;

        // 현재 점수가 최대 기록보다 높으면 갱신
        if (currentScore > highScore)
        {
            highScore = currentScore;
        }

        // UI 갱신
        UpdateScoreUI();
    }


    // Interaction 중 획득하는 점수
    public void AddInteractionScore()
    {
        AddScore(interactionScoreAmount);
    }


    // 점수를 감소시킨다.
    public void DecreaseScore(int amount)
    {
        currentScore -= amount;

        // 점수가 0보다 작아지지 않도록 한다.
        currentScore = Mathf.Max(currentScore, 0);

        // UI 갱신
        UpdateScoreUI();
    }


    // 현재 점수를 초기화한다.
    public void ResetScore()
    {
        currentScore = 0;

        UpdateScoreUI();
    }


    // 현재 점수를 반환한다.
    public int GetCurrentScore()
    {
        return currentScore;
    }


    // 최대 기록을 반환한다.
    public int GetHighScore()
    {
        return highScore;
    }


    // ========================================
    // UI
    // ========================================

    // 점수 UI를 갱신한다.
    private void UpdateScoreUI()
    {
        if (currentScoreText != null)
        {
            currentScoreText.text = "현재 기록: " + currentScore;
        }

        if (highScoreText != null)
        {
            highScoreText.text = "최대 기록: " + highScore;
        }
    }
}