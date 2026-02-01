using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsPlayer, whatIsGround;
    public Animator anim;

    [Header("Patrolling")]
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    [Header("States")]
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    [SerializeField] private float wallCheckDistance = 2f;

    //public killCam kill;

    private void Awake()
    {
        player = GameObject.Find("character").transform;
        agent = GetComponent<NavMeshAgent>();
        Rigidbody rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInAttackRange && !playerInSightRange) Patrolling();
        if (!playerInAttackRange && playerInSightRange) Chase();
        if (playerInAttackRange && playerInSightRange) Attack();
    }

    private void Patrolling()
    {
        if (!walkPointSet) searchWalkPoint();

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 walkDistance = transform.position - walkPoint;

        if(walkDistance.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    private void searchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;

    }

    private void Chase()
    {
       
        agent.SetDestination(player.position);
        transform.LookAt(player);
    }

    private void Attack()
    {
        agent.GetComponent<Animator>().SetBool("isAttack", true);
        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponent<cameraMovement>().enabled = false;
        player.transform.rotation = Quaternion.identity;
        //kill.killcam.Priority = 10;
        StartCoroutine(Death());
    }

    IEnumerator Death()
    {
        yield return new WaitForSeconds(7f);

        SceneManager.LoadScene(0);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall") && !this.CompareTag("Player"))
        {
            // Stop current movement
            agent.ResetPath();

            // Get the wall collision normal
            ContactPoint contact = collision.contacts[0];
            Vector3 bounceDirection = Vector3.Reflect(transform.forward, contact.normal);
            bounceDirection.y = 0f;

            // Calculate new destination
            Vector3 newDestination = transform.position + bounceDirection.normalized * walkPointRange;

            // Ensure destination is on NavMesh
            NavMeshHit hit;
            if (NavMesh.SamplePosition(newDestination, out hit, walkPointRange, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
                walkPoint = hit.position;
                walkPointSet = true;
            }
        }
    }

    private IEnumerator smoothRotation(Vector3 updatedDirection)
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.LookRotation(updatedDirection);
        float duration = 0.2f;
        float elasped = 0;

        if(elasped < duration)
        {
            Quaternion.Slerp(startRotation, endRotation, duration / elasped);
            elasped += Time.deltaTime;
            yield return null;
        }

        endRotation = transform.rotation;
    }


    private Vector3 GetRandomDirection()
    {
        int direction = Random.Range(0, 6);
        switch (direction)
        {
            case 0: return Vector3.up;
            case 1: return Vector3.down;
            case 2: return Vector3.left;
            case 3: return Vector3.right;
            case 4: return Vector3.forward;
            case 5: return Vector3.back;
            default: return Vector3.forward;
        }
    }
}
