using System.Collections.Generic;
using UnityEngine;

[System.Serializable()]
public class Pooler
{
    [SerializeField()]
    private GameObject[] _prefabs;
    [SerializeField()]
    private int _maximumSize;

    private List<GameObject> _pool;

    public Pooler()
    {
        _prefabs = null;
        _maximumSize = int.MaxValue;
        _pool = new List<GameObject>();
    }

    public GameObject Next()
    {
        foreach (var item in _pool)
        {
            if (!item.activeInHierarchy)
            {
                item.SetActive(true);
                return item;
            }
        }
        if (_pool.Count < _maximumSize)
        {
            var newItem = Object.Instantiate(_prefabs[Random.Range(0, _prefabs.Length)]);
            _pool.Add(newItem);
            return newItem;
        }
        return null;
    }

    public GameObject Next(Vector3 position)
    {
        foreach (var item in _pool)
        {
            if (!item.activeInHierarchy)
            {
                item.transform.position = position;
                item.SetActive(true);
                return item;
            }
        }
        if (_pool.Count < _maximumSize)
        {
            var newItem = Object.Instantiate(_prefabs[Random.Range(0, _prefabs.Length)]);
            newItem.transform.position = position;
            _pool.Add(newItem);
            return newItem;
        }
        return null;
    }

    public GameObject NextWithVelocity(Vector3 position, Vector3 velocity)
    {
        Rigidbody rigidbody = null;
        foreach (var item in _pool)
        {
            if (!item.activeInHierarchy)
            {
                item.transform.position = position;
                rigidbody = item.GetComponent<Rigidbody>();
                if (rigidbody != null)
                {
                    rigidbody.velocity = velocity;
                }
                item.SetActive(true);
                return item;
            }
        }
        if (_pool.Count < _maximumSize)
        {
            var newItem = Object.Instantiate(_prefabs[Random.Range(0, _prefabs.Length)]);
            newItem.transform.position = position;
            rigidbody = newItem.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.velocity = velocity;
            }
            _pool.Add(newItem);
            return newItem;
        }
        return null;
    }

    public GameObject NextWithVelocity2D(Vector3 position, Vector2 velocity)
    {
        Rigidbody2D rigidbody2D = null;
        foreach (var item in _pool)
        {
            if (!item.activeInHierarchy)
            {
                item.transform.position = position;
                rigidbody2D = item.GetComponent<Rigidbody2D>();
                if (rigidbody2D != null)
                {
                    rigidbody2D.velocity = velocity;
                }
                item.SetActive(true);
                return item;
            }
        }
        if (_pool.Count < _maximumSize)
        {
            var newItem = Object.Instantiate(_prefabs[Random.Range(0, _prefabs.Length)]);
            newItem.transform.position = position;
            rigidbody2D = newItem.GetComponent<Rigidbody2D>();
            if (rigidbody2D != null)
            {
                rigidbody2D.velocity = velocity;
            }
            _pool.Add(newItem);
            return newItem;
        }
        return null;
    }

    public GameObject Next(Transform parent)
    {
        foreach (var item in _pool)
        {
            if (!item.activeInHierarchy)
            {
                item.SetActive(true);
                return item;
            }
        }
        if (_pool.Count < _maximumSize)
        {
            var newItem = Object.Instantiate(_prefabs[Random.Range(0, _prefabs.Length)], parent);
            _pool.Add(newItem);
            return newItem;
        }
        return null;
    }

    public GameObject Next(Vector3 position, Transform parent)
    {
        foreach (var item in _pool)
        {
            if (!item.activeInHierarchy)
            {
                item.transform.position = position;
                item.SetActive(true);
                return item;
            }
        }
        if (_pool.Count < _maximumSize)
        {
            var newItem = Object.Instantiate(_prefabs[Random.Range(0, _prefabs.Length)], parent);
            newItem.transform.position = position;
            _pool.Add(newItem);
            return newItem;
        }
        return null;
    }

    public GameObject NextWithVelocity(Vector3 position, Vector3 velocity, Transform parent)
    {
        Rigidbody rigidbody = null;
        foreach (var item in _pool)
        {
            if (!item.activeInHierarchy)
            {
                item.transform.position = position;
                rigidbody = item.GetComponent<Rigidbody>();
                if (rigidbody != null)
                {
                    rigidbody.velocity = velocity;
                }
                item.SetActive(true);
                return item;
            }
        }
        if (_pool.Count < _maximumSize)
        {
            var newItem = Object.Instantiate(_prefabs[Random.Range(0, _prefabs.Length)], parent);
            newItem.transform.position = position;
            rigidbody = newItem.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.velocity = velocity;
            }
            _pool.Add(newItem);
            return newItem;
        }
        return null;
    }

    public GameObject NextWithVelocity2D(Vector3 position, Vector2 velocity, Transform parent)
    {
        Rigidbody2D rigidbody2D = null;
        foreach (var item in _pool)
        {
            if (!item.activeInHierarchy)
            {
                item.transform.position = position;
                rigidbody2D = item.GetComponent<Rigidbody2D>();
                if (rigidbody2D != null)
                {
                    rigidbody2D.velocity = velocity;
                }
                item.SetActive(true);
                return item;
            }
        }
        if (_pool.Count < _maximumSize)
        {
            var newItem = Object.Instantiate(_prefabs[Random.Range(0, _prefabs.Length)], parent);
            newItem.transform.position = position;
            rigidbody2D = newItem.GetComponent<Rigidbody2D>();
            if (rigidbody2D != null)
            {
                rigidbody2D.velocity = velocity;
            }
            _pool.Add(newItem);
            return newItem;
        }
        return null;
    }

    public void Remove(GameObject item)
    {
        if (_pool.Contains(item))
        {
            _pool.Remove(item);
        }
    }
}