using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GetCaught : MonoBehaviour

{
    public bool deathCountdownOn = true;

    //How long until next countdown
    public float timer = 1;

    //References a script from the Hiding_Spots tilemap
    public HidingSpots hs;

    public GameObject giant;

    //Each countdown sound in order
    public AudioSource countSound1;
    public AudioSource countSound2;
    public AudioSource countSound3;
    //huh
    public AudioSource huh;

    public GameObject blackSquare;
    public bool fadeScreen = false;
    void Start()
    {
        StartCoroutine(FadeInSquare());
        StartCoroutine(CD());
    }

    void Update()
    {
        if (fadeScreen)
        {
            StartCoroutine(FadeBlackOutSquare());
        }
    }

    IEnumerator CD()
    {
        while (deathCountdownOn)
        {
            giant.GetComponent<Giant>().BeIdle();
            Debug.Log("Liczymy");
            yield return new WaitForSeconds(timer);

            countSound1.PlayOneShot(countSound1.clip, 0.2f);
            yield return new WaitForSeconds(0.5f);

            countSound2.PlayOneShot(countSound2.clip, 0.2f);
            yield return new WaitForSeconds(0.5f);

            countSound3.PlayOneShot(countSound3.clip, 0.3f);
            huh.PlayOneShot(huh.clip, 0.5f);
            yield return new WaitForSeconds(0.8f);

            giant.GetComponent<Giant>().BeTurning();

            if (hs.hidden != true)
            {
                Debug.Log("Se zdechles");
                fadeScreen = true;
            }
            else
            {
                Debug.Log("PRZETRWALEŚ");
            }
            yield return new WaitForSeconds(0.7f);
        }
    }

    public IEnumerator FadeBlackOutSquare(bool fadeToBlack = true, int fadeSpeed = 3)
    {
        Color objectColor = blackSquare.GetComponent<Image>().color;
        float fadeAmount;

        if (fadeToBlack)
        {
            while (blackSquare.GetComponent<Image>().color.a < 1)
            {
                fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);

                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackSquare.GetComponent<Image>().color = objectColor;
                yield return null;
                yield return new WaitForSeconds(1f);
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    public IEnumerator FadeInSquare(int fadeSpeed = 1)
    {
        Color objectColor = blackSquare.GetComponent<Image>().color;
        float fadeAmount;

            while (blackSquare.GetComponent<Image>().color.a > 0)
            {
                fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);

                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackSquare.GetComponent<Image>().color = objectColor;
                yield return null;
            }
    }
}
