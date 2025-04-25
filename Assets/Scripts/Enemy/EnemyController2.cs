using Unity.VisualScripting;
using UnityEngine;

public class EnemyController2 : MonoBehaviour
{
    // Enemy Attributes
    [SerializeField] protected int maxHealth = 5;
    [SerializeField] protected float speed = 5.0f;
    [SerializeField] protected float attackRange = 5;
    [SerializeField] protected float attackCooldown = 2.0f;
    protected float timeSinceLastAttack = 0.0f;

    // Enemy States
    [SerializeField] protected bool isPatrolling = true;
    [SerializeField] protected bool canDetect = false;
    [SerializeField] protected bool canAttack = false;
    [SerializeField] protected bool canMove = false;

    // Components
    [SerializeField] protected Transform target;
    //[SerializeField] protected PlayerHealth playerHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

    }

    // Update is called once per frame
    void Update()
    {
        if(canDetect)
        {
            Move();
        }
        if (!canAttack)
        {
            timeSinceLastAttack += Time.deltaTime;
            if (timeSinceLastAttack >= attackCooldown)
            {
                canAttack = true; // Reset the attack availability
                timeSinceLastAttack = 0.0f; // Reset the cooldown timer
            }
        }
        if(canAttack & Vector2.Distance(target.position, transform.position) < attackRange)
        {
            Attack();
        }
    }

    protected void Move()
    {
        Vector2 direction = (transform.position - target.position).normalized;
        if (Vector2.Distance(transform.position, target.position) > attackRange && canMove) 
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }
    protected void Attack()
    {
        //playerHealth.TakeDamage();
    }
}
