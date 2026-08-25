using UnityEngine;

public class EnemyDetectionSystem : MonoBehaviour
{
    // Player의 Health를 감소시키는 감지 범위
    [SerializeField] private float decreaseHpRange = 5f;

    // Player를 즉시 Game Over시키는 감지 범위
    [SerializeField] private float killRange = 2f;

    // GameManager
    [SerializeField] private GameManager gameManager;


    // 현재 Decrease HP Raycast가 Player를 감지하고 있는지
    private bool isDecreaseHpDetecting = false;

    // 현재 Kill Raycast가 Player를 감지하고 있는지
    private bool isKillDetecting = false;


    private void Update()
    {
        DetectPlayer();
    }


    private void DetectPlayer()
    {
        // Enemy 정면 방향
        Vector3 forwardDirection = transform.forward;

        // Enemy 왼쪽 90도 방향
        Vector3 leftDirection = -transform.right;


        // ==============================
        // Kill Range
        // ==============================

        bool killDetected = Physics.Raycast(
            transform.position,
            forwardDirection,
            out RaycastHit killHit,
            killRange
        );


        // 현재 Kill Range에 Player가 있는지 확인
        bool isPlayerInKillRange =
            killDetected &&
            killHit.collider.gameObject.name == "Player";


        if (isPlayerInKillRange)
        {
            // 처음 들어왔을 때 GameManager에 전달
            if (!isKillDetecting)
            {
                gameManager.OnEnemyDetection(
                    GameManager.DetectionType.Kill
                );
            }

            isKillDetecting = true;
        }
        else
        {
            isKillDetecting = false;
        }


        // Kill Range에 Player가 이미 있는 상태라면
        // Interaction이 새롭게 True가 되었을 때도 확인
        if (isPlayerInKillRange &&
            playerIsInteracting())
        {
            gameManager.OnEnemyDetection(
                GameManager.DetectionType.Kill
            );
        }


        // ==============================
        // Decrease Health Range
        // ==============================

        bool decreaseHpDetected = Physics.Raycast(
            transform.position,
            leftDirection,
            out RaycastHit decreaseHpHit,
            decreaseHpRange
        );


        bool isPlayerInDecreaseHpRange =
            decreaseHpDetected &&
            decreaseHpHit.collider.gameObject.name == "Player";


        if (isPlayerInDecreaseHpRange)
        {
            // 처음 들어왔을 때만 전달
            if (!isDecreaseHpDetecting)
            {
                gameManager.OnEnemyDetection(
                    GameManager.DetectionType.DecreaseHealth
                );
            }

            isDecreaseHpDetecting = true;
        }
        else
        {
            isDecreaseHpDetecting = false;
        }
    }


    // Player가 현재 Interaction 중인지 확인한다.
    private bool playerIsInteracting()
    {
        return gameManager.GetPlayerControllerSystem()
            .IsInteracting();
    }


    // Scene에서 Raycast를 확인
    private void OnDrawGizmos()
    {
        // Kill Range
        Gizmos.color = Color.red;

        Gizmos.DrawRay(
            transform.position,
            transform.forward * killRange
        );


        // Decrease Health Range
        Gizmos.color = Color.yellow;

        Gizmos.DrawRay(
            transform.position,
            -transform.right * decreaseHpRange
        );
    }
}