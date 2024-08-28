using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdBoss : Enemy
{
    private int ChanceOfHeavyAttack = 35;

    // Start is called before the first frame update
    void Start()
    {
        AttackCooldown = 2f;
        Health = 1000f;

        CurrentState = State.Idle;
        Player = GameObject.Find("Robot");
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

    }

    public override void MoveToPlayer()
    {

    }

    public override void TakeDamage(float Damage)
    {
        Health -= Damage;

        if (Health < 0) ChangeState(State.Dead);
        if (Health < 500) ChanceOfHeavyAttack = 45;
    }
}
