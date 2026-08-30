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
            yield return new WaitForSeconds(
                scoreSystem.GetInteractionScoreInterval()
            );


            // Interaction 종료 확인
            if (!playerControllerSystem.IsInteracting())
            {
                interactionRewardRoutine = null;

                yield break;
            }


            // Score
            scoreSystem.AddInteractionScore();


            // Health 회복
            playerHealthSystem.IncreaseHealth(
                interactionHealthAmount
            );
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


        // Interaction 중이 아니면 처리하지 않는다.
        if (!playerControllerSystem.IsInteracting())
        {
            return;
        }


        switch (detectionType)
        {
            // ==================================
            // Health 감소
            // ==================================

            case DetectionType.DecreaseHealth:

                if (playerHealthSystem != null)
                {
                    playerHealthSystem.DecreaseHealth(
                        detectionHealthDecreaseAmount
                    );
                }


                // 즉시 Interaction 종료
                playerControllerSystem.EndInteraction();


                Debug.Log(
                    "Player 발각 : Health 감소 / Interaction 종료"
                );

                break;


            // ==================================
            // Kill
            // ==================================

            case DetectionType.Kill:

                Debug.Log(
                    "Player 발각 : Kill Range"
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


        isGameOver = true;


        // Interaction 종료
        if (playerControllerSystem != null &&
            playerControllerSystem.IsInteracting())
        {
            playerControllerSystem.EndInteraction();
        }


        // 최종 점수 표시 데이터
        if (lastScoreText != null &&
            scoreSystem != null)
        {
            lastScoreText.text =
                "최종 점수: " +
                scoreSystem.GetCurrentScore();
        }


        // ==================================
        // Game Over Animation
        // ==================================

        if (playerAnimationSystem != null)
        {
            playerAnimationSystem.PlayGameOver();
        }
        else
        {
            // Animation System이 없다면
            // 바로 UI 표시
            ShowGameOverUI();
        }


        Debug.Log(
            "Game Over"
        );
    }


    // ==============================
    // Game Over UI
    // ==============================

    public void ShowGameOverUI()
    {
        // 이미 UI가 표시된 상태라면 종료
        if (!isGameOver)
        {
            return;
        }


        // Game Over UI
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }


        // Restart
        if (restartButton != null)
        {
            restartButton.SetActive(true);
        }


        // Quit
        if (quitButton != null)
        {
            quitButton.SetActive(true);
        }


        // 이제 게임 정지
        Time.timeScale = 0f;


        Debug.Log(
            "Game Over UI 표시"
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


        if (playerControllerSystem != null &&
            playerControllerSystem.IsInteracting())
        {
            playerControllerSystem.EndInteraction();
        }


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