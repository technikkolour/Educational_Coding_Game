using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float Power;
    public Color Color;
    public Vector2 Direction;

    private Rigidbody2D Rigidbody;

    private float Speed = 7f;
    private Vector2 LastPosition;
    private float DistanceTravelled;
    private float ScreenDistance;

    // Start is called before the first frame update;
    void Start()
    {
        DistanceTravelled = 0f;
        gameObject.GetComponent<SpriteRenderer>().color = Color;
        Rigidbody = GetComponent<Rigidbody2D>();        
        
        // Compute the screen width limit;
        // The screen distance is measured in pixels, while the distance is computed as Unity units;
        // The default setting is 1 unit = 100 pixels, so, to convert the distance into units, the total will be divided by 100;
        ScreenDistance = (Screen.width * 25) / 10000;
    }

    // Update is called once per frame;
    void Update()
    { 
        // Move the projectile in the given direction;
        Rigidbody.MovePosition(Rigidbody.position + Direction * Speed * Time.deltaTime);

        // Compute the distance travelled;
        DistanceTravelled += Vector2.Distance(Rigidbody.position, LastPosition);
        if (DistanceTravelled > ScreenDistance)
        {
            UpdatePower();

            // The screen distance variable increases every time the power of the attack is updated to ensure that the value does not drop to 0;
            ScreenDistance *= 1.05f;
        }

        LastPosition = Rigidbody.position;
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
        Power -= (Power * 5f)/100;
    }
}
