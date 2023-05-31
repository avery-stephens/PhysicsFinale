//using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEditor.Experimental.GraphView;
//using UnityEditor.Search;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class AICharacter2D : MonoBehaviour, IDamagable
{
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer renderer;
    [SerializeField] float speed;
    [SerializeField] float jumpHeight;
    [SerializeField] float doubleJumpHeight;
    //[SerializeField] float hitForce;
    [SerializeField, Range(1, 5)] float fallRateMultiplier;
    [SerializeField, Range(1, 5)] float lowJumpRateMultiplier;
    [Header("Ground")]
    [SerializeField] Transform groundTransform;
    [SerializeField] LayerMask groundLayerMask;
    [SerializeField] float groundRadius;
    //CharacterController characterController;
    [Header("AI")]
    [SerializeField] Transform[] waypoints;
    [SerializeField] float rayDistance = 1;
    [SerializeField] string enemyTag;
    [SerializeField] LayerMask raycastLayerMask;
    [SerializeField] float wanderDistance;
    [Header("Attack")]
    [SerializeField] Transform attackTransform;
    [SerializeField] float attackRadius;
    [SerializeField] float attackCooldownAmount = 1;
    [SerializeField] float hitStunAmount = 1;
    [Header("Sounds")]
    [SerializeField] GameObject attack;
    [SerializeField] GameObject hit;
    [SerializeField] GameObject death;

    public float health = 100;
    private float hitTimer = 0;
    private float attackCooldown = 0;
    [SerializeField] int scoreAmount = 50;

    GameObject enemy;
    Rigidbody2D rb;
    Vector2 velocity = Vector2.zero;
    bool faceRight = true;
    [SerializeField] bool invertFacing = false;
    float groundAngle = 0;
    Transform targetWaypoint = null;
    Transform startingPosition = null;
    enum State
    { 
        IDLE,
        PATROL,
        CHASE,
        ATTACK,
        DEATH
    }
    State state = State.IDLE;
    float stateTimer = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startingPosition = transform;
    }
    void Update()
    {
        bool onGround = UpdateGroundCheck() && (velocity.y <= 0);
        Vector2 direction = Vector2.zero;
        //update ai
        CheckEnemySeen();
        //Debug.Log(state.ToString());
        if (health <= 0)
        {
            state = State.DEATH;
            animator.SetTrigger("Death");
            Death();
        }
        if (onGround)
        {
            switch (state)
            {
                case State.IDLE:
                    //if (CanSeePlayer()) state = State.CHASE;
                    if (enemy != null) state = State.CHASE;
                    stateTimer += Time.deltaTime;
                    if (stateTimer >= 2f)
                    {
                        SetNewWayPointTarget();
                        state = State.PATROL;
                    }
                    break;
                case State.PATROL:
                    {
                        //if (CanSeePlayer()) state = State.CHASE;
                        if (enemy != null) state = State.CHASE;

                        direction.x = Mathf.Sign(targetWaypoint.position.x - transform.position.x); //sign returns either 1 or -1 depending on if it is negative or positive
                        float dx = Mathf.Abs(targetWaypoint.position.x - transform.position.x);
                        if (dx <= 0.25f)
                        {
                            state = State.IDLE;
                            stateTimer = 1;
                            direction.x = 0;
                        }
                    }
                    break;
                case State.CHASE:
                    {
                        if (enemy == null)
                        {
                            state = State.IDLE;
                            stateTimer = 1;
                            break;
                        }
                        float dx = Mathf.Abs(enemy.transform.position.x - transform.position.x);
                        if (dx <= 1.5f)
                        {
                            if (attackCooldown <= 0)
                            {
                                state = State.ATTACK;
                                animator.SetTrigger("Attack");
                            }
                        }
                        else
                        {
                            direction.x = Mathf.Sign(enemy.transform.position.x - transform.position.x);
                        }
                    }
                    break;
                case State.ATTACK:
                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
                    {
                        attackCooldown = attackCooldownAmount;
                        state = State.CHASE;
                    }
                    break;
                case State.DEATH:
                    //Death();
                    break;
                default:
                    break;
            }
        }
        // check if the character is on the ground
        //bool onGround = Physics2D.OverlapCircle(groundTransform.position, 0.02f) != null;
        // get direction input
        //direction.x = Input.GetAxis("Horizontal");
        // set velocity
        if (onGround)
        {
            velocity.x = direction.x * speed;
            if (velocity.y < 0) velocity.y = 0;
            //if (Input.GetButtonDown("Jump"))
            //{
            //    velocity.y += Mathf.Sqrt(jumpHeight * -2 * Physics.gravity.y);
            //    StartCoroutine(DoubleJump());
            //    animator.SetTrigger("Jump");
            //}
            
        }
        if (!onGround) 
        {
            velocity.x = direction.x * speed * .75f;
        }
        //adjust gravity for jump
        float gravityMultiplier = 1;
        //makes it so that you have less force of gravity when moving up
        //if (!onGround && velocity.y < 0) gravityMultiplier = fallRateMultiplier;
        //if (!onGround && velocity.y > 0 && !Input.GetButton("Jump")) gravityMultiplier = fallRateMultiplier;
        velocity.y += Physics.gravity.y * gravityMultiplier * Time.deltaTime;

        // move character if it isn't "stunned"
        if (hitTimer >= 0)
        {
            hitTimer -= Time.deltaTime;
        }
        else rb.velocity = velocity;
        
        if (attackCooldown > 0) attackCooldown -= Time.deltaTime;
        // flip character to face direction of movement (velocity)
        if (invertFacing)
        {
            if (velocity.x < 0 && !faceRight) Flip();
            if (velocity.x > 0 && faceRight) Flip();
        }
        else
        {
            if (velocity.x > 0 && !faceRight) Flip();
            if (velocity.x < 0 && faceRight) Flip();
        }
        
        //update animator
        animator.SetFloat("Speed", Mathf.Abs(velocity.x));
    }

    private void Death()
    {
        
        targetWaypoint = null;
        speed = 0;
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
        {
            FindObjectOfType<GameManager>().AddScore(scoreAmount);
            gameObject.SetActive(false);
            GameManager.Instance.AddKill();
            Instantiate(death, transform);
        }
    }
    private bool UpdateGroundCheck()
    {
        // check if the character is on the ground
        Collider2D collider = Physics2D.OverlapCircle(groundTransform.position, groundRadius, groundLayerMask);
        if (collider != null)
        {
            RaycastHit2D raycastHit = Physics2D.Raycast(groundTransform.position, Vector2.down, groundRadius, groundLayerMask);
            if (raycastHit.collider != null)
            {
                // get the angle of the ground (angle between up vector and ground normal)
                groundAngle = Vector2.SignedAngle(Vector2.up, raycastHit.normal);
                Debug.DrawRay(raycastHit.point, raycastHit.normal, Color.red);
            }
        }

        return (collider != null);
    }

    private void Flip()
    {
        faceRight = !faceRight;
        renderer.flipX = !faceRight;
        attackTransform.transform.localPosition *= (Vector2.right * -1) + Vector2.up;
    }

    private void SetNewWayPointTarget()
    {
        Transform waypoint = null;
        if (waypoints.Length == 0)
        {
            waypoint = startingPosition;
            waypoint.position += Vector3.right * (Random.Range(-1 * wanderDistance, wanderDistance));
        }
        else
        {
            do
            {
                waypoint = waypoints[Random.Range(0, waypoints.Length)];
            } while (waypoint == targetWaypoint);
        }
        targetWaypoint = waypoint;
    }

    private void CheckAttack()
    {
        Instantiate(attack, transform);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackTransform.position, attackRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject == gameObject) continue;

            if (collider.gameObject.TryGetComponent<IDamagable>(out var damagable))
            {
                damagable.Damage(10);
                if (collider.TryGetComponent<Rigidbody2D>(out var rb))
                {
                    rb.AddForce((collider.gameObject.transform.position - transform.position).normalized * 5 + (Vector3.up * 5), ForceMode2D.Impulse);
                }
            }
        }
    }

    //private bool CanSeePlayer()
    //{
    //    RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, ((faceRight) ? Vector2.right : Vector2.left) * rayDistance);
    //    //Debug.DrawRay(transform.position, ((faceRight) ? Vector2.right : Vector2.left) * rayDistance);

    //    return raycastHit.collider != null &&  raycastHit.collider.gameObject.CompareTag("Player");
    //}

    private void CheckEnemySeen()
    {
        enemy = null;
        RaycastHit2D raycastHit;
        if (invertFacing) raycastHit = Physics2D.Raycast(transform.position, ((faceRight) ? Vector2.left : Vector2.right), rayDistance, raycastLayerMask);
        else raycastHit = Physics2D.Raycast(transform.position, ((faceRight) ? Vector2.right : Vector2.left), rayDistance, raycastLayerMask);
        if (raycastHit.collider != null && raycastHit.collider.gameObject.CompareTag(enemyTag))
        {
            enemy = raycastHit.collider.gameObject;
            Debug.DrawRay(transform.position, ((faceRight) ? Vector2.left : Vector2.right) * rayDistance, Color.red);
        }
    }

    public void Damage(int damage)
    {
        Instantiate(hit, transform);
        hitTimer = hitStunAmount;
        animator.SetTrigger("Hit");
        health -= damage;
        //print(health);
    }
}


