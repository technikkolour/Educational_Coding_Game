using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondBoss : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        AttackCooldown = 3f;
        Health = 500f;

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
    }
}
