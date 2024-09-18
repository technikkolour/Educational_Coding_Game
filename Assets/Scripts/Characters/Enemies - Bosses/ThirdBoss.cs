using System.Collections.Generic;
using UnityEngine;

public class ThirdBoss : Enemy
{
    private SpriteRenderer SpriteRenderer;

    public List<Attack> Attacks;

    private int ChanceOfHeavyAttack = 35;

    private bool IsInAir = false;
    private float JumpCooldown = 5f;
    private float LastJumpTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        CurrentState = State.Idle;
        Player = GameObject.Find("Robot");
        LastAttackTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        switch (CurrentState)
        {
            case State.Idle:
                DecideOnNextMove();
                break;
            case State.Attacking:
                Attack();
                break;
            case State.TakingDamage:
                break;
            case State.Moving:
                MoveToPlayer();
                break;
            case State.Dead:
                EnemyDied();
                break;
        }
    }

    public void DecideOnNextMove()
    {
        // The further the enemy is from the player, the higher the chance of moving closer;
        float ChanceOfMovement = (Vector2.Distance(Player.transform.position, gameObject.transform.position)) * 5f;
        int RandomValue = Random.Range(1, 100);

        if (!IsInAir)
        {
            if ((RandomValue < ChanceOfMovement) && (PreviousState != State.Moving)) ChangeState(State.Moving);
            else ChangeState(State.Attacking);
        }
    }

    // Randomly decide whether to use the base attack or the heavy attack;
    public override void Attack()
    {
        int RandomValue = Random.Range(1, 100);

        if ((RandomValue > ChanceOfHeavyAttack) && (Vector2.Distance(Player.transform.position, gameObject.transform.position) > 2.5))
        {
            BaseAttack();
            AttackCooldown = 2;
        }
        else
        {
            HeavyAttack();
            AttackCooldown = 5;
        }
    }
    private void BaseAttack()
    {
        Attack Attack = null;

        // Check whether the time passed since the last attack is greater than the cooldown time;
        if ((Time.time - LastAttackTime) > AttackCooldown)
        {
            Vector2 Direction = (Player.transform.position - gameObject.transform.position).normalized;

            // Spawn in the attack;
            Attack = Instantiate(Attacks[0]);

            // Position the attack in front of the enemy;
            Attack.transform.position = new Vector2(gameObject.transform.position.x + Mathf.Sign(Direction.x), gameObject.transform.position.y);
            Attack.Direction = Direction;
            Attack.Power = 75;

            // Ignore the collision between the enemy and the attack;
            Physics2D.IgnoreCollision(gameObject.GetComponent<CapsuleCollider2D>(), Attack.GetComponent<CircleCollider2D>());

            LastAttackTime = Time.time;
        }

        if (Attack == null)
        {
            PreviousState = State.Attacking;
            ChangeState(State.Idle);
        }
    }
    private void HeavyAttack()
    {
        Attack Attack = null;

        // Check whether cooldpon has run out;
        if ((Time.time - LastAttackTime) > AttackCooldown)
        {
            Vector2 Center = Player.transform.position;

            // Randomly select number of projectiles to spawn;
            int NumberOfAttacks = Random.Range(3, 5);

            while (NumberOfAttacks > 0)
            {
                // Spawn projectile;
                Attack = Instantiate(Attacks[1]);

                // Position the attack above the player's last known location;
                float RandomX = Random.Range((Center.x - 1.5f), (Center.x + 1.5f));
                Attack.transform.position = new Vector2(RandomX, 2);

                // Ignore the collision between the enemy and the attack;
                Physics2D.IgnoreCollision(gameObject.GetComponent<CapsuleCollider2D>(), Attack.GetComponent<CapsuleCollider2D>());

                // Decrease number of attacks still to spawn;
                NumberOfAttacks -= 1;
            }

            LastAttackTime = Time.time;
        }

        if (Attack == null)
        {
            PreviousState = State.Attacking;
            ChangeState(State.Idle);
        }

    }

    // Move closer to the player if they are a set distance away;
    public override void MoveToPlayer()
    {
        float Speed = 12.5f;
        Rigidbody2D Rigidbody = GetComponent<Rigidbody2D>();

        // Compute the Direction vector as the difference between the positions of the player and the enemy, normalised;
        Vector2 Direction = (Player.transform.position - gameObject.transform.position).normalized;

        int RandomValue = Random.Range(1, 100);

        // If the enemy is far from the player;
        if (Vector2.Distance(Player.transform.position, gameObject.transform.position) > 3.5)
        {
            Rigidbody.MovePosition(Rigidbody.position + Speed * Time.deltaTime * Direction);

            // If the player is not next to a wall, the enemy has a 10% chance of jumping over them;
            if ((Mathf.Abs(Player.transform.position.x) < 6) && (RandomValue <= 10) && (Time.time - LastJumpTime > JumpCooldown))
            {
                float JumpForce = 10f;
                float ForwardSpeed = 5f;
                Rigidbody.velocity = new Vector2(Direction.x * ForwardSpeed, JumpForce);

                LastJumpTime = Time.time;
                IsInAir = true;
            }
        }
        else
        {
            PreviousState = State.Moving;
            ChangeState(State.Idle);
        }
    }

    // Take damage when hit and verify whether the object is dead;
    public override void TakeDamage(float Damage)
    {
        Health -= Damage;

        if (Health < 0)
        {
            Health = 0;

            // Unfreeze the rotation of the game object around the Z axis;
            // Rotation is frozen at the start of the game to ensure that the enemy sprite does not begin spinning while moving towards the player, or falls over after attacks;
            gameObject.GetComponent<Rigidbody2D>().freezeRotation = false;
            ChangeState(State.Dead);
        }
        else ChangeState(State.Idle);
    }

    // When the enemy dies load back into the Warehouse and mark boss as cleared;
    public void EnemyDied()
    {
        GameManager GameManager = GameObject.FindObjectOfType<GameManager>();

        GameManager.DefeatedBoss(2);
        GameManager.EnterWarehouse();
    }

    // Check if the enemy has landed;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.name == "Floor") IsInAir = false;
    }
}
