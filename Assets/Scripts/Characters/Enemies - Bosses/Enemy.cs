using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    Idle,
    Attacking,
    TakingDamage,
    Moving,
    Dead
}

public abstract class Enemy : MonoBehaviour
{
    private float Health;
    private State CurrentState;
    private float AttackCooldown;
    private float Timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        CurrentState = State.Idle;
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

    private void ChangeState(State NewState)
    {
        CurrentState = NewState;
    }

    // The attack method will be overridden by each child class;
    public abstract void Attack();

    public void TakeDamage(float Damage)
    {
        Health -= Damage;
        if (Health < 0) ChangeState(State.Dead);
    }

    // The movement will be defined in each child class;
    public abstract void MoveToPlayer(); 
}