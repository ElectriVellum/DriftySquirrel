using UnityEngine;

public class CollectibleScript : MonoBehaviour
{
    [SerializeField()]
    private int _acornsCount;
    [SerializeField()]
    private int _scoreCount;
    [SerializeField()]
    private bool _driftNut;
    [SerializeField()]
    private bool _powerLeaf;

    public CollectibleScript()
    {
        _acornsCount = 0;
        _scoreCount = 0;
        _driftNut = false;
        _powerLeaf = false;
    }

    public int AcornsCount
    {
        get
        {
            return _acornsCount;
        }
    }

    public int ScoreCount
    {
        get
        {
            return _acornsCount;
        }
    }

    public bool DriftNut
    {
        get
        {
            return _driftNut;
        }
    }

    public bool PowerLeaf
    {
        get
        {
            return _powerLeaf;
        }
    }
}