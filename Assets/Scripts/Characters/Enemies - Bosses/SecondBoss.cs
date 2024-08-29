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

        if (RandomValue < ChanceOfHeavyAttack) BaseAttack();
        else HeavyAttack();
    }
    private void BaseAttack()
    {
        // Check whether the time passed since the last attack is greater than the cooldown time;
        if ((Time.time - LastAttackTime) > AttackCooldown)
        {
            // Spawn in the attack;
            Attack Attack = Instantiate(Attacks[0]);
            // Position the attack in front of the enemy;
            Attack.transform.position = new Vector2(gameObject.transform.position.x - 1, gameObject.transform.position.y);
            LastAttackTime = Time.time;
        }
    }
    private void HeavyAttack()
    {
        // Check whether the time passed since the last attack is greater than the cooldown time;
        if ((Time.time - LastAttackTime) > AttackCooldown)
        {
            // Spawn in the attack;
            Attack Attack = Instantiate(Attacks[1]);

            // Position the attack in front of the enemy;
            Attack.transform.position = new Vector2(gameObject.transform.position.x - 1, gameObject.transform.position.y);
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
        if (Health < 0) ChangeState(State.Dead);
    }
}
