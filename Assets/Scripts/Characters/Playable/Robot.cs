using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Robot : MonoBehaviour
{
    public float Health = 150f;
    public float Strength = 250f;
    public float Speed = 7f;

    public Attack AttackPrefab;

    // Movement properties;
    private Vector2 MovementDirection;
    private Vector2 LastPosition = Vector2.zero;
    private Rigidbody2D RBComponent;

    private bool SpecialMovementUsed = false;
    private float SpecialMovementDuration = 0f;

    // Start is called before the first frame update
    void Start()
    {
        RBComponent = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Strength < 250) RegenerateStrength();

        // Key bindings;
        if (Input.GetKeyDown(KeyCode.Q))
        {

        }
        if (Input.GetKeyDown(KeyCode.E))
        {

        }
        if (Input.GetKeyDown(KeyCode.Space))
        {        

        }


    }

    private void FixedUpdate()
    {
        // If the character is not using their special movement, allow them to move as normal;
        if (!SpecialMovementUsed)
        {
            MovementDirection.x = Input.GetAxisRaw("Horizontal");
            RBComponent.velocity = new Vector2(Speed * MovementDirection.x, RBComponent.velocity.y);
        }
        else SpecialMovementDuration += Time.deltaTime;

        // If the character has used their special movement for 0.5 seconds, stop their movement;
        if (SpecialMovementDuration >= 0.2f)
        {
            SpecialMovementUsed = false;
            SpecialMovementDuration = 0f;
        }

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
        if (Health < 0)
        {
            Health = 0;
            Invoke("RestartLevel", 5);
        }
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
    public void MoveInDirection(Vector2 Direction, float Speed)
    {
        SpecialMovementUsed = true;

        if (Direction == Vector2.up) 
            RBComponent.velocity = new Vector2(RBComponent.velocity.x, Speed);
        else if (Direction == Vector2.left)
            RBComponent.velocity = new Vector2(-Speed, RBComponent.velocity.y);
        else if (Direction == Vector2.right)
            RBComponent.velocity = new Vector2(Speed, RBComponent.velocity.y);
    }
/*    public KeyCode AssignStringToKey(string Key)
    {
        switch (Key)
        {
            case "E":
                return KeyCode.E;
            case "Q":
                return KeyCode.Q;
            case "SPACE":
                return KeyCode.Space; 
        }

        return null;
    }*/

    // Restart the level if the player dies;
    public void RestartLevel()
    {
        GameObject.FindObjectOfType<GameManager>().EnterBattle();
    }
}
