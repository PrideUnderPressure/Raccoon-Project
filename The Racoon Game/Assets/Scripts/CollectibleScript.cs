using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleScript : MonoBehaviour
{

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.CompareTag("Racoon"))
        {

            Debug.Log("Collided");
            other.GetComponent<CollectiblesCount>().CollectibleUP();
            Destroy(gameObject);
        }
    }
}
