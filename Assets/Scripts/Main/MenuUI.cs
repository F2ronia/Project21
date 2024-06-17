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
        yield return StartCoroutine(StoryTextSet("��� ��...", 2f));

        StartCoroutine(StartFadeIn(_fadeTime));

        yield return StartCoroutine(StoryTextSet("����� ������ ���� ���� ��ݺ��� �����ϰ� �Ǿ���.", 2f));
        yield return StartCoroutine(StoryTextSet("��� ������ ������ ���۽����� ���� �Ŵ��ϰ� �ź�ο� '���׸����� ž'.", 2f));
        yield return StartCoroutine(StoryTextSet("�� ž�� �ڿ��� ��Ģ�� ����ȭ��Ű�� �� �ֺ��� ���Ӿ��� Ȯ���� ���ư�", 2f));
        yield return StartCoroutine(StoryTextSet("�η��� �� �� �ִ� ���� ���� ��������.", 2f));

        StartCoroutine(StartFadeOut(_fadeTime));
        yield return new WaitForSeconds(_fadeTime);

        storyImage[0].SetActive(false);
        storyImage[1].SetActive(true);
        mainCamera.GetComponent<CameraMovement>().CutSceneSet(2, new Vector3(0, 2.5f, -21f), 10f);

        StartCoroutine(StartFadeIn(_fadeTime));

        yield return StartCoroutine(StoryTextSet("�η��� ž�� �����ϱ� ���� Ư���� �� '�Ƹ�ī��'�� �Ἲ�Ͽ���", 2f));
        yield return StartCoroutine(StoryTextSet("�׵��� ž�� �����ϰ� �ȴ�.", 2f));


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
