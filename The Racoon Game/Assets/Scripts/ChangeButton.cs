using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeButton : MonoBehaviour
{
    private SpriteRenderer sR;
    public Sprite unselected;
    public Sprite selected;

    void Start()
    {
        sR = gameObject.GetComponent<SpriteRenderer>();
    }

    public void Selected()
    {
        sR.sprite = selected;     
    }

    public void Unselected()
    {
        sR.sprite = unselected;
    }

}
