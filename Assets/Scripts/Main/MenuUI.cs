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

    private AudioSource audio;
    public AudioClip[] clips;
    Image fade;
    Image skipFade;
    Text storyText;


    void Start()
    {
        audio = GetComponent<AudioSource>();
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
        GameManager.Instance.CallAnyScene("Build_Stage");
    }

    IEnumerator StartIntro(float _fadeTime)
    {
        StartCoroutine(StartFadeOut(_fadeTime));

        yield return new WaitForSeconds(_fadeTime * 1.2f);
        introUI.transform.GetChild(0).gameObject.SetActive(true);
        introUI.transform.GetChild(1).gameObject.SetActive(true);
        menuUI.SetActive(false);
        storyImage[0].SetActive(true);
        mainCamera.GetComponent<CameraMovement>().CutSceneSet(1, new Vector3(1.5f, -2f, -21f), 15f);
        yield return StartCoroutine(StoryTextSet("몇년 전...", 2f));

        StartCoroutine(StartFadeIn(_fadeTime));

        yield return StartCoroutine(StoryTextSet("세계는 미지의 힘에 의해 대격변을 맞이하게 되었다.", 2f));
        yield return StartCoroutine(StoryTextSet("모든 나라의 수도에 갑작스럽게 생긴 거대하고 신비로운 '에테르나의 탑'.", 2f));
        yield return StartCoroutine(StoryTextSet("이 탑은 자연의 법칙을 무력화시키고 그 주변을 끊임없이 확장해 나아가", 2f));
        yield return StartCoroutine(StoryTextSet("인류가 살 수 있는 땅을 점점 좁혀갔다.", 2f));

        StartCoroutine(StartFadeOut(_fadeTime));
        yield return new WaitForSeconds(_fadeTime);

        storyImage[0].SetActive(false);
        storyImage[1].SetActive(true);
        mainCamera.GetComponent<CameraMovement>().CutSceneSet(2, new Vector3(0, 2.5f, -21f), 10f);

        StartCoroutine(StartFadeIn(_fadeTime));

        yield return StartCoroutine(StoryTextSet("인류는 탑을 공략하기 위해 특별한 팀 '아르카나'를 결성하였고", 2f));
        yield return StartCoroutine(StoryTextSet("그들은 탑을 도전하게 된다.", 2f));


        StartCoroutine(StartFadeOut(_fadeTime * 0.5f));
        yield return new WaitForSeconds(_fadeTime * 3f);

        GameManager.Instance.CallAnyScene("Build_Stage");
    }

    IEnumerator StoryTextSet(string _text, float _waitTime)
    {
        storyText.DOText(_text, _waitTime).SetEase(Ease.Linear);
        int randomNum = Random.Range(0,2);
        audio.PlayOneShot(clips[randomNum]);
        yield return new WaitForSeconds(_waitTime);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        storyText.text = "";
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
