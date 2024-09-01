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
    // The variables that all enemies have in common;
    public float Health;
    public State CurrentState;
    public State PreviousState;
    public float AttackCooldown;
    public float LastAttackTime;

    // A reference to the player's robot;
    public GameObject Player;

    public void ChangeState(State NewState)
    {
        CurrentState = NewState;
    }

    // The attack method will be overridden by each child class;
    public abstract void Attack();

    // The damage taking behaviour will be overridden by each child class;
    public abstract void TakeDamage(float Damage);

    // The movement will be defined in each child class;
    public abstract void MoveToPlayer();
}