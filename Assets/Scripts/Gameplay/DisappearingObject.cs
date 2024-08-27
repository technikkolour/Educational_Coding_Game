using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingObject : MonoBehaviour
{
    // The SpriteRenderer component allows for modifying the colour channel;
    private SpriteRenderer SpriteRenderer;
    private float Alpha = 1;

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // This function is called once per frame, while an object is colliding with the trigger;
    private void OnTriggerStay2D(Collider2D collision)
    {
        // Slowly lower the opacity of the component, to create a fade effect;
        if (Alpha > 0) Alpha -= 0.2f;
        SpriteRenderer.color = new Color(1f, 1f, 1f, Alpha);
    }

    // This function is called once, when the collision is no longer occuring;
    private void OnTriggerExit2D(Collider2D collision)
    {
        SpriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        Alpha = 1;
    }
}
