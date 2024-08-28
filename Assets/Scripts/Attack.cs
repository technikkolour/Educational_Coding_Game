using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float Power;
    public Color Color;
    public Vector2 Direction;

    private float Speed = 7f;
    private float DistanceTravelled;
    private Rigidbody2D Rigidbody;

    // Start is called before the first frame update;
    void Start()
    {
        DistanceTravelled = 0f;

        gameObject.GetComponent<SpriteRenderer>().color = Color;

        Rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame;
    void Update()
    { 
        // Compute the screen width limit;
        float ScreenWidth = (Screen.width * 15) / 100;

        DistanceTravelled = Speed * Time.deltaTime;

        if (DistanceTravelled > ScreenWidth)
            UpdatePower();

        Rigidbody.MovePosition(Rigidbody.position + Direction * Speed * Time.deltaTime); ;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the attack collides with the robot, the attack should damage the player;
        Robot Robot = collision.collider.gameObject.GetComponent<Robot>();
        if (Robot != null) Robot.TakeDamage(Power);

        // The attack should despawn in the event that it collides with an object;
        GameObject.Destroy(gameObject);
    }

    // The power of an attack decreases the longer it travels;
    private void UpdatePower()
    {
        Power -= (Power * 2.5f)/100;
    }
}
