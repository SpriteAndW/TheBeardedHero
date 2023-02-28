using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;



public class TransitionManager : Singleton<TransitionManager>
{
    [SceneName]public string startSceneName = string.Empty;

    private CanvasGroup fadeCanvas;
    private bool isFade;
    public float fadeDurating;
    
    public bool isLoad;


    protected override void Awake()
    {
        base.Awake();
        //方便测试
        if(isLoad)
            SceneManager.LoadScene("UI", LoadSceneMode.Additive);
    }

    private void OnEnable()
    {
        EventHandler.TransitionEvent += TransitionEvent;
    }

    private void OnDisable()
    {
        EventHandler.TransitionEvent -= TransitionEvent;
    }

    private void TransitionEvent(string sceneName, Vector3 targetPos)
    {
        if (!isFade)
        {
            StartCoroutine(Transition(sceneName, targetPos));
        }
    }

    private void Start()
    {
        if (startSceneName != String.Empty)
        {
            //TODO:方便测试
            if(isLoad)
                StartCoroutine(LoadSceneSetActive(startSceneName));
        }
        fadeCanvas = FindObjectOfType<CanvasGroup>();
    }


    private IEnumerator Transition(string sceneName, Vector3 targetPosition)
    {
        yield return Fade(1);
        EventHandler.CallBeforeSceneUnloadEvent();

        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

        EventHandler.CallMoveToPosition(targetPosition);


        yield return LoadSceneSetActive(sceneName);


        EventHandler.CallAfterSceneLoadedEvent();
        yield return Fade(0);
    }

    private IEnumerator LoadSceneSetActive(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        EventHandler.CallAfterSceneLoadedEvent();

        Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        SceneManager.SetActiveScene(newScene);
    }

    private IEnumerator Fade(float targetFade)
    {
        isFade = true;
        fadeCanvas.blocksRaycasts = true;

        float speed = MathF.Abs(fadeCanvas.alpha - targetFade) / fadeDurating;
        fadeCanvas.gameObject.transform.GetChild(0)
            .DOScale(new Vector3(targetFade, targetFade, targetFade), fadeDurating);

        while (!Mathf.Approximately(fadeCanvas.alpha, targetFade))
        {
            fadeCanvas.alpha = Mathf.MoveTowards(fadeCanvas.alpha, targetFade, speed * Time.deltaTime);
            yield return null;
        }

        isFade = false;
        fadeCanvas.blocksRaycasts = false;
    }
}