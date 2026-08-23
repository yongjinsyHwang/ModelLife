using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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

    // Decrease HP Range에 감지되었을 때
    // 감소할 Health
    [SerializeField] private int detectionHealthDecreaseAmount = 10;


    // ==============================
    // Interaction 보상 설정
    // ==============================

    // Interaction 점수 획득 시 회복되는 Health
    [SerializeField] private int interactionHealthAmount = 1;


    // ==============================
    // Game Over UI
    // ==============================

    // Game Over UI 전체 오브젝트
    [SerializeField] private GameObject gameOverUI;

    // Game Over 시 최종 점수를 표시하는 Text
    [SerializeField] private Text lastScoreText;


    // ==============================
    // Game Clear UI
    // ==============================

    // 목표 점수 달성 후 활성화할 버튼
    [SerializeField] private GameObject gameClearButton;

    // 버튼 클릭 후 표시할 Game Clear UI
    [SerializeField] private GameObject gameClearUI;


    // ==============================
    // Interaction 보상 Coroutine
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

        // 게임 시간 정상화
        Time.timeScale = 1f;
    }


    private void Update()
    {
        // 게임이 끝난 상태라면 목표 점수를 확인하지 않는다.
        if (isGameOver || isGameClear)
        {
            return;
        }

        // 목표 점수 도달 여부 확인
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

        // 이미 실행 중이라면 중복 실행하지 않는다.
        if (interactionRewardRoutine != null)
        {
            return;
        }

        // Interaction 보상 Coroutine 시작
        interactionRewardRoutine =
            StartCoroutine(InteractionRewardRoutine());
    }


    // ==============================
    // Interaction 종료
    // ==============================

    public void EndInteraction()
    {
        // 실행 중인 Coroutine이 있다면 중지한다.
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

            // 대기 중 Interaction이 종료되었다면 종료
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

        // Coroutine 참조 초기화
        interactionRewardRoutine = null;
    }


    // ==============================
    // Enemy Detection 결과
    // ==============================

    public void OnEnemyDetection(DetectionType detectionType)
    {
        // 게임 종료 상태라면 실행하지 않는다.
        if (isGameOver || isGameClear)
        {
            return;
        }

        // Interaction 중이 아니라면
        // Detection 결과를 처리하지 않는다.
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

                // Kill Range 감지 → Game Over
                GameOver();

                break;
        }
    }


    // ==============================
    // Health 0 처리
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
                "최종 점수: " + scoreSystem.GetCurrentScore();
        }

        // Game Over UI 표시
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
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

        Debug.Log("Game Clear");
    }


    // ==============================
    // Game Clear UI
    // ==============================

    // Game Clear 버튼을 눌렀을 때 호출
    public void ShowGameClearUI()
    {
        if (!isGameClear)
        {
            return;
        }

        // Game Clear UI 표시
        if (gameClearUI != null)
        {
            gameClearUI.SetActive(true);
        }

        // 버튼 숨김
        if (gameClearButton != null)
        {
            gameClearButton.SetActive(false);
        }

        // 버튼을 눌렀을 때 게임 정지
        Time.timeScale = 0f;
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
    // System 반환
    // ==============================

    // EnemyDetectionSystem에서
    // Player의 Interaction 상태를 확인할 수 있도록 반환한다.
    public PlayerControllerSystem GetPlayerControllerSystem()
    {
        return playerControllerSystem;
    }
}