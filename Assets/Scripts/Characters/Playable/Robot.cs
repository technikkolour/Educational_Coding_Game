using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    public float Health;
    public float Strength = 100f;
    public float Speed = 7f;

    public Attack AttackPrefab;

    // Movement properties;
    private Vector2 MovementDirection;
    private Vector2 LastPosition = Vector2.zero;
    private Rigidbody2D RBComponent;
    private GameManager GameManager;
    private bool SpecialMovementUsed = false;
    private float SpecialMovementDuration = 0f;
    public Dictionary<string, List<CodeBlock>> KeyBindings = new() { { "Q", new() { } }, { "E", new() { } }, { "Space", new() { } } };

    // Start is called before the first frame update;
    void Start()
    {
        RBComponent = GetComponent<Rigidbody2D>();
        GameManager = FindObjectOfType<GameManager>();

        if (GameManager != null)
            Health = GameManager.CurrentTotalHealth();
        else Health = 150f;
    }

    // Update is called once per frame;
    void Update()
    {
        if (Strength < 100f) RegenerateStrength();

        // Key bindings;
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ExecuteBlocksForKey("Q");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            ExecuteBlocksForKey("E");
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ExecuteBlocksForKey("Space");
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

    // Functions relating to the health of the robot;
    public void TakeDamage(float Damage)
    {
        Health -= Damage;

        // If the robot's health drops below 0 thy are dead and the level is restarted;
        if (Health < 0)
        {
            Health = 0;
            GameManager.PauseGame();
            RestartLevel();
        }
    }

    // Functions relating to the strength of the robot.
    private void DecreaseStrength(float AttackPower)
    {
        Strength -= AttackPower;
    }
    private void RegenerateStrength()
    {
        Strength = (2*Strength + .5f) / 2.0f;

        // Cap the strength at 100;
        if (Strength > 100) Strength = 100;
    }

    // Robot Building Blocks;
    // Spawn an attack that deals the sepcified amount of power;
    public void AttackWithPower(float Power)
    {
        // Check that the character has enough strength to use the attack;
        // In the event that they do not, do nothing;

        if (Power >= 100) Power = 100;

        if (Strength >= Power)
        {
            Attack Attack = Instantiate(AttackPrefab);
            Attack.transform.position = new Vector2(gameObject.transform.position.x + 1, gameObject.transform.position.y);
            Attack.Power = Power;

            DecreaseStrength(Power);
        }
    }
    // Move the character in the specified direction at a specified speed;
    public void MoveInDirection(Vector2 Direction, float Speed)
    {
        SpecialMovementUsed = true;

        if (Speed >= 10) Speed = 10;

        // This is intended to simulate jumping and dashing to either side;
        if (Direction == Vector2.up) 
            RBComponent.velocity = new Vector2(RBComponent.velocity.x, Speed);
        else if (Direction == Vector2.left)
            RBComponent.velocity = new Vector2(-Speed, RBComponent.velocity.y);
        else if (Direction == Vector2.right)
            RBComponent.velocity = new Vector2(Speed, RBComponent.velocity.y);
    }
    // Convert string directions to Vector2; 
    public Vector2 StringToVector(string Direction)
    {
        return Direction switch
        {
            "Up" => Vector2.up,
            "Left" => Vector2.left,
            "Right" => Vector2.right,
            _ => Vector2.zero,
        };
    }
    // This calls the actions corresponding to the keys bindings that have been set by the player;
    public void ExecuteBlocksForKey(string Key)
    {
        if (KeyBindings.ContainsKey(Key))
        {
            foreach (CodeBlock Block in KeyBindings[Key])
            {
                switch (Block.Type)
                {
                    case "Attack With Power":
                        // Validate the values entered by the player;
                        if (!string.IsNullOrEmpty(Block.Values[0]) && float.TryParse(Block.Values[0], out _))
                            AttackWithPower(float.Parse(Block.Values[0]));
                        break;
                    case "Move In Direction":
                        // Validate the values entered by the player;
                        if (!string.IsNullOrEmpty(Block.Values[0]) && float.TryParse(Block.Values[0], out _))
                            MoveInDirection(StringToVector(Block.Values[3]), float.Parse(Block.Values[0]));
                        break;
                }
            }
        }
    }

    // Restart the level if the player dies;
    public void RestartLevel()
    {
        GameManager.DisplayDeathScreen();
    }
}
