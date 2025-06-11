using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectsPool : MonoBehaviour
{
    [SerializeField] private GameObject _pooledObject;
    [SerializeField] private int _objectsToSpawn = 10;
    [SerializeField] private ObjectsPool _effectsPool;
    private Queue<GameObject> _pool;
    private void Start()
    {
        _pool = new Queue<GameObject>();
        for (int i = 0; i < _objectsToSpawn; i++)
        {
            var obj = Instantiate(_pooledObject);
            if(obj.TryGetComponent(out Bullet bullet))
                bullet.EffectsPool= _effectsPool;
            obj.SetActive(false);
            _pool.Enqueue(obj);
        }
    }
    public GameObject GetObject(Vector3 spawnPosition, Quaternion rotation)
    {
        var effect = _pool.Where(t => !t.activeSelf).First();
        effect.transform.position = spawnPosition;
        effect.transform.rotation = rotation;
        effect.SetActive(true);
        return effect;
    }
}
