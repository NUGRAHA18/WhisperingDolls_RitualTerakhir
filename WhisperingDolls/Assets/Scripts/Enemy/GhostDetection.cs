using UnityEngine;

public class GhostDetection : MonoBehaviour
{
    [Header("Sight")]
    [SerializeField] float sightRange = 10f;
    [SerializeField] float fovAngle = 90f;
    [SerializeField] LayerMask sightBlockLayer;

    [Header("Hearing")]
    [SerializeField] float maxHearingRange = 8f;

    Transform playerTransform;
    PlayerController playerController;

    void Awake()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
            playerController = playerObj.GetComponent<PlayerController>();
        }
    }

    public bool CanSeePlayer()
    {
        if (playerTransform == null) return false;

        Vector3 dirToPlayer = playerTransform.position - transform.position;
        float distance = dirToPlayer.magnitude;

        if (distance > sightRange) return false;

        float angle = Vector3.Angle(transform.forward, dirToPlayer);
        if (angle > fovAngle / 2f) return false;

        // Cek ada dinding/penghalang antara ghost dan player
        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dirToPlayer.normalized, distance, sightBlockLayer))
            return false;

        return true;
    }

    public bool CanHearPlayer()
    {
        if (playerTransform == null || playerController == null) return false;

        float noiseLevel = playerController.NoiseLevel;
        if (noiseLevel <= 0f) return false;

        float effectiveRange = maxHearingRange * noiseLevel;
        return Vector3.Distance(transform.position, playerTransform.position) <= effectiveRange;
    }

    // Gizmos: tampilkan FOV dan hearing range di Scene view (tidak muncul saat game)
    void OnDrawGizmosSelected()
    {
        // Hearing range
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, maxHearingRange);

        // Sight range & FOV
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.red;
        Vector3 leftBound  = Quaternion.Euler(0, -fovAngle / 2f, 0) * transform.forward * sightRange;
        Vector3 rightBound = Quaternion.Euler(0,  fovAngle / 2f, 0) * transform.forward * sightRange;
        Gizmos.DrawRay(transform.position, leftBound);
        Gizmos.DrawRay(transform.position, rightBound);
    }
}
