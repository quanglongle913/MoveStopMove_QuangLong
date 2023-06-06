
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PooledObject : MonoBehaviour
{
    private ObjectPool pool;
    public ObjectPool Pool { get => pool; set => pool = value; }

    public void Release()
    {
        pool.ReturnToPool(this);
    }
    public void OnDespawn()
    {
        Destroy(pool);
    }

    public PooledObject Spawner(ObjectPool a_obj, GameObject a_root,bool enable)
    {
        PooledObject _pooledObject = a_obj.GetPooledObject();
        _pooledObject.transform.SetParent(a_root.transform);
        _pooledObject.gameObject.SetActive(enable);
        return _pooledObject;
    }
    public PooledObject Spawner(ObjectPool a_obj, GameObject a_root)
    {
        PooledObject _pooledObject = a_obj.GetPooledObject();
        _pooledObject.transform.SetParent(a_root.transform);
        return _pooledObject;
    }
    public PooledObject Spawner(ObjectPool a_obj, bool enable)
    {
        PooledObject _pooledObject = a_obj.GetPooledObject();
        return _pooledObject;
    }
}

