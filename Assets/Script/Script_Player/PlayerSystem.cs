using UnityEngine;

public class PlayerSystem : MonoBehaviour
{
    private PlayerHealthSystem playerHealthSystem;
    private PlayerControllerSystem playerControllerSystem;


    private void Awake()
    {
        playerHealthSystem = GetComponent<PlayerHealthSystem>();
        playerControllerSystem = GetComponent<PlayerControllerSystem>();
    }
}