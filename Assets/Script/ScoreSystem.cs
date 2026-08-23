using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreSystem : MonoBehaviour
{
    // ==============================
    // 기본 점수
    // ==============================

    // 일정 시간마다 증가하는 점수
    [SerializeField] private int passiveScoreAmount = 1;

    // 기본 점수를 획득하는 시간 간격
    [SerializeField] private float passiveScoreInterval = 1f;


    // ==============================
    // Interaction 점수
    // ==============================

    // Interaction 중 일정 시간마다 증가하는 점수
    [SerializeField] private int interactionScoreAmount = 10;

    // Interaction 점수를 획득하는 시간 간격
    [SerializeField] private float interactionScoreInterval = 1f;


    // ==============================
    // 점수 데이터
    // ==============================

    // 현재 점수
    [SerializeField] private int currentScore = 0;

    // 목표 점수
    [SerializeField] private int targetScore = 100;


    // ==============================
    // UI
    // ==============================

    // 현재 점수를 표시하는 Text
    [SerializeField] private Text currentScoreText;

    // 목표 점수를 표시하는 Text
    [SerializeField] private Text targetScoreText;


    private void Start()
    {
        // 기본 점수 증가 시작
        StartCoroutine(PassiveScoreRoutine());

        // 시작 시 UI 갱신
        UpdateScoreUI();
    }


    // ==============================
    // 기본 점수
    // ==============================

    // 일정 시간마다 기본 점수를 증가시킨다.
    private IEnumerator PassiveScoreRoutine()
    {
        while (true)
        {
            // 설정한 시간만큼 대기
            yield return new WaitForSeconds(passiveScoreInterval);

            // 기본 점수 추가
            AddScore(passiveScoreAmount);
        }
    }


    // ==============================
    // 점수 증가
    // ==============================

    // 점수를 증가시킨다.
    public void AddScore(int amount)
    {
        currentScore += amount;

        // UI 갱신
        UpdateScoreUI();
    }


    // Interaction으로 점수를 추가한다.
    public void AddInteractionScore()
    {
        AddScore(interactionScoreAmount);
    }


    // ==============================
    // 점수 감소
    // ==============================

    // 점수를 감소시킨다.
    public void DecreaseScore(int amount)
    {
        currentScore -= amount;

        // 점수가 0보다 작아지지 않도록 한다.
        currentScore = Mathf.Max(currentScore, 0);

        // UI 갱신
        UpdateScoreUI();
    }


    // ==============================
    // 점수 초기화
    // ==============================

    // 현재 점수를 초기화한다.
    public void ResetScore()
    {
        currentScore = 0;

        // UI 갱신
        UpdateScoreUI();
    }


    // ==============================
    // 점수 정보 반환
    // ==============================

    // 현재 점수를 반환한다.
    public int GetCurrentScore()
    {
        return currentScore;
    }


    // 목표 점수를 반환한다.
    public int GetTargetScore()
    {
        return targetScore;
    }


    // 목표 점수에 도달했는지 확인한다.
    public bool HasReachedTargetScore()
    {
        return currentScore >= targetScore;
    }


    // Interaction 점수 획득 간격을 반환한다.
    public float GetInteractionScoreInterval()
    {
        return interactionScoreInterval;
    }


    // ==============================
    // UI 갱신
    // ==============================

    private void UpdateScoreUI()
    {
        if (currentScoreText != null)
        {
            currentScoreText.text =
                "현재 기록: " + currentScore;
        }

        if (targetScoreText != null)
        {
            targetScoreText.text =
                "목표 기록: " + targetScore;
        }
    }
}