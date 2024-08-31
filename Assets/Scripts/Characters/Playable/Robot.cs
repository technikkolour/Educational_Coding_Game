using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    public float Health = 150f;
    public float Strength = 250f;
    public float Speed = 7f;

    public Attack AttackPrefab;

    // Movement functions;
    private UnityEngine.Vector2 MovementDirection;
    private Rigidbody2D RBComponent;

    // Start is called before the first frame update
    void Start()
    {
        RBComponent = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Strength < 250) RegenerateStrength();
        if (Input.GetKeyDown(KeyCode.Space)) MoveInDirection(new Vector2(0f, 1f), 10);
    }

    private void FixedUpdate()
    {
        MovementDirection.x = Input.GetAxisRaw("Horizontal");
        RBComponent.MovePosition(RBComponent.position + Speed * Time.deltaTime * MovementDirection);
    }

    // Getters;
    public float GetStrength()
    {
        return Strength;
    }
    public float GetHealth()
    {
        return Health;
    }

    // Functions relating to the health of the robot.
    public void TakeDamage(float Damage)
    {
        Health -= Damage;
    }
    public void IncreaseHealth()
    {
        Health += Health + 50f;
    }

    // Functions relating to the strength of the robot.
    private void DecreaseStrength(float AttackPower)
    {
        Strength -= AttackPower;
    }
    private void RegenerateStrength()
    {
        Strength = (2*Strength + 2.5f) / 2.0f;

        // Cap the strength at 250;
        if (Strength > 250) Strength = 250;
    }

    // Robot Building Blocks;
    public void AttackWithPower(float Power)
    {
        if (Strength >= Power)
        {
            Attack Attack = Instantiate(AttackPrefab);
            Attack.transform.position = new Vector2(gameObject.transform.position.x + 1, gameObject.transform.position.y);
            Attack.Power = Power;

            DecreaseStrength(Power);
        }
    }
    public void MoveInDirection(Vector2 Direction, float Distance)
    {
        RBComponent.MovePosition(RBComponent.position + Speed * Time.deltaTime * Direction * Distance);
    }
}
