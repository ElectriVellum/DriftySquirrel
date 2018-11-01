using UnityEngine;

public class TreeScript : MonoBehaviour
{
    [SerializeField()]
    private GameObject[] _branches;
    [SerializeField()]
    private int _minimumLeftBranch1Y;
    [SerializeField()]
    private int _maximumLeftBranch1Y;
    [SerializeField()]
    private int _minimumLeftBranch2YOffset;
    [SerializeField()]
    private int _maximumLeftBranch2YOffset;

    [SerializeField()]
    private float _minimumXOffset;
    [SerializeField()]
    private float _maximumXOffset;
    [SerializeField()]
    private float _minimumYOffset;
    [SerializeField()]
    private float _maximumYOffset;
    [SerializeField()]
    private Vector2 _tileSize;
    [SerializeField()]
    private int _height;
    [SerializeField()]
    private TileInfoScript[] _trunkTiles;

    public TreeScript()
    {
        _minimumXOffset = 0f;
        _maximumXOffset = 0f;
        _minimumYOffset = -1f;
        _maximumYOffset = 1f;
        _tileSize = Vector2.zero;
        _height = 22;
        _trunkTiles = null;
    }

    public void Generate()
    {
        transform.position = new Vector3(transform.position.x + Random.Range(_minimumXOffset, _maximumXOffset), transform.position.y + Random.Range(_minimumYOffset, _maximumYOffset), 0f);
        var yPosition = 0f;
        var branch1Height = Random.Range(_minimumLeftBranch1Y, _maximumLeftBranch1Y);
        var branch2Height = branch1Height + Random.Range(_minimumLeftBranch2YOffset, _maximumLeftBranch2YOffset);
        var branch3Height = branch1Height + (branch1Height - branch2Height / 2);
        var firstLeft = Random.Range(0, 100) >= 50;
        for (int heightIndex = 0; heightIndex < _height; heightIndex++)
        {
            var trunkTile = _trunkTiles[Random.Range(0, _trunkTiles.Length)];
            var trunk = Instantiate(trunkTile.Prefab, transform.position + new Vector3(0f, yPosition, 0f), Quaternion.identity, transform);
            if (heightIndex == branch1Height || heightIndex == branch2Height || heightIndex == branch3Height)
            {
                if ((firstLeft && (heightIndex == branch1Height || heightIndex == branch3Height)) || (!firstLeft && heightIndex == branch2Height))
                {
                    var position = trunk.transform.position + new Vector3(-_tileSize.x / 2f, _tileSize.y / 2f, 0f);
                    Instantiate(_branches[Random.Range(0, _branches.Length)], position, Quaternion.identity, transform);
                }
                else if ((!firstLeft && (heightIndex == branch1Height || heightIndex == branch3Height)) || (firstLeft && heightIndex == branch2Height))
                {
                    var position = trunk.transform.position + new Vector3(_tileSize.x / 2f, _tileSize.y / 2f, 0f);
                    Instantiate(_branches[Random.Range(0,_branches.Length)], position, Quaternion.identity, transform).transform.localScale = new Vector3(-1f,1f,1f);
                }
            }
            yPosition += (_tileSize.y * trunkTile.Size.y);
        }
    }
}