using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    private float Health;
    private float Strength;

    // Movement functions;
    private UnityEngine.Vector2 MovementDirection;
    private Rigidbody2D RBComponent;

    // Start is called before the first frame update
    void Start()
    {
        Health = 150f;
        Strength = 250f;

        RBComponent = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Strength < 250) RegenerateStrength();
    }

    private void FixedUpdate()
    {
        float speed = 7;

        MovementDirection.x = Input.GetAxisRaw("Horizontal");
        RBComponent.MovePosition(RBComponent.position + MovementDirection * speed * Time.deltaTime);
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
        Strength = (Strength + (Strength + 5) / 2.0f) % 250;
    }
}
