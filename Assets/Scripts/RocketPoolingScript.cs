using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketPoolingScript : MonoBehaviour
{

    public int pooledObjectsNumber = 5;
    public GameObject pooledObject;
    private List<GameObject> pooledObjects;

    public static RocketPoolingScript current;

    void Awake()
    {
        current = this;
    }


    void Start()
    {
        pooledObjects = new List<GameObject>(pooledObjectsNumber);
        for (int i = 0; i < pooledObjectsNumber; i++)
        {
            GameObject obj = (GameObject)Instantiate(pooledObject);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }
    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }
}
