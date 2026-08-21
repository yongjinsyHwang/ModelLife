using UnityEngine;

public class EnemySystem : MonoBehaviour
{
    //Enemy의 기능적 함수는 해당 스크립트를 상위로 다룰 것
    private EnemyTurnSystem turnSystem;
    private void Awake()
    {
        turnSystem = GetComponent<EnemyTurnSystem>();
    }

    private void Start()
    {
        turnSystem.StartTurn();
    }
}