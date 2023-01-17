using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button_Controller : MonoBehaviour
{
    //BEST, CLEANEST SCRIPT IN THE WHOLE GAME

    public GameObject startGame;
    public GameObject levelSelect;
    public GameObject quit;

    public int selectedButton = 1;

    public AudioSource click;
    public AudioSource clickSelected;
    void Start()
    {

    }

    void Update()
    {
        if (selectedButton != 3 && Input.GetKeyDown(KeyCode.S))
        {
            click.PlayOneShot(click.clip, 0.5f);
            selectedButton++;
        }
        if (selectedButton != 1 && Input.GetKeyDown(KeyCode.W))
        {
            click.PlayOneShot(click.clip, 0.5f);
            selectedButton--;
        }

        
        //This CAN be written as some kind of a smart... loop? Or something, but anyway it's C L E A N;
        //Ok, maybe those could be methods...
        if (selectedButton == 1)
        {
            startGame.GetComponent<ChangeButton>().Selected();
            levelSelect.GetComponent<ChangeButton>().Unselected();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                clickSelected.PlayOneShot(clickSelected.clip, 0.5f);
                SceneManager.LoadScene("Level_1");
            }
        }
        if (selectedButton == 2)
        {
            startGame.GetComponent<ChangeButton>().Unselected();
            quit.GetComponent<ChangeButton>().Unselected();
            levelSelect.GetComponent<ChangeButton>().Selected();
        }
        if (selectedButton == 3)
        {
            levelSelect.GetComponent<ChangeButton>().Unselected();
            quit.GetComponent<ChangeButton>().Selected();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                clickSelected.PlayOneShot(clickSelected.clip, 0.5f);
                Application.Quit();
            }
        }

    }
}
