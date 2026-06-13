using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(GhostDetection))]
public class GhostAI : MonoBehaviour
{
    public enum GhostState { Patrol, Chase, Calm }

    [Header("Patrol")]
    [SerializeField] Transform[] waypoints;
    [SerializeField] float patrolSpeed = 2f;
    [SerializeField] float waypointWaitTime = 2f;

    [Header("Chase")]
    [SerializeField] float chaseSpeed = 4.5f;
    [SerializeField] float catchDistance = 1.2f;
    [SerializeField] float loseSightTime = 3f;

    [Header("Calm")]
    [SerializeField] float calmDuration = 8f;

    [Header("Give Doll")]
    [SerializeField] float giveDollRange = 2f;

    NavMeshAgent agent;
    GhostDetection detection;
    Transform player;

    GhostState currentState = GhostState.Patrol;
    int currentWaypointIndex;
    bool isWaitingAtWaypoint;
    float loseSightTimer;

    public GhostState CurrentState => currentState;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        detection = GetComponent<GhostDetection>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (!GameManager.Instance.IsPlaying) return;

        HandleGiveDollInput();

        switch (currentState)
        {
            case GhostState.Patrol: UpdatePatrol(); break;
            case GhostState.Chase:  UpdateChase();  break;
            case GhostState.Calm:                   break;
        }
    }

    void UpdatePatrol()
    {
        agent.speed = patrolSpeed;

        if (detection.CanSeePlayer() || detection.CanHearPlayer())
        {
            SetState(GhostState.Chase);
            return;
        }

        if (waypoints.Length == 0 || isWaitingAtWaypoint) return;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            StartCoroutine(MoveToNextWaypoint());
    }

    void UpdateChase()
    {
        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);

        if (Vector3.Distance(transform.position, player.position) <= catchDistance)
        {
            GameManager.Instance.GameOver();
            return;
        }

        if (!detection.CanSeePlayer() && !detection.CanHearPlayer())
        {
            loseSightTimer += Time.deltaTime;
            if (loseSightTimer >= loseSightTime)
                SetState(GhostState.Patrol);
        }
        else
        {
            loseSightTimer = 0f;
        }
    }

    IEnumerator MoveToNextWaypoint()
    {
        isWaitingAtWaypoint = true;
        agent.SetDestination(waypoints[currentWaypointIndex].position);

        yield return new WaitUntil(() => !agent.pathPending && agent.remainingDistance < 0.5f);
        yield return new WaitForSeconds(waypointWaitTime);

        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        isWaitingAtWaypoint = false;
    }

    IEnumerator CalmCoroutine()
    {
        SetState(GhostState.Calm);
        agent.ResetPath();
        Debug.Log("[Ghost] Tenang selama " + calmDuration + " detik.");
        yield return new WaitForSeconds(calmDuration);
        SetState(GhostState.Patrol);
    }

    // Pemain tekan E saat sedang dikejar dan punya boneka baik → ghost tenang
    void HandleGiveDollInput()
    {
        if (!Input.GetKeyDown(KeyCode.E)) return;
        if (currentState != GhostState.Chase) return;
        if (player == null) return;
        if (Vector3.Distance(transform.position, player.position) > giveDollRange) return;
        if (!InventoryManager.Instance.HasItemOfType(ItemType.DollGood)) return;

        ItemData doll = InventoryManager.Instance.GetFirstOfType(ItemType.DollGood);
        InventoryManager.Instance.RemoveItem(doll);
        StartCoroutine(CalmCoroutine());
        Debug.Log("[Ghost] Diberi boneka baik → tenang.");
    }

    void SetState(GhostState newState)
    {
        currentState = newState;
        loseSightTimer = 0f;
        if (newState == GhostState.Patrol)
            isWaitingAtWaypoint = false;
        Debug.Log("[Ghost] State → " + newState);
    }
}
