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

    [SerializeField()]
    private GameObject _zeroTile;
    [SerializeField()]
    private Vector2 _tileSize;
    [SerializeField()]
    private Pooler _landFillLeftTile;
    [SerializeField()]
    private Pooler _landFillTile;
    [SerializeField()]
    private Pooler _landFillRightTile;
    [SerializeField()]
    private Pooler _landSurfaceLeftTile;
    [SerializeField()]
    private Pooler _landSurfaceUp2251Tile;
    [SerializeField()]
    private Pooler _landSurfaceUp2252Tile;
    [SerializeField()]
    private Pooler _landSurfaceUp45225Tile;
    [SerializeField()]
    private Pooler _landSurfaceTile;
    [SerializeField()]
    private Pooler _landSurfaceDown2251Tile;
    [SerializeField()]
    private Pooler _landSurfaceDown2252Tile;
    [SerializeField()]
    private Pooler _landSurfaceDown45225Tile;
    [SerializeField()]
    private Pooler _landSurfaceRightTile;
    [SerializeField()]
    private Pooler _waterFillTile;
    [SerializeField()]
    private Pooler _waterSurfaceTile;
    [SerializeField()]
    private Pooler _animatedDriftNutTile;
    [SerializeField()]
    private Pooler _animatedSpikesTile;
    [SerializeField()]
    private Pooler _animatedCollectibleTile;
    [SerializeField()]
    private Pooler _animatedChestTile;
    [SerializeField()]
    private Pooler _treeTrunkTile;
    [SerializeField()]
    private Pooler _canopy1LeftTile;
    [SerializeField()]
    private Pooler _canopy1RightTile;

    [SerializeField()]
    private Transform _tilesHolder;
    [SerializeField()]
    private int _pitSpikesChance;
    [SerializeField()]
    private int _pitCollectibleChance;
    [SerializeField()]
    private int _pitChestChance;
    [SerializeField()]
    private int _nextCollectibleChance;
    [SerializeField()]
    private int _minimumBranchOffset;
    [SerializeField()]
    private int _maximumBranchOffset;

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
    private bool _pitSpikes;
    private bool _pitCollectible;
    private bool _pitChest;
    private int _nextTreeIndex;

    public SpawnerScript()
    {
        _zeroTile = null;
        _tileSize = Vector2.zero;
        _landFillLeftTile = null;
        _landFillTile = null;
        _landFillRightTile = null;
        _landSurfaceLeftTile = null;
        _landSurfaceUp2251Tile = null;
        _landSurfaceUp2252Tile = null;
        _landSurfaceUp45225Tile = null;
        _landSurfaceTile = null;
        _landSurfaceDown2251Tile = null;
        _landSurfaceDown2252Tile = null;
        _landSurfaceDown45225Tile = null;
        _landSurfaceRightTile = null;
        _waterFillTile = null;
        _waterSurfaceTile = null;
        _animatedDriftNutTile = null;
        _animatedSpikesTile = null;
        _animatedCollectibleTile = null;
        _animatedChestTile = null;
        _treeTrunkTile = null;
        _canopy1LeftTile = null;
        _canopy1RightTile = null;

        _tilesHolder = null;
        _pitSpikesChance = 40;
        _pitCollectibleChance = 70;
        _pitChestChance = 10;
        _nextCollectibleChance = 100;
        _minimumBranchOffset = 3;
        _maximumBranchOffset = 4;

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
        _pitSpikes = false;
        _pitCollectible = false;
        _pitChest = false;
        _nextTreeIndex = 20;
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
                            _nextGenerationElevation = Mathf.Clamp(_nextGenerationElevation, 2, 4);
                            _nextGenerationType = GenerationType.Land;
                            break;
                        case 1:
                            _nextGenerationLength = Random.Range(2, 5);
                            _nextGenerationElevation = _currentGenerationElevation;
                            _nextGenerationType = GenerationType.Pit;
                            _pitSpikes = Random.Range(0, 101) <= _pitSpikesChance;
                            _pitCollectible = Random.Range(0, 101) <= _pitCollectibleChance;
                            _pitChest = Random.Range(0, 101) <= _pitChestChance;
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
            if (_globalGenerationIndex == _nextTreeIndex)
            {
                if (_currentGenerationType == GenerationType.Land || _currentGenerationType == GenerationType.Gorge)
                {
                    var leftBranch = (Random.Range(0, 101) <= 50);
                    var nextBranchHeightIndex = Random.Range(_minimumBranchOffset, _maximumBranchOffset);

                    for (int treeHeightIndex = 0; treeHeightIndex < 12; treeHeightIndex++)
                    {
                        _treeTrunkTile.Next(GridPosition(_globalGenerationIndex, treeHeightIndex), _tilesHolder);
                        if (treeHeightIndex == nextBranchHeightIndex)
                        {
                            var branchSize = 1; //Random.Range(1, 4);
                            if (leftBranch && branchSize == 1)
                            {
                                SpawnCollectible(GridPosition(_globalGenerationIndex - 1, treeHeightIndex + 1));
                                _canopy1LeftTile.Next(GridPosition(_globalGenerationIndex - 1, treeHeightIndex), _tilesHolder);
                            }
                            else if (!leftBranch && branchSize == 1)
                            {
                                SpawnCollectible(GridPosition(_globalGenerationIndex + 1, treeHeightIndex + 1));
                                _canopy1RightTile.Next(GridPosition(_globalGenerationIndex + 1, treeHeightIndex), _tilesHolder);
                            }
                            else if (leftBranch && branchSize == 2)
                            {
                                SpawnCollectible(GridPosition(_globalGenerationIndex - 1, treeHeightIndex + 1));
                                _canopy1LeftTile.Next(GridPosition(_globalGenerationIndex - 1, treeHeightIndex), _tilesHolder);
                            }
                            else if (!leftBranch && branchSize == 2)
                            {
                                SpawnCollectible(GridPosition(_globalGenerationIndex + 1, treeHeightIndex + 1));
                                _canopy1RightTile.Next(GridPosition(_globalGenerationIndex + 1, treeHeightIndex), _tilesHolder);
                            }
                            //TODO: Finish Branches
                            leftBranch = !leftBranch;
                            nextBranchHeightIndex += Random.Range(_minimumBranchOffset, _maximumBranchOffset);
                        }
                    }
                }
                _nextTreeIndex += Random.Range(6, 9);
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
                                _landSurfaceLeftTile.Next(GridPosition(_globalGenerationIndex, currentElevation), _tilesHolder);
                            }
                            else
                            {
                                _landFillLeftTile.Next(GridPosition(_globalGenerationIndex, currentElevation), _tilesHolder);
                            }
                        }
                        else if (_currentGenerationIndex == _currentGenerationLength - 1 && _nextGenerationType != GenerationType.Land)
                        {
                            if (currentElevation == _currentGenerationElevation - 1)
                            {
                                _landSurfaceRightTile.Next(GridPosition(_globalGenerationIndex, currentElevation), _tilesHolder);
                            }
                            else
                            {
                                _landFillRightTile.Next(GridPosition(_globalGenerationIndex, currentElevation), _tilesHolder);
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
                                        _landSurfaceUp2251Tile.Next(GridPosition(_globalGenerationIndex, currentElevation), _tilesHolder);
                                    }
                                    else if (currentElevation == _currentGenerationElevation - 2)
                                    {
                                        _landSurfaceUp45225Tile.Next(GridPosition(_globalGenerationIndex, currentElevation), _tilesHolder);
                                    }
                                    else if (currentElevation < _currentGenerationElevation - 2)
                                    {
                                        _landFillTile.Next(GridPosition(_globalGenerationIndex, currentElevation), _tilesHolder);
                                    }
                                }
                                else if (_currentGenerationIndex == 1)
                                {
                                    if (currentElevation == _currentGenerationElevation - 1)
                                    {
                                        _landSurfaceUp2252Tile.Next(GridPosition(_globalGenerationIndex, currentElevation), _tilesHolder);
                                    }
                                    else if (currentElevation < _currentGenerationElevation - 1)
                                    {
                                        _landFillTile.Next(GridPosition(_globalGenerationIndex, currentElevation), _tilesHolder);
                                    }
                                }
                                else
                                {
                                    if (currentElevation == _currentGenerationElevation - 1)
                                    {
                                        _landSurfaceTile.Next(GridPosition(_globalGenerationIndex, currentElevation), _tilesHolder);
                                    }
                                    else
                                    {
                                        _landFillTile.Next(GridPosition(_globalGenerationIndex, currentElevation), _tilesHolder);
                                    }
                                }
                            }
                            else if (_lastGenerationElevation > _currentGenerationElevation)
                            {
                                if (_currentGenerationIndex == 0)
                                {
                                    if (currentElevation == _currentGenerationElevation - 1)
                                    {
                                        _landSurfaceDown2251Tile.Next(GridPosition(_globalGenerationIndex, currentElevation + 1), _tilesHolder);
                                        _landFillTile.Next(GridPosition(_globalGenerationIndex, currentElevation), _tilesHolder);
                                    }
                                    else if (currentElevation < _currentGenerationElevation - 1)
                                    {
                                        _landFillTile.Next(GridPosition(_globalGenerationIndex, currentElevation), _tilesHolder);
                                    }
                                }
                                else if (_currentGenerationIndex == 1)
                                {
                                    if (currentElevation == _currentGenerationElevation - 1)
                                    {
                                        _landSurfaceDown2252Tile.Next(GridPosition(_globalGenerationIndex, currentElevation + 1), _tilesHolder);
                                        _landSurfaceDown45225Tile.Next(GridPosition(_globalGenerationIndex, currentElevation), _tilesHolder);
                                    }
                                    else if (currentElevation < _currentGenerationElevation - 1)
                                    {
                                        _landFillTile.Next(GridPosition(_globalGenerationIndex, currentElevation), _tilesHolder);
                                    }
                                }
                                else
                                {
                                    if (currentElevation == _currentGenerationElevation - 1)
                                    {
                                        _landSurfaceTile.Next(GridPosition(_globalGenerationIndex, currentElevation), _tilesHolder);
                                    }
                                    else
                                    {
                                        _landFillTile.Next(GridPosition(_globalGenerationIndex, currentElevation), _tilesHolder);
                                    }
                                }
                            }
                            else
                            {
                                if (currentElevation == _currentGenerationElevation - 1)
                                {
                                    _landSurfaceTile.Next(GridPosition(_globalGenerationIndex, currentElevation), _tilesHolder);
                                }
                                else
                                {
                                    _landFillTile.Next(GridPosition(_globalGenerationIndex, currentElevation), _tilesHolder);
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
                            _landFillTile.Next(GridPosition(_globalGenerationIndex - 1, currentElevation), _tilesHolder);
                        }
                        if (currentElevation == _currentGenerationElevation - 1)
                        {
                            if (_pitSpikes)
                            {
                                _animatedSpikesTile.Next(GridPosition(_globalGenerationIndex, currentElevation), _tilesHolder);
                            }
                            else if (_pitCollectible)
                            {
                                SpawnCollectible(GridPosition(_globalGenerationIndex, currentElevation));
                            }
                            else if (_pitChest && _currentGenerationIndex == 1)
                            {
                                _animatedChestTile.Next(GridPosition(_globalGenerationIndex, currentElevation), _tilesHolder);
                            }
                        }
                        else if (currentElevation == _currentGenerationElevation - 2)
                        {
                            _landSurfaceTile.Next(GridPosition(_globalGenerationIndex, currentElevation), _tilesHolder);
                        }
                        else if (currentElevation < _currentGenerationElevation - 2)
                        {
                            _landFillTile.Next(GridPosition(_globalGenerationIndex, currentElevation), _tilesHolder);
                        }
                        if (currentElevation != _currentGenerationElevation - 1 && _currentGenerationIndex == _currentGenerationLength - 1)
                        {
                            _landFillTile.Next(GridPosition(_globalGenerationIndex + 1, currentElevation), _tilesHolder);
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
                            _waterSurfaceTile.Next(GridPosition(_globalGenerationIndex, currentElevation), _tilesHolder);
                        }
                        else
                        {
                            _waterFillTile.Next(GridPosition(_globalGenerationIndex, currentElevation), _tilesHolder);
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
        _nextTreeIndex += Random.Range(6, 9);
    }

    private Vector3 GridPosition(int distance, int elevation)
    {
        return _zeroPosition + new Vector3(distance * _tileSize.x, elevation * _tileSize.y, 0f);
    }

    private void SpawnCollectible(Vector3 position)
    {
        if (Random.Range(0, 101) <= _nextCollectibleChance)
        {
            _animatedCollectibleTile.Next(position, _tilesHolder);
        }
    }
}