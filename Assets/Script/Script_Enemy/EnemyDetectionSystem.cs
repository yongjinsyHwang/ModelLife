using UnityEngine;

public class EnemyDetectionSystem : MonoBehaviour
{
    // Player의 HP를 감소시키는 감지 범위
    [SerializeField] private float decreaseHpRange = 5f;

    // Player를 죽이는 감지 범위
    [SerializeField] private float killRange = 2f;


    // 현재 Kill Raycast가 Player를 감지하고 있는지
    private bool isKillDetecting = false;

    // 현재 Decrease HP Raycast가 Player를 감지하고 있는지
    private bool isDecreaseHpDetecting = false;


    private void Update()
    {
        DetectPlayer();
    }


    // 두 방향의 Raycast를 이용해 Player를 감지한다.
    private void DetectPlayer()
    {
        // Enemy가 바라보는 정면 방향
        Vector3 forwardDirection = transform.forward;

        // Enemy의 왼쪽 90도 방향
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

        // Raycast가 Player를 새롭게 감지했을 때
        if (killDetected && killHit.collider.gameObject.name == "Player")
        {
            if (!isKillDetecting)
            {
                Debug.Log("Player가 Kill Range에 감지되었습니다.");
            }

            // 현재 Player를 감지하고 있는 상태
            isKillDetecting = true;
        }
        else
        {
            // Player 감지가 끊긴 상태
            isKillDetecting = false;
        }


        // ==============================
        // Decrease HP Range
        // ==============================

        bool decreaseHpDetected = Physics.Raycast(
            transform.position,
            leftDirection,
            out RaycastHit decreaseHpHit,
            decreaseHpRange
        );

        // Raycast가 Player를 새롭게 감지했을 때
        if (decreaseHpDetected &&
            decreaseHpHit.collider.gameObject.name == "Player")
        {
            if (!isDecreaseHpDetecting)
            {
                Debug.Log("Player가 Decrease HP Range에 감지되었습니다.");
            }

            // 현재 Player를 감지하고 있는 상태
            isDecreaseHpDetecting = true;
        }
        else
        {
            // Player 감지가 끊긴 상태
            isDecreaseHpDetecting = false;
        }
    }


    // Scene 창에서 Raycast의 방향과 범위를 확인한다.
    private void OnDrawGizmos()
    {
        // Kill Range
        Gizmos.color = Color.red;

        Gizmos.DrawRay(
            transform.position,
            transform.forward * killRange
        );


        // Decrease HP Range
        Gizmos.color = Color.yellow;

        Gizmos.DrawRay(
            transform.position,
            -transform.right * decreaseHpRange
        );
    }
}