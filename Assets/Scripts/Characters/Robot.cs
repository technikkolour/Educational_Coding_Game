using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    private float Health;
    private float Stamina;

    // Start is called before the first frame update
    void Start()
    {
        Health = 150f;
        Stamina = 250f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float ReturnStamina()
    {
        return Stamina;
    }
    public float ReturnHealth()
    {
        return Health;
    }
}
