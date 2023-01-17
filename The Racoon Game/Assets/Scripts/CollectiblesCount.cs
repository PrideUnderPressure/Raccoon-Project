using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiblesCount : MonoBehaviour
{
    public int collectiblesCount = 0;
    public bool canEscape = false;

    void Start()
    {
        
    }

    void Update()
    {
        if (collectiblesCount == 1 && canEscape != true)
        {
            Debug.Log("You got them all!");
            canEscape = true;
        }
    }

    public void CollectibleUP()
    {
        collectiblesCount += 1;
    }
}
