using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

/*
- Interaction 시작/종료
- Interaction 중 일정 시간마다 점수 + Health 회복
- Decrease Health Raycast → Health 감소 + Interaction 종료
- Kill Range + Interaction → Game Over
- Health 0 → Game Over
- 목표 점수 도달 → Game Clear 버튼 활성화
- Game Clear 버튼 → Clear UI 표시
- Restart 버튼 → 현재 씬 재시작
- Quit 버튼 → 게임 종료
- Game Over 시 최종 점수 표시
- GetPlayerControllerSystem()
- IsGameOver(), IsGameClear()
*/
public class GameManager : MonoBehaviour
{
    // ==============================
    // System References
    // ==============================

    // Score System
    [SerializeField] private ScoreSystem scoreSystem;

    // Player Health System
    [SerializeField] private PlayerHealthSystem playerHealthSystem;

    // Player Controller System
    [SerializeField] private PlayerControllerSystem playerControllerSystem;


    // ==============================
    // Detection Settings
    // ==============================

    // Decrease Health Raycast에 감지되었을 때 감소할 Health
    [SerializeField] private int detectionHealthDecreaseAmount = 10;


    // ==============================
    // Interaction 보상
    // ==============================

    // Interaction 점수 획득 시 회복되는 Health
    [SerializeField] private int interactionHealthAmount = 1;


    // ==============================
    // Game Over UI
    // ==============================

    // Game Over UI 전체
    [SerializeField] private GameObject gameOverUI;

    // Game Over 시 최종 점수를 표시하는 Text
    [SerializeField] private Text lastScoreText;


    // ==============================
    // Game Clear UI
    // ==============================

    // 목표 점수 달성 시 활성화되는 버튼
    [SerializeField] private GameObject gameClearButton;

    // Game Clear 버튼 클릭 후 표시되는 UI
    [SerializeField] private GameObject gameClearUI;


    // ==============================
    // Game Control Buttons
    // ==============================

    // 게임 재시작 버튼
    [SerializeField] private GameObject restartButton;

    // 게임 종료 버튼
    [SerializeField] private GameObject quitButton;


    // ==============================
    // Interaction Coroutine
    // ==============================

    private Coroutine interactionRewardRoutine;


    // ==============================
    // Game State
    // ==============================

    // Game Over 상태
    private bool isGameOver = false;

    // Game Clear 상태
    private bool isGameClear = false;


    // ==============================
    // Detection Type
    // ==============================

    public enum DetectionType
    {
        DecreaseHealth,
        Kill
    }


    private void Start()
    {
        // Game Over UI 비활성화
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false);
        }

        // Game Clear 버튼 비활성화
        if (gameClearButton != null)
        {
            gameClearButton.SetActive(false);
        }

        // Game Clear UI 비활성화
        if (gameClearUI != null)
        {
            gameClearUI.SetActive(false);
        }

        // Restart 버튼 비활성화
        if (restartButton != null)
        {
            restartButton.SetActive(false);
        }

        // Quit 버튼 비활성화
        if (quitButton != null)
        {
            quitButton.SetActive(false);
        }

        // 게임 시작 시 시간 정상화
        Time.timeScale = 1f;
    }


    private void Update()
    {
        // Game Over 또는 Game Clear 상태에서는
        // 목표 점수를 검사하지 않는다.
        if (isGameOver || isGameClear)
        {
            return;
        }

        // 현재 점수가 목표 점수 이상인지 확인
        if (scoreSystem.HasReachedTargetScore())
        {
            GameClear();
        }
    }


    // ==============================
    // Interaction 시작
    // ==============================

    public void StartInteraction()
    {
        // 게임 종료 상태라면 실행하지 않는다.
        if (isGameOver || isGameClear)
        {
            return;
        }

        // 이미 Interaction 보상이 실행 중이라면
        // 중복 실행하지 않는다.
        if (interactionRewardRoutine != null)
        {
            return;
        }

        interactionRewardRoutine =
            StartCoroutine(InteractionRewardRoutine());
    }


    // ==============================
    // Interaction 종료
    // ==============================

    public void EndInteraction()
    {
        // 실행 중인 Coroutine이 있다면 중지
        if (interactionRewardRoutine != null)
        {
            StopCoroutine(interactionRewardRoutine);

            interactionRewardRoutine = null;
        }
    }


    // ==============================
    // Interaction 보상
    // ==============================

    private IEnumerator InteractionRewardRoutine()
    {
        while (playerControllerSystem.IsInteracting())
        {
            // Interaction 점수 획득 간격만큼 대기
            yield return new WaitForSeconds(
                scoreSystem.GetInteractionScoreInterval()
            );

            // 기다리는 동안 Interaction이 종료되었다면 종료
            if (!playerControllerSystem.IsInteracting())
            {
                yield break;
            }

            // 점수 획득
            scoreSystem.AddInteractionScore();

            // 점수 획득에 따른 Health 회복
            playerHealthSystem.IncreaseHealth(
                interactionHealthAmount
            );
        }

        interactionRewardRoutine = null;
    }


    // ==============================
    // Enemy Detection 결과
    // ==============================

    public void OnEnemyDetection(DetectionType detectionType)
    {
        // 게임이 끝난 상태라면 처리하지 않는다.
        if (isGameOver || isGameClear)
        {
            return;
        }

        // Interaction 중이 아니라면 처리하지 않는다.
        if (!playerControllerSystem.IsInteracting())
        {
            return;
        }

        switch (detectionType)
        {
            // ==============================
            // Decrease Health
            // ==============================

            case DetectionType.DecreaseHealth:

                // Health 감소
                playerHealthSystem.DecreaseHealth(
                    detectionHealthDecreaseAmount
                );

                // Interaction 종료
                playerControllerSystem.EndInteraction();

                Debug.Log(
                    "Player 발각 : Health 감소 / Interaction 종료"
                );

                break;


            // ==============================
            // Kill
            // ==============================

            case DetectionType.Kill:

                // Kill Range에서 감지되면 Game Over
                GameOver();

                break;
        }
    }


    // ==============================
    // Health 0 처리
    // ==============================

    public void OnPlayerHealthDepleted()
    {
        // 이미 종료된 상태라면 실행하지 않는다.
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
        // 이미 종료된 상태라면 실행하지 않는다.
        if (isGameOver || isGameClear)
        {
            return;
        }

        // Game Over 상태
        isGameOver = true;


        // Interaction 종료
        if (playerControllerSystem.IsInteracting())
        {
            playerControllerSystem.EndInteraction();
        }


        // 최종 점수 표시
        if (lastScoreText != null)
        {
            lastScoreText.text =
                "최종 점수: " +
                scoreSystem.GetCurrentScore();
        }


        // Game Over UI 표시
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }


        // Restart 버튼 활성화
        if (restartButton != null)
        {
            restartButton.SetActive(true);
        }


        // Quit 버튼 활성화
        if (quitButton != null)
        {
            quitButton.SetActive(true);
        }


        // 게임 정지
        Time.timeScale = 0f;


        Debug.Log("Game Over");
    }


    // ==============================
    // Game Clear
    // ==============================

    public void GameClear()
    {
        // 이미 종료된 상태라면 실행하지 않는다.
        if (isGameOver || isGameClear)
        {
            return;
        }

        // Game Clear 상태
        isGameClear = true;


        // Interaction 종료
        if (playerControllerSystem.IsInteracting())
        {
            playerControllerSystem.EndInteraction();
        }


        // Game Clear 버튼 활성화
        if (gameClearButton != null)
        {
            gameClearButton.SetActive(true);
        }


        // Restart 버튼 활성화
        if (restartButton != null)
        {
            restartButton.SetActive(true);
        }


        // Quit 버튼 활성화
        if (quitButton != null)
        {
            quitButton.SetActive(true);
        }


        // Game Clear 시에는
        // 게임을 바로 정지하지 않는다.
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
        // Game Clear 상태가 아니면 실행하지 않는다.
        if (!isGameClear)
        {
            return;
        }


        // Game Clear UI 표시
        if (gameClearUI != null)
        {
            gameClearUI.SetActive(true);
        }


        // Game Clear 버튼 숨김
        if (gameClearButton != null)
        {
            gameClearButton.SetActive(false);
        }


        // Game Clear UI를 연 시점에 게임 정지
        Time.timeScale = 0f;
    }


    // ==============================
    // Restart
    // ==============================

    public void RestartGame()
    {
        // 게임 시간 정상화
        Time.timeScale = 1f;

        // 현재 씬을 다시 로드
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }


    // ==============================
    // Quit
    // ==============================

    public void QuitGame()
    {
        // 게임 시간 정상화
        Time.timeScale = 1f;

        // 빌드된 게임 종료
        Application.Quit();

#if UNITY_EDITOR
        // Unity Editor에서 Play Mode 종료
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