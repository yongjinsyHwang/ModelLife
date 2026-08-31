using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    // ==============================
    // System References
    // ==============================

    [SerializeField] private ScoreSystem scoreSystem;

    [SerializeField] private PlayerHealthSystem playerHealthSystem;

    [SerializeField] private PlayerControllerSystem playerControllerSystem;

    [SerializeField] private PlayerAnimationSystem playerAnimationSystem;

    [SerializeField] private CameraManager cameraManager;

    [SerializeField] private SoundManager soundManager;


    // ==============================
    // Detection Settings
    // ==============================

    [SerializeField] private int detectionHealthDecreaseAmount = 10;


    // ==============================
    // Interaction 보상
    // ==============================

    [SerializeField] private int interactionHealthAmount = 1;


    // ==============================
    // Game Over UI
    // ==============================

    [SerializeField] private GameObject gameOverUI;

    [SerializeField] private Text lastScoreText;


    // ==============================
    // Game Clear UI
    // ==============================

    [SerializeField] private GameObject gameClearButton;

    [SerializeField] private GameObject gameClearUI;


    // ==============================
    // Game Control Buttons
    // ==============================

    [SerializeField] private GameObject restartButton;

    [SerializeField] private GameObject quitButton;


    // ==============================
    // Interaction Coroutine
    // ==============================

    private Coroutine interactionRewardRoutine;


    // ==============================
    // Game State
    // ==============================

    private bool isGameOver = false;

    private bool isGameClear = false;


    // ==============================
    // Game Over Final Score
    // ==============================

    private int finalScore = 0;


    // ==============================
    // Detection Type
    // ==============================

    public enum DetectionType
    {
        DecreaseHealth,
        Kill
    }


    // ==============================
    // Start
    // ==============================

    private void Start()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false);
        }


        if (gameClearButton != null)
        {
            gameClearButton.SetActive(false);
        }


        if (gameClearUI != null)
        {
            gameClearUI.SetActive(false);
        }


        if (restartButton != null)
        {
            restartButton.SetActive(false);
        }


        if (quitButton != null)
        {
            quitButton.SetActive(false);
        }


        Time.timeScale = 1f;
    }


    // ==============================
    // Update
    // ==============================

    private void Update()
    {
        if (isGameOver || isGameClear)
        {
            return;
        }


        if (scoreSystem != null &&
            scoreSystem.HasReachedTargetScore())
        {
            GameClear();
        }
    }


    // ==============================
    // Interaction 시작
    // ==============================

    public void StartInteraction()
    {
        if (isGameOver || isGameClear)
        {
            return;
        }


        if (interactionRewardRoutine != null)
        {
            return;
        }


        interactionRewardRoutine =
            StartCoroutine(
                InteractionRewardRoutine()
            );
    }


    // ==============================
    // Interaction 종료
    // ==============================

    public void EndInteraction()
    {
        if (interactionRewardRoutine != null)
        {
            StopCoroutine(
                interactionRewardRoutine
            );

            interactionRewardRoutine = null;
        }
    }


    // ==============================
    // Interaction 보상
    // ==============================

    private IEnumerator InteractionRewardRoutine()
    {
        while (
            playerControllerSystem != null &&
            playerControllerSystem.IsInteracting()
        )
        {
            if (scoreSystem == null)
            {
                interactionRewardRoutine = null;
                yield break;
            }


            yield return new WaitForSeconds(
                scoreSystem.GetInteractionScoreInterval()
            );


            if (isGameOver || isGameClear)
            {
                interactionRewardRoutine = null;
                yield break;
            }


            if (!playerControllerSystem.IsInteracting())
            {
                interactionRewardRoutine = null;
                yield break;
            }


            scoreSystem.AddInteractionScore();


            if (playerHealthSystem != null)
            {
                playerHealthSystem.IncreaseHealth(
                    interactionHealthAmount
                );
            }
        }


        interactionRewardRoutine = null;
    }


    // ==============================
    // Enemy Detection
    // ==============================

    public void OnEnemyDetection(
        DetectionType detectionType
    )
    {
        if (isGameOver || isGameClear)
        {
            return;
        }


        if (playerControllerSystem == null)
        {
            return;
        }


        if (!playerControllerSystem.IsInteracting())
        {
            return;
        }


        switch (detectionType)
        {
            // ==============================
            // Health 감소
            // ==============================

            case DetectionType.DecreaseHealth:

                Debug.Log(
                    "Enemy 발각 : Health 감소"
                );


                if (playerHealthSystem != null)
                {
                    playerHealthSystem.DecreaseHealth(
                        detectionHealthDecreaseAmount
                    );
                }


                // HP 0이면 이미 GameOver 처리됨
                if (!isGameOver &&
                    playerHealthSystem != null &&
                    playerHealthSystem.GetCurrentHealth() > 0)
                {
                    if (playerAnimationSystem != null)
                    {
                        playerAnimationSystem.PlayHpDecrease();
                    }
                }


                if (playerControllerSystem.IsInteracting())
                {
                    playerControllerSystem.EndInteraction();
                }


                Debug.Log(
                    "Player 발각 : Health 감소 / Interaction 종료"
                );

                break;


            // ==============================
            // Kill
            // ==============================

            case DetectionType.Kill:

                Debug.Log(
                    "Enemy 발각 : Kill Range"
                );


                GameOver();

                break;
        }
    }


    // ==============================
    // Health 0
    // ==============================

    public void OnPlayerHealthDepleted()
    {
        if (isGameOver || isGameClear)
        {
            return;
        }


        GameOver();
    }


    // ==============================
    // Game Over
    // ==============================

    public void GameOver()
    {
        if (isGameOver || isGameClear)
        {
            return;
        }


        // ==================================
        // Game Over 즉시 확정
        // ==================================

        isGameOver = true;


        // ==================================
        // 최종 점수 확정
        // ==================================

        if (scoreSystem != null)
        {
            finalScore =
                scoreSystem.GetCurrentScore();
        }


        // ==================================
        // Interaction 종료
        // ==================================

        if (playerControllerSystem != null &&
            playerControllerSystem.IsInteracting())
        {
            playerControllerSystem.EndInteraction();
        }


        // ==================================
        // 최종 점수 UI
        // ==================================

        if (lastScoreText != null)
        {
            lastScoreText.text =
                "최종 점수: " +
                finalScore;
        }


        // ==================================
        // Game Over 사운드
        // ==================================

        if (soundManager != null)
        {
            soundManager.PlayGameOverSound();
        }


        // ==================================
        // Game Over Animation
        // ==================================

        if (playerAnimationSystem != null)
        {
            playerAnimationSystem.PlayGameOver();

            StartCoroutine(
                ShowGameOverUIAfterAnimation()
            );
        }
        else
        {
            ShowGameOverUI();
        }


        Debug.Log(
            "Game Over / 최종 점수 : " +
            finalScore
        );
    }


    // ==============================
    // Game Over Animation 대기
    // ==============================

    private IEnumerator ShowGameOverUIAfterAnimation()
    {
        yield return null;


        float animationLength = 0f;


        if (playerAnimationSystem != null)
        {
            animationLength =
                playerAnimationSystem
                    .GetGameOverAnimationLength();
        }


        if (animationLength <= 0f)
        {
            animationLength = 1f;
        }


        yield return new WaitForSeconds(
            animationLength
        );


        ShowGameOverUI();
    }


    // ==============================
    // Game Over UI
    // ==============================

    public void ShowGameOverUI()
    {
        if (!isGameOver)
        {
            return;
        }


        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }


        if (restartButton != null)
        {
            restartButton.SetActive(true);
        }


        if (quitButton != null)
        {
            quitButton.SetActive(true);
        }


        Time.timeScale = 0f;


        Debug.Log(
            "Game Over UI 표시 / 최종 점수 : " +
            finalScore
        );
    }


    // ==============================
    // Game Clear
    // ==============================

    public void GameClear()
    {
        if (isGameOver || isGameClear)
        {
            return;
        }


        isGameClear = true;


        // ==================================
        // Game Clear 사운드
        // ==================================

        if (soundManager != null)
        {
            soundManager.PlayGameClearSound();
        }


        // ==================================
        // PlayerC 카메라 고정
        // ==================================

        if (cameraManager != null)
        {
            cameraManager.LockGameClearCamera();

            cameraManager.PlayGameClearEffect();
        }


        // ==================================
        // Interaction 종료
        // ==================================

        if (playerControllerSystem != null &&
            playerControllerSystem.IsInteracting())
        {
            playerControllerSystem.EndInteraction();
        }


        // ==================================
        // UI
        // ==================================

        if (gameClearButton != null)
        {
            gameClearButton.SetActive(true);
        }


        if (restartButton != null)
        {
            restartButton.SetActive(true);
        }


        if (quitButton != null)
        {
            quitButton.SetActive(true);
        }


        Debug.Log(
            "Game Clear : " +
            scoreSystem.GetCurrentScore()
        );
    }


    // ==============================
    // Game Clear UI
    // ==============================

    public void ShowGameClearUI()
    {
        if (!isGameClear)
        {
            return;
        }


        if (gameClearUI != null)
        {
            gameClearUI.SetActive(true);
        }


        if (gameClearButton != null)
        {
            gameClearButton.SetActive(false);
        }


        Time.timeScale = 0f;
    }


    // ==============================
    // Restart
    // ==============================

    public void RestartGame()
    {
        Time.timeScale = 1f;


        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }


    // ==============================
    // Quit
    // ==============================

    public void QuitGame()
    {
        Time.timeScale = 1f;


        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }


    // ==============================
    // 상태 확인
    // ==============================

    public bool IsGameOver()
    {
        return isGameOver;
    }


    public bool IsGameClear()
    {
        return isGameClear;
    }


    // ==============================
    // Player Controller 반환
    // ==============================

    public PlayerControllerSystem GetPlayerControllerSystem()
    {
        return playerControllerSystem;
    }
}