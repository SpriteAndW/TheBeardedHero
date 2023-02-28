using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    public List<GameObject> poolPrefabs;

    private List<ObjectPool<GameObject>> poolEffectList = new List<ObjectPool<GameObject>>();
    private Queue<GameObject> bulletQueue = new Queue<GameObject>();


    private void OnEnable()
    {
        EventHandler.InitBulletPool += InitBulletEffect;
    }

    private void OnDisable()
    {
        EventHandler.InitBulletPool -= InitBulletEffect;

    }


    private void CreatPool()
    {
        foreach (GameObject item in poolPrefabs)
        {
            Transform parent = new GameObject(item.name).transform;
            parent.SetParent(transform);

            var newPool = new ObjectPool<GameObject>(
                () => Instantiate(item, parent),
                e => { e.gameObject.SetActive(true); },
                e => { e.gameObject.SetActive(false); },
                e => { Destroy(e.gameObject); }
            );
            poolEffectList.Add(newPool);
        }
    }
    
    private void InitBulletEffect(int bulletID)
    {
        var obj = GetPoolObject();

        obj.GetComponent<Bullet>().Init(bulletID);
        obj.SetActive(true);
        StartCoroutine(DisableSound(obj, 3f));
    }
    private GameObject GetPoolObject()
    {
        if (bulletQueue.Count < 2)
        {
            CreateBulletPool();
        }
        return bulletQueue.Dequeue(); //从Queue的开头删除一个对象/元素，并返回该对象/元素。
    }
    
    
    private void CreateBulletPool()
    {
        var parent = new GameObject(poolPrefabs[0].name).transform;
        parent.SetParent(transform);

        for (int i = 0; i < 20; i++)
        {
            GameObject newObj = Instantiate(poolPrefabs[0], parent);
            newObj.SetActive(false);
            bulletQueue.Enqueue(newObj); //在队列的末尾添加一个元素
        }
    }
    private IEnumerator DisableSound(GameObject obj, float duration)
    {
        yield return new WaitForSeconds(duration);
        obj.SetActive(false);
        bulletQueue.Enqueue(obj);
    }
    
    
    
}