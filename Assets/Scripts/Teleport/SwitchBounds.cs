using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
using DG.Tweening;


public class SwitchBounds : MonoBehaviour
{
    private CinemachineVirtualCamera cinemach;
    private GameObject cameraFlower;
    public float durating;

    private void Awake()
    {
        cinemach = GetComponent<CinemachineVirtualCamera>();
        cameraFlower = GameObject.FindWithTag("CameraFlower");
    }


    private void OnEnable()
    {
        EventHandler.AfterSceneLoadedEvent += SwitchConfinerShape;
        EventHandler.CameraLookEvent += OnCameraLookEvent;
    }

    private void OnDisable()
    {
        EventHandler.AfterSceneLoadedEvent -= SwitchConfinerShape;
        EventHandler.CameraLookEvent -= OnCameraLookEvent;
    }

    private void OnCameraLookEvent(GameObject obj, bool isTalking)
    {
        StopAllCoroutines();
        if (isTalking)
        {
            cinemach.Follow = obj.transform;
            StartCoroutine(ToTarget(5));
        }
        else
        {
            cinemach.Follow = cameraFlower.transform;
            StartCoroutine(ToTarget(8));
        }
    }

    private IEnumerator ToTarget(float target)
    {
        
        float speed = Mathf.Abs(cinemach.m_Lens.OrthographicSize - target) / durating;
        while (!Mathf.Approximately(cinemach.m_Lens.OrthographicSize, target))
        {
            cinemach.m_Lens.OrthographicSize =
                Mathf.MoveTowards(cinemach.m_Lens.OrthographicSize, target, speed * Time.deltaTime);
            yield return null;
        }
    }


    private void SwitchConfinerShape()
    {
        PolygonCollider2D confinerShape =
            GameObject.FindGameObjectWithTag("BoundsConfiner").GetComponent<PolygonCollider2D>();

        CinemachineConfiner2D confiner = GetComponent<CinemachineConfiner2D>();

        confiner.m_BoundingShape2D = confinerShape;

        confiner.InvalidateCache();
    }
}