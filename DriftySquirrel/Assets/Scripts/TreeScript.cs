using UnityEngine;

public class TreeScript : MonoBehaviour
{
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
        for (int heightIndex = 0; heightIndex < _height; heightIndex++)
        {
            var trunkTile = _trunkTiles[Random.Range(0, _trunkTiles.Length)];
            Instantiate(trunkTile.Prefab, transform.position + new Vector3(0f, yPosition, 0f), Quaternion.identity, transform);
            yPosition += (_tileSize.y * trunkTile.Size.y);
        }
    }
}