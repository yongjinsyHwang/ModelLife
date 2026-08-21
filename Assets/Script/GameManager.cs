using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ScoreSystem scoreSystem;

    public void AddInteractionScore()
    {
        scoreSystem.AddInteractionScore();
    }
}