using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MenuUI : MonoBehaviour
{
    public GameObject mainCamera;

    public GameObject menuUI;
    public GameObject introUI;

    public GameObject[] storyImage;
    
    Image fade;
    Image skipFade;
    Text storyText;


    void Start()
    {
        fade = introUI.transform.GetChild(2).GetComponent<Image>();
        skipFade = introUI.transform.GetChild(5).GetComponent<Image>();
        storyText = introUI.transform.GetChild(3).GetComponent<Text>();
    }

    public void StartIntro()
    {
        introUI.SetActive(true);
        StartCoroutine(StartIntro(1f));
    }
    public void SkipIntro()
    {
        StartCoroutine(SkipIntroCoroutine());
    }
    IEnumerator SkipIntroCoroutine()
    {
        skipFade.gameObject.SetActive(true);
        skipFade.DOFade(1, 0.5f);
        yield return new WaitForSeconds(0.6f);
        GameManager.Instance.CallAnyScene("temp_stage");
        // GameManager.Instance.CallAnyScene("MainStage");
    }

    IEnumerator StartIntro(float _fadeTime)
    {
        StartCoroutine(StartFadeOut(_fadeTime));

        yield return new WaitForSeconds(_fadeTime * 1.2f);
        introUI.transform.GetChild(0).gameObject.SetActive(true);
        introUI.transform.GetChild(1).gameObject.SetActive(true);
        menuUI.SetActive(false);
        storyImage[0].SetActive(true);
        mainCamera.GetComponent<CameraMovement>().CutSceneSet(1, new Vector3(1.5f, -2f, -21f));
        storyText.DOText("test 1", 3f);

        StartCoroutine(StartFadeIn(_fadeTime));

        yield return new WaitForSeconds(_fadeTime * 5f);

        StartCoroutine(StartFadeOut(_fadeTime));

        storyText.text = "";
        yield return new WaitForSeconds(_fadeTime);
        storyImage[0].SetActive(false);
        storyImage[1].SetActive(true);
        mainCamera.GetComponent<CameraMovement>().CutSceneSet(2, new Vector3(0, 2.5f, -21f));
        storyText.DOText("text 2", 3f);

        StartCoroutine(StartFadeIn(_fadeTime));

        yield return new WaitForSeconds(_fadeTime * 5f);
        storyText.text = "";

        StartCoroutine(StartFadeOut(_fadeTime * 0.5f));
        yield return new WaitForSeconds(_fadeTime * 3f);

        GameManager.Instance.CallAnyScene("temp_stage");
    }

    IEnumerator StartFadeOut(float _fadeTime)
    {
        Color alhpa = fade.color;
        alhpa.a = 0f;
        while (fade.color.a < 1)
        {
            alhpa.a += Time.deltaTime * _fadeTime;
            fade.color = alhpa;
            yield return null;
        }
    }
    IEnumerator StartFadeIn(float _fadeTime)
    {
        Color alhpa = fade.color;
        alhpa.a = 1f;
        while (fade.color.a > 0)
        {
            alhpa.a -= Time.deltaTime * _fadeTime;
            fade.color = alhpa;
            yield return null;
        }
    }
}
