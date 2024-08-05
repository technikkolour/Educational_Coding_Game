using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingObject : MonoBehaviour
{
    private SpriteRenderer SpriteRenderer;
    private float Alpha = 1;

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Alpha > 0) Alpha -= 0.2f;
        SpriteRenderer.color = new Color(1f, 1f, 1f, Alpha);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        SpriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        Alpha = 1;
    }
}
