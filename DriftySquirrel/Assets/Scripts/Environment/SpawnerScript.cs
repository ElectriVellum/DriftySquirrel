using System.Collections.Generic;
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

    private List<GameObject> _landFillLeftTiles;
    private List<GameObject> _landFillTiles;
    private List<GameObject> _landFillRightTiles;
    private List<GameObject> _landSurfaceLeftTiles;
    private List<GameObject> _landSurfaceUp1Tiles;
    private List<GameObject> _landSurfaceUp2Tiles;
    private List<GameObject> _landSurfaceUp3Tiles;
    private List<GameObject> _landSurfaceTiles;
    private List<GameObject> _landSurfaceDown1Tiles;
    private List<GameObject> _landSurfaceDown2Tiles;
    private List<GameObject> _landSurfaceDown3Tiles;
    private List<GameObject> _landSurfaceRightTiles;
    private List<GameObject> _waterFillTiles;
    private List<GameObject> _waterSurfaceTiles;
    private List<GameObject> _animatedSpikesTiles;

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
    private GameObject _animatedSpikesTile;
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
        _landFillLeftTiles = new List<GameObject>();
        _landFillTiles = new List<GameObject>();
        _landFillRightTiles = new List<GameObject>();
        _landSurfaceLeftTiles = new List<GameObject>();
        _landSurfaceUp1Tiles = new List<GameObject>();
        _landSurfaceUp2Tiles = new List<GameObject>();
        _landSurfaceUp3Tiles = new List<GameObject>();
        _landSurfaceTiles = new List<GameObject>();
        _landSurfaceDown1Tiles = new List<GameObject>();
        _landSurfaceDown2Tiles = new List<GameObject>();
        _landSurfaceDown3Tiles = new List<GameObject>();
        _landSurfaceRightTiles = new List<GameObject>();
        _waterFillTiles = new List<GameObject>();
        _waterSurfaceTiles = new List<GameObject>();
        _animatedSpikesTiles = new List<GameObject>();

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
        _animatedSpikesTile = null;
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
                    switch (Random.Range(0, 4))
                    {
                        case 0:
                            _nextGenerationLength = Random.Range(4, 31);
                            _nextGenerationElevation = Random.Range(_currentGenerationElevation - 1, _currentGenerationElevation + 2);
                            _nextGenerationElevation = Mathf.Clamp(_nextGenerationElevation, 2, 5);
                            _nextGenerationType = GenerationType.Land;
                            break;
                        case 1:
                            _nextGenerationLength = Random.Range(2, 5);
                            _nextGenerationElevation = _currentGenerationElevation;
                            _nextGenerationType = GenerationType.Pit;
                            break;
                        case 2:
                            _nextGenerationLength = Random.Range(4, 9);
                            _nextGenerationElevation = _currentGenerationElevation;
                            _nextGenerationType = GenerationType.River;
                            break;
                        case 3:
                            _nextGenerationLength = Random.Range(4, 41);
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
                            if (currentElevation == _currentGenerationElevation - 1)
                            {
                                NextLandSurfaceLeftTile(_zeroPosition + new Vector3(_globalGenerationIndex * _tileSize.x, currentElevation * _tileSize.y, 0f));
                            }
                            else
                            {
                                NextLandFillLeftTile(_zeroPosition + new Vector3(_globalGenerationIndex * _tileSize.x, currentElevation * _tileSize.y, 0f));
                            }
                        }
                        else if (_currentGenerationIndex == _currentGenerationLength - 1 && _nextGenerationType != GenerationType.Land)
                        {
                            if (currentElevation == _currentGenerationElevation - 1)
                            {
                                NextLandSurfaceRightTile(_zeroPosition + new Vector3(_globalGenerationIndex * _tileSize.x, currentElevation * _tileSize.y, 0f));
                            }
                            else
                            {
                                NextLandFillRightTile(_zeroPosition + new Vector3(_globalGenerationIndex * _tileSize.x, currentElevation * _tileSize.y, 0f));
                            }
                        }
                        else
                        {
                            if (_lastGenerationElevation < _currentGenerationElevation)
                            {
                                if (_currentGenerationIndex == 0)
                                {
                                    if (currentElevation == _currentGenerationElevation - 1)
                                    {
                                        NextLandSurfaceUp2Tile(_zeroPosition + new Vector3(_globalGenerationIndex * _tileSize.x, currentElevation * _tileSize.y, 0f));
                                    }
                                    else if (currentElevation == _currentGenerationElevation - 2)
                                    {
                                        NextLandSurfaceUp1Tile(_zeroPosition + new Vector3(_globalGenerationIndex * _tileSize.x, currentElevation * _tileSize.y, 0f));
                                    }
                                    else if (currentElevation < _currentGenerationElevation - 2)
                                    {
                                        NextLandFillTile(_zeroPosition + new Vector3(_globalGenerationIndex * _tileSize.x, currentElevation * _tileSize.y, 0f));
                                    }
                                }
                                else if (_currentGenerationIndex == 1)
                                {
                                    if (currentElevation == _currentGenerationElevation - 1)
                                    {
                                        NextLandSurfaceUp3Tile(_zeroPosition + new Vector3(_globalGenerationIndex * _tileSize.x, currentElevation * _tileSize.y, 0f));
                                    }
                                    else if (currentElevation < _currentGenerationElevation - 1)
                                    {
                                        NextLandFillTile(_zeroPosition + new Vector3(_globalGenerationIndex * _tileSize.x, currentElevation * _tileSize.y, 0f));
                                    }
                                }
                                else
                                {
                                    if (currentElevation == _currentGenerationElevation - 1)
                                    {
                                        NextLandSurfaceTile(_zeroPosition + new Vector3(_globalGenerationIndex * _tileSize.x, currentElevation * _tileSize.y, 0f));
                                    }
                                    else
                                    {
                                        NextLandFillTile(_zeroPosition + new Vector3(_globalGenerationIndex * _tileSize.x, currentElevation * _tileSize.y, 0f));
                                    }
                                }
                            }
                            else if (_lastGenerationElevation > _currentGenerationElevation)
                            {
                                if (_currentGenerationIndex == 0)
                                {
                                    if (currentElevation == _currentGenerationElevation - 1)
                                    {
                                        NextLandSurfaceDown1Tile(_zeroPosition + new Vector3(_globalGenerationIndex * _tileSize.x, (currentElevation + 1) * _tileSize.y, 0f));
                                        NextLandFillTile(_zeroPosition + new Vector3(_globalGenerationIndex * _tileSize.x, currentElevation * _tileSize.y, 0f));
                                    }
                                    else if (currentElevation < _currentGenerationElevation - 1)
                                    {
                                        NextLandFillTile(_zeroPosition + new Vector3(_globalGenerationIndex * _tileSize.x, currentElevation * _tileSize.y, 0f));
                                    }
                                }
                                else if (_currentGenerationIndex == 1)
                                {
                                    if (currentElevation == _currentGenerationElevation - 1)
                                    {
                                        NextLandSurfaceDown2Tile(_zeroPosition + new Vector3(_globalGenerationIndex * _tileSize.x, (currentElevation + 1) * _tileSize.y, 0f));
                                        NextLandSurfaceDown3Tile(_zeroPosition + new Vector3(_globalGenerationIndex * _tileSize.x, currentElevation * _tileSize.y, 0f));
                                    }
                                    else if (currentElevation < _currentGenerationElevation - 1)
                                    {
                                        NextLandFillTile(_zeroPosition + new Vector3(_globalGenerationIndex * _tileSize.x, currentElevation * _tileSize.y, 0f));
                                    }
                                }
                                else
                                {
                                    if (currentElevation == _currentGenerationElevation - 1)
                                    {
                                        NextLandSurfaceTile(_zeroPosition + new Vector3(_globalGenerationIndex * _tileSize.x, currentElevation * _tileSize.y, 0f));
                                    }
                                    else
                                    {
                                        NextLandFillTile(_zeroPosition + new Vector3(_globalGenerationIndex * _tileSize.x, currentElevation * _tileSize.y, 0f));
                                    }
                                }
                            }
                            else
                            {
                                if (currentElevation == _currentGenerationElevation - 1)
                                {
                                    NextLandSurfaceTile(_zeroPosition + new Vector3(_globalGenerationIndex * _tileSize.x, currentElevation * _tileSize.y, 0f));
                                }
                                else
                                {
                                    NextLandFillTile(_zeroPosition + new Vector3(_globalGenerationIndex * _tileSize.x, currentElevation * _tileSize.y, 0f));
                                }
                            }
                        }

                    }
                    _globalGenerationIndex++;
                    _currentGenerationIndex++;
                    break;
                case GenerationType.Pit:
                    for (int currentElevation = 0; currentElevation < _currentGenerationElevation; currentElevation++)
                    {
                        if (currentElevation != _currentGenerationElevation - 1 && _currentGenerationIndex == 0)
                        {
                            NextLandFillTile(_zeroPosition + new Vector3((_globalGenerationIndex * _tileSize.x) - _tileSize.x, currentElevation * _tileSize.y, 0f));
                        }
                        if (currentElevation == _currentGenerationElevation - 1)
                        {
                            if (Random.Range(0,11) > 6)
                            {
                                NextAnimatedSpikesTile(_zeroPosition + new Vector3(_globalGenerationIndex * _tileSize.x, currentElevation * _tileSize.y, 0f));
                            }
                        }
                        if (currentElevation == _currentGenerationElevation - 2)
                        {
                            NextLandSurfaceTile(_zeroPosition + new Vector3(_globalGenerationIndex * _tileSize.x, currentElevation * _tileSize.y, 0f));
                        }
                        else if (currentElevation < _currentGenerationElevation - 2)
                        {
                            NextLandFillTile(_zeroPosition + new Vector3(_globalGenerationIndex * _tileSize.x, currentElevation * _tileSize.y, 0f));
                        }
                        if (currentElevation != _currentGenerationElevation - 1 && _currentGenerationIndex == _currentGenerationLength - 1)
                        {
                            NextLandFillTile(_zeroPosition + new Vector3((_globalGenerationIndex * _tileSize.x) + _tileSize.x, currentElevation * _tileSize.y, 0f));
                        }
                    }
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
                            NextWaterSurfaceTile(_zeroPosition + new Vector3(_globalGenerationIndex * _tileSize.x, currentElevation * _tileSize.y, 0f));
                        }
                        else
                        {
                            NextWaterFillTile(_zeroPosition + new Vector3(_globalGenerationIndex * _tileSize.x, currentElevation * _tileSize.y, 0f));
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

    private GameObject NextLandFillLeftTile(Vector3 position)
    {
        foreach (var tile in _landFillLeftTiles)
        {
            if (!tile.activeSelf)
            {
                tile.transform.position = position;
                tile.SetActive(true);
                return tile;
            }
        }
        var newTile = Instantiate(_landFillLeftTile, position, Quaternion.identity, _tilesHolder);
        _landFillLeftTiles.Add(newTile);
        return newTile;
    }

    private GameObject NextLandFillTile(Vector3 position)
    {
        foreach (var tile in _landFillTiles)
        {
            if (!tile.activeSelf)
            {
                tile.transform.position = position;
                tile.SetActive(true);
                return tile;
            }
        }
        var newTile = Instantiate(_landFillTile, position, Quaternion.identity, _tilesHolder);
        _landFillTiles.Add(newTile);
        return newTile;
    }

    private GameObject NextLandFillRightTile(Vector3 position)
    {
        foreach (var tile in _landFillRightTiles)
        {
            if (!tile.activeSelf)
            {
                tile.transform.position = position;
                tile.SetActive(true);
                return tile;
            }
        }
        var newTile = Instantiate(_landFillRightTile, position, Quaternion.identity, _tilesHolder);
        _landFillRightTiles.Add(newTile);
        return newTile;
    }

    private GameObject NextLandSurfaceLeftTile(Vector3 position)
    {
        foreach (var tile in _landSurfaceLeftTiles)
        {
            if (!tile.activeSelf)
            {
                tile.transform.position = position;
                tile.SetActive(true);
                return tile;
            }
        }
        var newTile = Instantiate(_landSurfaceLeftTile, position, Quaternion.identity, _tilesHolder);
        _landSurfaceLeftTiles.Add(newTile);
        return newTile;
    }

    private GameObject NextLandSurfaceUp1Tile(Vector3 position)
    {
        foreach (var tile in _landSurfaceUp1Tiles)
        {
            if (!tile.activeSelf)
            {
                tile.transform.position = position;
                tile.SetActive(true);
                return tile;
            }
        }
        var newTile = Instantiate(_landSurfaceUp1Tile, position, Quaternion.identity, _tilesHolder);
        _landSurfaceUp1Tiles.Add(newTile);
        return newTile;
    }

    private GameObject NextLandSurfaceUp2Tile(Vector3 position)
    {
        foreach (var tile in _landSurfaceUp2Tiles)
        {
            if (!tile.activeSelf)
            {
                tile.transform.position = position;
                tile.SetActive(true);
                return tile;
            }
        }
        var newTile = Instantiate(_landSurfaceUp2Tile, position, Quaternion.identity, _tilesHolder);
        _landSurfaceUp2Tiles.Add(newTile);
        return newTile;
    }

    private GameObject NextLandSurfaceUp3Tile(Vector3 position)
    {
        foreach (var tile in _landSurfaceUp3Tiles)
        {
            if (!tile.activeSelf)
            {
                tile.transform.position = position;
                tile.SetActive(true);
                return tile;
            }
        }
        var newTile = Instantiate(_landSurfaceUp3Tile, position, Quaternion.identity, _tilesHolder);
        _landSurfaceUp3Tiles.Add(newTile);
        return newTile;
    }

    private GameObject NextLandSurfaceTile(Vector3 position)
    {
        foreach (var tile in _landSurfaceTiles)
        {
            if (!tile.activeSelf)
            {
                tile.transform.position = position;
                tile.SetActive(true);
                return tile;
            }
        }
        var newTile = Instantiate(_landSurfaceTile, position, Quaternion.identity, _tilesHolder);
        _landSurfaceTiles.Add(newTile);
        return newTile;
    }

    private GameObject NextLandSurfaceDown1Tile(Vector3 position)
    {
        foreach (var tile in _landSurfaceDown1Tiles)
        {
            if (!tile.activeSelf)
            {
                tile.transform.position = position;
                tile.SetActive(true);
                return tile;
            }
        }
        var newTile = Instantiate(_landSurfaceDown1Tile, position, Quaternion.identity, _tilesHolder);
        _landSurfaceDown1Tiles.Add(newTile);
        return newTile;
    }

    private GameObject NextLandSurfaceDown2Tile(Vector3 position)
    {
        foreach (var tile in _landSurfaceDown2Tiles)
        {
            if (!tile.activeSelf)
            {
                tile.transform.position = position;
                tile.SetActive(true);
                return tile;
            }
        }
        var newTile = Instantiate(_landSurfaceDown2Tile, position, Quaternion.identity, _tilesHolder);
        _landSurfaceDown2Tiles.Add(newTile);
        return newTile;
    }

    private GameObject NextLandSurfaceDown3Tile(Vector3 position)
    {
        foreach (var tile in _landSurfaceDown3Tiles)
        {
            if (!tile.activeSelf)
            {
                tile.transform.position = position;
                tile.SetActive(true);
                return tile;
            }
        }
        var newTile = Instantiate(_landSurfaceDown3Tile, position, Quaternion.identity, _tilesHolder);
        _landSurfaceDown3Tiles.Add(newTile);
        return newTile;
    }

    private GameObject NextLandSurfaceRightTile(Vector3 position)
    {
        foreach (var tile in _landSurfaceRightTiles)
        {
            if (!tile.activeSelf)
            {
                tile.transform.position = position;
                tile.SetActive(true);
                return tile;
            }
        }
        var newTile = Instantiate(_landSurfaceRightTile, position, Quaternion.identity, _tilesHolder);
        _landSurfaceRightTiles.Add(newTile);
        return newTile;
    }

    private GameObject NextWaterFillTile(Vector3 position)
    {
        foreach (var tile in _waterFillTiles)
        {
            if (!tile.activeSelf)
            {
                tile.transform.position = position;
                tile.SetActive(true);
                return tile;
            }
        }
        var newTile = Instantiate(_waterFillTile, position, Quaternion.identity, _tilesHolder);
        _waterFillTiles.Add(newTile);
        return newTile;
    }

    private GameObject NextWaterSurfaceTile(Vector3 position)
    {
        foreach (var tile in _waterSurfaceTiles)
        {
            if (!tile.activeSelf)
            {
                tile.transform.position = position;
                tile.SetActive(true);
                return tile;
            }
        }
        var newTile = Instantiate(_waterSurfaceTile, position, Quaternion.identity, _tilesHolder);
        _waterSurfaceTiles.Add(newTile);
        return newTile;
    }

    private GameObject NextAnimatedSpikesTile(Vector3 position)
    {
        foreach (var tile in _animatedSpikesTiles)
        {
            if (!tile.activeSelf)
            {
                tile.transform.position = position;
                tile.SetActive(true);
                return tile;
            }
        }
        var newTile = Instantiate(_animatedSpikesTile, position, Quaternion.identity, _tilesHolder);
        _animatedSpikesTiles.Add(newTile);
        return newTile;
    }
}