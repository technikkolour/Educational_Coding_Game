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
                gameObject.GetComponent<Rigidbody2D>().freezeRotation = false;
                break;
        }
    }

    public override void Attack()
    {
        if ((Time.time - LastAttackTime) > AttackCooldown)
        {
            Attack Attack = Instantiate(Attacks[0]);
            Attack.transform.position = new Vector2(gameObject.transform.position.x - 1, gameObject.transform.position.y);
            LastAttackTime = Time.time;
        }
    }

    public override void TakeDamage(float Damage)
    {
        Health -= Damage;

        if (Health < 0) ChangeState(State.Dead);
        else ChangeState(State.Idle);
    }

    public override void MoveToPlayer()
    {
        float Speed = 15f;
        Rigidbody2D Rigidbody = GetComponent<Rigidbody2D>();

        Vector2 Direction = (Player.transform.position - gameObject.transform.position).normalized;

        if (Vector2.Distance(Player.transform.position, gameObject.transform.position) > 4.5) Rigidbody.MovePosition(Rigidbody.position + Direction * Speed * Time.deltaTime);
    }
}
