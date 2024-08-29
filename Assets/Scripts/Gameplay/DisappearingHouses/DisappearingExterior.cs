using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingExterior : MonoBehaviour
{
    // The SpriteRenderer component allows for modifying the colour channel;
    private SpriteRenderer SpriteRenderer;
    private float Alpha = 1;

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Disappear()
    {
        // Alpha may not be exactly 0, so verify whether the value is below or equal to 0;
        if (Alpha <= 0)
            gameObject.SetActive(false);

        // Slowly lower the opacity of the component, to create a fade effect;
        if (Alpha > 0) Alpha -= 0.2f;
        SpriteRenderer.color = new Color(1f, 1f, 1f, Alpha);
    }
    public void Reappear()
    {
        SpriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        Alpha = 1;
    }
}
