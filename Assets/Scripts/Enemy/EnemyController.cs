using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// TODO: when  spriteRenderer.flipX = true; our enemy sprite goes out of the collider to the right.
// TODO: attack condition does not work as intended, related to cooldown logic of the attack

/// <summary>
/// Our Parent Enemy Class.
/// Also it is the script of our first enemy URSUS METALICUS
/// </summary>
public class EnemyController : MonoBehaviour
{
    [Header("Enemy Attributes")]
    [SerializeField] protected private float moveSpeed;                 // Speed at which the enemy moves
    [SerializeField] protected private int currentHealth;        // Current health of the enemy
    [SerializeField] protected private int maxHealth;                   // Maximum health of the enemy
    [SerializeField] protected private int strength;                    // Strength of the enemy's attacks
    [SerializeField] protected private float attackRange;               // Range within which the enemy can attack
    [SerializeField] protected private float attackSpeed;               // Speed at which the enemy attacks
    [SerializeField] protected private float agroRange;                 // Range at which the enemy becomes aggressive
    // Cooldown related fields. Being used in Update() method.
    protected private float timeSinceLastAttack = 0.0f;                 // Time passed since the last attack
    [SerializeField] protected private float attackCooldown = 2.0f;                      // Cooldown duration between attacks (adjust as needed)

    [Header("Enemy State")]
    [SerializeField] protected private bool isAgro;          // Flag indicating if the enemy is aggressive
    [SerializeField] protected private bool isSearching;     // Flag indicating if the enemy is searching for the player
    [SerializeField] protected private bool isPatrolling;    // Flag indicating if the enemy is patrolling
    [SerializeField] protected private bool canSee;          // Flag indicating if the enemy is can see the target
    [SerializeField] protected private bool canAttack;       // Flag indicating if the enemy is can attack the target

    [Header("Components")]
    protected private Transform target;                      // Reference to the player's transform
    protected private SpriteRenderer spriteRenderer;         // Reference to the SpriteRenderer component
    protected private Rigidbody2D rigidBody;                 // Reference to the Rigidbody2D component

    [Header("Enemy Search")]
    // Timer and duration for continuing movement after player leaves agro range
    protected private float moveTimer = 0f; // Timer for continuing movement
    protected private float moveDuration = 1.6f; // Duration for continuing movement after player leaves agro range

    /// <summary>
    /// Initialization method for enemies.
    /// </summary>
    protected virtual void Start()
    {
        currentHealth = maxHealth; // Set the initial health to maximum
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>(); // Find and store the player's transform
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component of the enemy
        rigidBody = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component of the enemy
        canAttack = true;
    }

    /// <summary>
    /// Update method handling common behavior for enemies.
    /// </summary>
    protected virtual void Update()
    {
        Move(); // Handle enemy movement
        TurnDirection(); // Turn the enemy towards the player's direction
        canSee = CanSeePlayer(); // canSee bool get the CanSeePlayer() method's value.

        // Cooldown logic
        if (!canAttack)
        {
            timeSinceLastAttack += Time.deltaTime;
            if (timeSinceLastAttack >= attackCooldown)
            {
                canAttack = true; // Reset the attack availability
                timeSinceLastAttack = 0.0f; // Reset the cooldown timer
            }
        }

        // if is agro and attack ready and target is within attack range, attack the player
        if (isAgro && canAttack && Vector2.Distance(transform.position, target.position) < attackRange)
        {
            Attack();
            canAttack = false;
        }
    }

    // Common methods for all enemies can be defined here
    // *************************************************************************

    /// <summary>
    /// Handles the movement behavior of the enemy.
    /// </summary>
    protected virtual void Move()
    {
        Vector2 direction = (target.position - transform.position).normalized; // Calculate direction towards the player

        // Check if the player is within the agro range and if enemy can see the player.
        if (Vector2.Distance(transform.position, target.position) < agroRange)
        {
            isSearching = false;
            isAgro = true;

            if (isAgro && canSee)
            {
                transform.Translate(direction * moveSpeed * Time.deltaTime); // Move towards the player's position

                spriteRenderer.flipX = direction.x < 0; // Face the target's direction

                moveTimer = moveDuration; // Reset the timer when the player is within agro range
            }
        }
        else
        {
            isAgro = false;

            // Continue moving if the timer hasn't expired yet
            if (moveTimer > 0f)
            {
                isSearching = true;
                var moveDirection = target.position.x > transform.position.x ? 1 : -1;
                rigidBody.linearVelocity = new Vector2(moveDirection * moveSpeed, rigidBody.linearVelocity.y);
                moveTimer -= Time.deltaTime; // Decrease the timer
            }
            else
            {
                isSearching = false;
                rigidBody.linearVelocity = Vector2.zero; // Stop moving if the timer has expired
            }
        }
    }

    /// <summary>
    /// Turn the enemy sprite towards the player's direction.
    /// </summary>
    protected virtual void TurnDirection()
    {
        if (transform.position.x > target.position.x)
        {
            spriteRenderer.flipX = true; // Flip sprite if player is on the left side
        }
        else
        {
            spriteRenderer.flipX = false; // Do not flip sprite if player is on the right side
        }
    }

    /// <summary>
    /// Method for handling damage to the enemy.
    /// </summary>
    /// <param name="damageAmount">Amount of damage to be inflicted.</param>
    protected virtual void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount; // taken damage.

        // if enemy health is less then zero, call Death() method and destroy the enemy
        if (currentHealth < 0)
        {
            Death();
        }
    }

    /// <summary>
    /// Logic for enemy attacks.
    /// </summary>
    protected virtual void Attack()
    {
        //PlayerHealth playerHealth = target.GetComponent<PlayerHealth>(); // reference to the PlayerHealth
        // Calculate direction towards the player
        Vector2 directionToPlayer = target.position - transform.position;

        // Perform raycast towards the player to check if it's within attack range
        RaycastHit2D hit = Physics2D.Raycast(transform.position, target.position - transform.position, attackRange, LayerMask.GetMask("Player"));

        // Draw debug line to visualize the raycast
        Debug.DrawRay(transform.position, directionToPlayer.normalized * attackRange, Color.red);

        if (hit.collider != null)
        {
            // Player is within attack range, deal damage or trigger attack behavior

            // Reference to the PlayerHealth's take damage method.
            /*if (playerHealth != null)
            {
                playerHealth.TakeDamage(strength);
                Debug.Log("Damage given: " + strength);
            }
            else
            {
                throw new InvalidOperationException("PlayerHealth component not found on the target.");
            }*/
        }
        canAttack = false; // Reset the attack availability
    }

    /// <summary>
    /// Destroy enemy object on Death.
    /// </summary>
    protected virtual void Death()
    {
        Debug.Log("Enemy has died!");
        Destroy(gameObject); // Destroy the enemy.
    }

    /// <summary>
    /// Checks if the enemy can visually perceive the player within the agro range and line of sight.
    /// </summary>
    /// <returns>
    /// True if the player is within agro range and has a clear line of sight; otherwise, false.
    /// </returns>
    protected virtual bool CanSeePlayer()
    {
        // Get the positions and direction between the enemy and the player
        Vector2 enemyPos = transform.position;
        Vector2 playerPos = target.position;
        //Vector2 directionToPlayer = playerPos - enemyPos;

        // Check if the enemy is facing left or right based on the direction to the player
        //bool facingLeft = directionToPlayer.x < 0;

        // Determine linecast direction based on the enemy's facing direction
        //Vector2 linecastDirection = facingLeft ? -directionToPlayer.normalized : directionToPlayer.normalized;
        Vector2 linecastOrigin = enemyPos;

        // Perform a linecast from the enemy towards the player
        RaycastHit2D hit = Physics2D.Linecast(linecastOrigin, playerPos, LayerMask.GetMask("Obstacles", "Ground"));

        // Set the end position of the line to the player's position initially
        Vector2 endLinePos = playerPos;

        // If an obstacle is hit, update the end position to the obstacle hit point
        if (hit.collider != null)
        {
            endLinePos = hit.point;
            Debug.DrawLine(linecastOrigin, endLinePos, Color.yellow); // Visualize the hit point in red
            return false; // Obstacle detected between enemy and player
        }

        // Draw a line in yellow from the enemy to the player or obstacle hit point
        Debug.DrawLine(linecastOrigin, endLinePos, Color.blue);

        // Return true if the player is within aggro range and has a clear line of sight
        return true;
    }
}
