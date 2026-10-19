using UnityEngine;

/// <summary>Collectible Golden Thread pickup that updates the run counter.</summary>
public class GoldenThreadCollectible : MonoBehaviour
{
    [SerializeField] private int amount = 1;
    [SerializeField] private float spinSpeed = 90f;

    private void Update() { transform.Rotate(0f, 0f, spinSpeed * Time.deltaTime); }

    private void OnTriggerEnter(Collider other)
    {
        GoldenThreadCounter counter = other.GetComponentInParent<GoldenThreadCounter>();
        if (counter == null) return;
        counter.Add(amount);
        Destroy(gameObject);
    }
}
