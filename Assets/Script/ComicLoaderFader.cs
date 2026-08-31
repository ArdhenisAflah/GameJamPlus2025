using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ComicLoaderFader : MonoBehaviour
{
    public Sprite[] Comics;
    public Image comicsComponent1;
    public Image comicsComponent2;
    public string NextSceneLoad;

    public void FadeOut(float duration, Image comics)
    {
        // Fades alpha target towards 1 (Transparent)
        comics.CrossFadeAlpha(0f, duration, false);
    }
    public void FadeIn(float duration, Image comics)
    {
        // Fades alpha target towards 1 (Transparent)
        comics.CrossFadeAlpha(1f, duration, false);
    }
    // Start is called before the first frame update
    void Start()
    {
        comicsComponent2.canvasRenderer.SetAlpha(0f);
        comicsComponent1.canvasRenderer.SetAlpha(0f);
        StartCoroutine(StartLoader());
    }

    private IEnumerator WaitOrSkipFunction(float fadeCountdown, Image activeImage, float target, string mode = "fadein")
    {
        float fadeTimer = 0;
           while (fadeTimer < fadeCountdown)
            {
                if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
                {
                    // Instan selesaikan transisi
                    if(mode == "fadein")
                    {
                        FadeIn(0f, activeImage);            
                    }else if(mode == "fadeout")
                    {
                        FadeOut(0f, activeImage);
                    }
                    activeImage.canvasRenderer.SetAlpha(target);
                    break;
                }
                fadeTimer += Time.deltaTime;
                yield return null;
            }
            Debug.Log("Skipping");
    }

    // Update is called once per frame
    private IEnumerator StartLoader()
    {
        int firstImg = 0;
        int secondImg = 1;
        for(int i = 0; i < Comics.Length/2+1; i++){
           
            if(firstImg >= Comics.Length)
            {
                break;
            }
            comicsComponent1.sprite = Comics[firstImg];
            //skippable
            FadeIn(1f, comicsComponent1);
            yield return StartCoroutine(WaitOrSkipFunction(1f, comicsComponent1, 1, mode:"fadein"));
            FadeOut(2f, comicsComponent1);
            yield return StartCoroutine(WaitOrSkipFunction(2f, comicsComponent1, 0, mode:"fadeout"));


            if(secondImg >= Comics.Length)
            {
                break;
            }
            comicsComponent2.sprite = Comics[secondImg];
            //skippable
            FadeIn(1f, comicsComponent2);
            yield return StartCoroutine(WaitOrSkipFunction(1f, comicsComponent2, 1, mode:"fadein"));
            FadeOut(2f, comicsComponent2);
            yield return StartCoroutine(WaitOrSkipFunction(2f, comicsComponent2, 0, mode:"fadeout"));


            firstImg += 2;
            secondImg += 2;
        }
        LoadNextScene();
    } 

    private void LoadNextScene()
    {
        SceneManager.LoadScene(NextSceneLoad);
    }
}
