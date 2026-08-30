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

    [SerializeField] private Transform raycastOrigin;


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

    // Health 감소를 이미 처리했는지
    private bool hasDetectedDecreaseHealth = false;

    // Kill을 이미 처리했는지
    private bool hasDetectedKill = false;


    // ==========================================
    // Update
    // ==========================================

    private void Update()
    {
        // 필요한 참조가 없다면 종료
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
        // Interaction 중이 아니면
        // 두 Raycast 모두 검사하지 않는다.
        // ==========================================

        if (!playerController.IsInteracting())
        {
            hasDetectedKill = false;
            hasDetectedDecreaseHealth = false;

            return;
        }


        // Interaction 중일 때만 감지
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


        Vector3 direction =
            transform.forward;


        bool detected =
            Physics.Raycast(
                origin,
                direction,
                out RaycastHit hit,
                killRange
            );


        // 감지되지 않으면 다음 감지를 허용
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


        // 이미 처리했다면 중복 실행하지 않는다.
        if (hasDetectedKill)
        {
            return;
        }


        hasDetectedKill = true;


        Debug.Log(
            "Kill Range에서 Player 감지"
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


        // 감지되지 않으면 다시 감지 가능
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


        // 이미 Health 감소를 처리했다면 종료
        if (hasDetectedDecreaseHealth)
        {
            return;
        }


        // 다시 한 번 Interaction 상태 확인
        if (!PlayerIsInteracting())
        {
            hasDetectedDecreaseHealth = false;

            return;
        }


        hasDetectedDecreaseHealth = true;


        Debug.Log(
            "Health 감소 Range에서 Player 감지"
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
    // Raycast 시작 위치
    // ==========================================

    private Vector3 GetRaycastOrigin()
    {
        if (raycastOrigin != null)
        {
            return raycastOrigin.position;
        }


        return transform.position;
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
        // 정면
        // ------------------------------------------

        Gizmos.color =
            Color.red;


        Gizmos.DrawRay(
            origin,
            transform.forward * killRange
        );


        // ------------------------------------------
        // Health 감소 Range
        // 왼쪽 90도
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
    }
}