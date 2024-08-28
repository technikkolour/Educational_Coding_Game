using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int Power;
    public Color Color;

    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("AttackSprite").GetComponent<SpriteRenderer>().color = Color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Despaw()
    {
        GameObject.Destroy(gameObject);
    }
}
