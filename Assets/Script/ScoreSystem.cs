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

    // 기본 점수 획득 간격
    [SerializeField] private float passiveScoreInterval = 1f;


    // ==============================
    // Interaction 점수
    // ==============================

    // Interaction 중 일정 시간마다 증가하는 점수
    [SerializeField] private int interactionScoreAmount = 10;

    // Interaction 점수 획득 간격
    [SerializeField] private float interactionScoreInterval = 1f;


    // ==============================
    // 점수 데이터
    // ==============================

    [SerializeField] private int currentScore = 0;

    [SerializeField] private int targetScore = 100;


    // ==============================
    // UI
    // ==============================

    [SerializeField] private Text currentScoreText;

    [SerializeField] private Text targetScoreText;


    // ==============================
    // Game Manager
    // ==============================

    [SerializeField] private GameManager gameManager;


    // ==============================
    // 기본 점수 Coroutine
    // ==============================

    private Coroutine passiveScoreRoutine;


    // ==============================
    // Start
    // ==============================

    private void Start()
    {
        // 기본 점수 증가 시작
        passiveScoreRoutine =
            StartCoroutine(
                PassiveScoreRoutine()
            );


        // 시작 시 UI
        UpdateScoreUI();
    }


    // ==============================
    // 기본 점수
    // ==============================

    private IEnumerator PassiveScoreRoutine()
    {
        while (true)
        {
            // 점수 증가 간격 대기
            yield return new WaitForSeconds(
                passiveScoreInterval
            );


            // 게임 종료 후에는 증가하지 않는다.
            if (IsGameFinished())
            {
                yield break;
            }


            AddScore(
                passiveScoreAmount
            );
        }
    }


    // ==============================
    // 점수 증가
    // ==============================

    public void AddScore(int amount)
    {
        // Game Over / Game Clear 이후에는
        // 점수를 증가시키지 않는다.
        if (IsGameFinished())
        {
            return;
        }


        currentScore += amount;


        UpdateScoreUI();
    }


    // ==============================
    // Interaction 점수
    // ==============================

    public void AddInteractionScore()
    {
        if (IsGameFinished())
        {
            return;
        }


        AddScore(
            interactionScoreAmount
        );
    }


    // ==============================
    // 점수 감소
    // ==============================

    public void DecreaseScore(int amount)
    {
        if (IsGameFinished())
        {
            return;
        }


        currentScore -= amount;


        currentScore =
            Mathf.Max(
                currentScore,
                0
            );


        UpdateScoreUI();
    }


    // ==============================
    // 점수 초기화
    // ==============================

    public void ResetScore()
    {
        currentScore = 0;

        UpdateScoreUI();
    }


    // ==============================
    // 현재 점수
    // ==============================

    public int GetCurrentScore()
    {
        return currentScore;
    }


    // ==============================
    // 목표 점수
    // ==============================

    public int GetTargetScore()
    {
        return targetScore;
    }


    // ==============================
    // 목표 달성 여부
    // ==============================

    public bool HasReachedTargetScore()
    {
        return currentScore >= targetScore;
    }


    // ==============================
    // Interaction 점수 간격
    // ==============================

    public float GetInteractionScoreInterval()
    {
        return interactionScoreInterval;
    }


    // ==============================
    // 게임 종료 여부
    // ==============================

    private bool IsGameFinished()
    {
        if (gameManager == null)
        {
            return false;
        }


        return gameManager.IsGameOver() ||
               gameManager.IsGameClear();
    }


    // ==============================
    // UI 갱신
    // ==============================

    private void UpdateScoreUI()
    {
        if (currentScoreText != null)
        {
            currentScoreText.text =
                "현재 기록: " +
                currentScore;
        }


        if (targetScoreText != null)
        {
            targetScoreText.text =
                "목표 기록: " +
                targetScore;
        }
    }
}