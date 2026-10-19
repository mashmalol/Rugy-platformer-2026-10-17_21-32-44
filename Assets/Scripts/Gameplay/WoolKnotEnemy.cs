using UnityEngine;

/// <summary>Simple predictable left-right patrol enemy for the first Rugy prototype.</summary>
public class WoolKnotEnemy : MonoBehaviour
{
    [SerializeField] private float patrolDistance = 3f;
    [SerializeField] private float patrolSpeed = 1.5f;
    [SerializeField] private float contactDamage = 1f;
    private Vector3 startPosition;
    private int direction = 1;

    private void Awake() { startPosition = transform.position; }

    private void Update()
    {
        transform.position += Vector3.right * direction * patrolSpeed * Time.deltaTime;
        if (Mathf.Abs(transform.position.x - startPosition.x) >= patrolDistance)
        {
            direction *= -1;
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * direction;
            transform.localScale = scale;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        RugyDamageReceiver receiver = hit.collider.GetComponentInParent<RugyDamageReceiver>();
        if (receiver != null) receiver.ReceiveDamage(contactDamage);
    }
}
