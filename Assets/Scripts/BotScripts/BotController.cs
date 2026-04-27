using UnityEngine;
using UnityEngine.AI;

public class BotController : MonoBehaviour
{
    public enum Mode { Idle, GoToPoint, Follow }

    [SerializeField] public Transform player;
    [SerializeField] public float followRefreshRate = 0.2f;

    
    [SerializeField] private float followDistance = 1.5f;

  
    [SerializeField] private float stopIfCloserThan = 1.2f;

    private NavMeshAgent agent;
    private Mode mode = Mode.Idle;
    private float nextFollowUpdate;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
    }

    void Update()
    {
        if (mode != Mode.Follow) return;
        if (!agent.enabled) return;
        if (player == null) return;

        if (player != null)
        {
            Vector3 look = player.position - transform.position;
            look.y = 0f; // non inclinare su/giù

            if (look.sqrMagnitude > 0.0001f)
            {
                Quaternion targetRot = Quaternion.LookRotation(look);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 10f * Time.deltaTime);
            }
        }


        Vector3 botPosFlat = transform.position; botPosFlat.y = 0f;
        Vector3 playerPosFlat = player.position; playerPosFlat.y = 0f;

        float dist = Vector3.Distance(botPosFlat, playerPosFlat);

       
        if (dist <= stopIfCloserThan)
        {
            if (!agent.isStopped) agent.isStopped = true;
            return;
        }

       
        if (agent.isStopped) agent.isStopped = false;

        if (Time.time < nextFollowUpdate) return;
        nextFollowUpdate = Time.time + followRefreshRate;

        
        Vector3 dirFromPlayerToBotFlat = (botPosFlat - playerPosFlat).normalized;

        
        if (dirFromPlayerToBotFlat.sqrMagnitude < 0.0001f)
        {
            Vector3 pf = player.forward; pf.y = 0f;
            dirFromPlayerToBotFlat = (pf.sqrMagnitude > 0.0001f) ? -pf.normalized : Vector3.back;
        }

        Vector3 desired = player.position + dirFromPlayerToBotFlat * followDistance;

        agent.SetDestination(desired);
    }

    public void GoToFixedPoint(Transform point)
    {
        if (point == null)
        {
            Debug.LogError("GoToPoint: point è null");
            return;
        }

        agent.enabled = true;
        agent.isStopped = false;

        mode = Mode.GoToPoint;
        agent.SetDestination(point.position);
    }

    public void StartFollow()
    {
        StartFollow(player);
    }

    public void StartFollow(Transform playerTarget)
    {
        if (playerTarget == null)
        {
            Debug.LogError("StartFollow: playerTarget è null");
            return;
        }

        player = playerTarget;

        agent.enabled = true;
        agent.isStopped = false;

        mode = Mode.Follow;
        nextFollowUpdate = 0f; // update immediato
    }

    public void StopBot()
    {
        mode = Mode.Idle;
        if (agent.enabled) agent.isStopped = true;
    }

    public Mode GetMode() => mode;

    private string ModeToString(Mode m)
    {
        switch (m)
        {
            case Mode.Idle: return "Idle";
            case Mode.GoToPoint: return "GoToPoint";
            case Mode.Follow: return "Follow";
            default: return "Unknown";
        }
    }

    public string getMode()
    {
        return ModeToString(mode);
    }
}