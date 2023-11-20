using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public float rotationSpeed = 5f;
    public float lineOfSightRange = 10f;
    public float roamingRadius = 1000f;

    public Transform player1;
    public Transform player2;

    private bool isChasing = false;
    private Transform target;

    private GameObject player1Torch;
    private GameObject player2Torch;

    private NavMeshAgent navMeshAgent;

    public bool shouldTeleport = false;

    private float attackInterval = 1f;
    private float attackTimer = 0f;
    private Transform targetPlayer;

    void Start()
    {
        player1Torch = GameObject.FindGameObjectWithTag("Player1Torch");
        player2Torch = GameObject.FindGameObjectWithTag("Player2Torch");

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.stoppingDistance = 0.1f;
    }

    void Update()
    {
        if (ShouldChasePlayer())
        {
            isChasing = true;
            target = GetClosestPlayer();
        }
        else
        {
            isChasing = false;
        }

        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Roam();
        }

        if (shouldTeleport)
        {
            TeleportToRandomLocation();
            shouldTeleport = false;
        }

        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                AttackPlayer();
                attackTimer = attackInterval;
            }
        }
    }

    bool ShouldChasePlayer()
    {
        bool isPlayerInSight = IsPlayerInSight(player1) || IsPlayerInSight(player2);
        bool isTorchAimingAtEnemy = IsTorchAimingAtEnemy();

        targetPlayer = isPlayerInSight ? GetClosestPlayer() : null;

        return isPlayerInSight && !isTorchAimingAtEnemy;
    }

    bool IsPlayerInSight(Transform player)
    {
        if (player == null)
            return false;

        RaycastHit hit;
        Vector3 direction = player.position - transform.position;

        if (Physics.Raycast(transform.position, direction, out hit, lineOfSightRange))
        {
            if (hit.collider.CompareTag("Player1") || hit.collider.CompareTag("Player2"))
            {
                return true;
            }
        }

        return false;
    }

    bool IsTorchAimingAtEnemy()
    {
        if (player1Torch == null || player2Torch == null)
            return false;

        Light torchLight1 = player1Torch.GetComponent<Light>();
        Light torchLight2 = player2Torch.GetComponent<Light>();

        if (torchLight1 == null || !torchLight1.enabled || torchLight2 == null || !torchLight2.enabled)
            return false;

        float distanceToEnemy1 = Vector3.Distance(transform.position, player1Torch.transform.position);
        float distanceToEnemy2 = Vector3.Distance(transform.position, player2Torch.transform.position);

        float lightRadiusThreshold = 5f;

        return distanceToEnemy1 <= lightRadiusThreshold || distanceToEnemy2 <= lightRadiusThreshold;
    }

    void Roam()
    {
        Debug.Log("Roaming");
        Vector3 randomDirection = Random.insideUnitSphere * roamingRadius;
        randomDirection += transform.position;
        navMeshAgent.SetDestination(randomDirection);
    }

    void ChasePlayer()
    {
        Debug.Log("Chasing");

        if (IsTorchAimingAtEnemy())
        {
            TeleportToRandomLocation();
            return;
        }

        navMeshAgent.SetDestination(target.position);
    }

    Transform GetClosestPlayer()
    {
        float distanceToPlayer1 = Vector3.Distance(transform.position, player1.position);
        float distanceToPlayer2 = Vector3.Distance(transform.position, player2.position);

        return (distanceToPlayer1 < distanceToPlayer2) ? player1 : player2;
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision Detected");

        if (collision.collider.CompareTag("Player1") || collision.collider.CompareTag("Player2"))
        {
            float distanceToPlayer = Vector3.Distance(transform.position, collision.collider.transform.position);

            Debug.Log("Distance to Player: " + distanceToPlayer);

            // check if the distance is 1.2 or below
            if (distanceToPlayer <= 1.2f)
            {
                targetPlayer = collision.collider.transform;
                attackTimer = attackInterval;

                Debug.Log("Attacking Player");
            }
        }
    }

    void AttackPlayer()
    {
        if (targetPlayer != null)
        {
            PlayerHealth targetPlayerHealth = targetPlayer.GetComponent<PlayerHealth>();

            if (targetPlayerHealth != null && targetPlayerHealth.GetCurrentHealth() > 0)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, targetPlayer.position);

                // attack distance
                float attackDistance = 1.2f;

                if (distanceToPlayer <= attackDistance)
                {
                    targetPlayerHealth.TakeDamage(10);
                }
                else
                {
                    Debug.Log("Player is too far for attack");
                }
            }
        }
    }

    public void SetTeleportFlag()
    {
        shouldTeleport = true;
    }

    public void TeleportToRandomLocation()
    {
        Debug.Log("Teleporting to random location");
        Vector3 randomDirection = Random.onUnitSphere * 100f;
        randomDirection += transform.position;
        transform.position = randomDirection;
    }

    public float teleportRadius = 2f;

    public float GetTeleportRadius()
    {
        return teleportRadius;
    }
}



