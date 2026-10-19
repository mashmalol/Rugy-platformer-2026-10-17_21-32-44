using System.Collections;
using UnityEngine;

/// <summary>Shows a boss intro card, pauses gameplay, then starts the encounter.</summary>
public class RugyBossIntroController : MonoBehaviour
{
    [SerializeField] private GameObject introCard;
    [SerializeField] private RugyBossController boss;
    [SerializeField] private float displaySeconds = 3f;

    public void PlayIntro()
    {
        StartCoroutine(PlayIntroRoutine());
    }

    private IEnumerator PlayIntroRoutine()
    {
        if (introCard != null) introCard.SetActive(true);
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(displaySeconds);
        Time.timeScale = 1f;
        if (introCard != null) introCard.SetActive(false);
        if (boss != null) boss.BeginEncounter();
    }
}
