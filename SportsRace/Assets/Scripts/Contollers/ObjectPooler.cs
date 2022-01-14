using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler<T> where T : MonoBehaviour
{
    private List<T> prefabPool;


    public T Prefab { get; }
    public bool AutoExpand { get; set; }
    public Transform PositionContainer { get; }


    public ObjectPooler(T prefab, Transform positionContainer)
    {
        this.Prefab = prefab;
        this.PositionContainer = positionContainer;
    }


    public void CreatePool(int quantity)
    {
        prefabPool = new List<T>();

        for (int i = 0; i < quantity; i++)
            CreateObject();
    }

    private T CreateObject(bool isActiveByDefault = false)
    {
        var createdObj = UnityEngine.Object.Instantiate(Prefab, PositionContainer);
        createdObj.gameObject.SetActive(isActiveByDefault);
        prefabPool.Add(createdObj);
        return createdObj;
    }

    public bool TryFindFreeObject(out T freeObject)
    {
        foreach (var obj in prefabPool)
        {
            if (!obj.gameObject.activeInHierarchy)
            {
                obj.gameObject.SetActive(true);
                freeObject = obj;
                return true;
            }
        }

        freeObject = null;
        return false;
    }

    public T GetFreeObject()
    {
        if (TryFindFreeObject(out var freeObj))
            return freeObj;
        else if (AutoExpand)
            return CreateObject(true);
        else
            throw new Exception($"Can't find any free objects of {typeof(T)}");
    }

    public List<T> GetAllActiveObjects()
    {
        List<T> returnedObjects = new List<T>();

        foreach (var obj in prefabPool)
        {
            if (obj.gameObject.activeInHierarchy)
            {
                returnedObjects.Add(obj);
                return returnedObjects;
            }
        }

        return null;
    }

    public List<T> GetPool()
    {
        return prefabPool;
    }
}
