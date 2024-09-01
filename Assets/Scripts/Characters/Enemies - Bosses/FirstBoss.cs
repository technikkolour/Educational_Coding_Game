using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBoss : Enemy
{
    public List<Attack> Attacks;

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
                break;
        }
    }

    public void DecideOnNextMove()
    {
        // The further the enemy is from the player, the higher the chance of moving closer;
        float ChanceOfMovement = (Vector2.Distance(Player.transform.position, gameObject.transform.position)) * 20;
        int RandomValue = Random.Range(1, 100);

        if (RandomValue < ChanceOfMovement) ChangeState(State.Moving);
        else ChangeState(State.Attacking);
    }

    public override void Attack()
    {
        // Check whether the time passed since the last attack is greater than the cooldown time;
        if ((Time.time - LastAttackTime) > AttackCooldown)
        {
            Vector2 Direction = (Player.transform.position - gameObject.transform.position).normalized;

            // Spawn in the attack;
            Attack Attack = Instantiate(Attacks[0]);
            // Position the attack in front of the enemy;
            Attack.transform.position = new Vector2(gameObject.transform.position.x + Mathf.Sign(Direction.x), gameObject.transform.position.y);
            Attack.Direction = Direction;

            // Ignore the collision with the attack;
            Physics2D.IgnoreCollision(gameObject.GetComponent<CapsuleCollider2D>(), Attack.GetComponent<CircleCollider2D>());

            LastAttackTime = Time.time;
            ChangeState(State.Idle);
        }
    }

    public override void TakeDamage(float Damage)
    {
        Health -= Damage;

        if (Health < 0)
        {
            // Unfreeze the rotation of the game object around the Z axis;
            // Rotation is frozen at the start of the game to ensure that the enemy sprite does not begin spinning while moving towards the player, or falls over after attacks;
            gameObject.GetComponent<Rigidbody2D>().freezeRotation = false;
            ChangeState(State.Dead);
        }
        else ChangeState(State.Idle);
    }

    public override void MoveToPlayer()
    {
        float Speed = 12.5f;
        Rigidbody2D Rigidbody = GetComponent<Rigidbody2D>();

        // Compute the Direction vector as the difference between the positions of the player and the enemy, normalised;
        // This has been done to ;
        Vector2 Direction = (Player.transform.position - gameObject.transform.position).normalized;

        if (Vector2.Distance(Player.transform.position, gameObject.transform.position) > 4.5) Rigidbody.MovePosition(Rigidbody.position + Direction * Speed * Time.deltaTime);
        else ChangeState(State.Attacking);
    }
}
