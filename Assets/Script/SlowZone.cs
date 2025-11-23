using UnityEngine;

public class SlowZone : MonoBehaviour
{
    public float enterSlow = 0.2f;
    public float stackSlowPerSecond = 0.1f;

    private SlowSource slowRef;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        SlowManager sm = collision.GetComponent<SlowManager>();
        if (sm == null) return;

        slowRef = sm.AddSlow(enterSlow, stackSlowPerSecond);
        slowRef.insideZone = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if (slowRef == null) return;

        // Mark as exited → SlowManager mulai recovery
        slowRef.insideZone = false;
    }
}
