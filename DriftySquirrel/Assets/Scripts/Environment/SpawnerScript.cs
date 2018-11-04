using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    private enum GenerationType
    {
        None,
        Land,
        Pit,
        Gorge,
        River,
    }

    [SerializeField()]
    private GameObject _zeroTile;
    [SerializeField()]
    private Vector2 _tileSize;
    [SerializeField()]
    private GameObject _landFillLeftTile;
    [SerializeField()]
    private GameObject _landFillTile;
    [SerializeField()]
    private GameObject _landFillRightTile;
    [SerializeField()]
    private GameObject _landSurfaceLeftTile;
    [SerializeField()]
    private GameObject _landSurfaceUp1Tile;
    [SerializeField()]
    private GameObject _landSurfaceUp2Tile;
    [SerializeField()]
    private GameObject _landSurfaceUp3Tile;
    [SerializeField()]
    private GameObject _landSurfaceTile;
    [SerializeField()]
    private GameObject _landSurfaceDown1Tile;
    [SerializeField()]
    private GameObject _landSurfaceDown2Tile;
    [SerializeField()]
    private GameObject _landSurfaceDown3Tile;
    [SerializeField()]
    private GameObject _landSurfaceRightTile;
    [SerializeField()]
    private GameObject _waterFillTile;
    [SerializeField()]
    private GameObject _waterSurfaceTile;
    [SerializeField()]
    private Transform _tilesHolder;

    private Vector3 _zeroPosition;
    private int _globalGenerationIndex;
    private int _currentGenerationIndex;
    private int _lastGenerationLength;
    private int _lastGenerationElevation;
    private GenerationType _lastGenerationType;
    private int _currentGenerationLength;
    private int _currentGenerationElevation;
    private GenerationType _currentGenerationType;
    private int _nextGenerationLength;
    private int _nextGenerationElevation;
    private GenerationType _nextGenerationType;

    public SpawnerScript()
    {
        _zeroTile = null;
        _tileSize = Vector2.zero;
        _landFillLeftTile = null;
        _landFillTile = null;
        _landFillRightTile = null;
        _landSurfaceLeftTile = null;
        _landSurfaceUp1Tile = null;
        _landSurfaceUp2Tile = null;
        _landSurfaceUp3Tile = null;
        _landSurfaceTile = null;
        _landSurfaceDown1Tile = null;
        _landSurfaceDown2Tile = null;
        _landSurfaceDown3Tile = null;
        _landSurfaceRightTile = null;
        _waterFillTile = null;
        _waterSurfaceTile = null;
        _tilesHolder = null;

        _zeroPosition = Vector3.zero;
        _globalGenerationIndex = 0;
        _currentGenerationIndex = 0;
        _lastGenerationLength = 10;
        _lastGenerationElevation = 3;
        _lastGenerationType = GenerationType.Land;
        _currentGenerationLength = 20;
        _currentGenerationElevation = 3;
        _currentGenerationType = GenerationType.Land;
        _nextGenerationLength = 4;
        _nextGenerationElevation = 3;
        _nextGenerationType = GenerationType.River;
    }

    private void Awake()
    {

    }

    private void Update()
    {
        while (transform.position.x - (_zeroPosition.x + _globalGenerationIndex * _tileSize.x) >= _tileSize.x)
        {
            if (_currentGenerationIndex == _currentGenerationLength)
            {
                _lastGenerationLength = _currentGenerationLength;
                _lastGenerationElevation = _currentGenerationElevation;
                _lastGenerationType = _currentGenerationType;
                _currentGenerationLength = _nextGenerationLength;
                _currentGenerationElevation = _nextGenerationElevation;
                _currentGenerationType = _nextGenerationType;
                if (_currentGenerationType == GenerationType.Pit || _currentGenerationType == GenerationType.Gorge || _currentGenerationType == GenerationType.River)
                {
                    _nextGenerationLength = Random.Range(4, 21);
                    _nextGenerationElevation = _lastGenerationElevation;
                    _nextGenerationType = GenerationType.Land;
                }
                else
                {
                    switch (Random.Range(0, 3))
                    {
                        case 0:
                            _nextGenerationLength = Random.Range(4, 21);
                            _nextGenerationElevation = Random.Range(_currentGenerationElevation - 1, _currentGenerationElevation + 2);
                            _nextGenerationElevation = Mathf.Clamp(_nextGenerationElevation, 2, 5);
                            _nextGenerationType = GenerationType.Land;
                            break;
                        case 1:
                            _nextGenerationLength = Random.Range(4, 7);
                            _nextGenerationElevation = _currentGenerationElevation;
                            _nextGenerationType = GenerationType.Pit;
                            break;
                        case 2:
                            _nextGenerationLength = Random.Range(4, 7);
                            _nextGenerationElevation = _currentGenerationElevation;
                            _nextGenerationType = GenerationType.River;
                            break;
                        case 3:
                            _nextGenerationLength = Random.Range(4, 21);
                            _nextGenerationElevation = _currentGenerationElevation;
                            _nextGenerationType = GenerationType.Gorge;
                            break;
                        default:
                            break;
                    }
                }
                _currentGenerationIndex = 0;
            }

            switch (_currentGenerationType)
            {
                case GenerationType.Land:
                    for (int currentElevation = 0; currentElevation < _currentGenerationElevation; currentElevation++)
                    {
                        if (_currentGenerationIndex == 0 && _lastGenerationType != GenerationType.Land)
                        {
                            //Left
                            if (currentElevation == _currentGenerationElevation - 1)
                            {
                                //Surface
                                var tile = Instantiate(_landSurfaceLeftTile, _zeroPosition + new Vector3(_globalGenerationIndex * _tileSize.x, currentElevation * _tileSize.y, 0f), Quaternion.identity, _tilesHolder);
                            }
                            else
                            {
                                //Fill
                                var tile = Instantiate(_landFillLeftTile, _zeroPosition + new Vector3(_globalGenerationIndex * _tileSize.x, currentElevation * _tileSize.y, 0f), Quaternion.identity, _tilesHolder);
                            }
                        }
                        else if (_currentGenerationIndex == _currentGenerationLength - 1 && _nextGenerationType != GenerationType.Land)
                        {
                            //Right
                            if (currentElevation == _currentGenerationElevation - 1)
                            {
                                //Surface
                                var tile = Instantiate(_landSurfaceRightTile, _zeroPosition + new Vector3(_globalGenerationIndex * _tileSize.x, currentElevation * _tileSize.y, 0f), Quaternion.identity, _tilesHolder);
                            }
                            else
                            {
                                //Fill
                                var tile = Instantiate(_landFillRightTile, _zeroPosition + new Vector3(_globalGenerationIndex * _tileSize.x, currentElevation * _tileSize.y, 0f), Quaternion.identity, _tilesHolder);
                            }
                        }
                        else
                        {
                            if (_lastGenerationElevation < _currentGenerationElevation)
                            {
                                //Raise 1
                                if (_currentGenerationIndex == 0)
                                {
                                    if (currentElevation == _currentGenerationElevation - 1)
                                    {
                                        //Surface
                                        var tile = Instantiate(_landSurfaceUp2Tile, _zeroPosition + new Vector3(_globalGenerationIndex * _tileSize.x, currentElevation * _tileSize.y, 0f), Quaternion.identity, _tilesHolder);
                                    }
                                    else if (currentElevation == _currentGenerationElevation - 2)
                                    {
                                        //Fill Surface
                                        var tile = Instantiate(_landSurfaceUp1Tile, _zeroPosition + new Vector3(_globalGenerationIndex * _tileSize.x, currentElevation * _tileSize.y, 0f), Quaternion.identity, _tilesHolder);
                                    }
                                    else if (currentElevation < _currentGenerationElevation - 2)
                                    {
                                        //Fill
                                        var tile = Instantiate(_landFillTile, _zeroPosition + new Vector3(_globalGenerationIndex * _tileSize.x, currentElevation * _tileSize.y, 0f), Quaternion.identity, _tilesHolder);
                                    }
                                }
                                //Raise 2
                                else if (_currentGenerationIndex == 1)
                                {
                                    if (currentElevation == _currentGenerationElevation - 1)
                                    {
                                        //Surface
                                        var tile = Instantiate(_landSurfaceUp3Tile, _zeroPosition + new Vector3(_globalGenerationIndex * _tileSize.x, currentElevation * _tileSize.y, 0f), Quaternion.identity, _tilesHolder);
                                    }
                                    else if (currentElevation < _currentGenerationElevation - 1)
                                    {
                                        //Fill
                                        var tile = Instantiate(_landFillTile, _zeroPosition + new Vector3(_globalGenerationIndex * _tileSize.x, currentElevation * _tileSize.y, 0f), Quaternion.identity, _tilesHolder);
                                    }
                                }
                                else
                                {
                                    if (currentElevation == _currentGenerationElevation - 1)
                                    {
                                        //Surface
                                        var tile = Instantiate(_landSurfaceTile, _zeroPosition + new Vector3(_globalGenerationIndex * _tileSize.x, currentElevation * _tileSize.y, 0f), Quaternion.identity, _tilesHolder);
                                    }
                                    else
                                    {
                                        //Fill
                                        var tile = Instantiate(_landFillTile, _zeroPosition + new Vector3(_globalGenerationIndex * _tileSize.x, currentElevation * _tileSize.y, 0f), Quaternion.identity, _tilesHolder);
                                    }
                                }
                            }
                            else if (_lastGenerationElevation > _currentGenerationElevation)
                            {
                                //Lower 1
                                if (_currentGenerationIndex == 0)
                                {
                                    if (currentElevation == _currentGenerationElevation - 1)
                                    {
                                        //Surface
                                        var tile = Instantiate(_landSurfaceDown1Tile, _zeroPosition + new Vector3(_globalGenerationIndex * _tileSize.x, (currentElevation + 1) * _tileSize.y, 0f), Quaternion.identity, _tilesHolder);
                                        //Fill Surface
                                        tile = Instantiate(_landFillTile, _zeroPosition + new Vector3(_globalGenerationIndex * _tileSize.x, currentElevation * _tileSize.y, 0f), Quaternion.identity, _tilesHolder);
                                    }
                                    else if (currentElevation < _currentGenerationElevation - 1)
                                    {
                                        //Fill
                                        var tile = Instantiate(_landFillTile, _zeroPosition + new Vector3(_globalGenerationIndex * _tileSize.x, currentElevation * _tileSize.y, 0f), Quaternion.identity, _tilesHolder);
                                    }
                                }
                                //Lower 2
                                else if (_currentGenerationIndex == 1)
                                {
                                    if (currentElevation == _currentGenerationElevation - 1)
                                    {
                                        //Surface
                                        var tile = Instantiate(_landSurfaceDown2Tile, _zeroPosition + new Vector3(_globalGenerationIndex * _tileSize.x, (currentElevation + 1) * _tileSize.y, 0f), Quaternion.identity, _tilesHolder);
                                        tile = Instantiate(_landSurfaceDown3Tile, _zeroPosition + new Vector3(_globalGenerationIndex * _tileSize.x, currentElevation * _tileSize.y, 0f), Quaternion.identity, _tilesHolder);
                                    }
                                    else if (currentElevation < _currentGenerationElevation - 1)
                                    {
                                        //Fill
                                        var tile = Instantiate(_landFillTile, _zeroPosition + new Vector3(_globalGenerationIndex * _tileSize.x, currentElevation * _tileSize.y, 0f), Quaternion.identity, _tilesHolder);
                                    }
                                }
                                else
                                {
                                    if (currentElevation == _currentGenerationElevation - 1)
                                    {
                                        //Surface
                                        var tile = Instantiate(_landSurfaceTile, _zeroPosition + new Vector3(_globalGenerationIndex * _tileSize.x, currentElevation * _tileSize.y, 0f), Quaternion.identity, _tilesHolder);
                                    }
                                    else
                                    {
                                        //Fill
                                        var tile = Instantiate(_landFillTile, _zeroPosition + new Vector3(_globalGenerationIndex * _tileSize.x, currentElevation * _tileSize.y, 0f), Quaternion.identity, _tilesHolder);
                                    }
                                }
                            }
                            else
                            {
                                //Middle
                                if (currentElevation == _currentGenerationElevation - 1)
                                {
                                    //Surface
                                    var tile = Instantiate(_landSurfaceTile, _zeroPosition + new Vector3(_globalGenerationIndex * _tileSize.x, currentElevation * _tileSize.y, 0f), Quaternion.identity, _tilesHolder);
                                }
                                else
                                {
                                    //Fill
                                    var tile = Instantiate(_landFillTile, _zeroPosition + new Vector3(_globalGenerationIndex * _tileSize.x, currentElevation * _tileSize.y, 0f), Quaternion.identity, _tilesHolder);
                                }
                            }
                        }

                    }
                    _globalGenerationIndex++;
                    _currentGenerationIndex++;
                    break;
                case GenerationType.Pit:

                    _globalGenerationIndex++;
                    _currentGenerationIndex++;
                    break;
                case GenerationType.Gorge:
                    _globalGenerationIndex++;
                    _currentGenerationIndex++;
                    break;
                case GenerationType.River:
                    for (int currentElevation = 0; currentElevation < _currentGenerationElevation; currentElevation++)
                    {
                        if (currentElevation == _currentGenerationElevation - 1)
                        {
                            //Surface
                            var tile = Instantiate(_waterSurfaceTile, _zeroPosition + new Vector3(_globalGenerationIndex * _tileSize.x, currentElevation * _tileSize.y, 0f), Quaternion.identity, _tilesHolder);
                        }
                        else
                        {
                            //Fill
                            var tile = Instantiate(_waterFillTile, _zeroPosition + new Vector3(_globalGenerationIndex * _tileSize.x, currentElevation * _tileSize.y, 0f), Quaternion.identity, _tilesHolder);
                        }
                    }
                    _globalGenerationIndex++;
                    _currentGenerationIndex++;
                    break;
                default:
                    break;
            }
        }
    }

    private void Start()
    {
        _zeroPosition = _zeroTile.transform.position;
        Destroy(_zeroTile);
    }
}