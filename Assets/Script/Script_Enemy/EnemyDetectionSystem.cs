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
    // Raycast Origin
    // ==========================================

    [SerializeField] private Transform raycastOrigin;

    // + = 위
    // - = 아래
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


        bool isInteracting =
            playerController.IsInteracting();


        // Interaction이 아니면 감지 초기화
        if (!isInteracting)
        {
            hasDetectedKill = false;
            hasDetectedDecreaseHealth = false;

            return;
        }


        DetectKillRange();
        DetectDecreaseHealthRange();
    }


    // ==========================================
    // Kill Range
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


        if (!detected)
        {
            hasDetectedKill = false;
            return;
        }


        if (!hit.collider.CompareTag(playerTag))
        {
            hasDetectedKill = false;
            return;
        }


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
    // Health 감소 Range
    // ==========================================

    private void DetectDecreaseHealthRange()
    {
        Vector3 origin =
            GetRaycastOrigin();


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


        if (!hit.collider.CompareTag(playerTag))
        {
            hasDetectedDecreaseHealth = false;
            return;
        }


        if (hasDetectedDecreaseHealth)
        {
            return;
        }


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


        if (raycastOrigin != null)
        {
            origin =
                raycastOrigin.position;
        }
        else
        {
            origin =
                transform.position;
        }


        origin.y +=
            raycastHeightOffset;


        return origin;
    }


    // ==========================================
    // Gizmos
    // ==========================================

    private void OnDrawGizmos()
    {
        Vector3 origin =
            GetRaycastOrigin();


        // Kill
        Gizmos.color =
            Color.red;

        Gizmos.DrawRay(
            origin,
            transform.forward *
            killRange
        );


        // Health Decrease
        Vector3 decreaseDirection =
            Quaternion.Euler(
                0f,
                -90f,
                0f
            ) * transform.forward;


        Gizmos.color =
            Color.yellow;

        Gizmos.DrawRay(
            origin,
            decreaseDirection *
            decreaseHealthRange
        );


        // Origin
        Gizmos.color =
            Color.white;

        Gizmos.DrawSphere(
            origin,
            0.05f
        );
    }
}