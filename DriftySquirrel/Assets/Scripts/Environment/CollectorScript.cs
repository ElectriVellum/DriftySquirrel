using UnityEngine;

public class CollectorScript : MonoBehaviour
{
    private GameObject[] _backgrounds;
    private float _lastBackgroundX;

    public CollectorScript()
    {
        _backgrounds = null;
        _lastBackgroundX = 0f;
    }

    private void Awake()
    {
        _backgrounds = GameObject.FindGameObjectsWithTag("Background");
        _lastBackgroundX = _backgrounds[0].transform.position.x;
        foreach (var item in _backgrounds)
        {
            if (item.transform.position.x > _lastBackgroundX)
            {
                _lastBackgroundX = item.transform.position.x;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Background")
        {
            var temp = collision.transform.position;
            var width = ((BoxCollider2D)collision).size.x;
            temp.x = _lastBackgroundX + width;
            collision.transform.position = temp;
            _lastBackgroundX = temp.x;
        }
        else if (collision.tag == "Ground" || collision.tag == "GroundWater" || collision.tag == "GroundSpikes" || collision.tag == "Collectible")
        {
            Destroy(collision.gameObject);
        }
    }
}