using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    public GameObject mainCamera;

    public GameObject menuUI;
    public GameObject introUI;

    public GameObject[] storyImage;
    
    Image fade;
    Text storyText;


    void Start()
    {
        fade = introUI.transform.GetChild(2).GetComponent<Image>();
        storyText = introUI.transform.GetChild(3).GetComponent<Text>();
    }

    public void StartIntro()
    {
        introUI.SetActive(true);
        StartCoroutine(StartIntro(1f));
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
        storyText.DOText("스토리 텍스트1번 입력", 3f);

        StartCoroutine(StartFadeIn(_fadeTime));

        yield return new WaitForSeconds(_fadeTime * 5f);

        StartCoroutine(StartFadeOut(_fadeTime));

        storyText.text = "";
        yield return new WaitForSeconds(_fadeTime);
        storyImage[0].SetActive(false);
        storyImage[1].SetActive(true);
        mainCamera.GetComponent<CameraMovement>().CutSceneSet(2, new Vector3(0, 2.5f, -21f));
        storyText.DOText("스토리 텍스트2번 입력", 3f);

        StartCoroutine(StartFadeIn(_fadeTime));

        yield return new WaitForSeconds(_fadeTime * 5f);
        storyText.text = "";

        StartCoroutine(StartFadeOut(_fadeTime * 0.5f));
        yield return new WaitForSeconds(_fadeTime * 3f);

        SceneManager.LoadScene("MainStage");
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
