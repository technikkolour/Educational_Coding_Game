using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interior : MonoBehaviour
{
    public GameObject Exterior;
    public DisappearingExterior Exterior_DisappearingComponent;

    // Start is called before the first frame update
    void Start()
    {
        Exterior_DisappearingComponent = Exterior.GetComponent<DisappearingExterior>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Exterior_DisappearingComponent.Disappear();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Exterior.gameObject.SetActive(true);
        Exterior_DisappearingComponent.Reappear();
    }
}
