using UnityEngine;

public class TreeScript : MonoBehaviour
{
    [SerializeField()]
    private GameObject[] _branches;
    [SerializeField()]
    private int _minimumBranchOffset;
    [SerializeField()]
    private int _maximumBranchOffset;

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
        _branches = null;
        _minimumBranchOffset = 3;
        _maximumBranchOffset = 5;
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
        var leftBranch = Random.Range(0, 100) >= 50;
        var nextBranchHeight = Random.Range(_minimumBranchOffset, _maximumBranchOffset);
        for (int heightIndex = 0; heightIndex < _height; heightIndex++)
        {
            var trunkTile = _trunkTiles[Random.Range(0, _trunkTiles.Length)];
            var trunk = Instantiate(trunkTile.Prefab, transform.position + new Vector3(0f, yPosition, 0f), Quaternion.identity, transform);
            if (heightIndex == nextBranchHeight)
            {
                if (leftBranch)
                {
                    var position = trunk.transform.position + new Vector3(-_tileSize.x / 2f, 0f, 0f);
                    Instantiate(_branches[Random.Range(0, _branches.Length)], position, Quaternion.identity, transform);
                }
                else
                {
                    var position = trunk.transform.position + new Vector3(_tileSize.x / 2f, 0f, 0f);
                    Instantiate(_branches[Random.Range(0, _branches.Length)], position, Quaternion.identity, transform).transform.localScale = new Vector3(-1f, 1f, 1f);
                }
                leftBranch = !leftBranch;
                nextBranchHeight += Random.Range(_minimumBranchOffset, _maximumBranchOffset);
            }
            yPosition += (_tileSize.y * trunkTile.Size.y);
        }
    }
}