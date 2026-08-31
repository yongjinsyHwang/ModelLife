using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreSystem : MonoBehaviour
{
    // ==============================
    // 기본 점수
    // ==============================

    [SerializeField] private int passiveScoreAmount = 1;

    [SerializeField] private float passiveScoreInterval = 1f;


    // ==============================
    // Interaction 점수
    // ==============================

    [SerializeField] private int interactionScoreAmount = 10;

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
    // Coroutine
    // ==============================

    private Coroutine passiveScoreRoutine;


    // ==============================
    // Start
    // ==============================

    private void Start()
    {
        passiveScoreRoutine =
            StartCoroutine(
                PassiveScoreRoutine()
            );


        UpdateScoreUI();
    }


    // ==============================
    // 기본 점수
    // ==============================

    private IEnumerator PassiveScoreRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(
                passiveScoreInterval
            );


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
    // 목표 달성
    // ==============================

    public bool HasReachedTargetScore()
    {
        return currentScore >= targetScore;
    }


    // ==============================
    // Interaction 간격
    // ==============================

    public float GetInteractionScoreInterval()
    {
        return interactionScoreInterval;
    }


    // ==============================
    // 게임 종료 확인
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