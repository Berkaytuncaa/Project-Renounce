using UnityEngine;

public class EnemyController2 : MonoBehaviour
{
    // Enemy Attributes
    [SerializeField] protected int maxHealth = 5;
    [SerializeField] protected float speed = 5.0f;
    [SerializeField] protected float attackRange = 5;
    [SerializeField] protected float attackSpeed = 5;

    // Enemy States
    [SerializeField] protected bool isPatrolling = true;
    [SerializeField] protected bool canSee = false;
    [SerializeField] protected bool canAttack = false;

    // Components
    [SerializeField] protected Transform target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    protected void Move()
    {
        Vector2 direction = (transform.position - target.position).normalized;
        if (Vector2.Distance(transform.position, target.position) < attackRange)
        {
            
        }
    }
}
