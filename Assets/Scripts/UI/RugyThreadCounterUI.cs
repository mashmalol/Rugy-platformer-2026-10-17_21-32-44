using TMPro;
using UnityEngine;

/// <summary>Updates a TextMeshPro counter from Golden Thread collection events.</summary>
public class RugyThreadCounterUI : MonoBehaviour
{
    [SerializeField] private TMP_Text counterText;
    [SerializeField] private string prefix = "THREADS " ;

    public void SetCount(int count)
    {
        if (counterText != null)
            counterText.text = prefix + count.ToString("00");
    }
}
