using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBoss : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        AttackCooldown = 4f;
        Health = 250f;

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

    public override void TakeDamage(float Damage)
    {
        Health -= Damage;
        if (Health < 0) ChangeState(State.Dead);
    }

    public override void MoveToPlayer()
    {

    }
}
