using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondBoss : Enemy
{
    public List<Attack> Attacks;
    private int ChanceOfHeavyAttack = 30;

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
    
    public override void Attack()
    {
        int RandomValue = Random.Range(1, 100);

        if (RandomValue > ChanceOfHeavyAttack) Attack(0);
        else Attack(1);
    }
    private void Attack(int Index)
    {
        // Check whether the time passed since the last attack is greater than the cooldown time;
        if ((Time.time - LastAttackTime) > AttackCooldown)
        {
            Vector2 Direction = (Player.transform.position - gameObject.transform.position).normalized;

            // Spawn in the attack;
            Attack Attack = Instantiate(Attacks[Index]);
            // Position the attack in front of the enemy;
            Attack.transform.position = new Vector2(gameObject.transform.position.x + Mathf.Sign(Direction.x), gameObject.transform.position.y);

            // Ignore the collision with the attack;
            if (Attack.name.Contains("Circular"))
                Physics2D.IgnoreCollision(gameObject.GetComponent<CapsuleCollider2D>(), Attack.GetComponent<BoxCollider2D>());

            LastAttackTime = Time.time;
        }
    }

    public override void MoveToPlayer()
    {
        float Speed = 12.5f;
        Rigidbody2D Rigidbody = GetComponent<Rigidbody2D>();

        // Compute the Direction vector as the difference between the positions of the player and the enemy, normalised;
        Vector2 Direction = (Player.transform.position - gameObject.transform.position).normalized;


        // If the player is not next to a wall, the enemy has a 50% chance of jumping over them;
        int RandomValue = Random.Range(1, 100);

        if (Vector2.Distance(Player.transform.position, gameObject.transform.position) > 4.5)
        {
            if (RandomValue >= 50)
            {
                Rigidbody.MovePosition(Rigidbody.position + Direction * Speed * Time.deltaTime);
            } 
            else
            {

            }
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
}
