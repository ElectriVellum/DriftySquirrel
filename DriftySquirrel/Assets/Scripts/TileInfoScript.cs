using System;
using UnityEngine;

[Serializable()]
public class TileInfoScript
{
    [SerializeField()]
    private GameObject _prefab;
    [SerializeField()]
    private Vector2Int _size;

    public GameObject Prefab
    {
        get
        {
            return _prefab;
        }
    }

    public Vector2Int Size
    {
        get
        {
            return _size;
        }
    }

    public TileInfoScript()
    {
        _prefab = null;
        _size = Vector2Int.zero;
    }
}