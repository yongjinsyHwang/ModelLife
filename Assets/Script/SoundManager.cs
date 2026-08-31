using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // ========================================
    // Audio Source
    // ========================================

    [SerializeField] private AudioSource audioSource;


    // ========================================
    // Sound Clips
    // ========================================

    // 1. 회전 시작 사운드
    [SerializeField] private AudioClip turnSound;

    // 2. 리턴 시작 사운드
    [SerializeField] private AudioClip returnSound;

    // 3. 상호작용 사운드
    [SerializeField] private AudioClip interactionSound;

    // 4. 게임 클리어 사운드
    [SerializeField] private AudioClip gameClearSound;

    // 5. 게임 오버 사운드
    [SerializeField] private AudioClip gameOverSound;


    // ========================================
    // Turn
    // ========================================

    public void PlayTurnSound()
    {
        PlaySound(turnSound);
    }


    // ========================================
    // Return
    // ========================================

    public void PlayReturnSound()
    {
        PlaySound(returnSound);
    }


    // ========================================
    // Interaction
    // ========================================

    public void PlayInteractionSound()
    {
        PlaySound(interactionSound);
    }


    // ========================================
    // Game Clear
    // ========================================

    public void PlayGameClearSound()
    {
        PlaySound(gameClearSound);
    }


    // ========================================
    // Game Over
    // ========================================

    public void PlayGameOverSound()
    {
        PlaySound(gameOverSound);
    }


    // ========================================
    // Sound Play
    // ========================================

    private void PlaySound(AudioClip clip)
    {
        if (audioSource == null)
        {
            return;
        }

        if (clip == null)
        {
            return;
        }

        audioSource.PlayOneShot(clip);
    }
}