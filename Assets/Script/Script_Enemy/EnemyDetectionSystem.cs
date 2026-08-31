using UnityEngine;

public class EnemyDetectionSystem : MonoBehaviour
{
    // ==========================================
    // Game Manager
    // ==========================================

    [SerializeField] private GameManager gameManager;


    // ==========================================
    // Player Tag
    // ==========================================

    [SerializeField] private string playerTag = "Player";


    // ==========================================
    // Raycast 시작 위치
    // ==========================================

    // 지정하지 않으면 Enemy 자신의 위치 사용
    [SerializeField] private Transform raycastOrigin;

    // Raycast 시작 높이 보정값
    // +값 = 위로
    // -값 = 아래로
    [SerializeField] private float raycastHeightOffset = 0f;


    // ==========================================
    // Kill Range
    // ==========================================

    [SerializeField] private float killRange = 5f;


    // ==========================================
    // Health 감소 Range
    // ==========================================

    [SerializeField] private float decreaseHealthRange = 3f;


    // ==========================================
    // Detection 상태
    // ==========================================

    private bool hasDetectedDecreaseHealth = false;

    private bool hasDetectedKill = false;


    // ==========================================
    // Update
    // ==========================================

    private void Update()
    {
        if (gameManager == null)
        {
            return;
        }


        PlayerControllerSystem playerController =
            gameManager.GetPlayerControllerSystem();


        if (playerController == null)
        {
            return;
        }


        // ==========================================
        // Interaction 상태
        // ==========================================

        bool currentInteractionState =
            playerController.IsInteracting();


        // Interaction 중이 아니면
        // 두 Raycast 모두 검사하지 않는다.
        if (!currentInteractionState)
        {
            hasDetectedKill = false;
            hasDetectedDecreaseHealth = false;

            return;
        }


        // ==========================================
        // Raycast 감지
        // ==========================================

        DetectKillRange();

        DetectDecreaseHealthRange();
    }


    // ==========================================
    // Kill Range Detection
    // ==========================================

    private void DetectKillRange()
    {
        Vector3 origin =
            GetRaycastOrigin();


        // Enemy 정면
        Vector3 direction =
            transform.forward;


        bool detected =
            Physics.Raycast(
                origin,
                direction,
                out RaycastHit hit,
                killRange
            );


        if (!detected)
        {
            hasDetectedKill = false;
            return;
        }


        // Player Tag 확인
        if (!hit.collider.CompareTag(playerTag))
        {
            hasDetectedKill = false;
            return;
        }


        // 이미 처리한 Kill이면 중복 방지
        if (hasDetectedKill)
        {
            return;
        }


        hasDetectedKill = true;


        Debug.Log(
            "Kill Raycast : Player 감지"
        );


        gameManager.OnEnemyDetection(
            GameManager.DetectionType.Kill
        );
    }


    // ==========================================
    // Health 감소 Range Detection
    // ==========================================

    private void DetectDecreaseHealthRange()
    {
        Vector3 origin =
            GetRaycastOrigin();


        // Enemy 기준 왼쪽 90도
        Vector3 direction =
            Quaternion.Euler(
                0f,
                -90f,
                0f
            ) * transform.forward;


        bool detected =
            Physics.Raycast(
                origin,
                direction,
                out RaycastHit hit,
                decreaseHealthRange
            );


        if (!detected)
        {
            hasDetectedDecreaseHealth = false;
            return;
        }


        // Player Tag 확인
        if (!hit.collider.CompareTag(playerTag))
        {
            hasDetectedDecreaseHealth = false;
            return;
        }


        // 이미 처리했다면 종료
        if (hasDetectedDecreaseHealth)
        {
            return;
        }


        // 다시 Interaction 상태 확인
        if (!PlayerIsInteracting())
        {
            hasDetectedDecreaseHealth = false;
            return;
        }


        hasDetectedDecreaseHealth = true;


        Debug.Log(
            "Health 감소 Raycast : Player 감지"
        );


        gameManager.OnEnemyDetection(
            GameManager.DetectionType.DecreaseHealth
        );
    }


    // ==========================================
    // Player Interaction 확인
    // ==========================================

    private bool PlayerIsInteracting()
    {
        if (gameManager == null)
        {
            return false;
        }


        PlayerControllerSystem playerController =
            gameManager.GetPlayerControllerSystem();


        if (playerController == null)
        {
            return false;
        }


        return playerController.IsInteracting();
    }


    // ==========================================
    // Raycast Origin
    // ==========================================

    private Vector3 GetRaycastOrigin()
    {
        Vector3 origin;


        // 지정된 Origin이 있으면 사용
        if (raycastOrigin != null)
        {
            origin = raycastOrigin.position;
        }
        else
        {
            origin = transform.position;
        }


        // 높이 보정
        origin.y += raycastHeightOffset;


        return origin;
    }


    // ==========================================
    // Scene View Raycast 표시
    // ==========================================

    private void OnDrawGizmos()
    {
        Vector3 origin =
            GetRaycastOrigin();


        // ------------------------------------------
        // Kill Range
        // ------------------------------------------

        Gizmos.color =
            Color.red;


        Gizmos.DrawRay(
            origin,
            transform.forward * killRange
        );


        // ------------------------------------------
        // Health 감소 Range
        // ------------------------------------------

        Vector3 decreaseHealthDirection =
            Quaternion.Euler(
                0f,
                -90f,
                0f
            ) * transform.forward;


        Gizmos.color =
            Color.yellow;


        Gizmos.DrawRay(
            origin,
            decreaseHealthDirection *
            decreaseHealthRange
        );


        // ------------------------------------------
        // Raycast Origin 위치 표시
        // ------------------------------------------

        Gizmos.color =
            Color.white;


        Gizmos.DrawSphere(
            origin,
            0.05f
        );
    }
}