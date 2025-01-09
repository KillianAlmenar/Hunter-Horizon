using UnityEngine;
using UnityEngine.AI;

public class RandomNavMeshDestination : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public float roamingRadius = 10f;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        // Appel de la fonction pour définir une destination aléatoire au démarrage
        SetRandomDestination();
    }

    void Update()
    {
        // Si l'agent a atteint sa destination ou est proche, définissez une nouvelle destination aléatoire
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f)
        {
            SetRandomDestination();
        }
        navMeshAgent.speed = GetComponent<EnemyStatScript>().speedStat;
    }

    void SetRandomDestination()
    {
        // Obtenez la position actuelle de l'agent
        Vector3 randomPosition = RandomNavSphere(transform.position, roamingRadius, -1);

        // Définissez la destination sur le NavMesh
        navMeshAgent.SetDestination(randomPosition);
    }

    // Fonction pour générer un point aléatoire dans un rayon spécifique sur le NavMesh
    Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;

        randomDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;
    }
}
