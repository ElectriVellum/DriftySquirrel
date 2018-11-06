using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class LeafScript : MonoBehaviour
{
    private float _fallSpeed;
    private Vector3 _fallDirection;

    public LeafScript()
    {
        _fallSpeed = 0f;
        _fallDirection = Vector2.down;
    }

    private void Start()
    {
        _fallSpeed = Random.Range(0.5f, 2f);
        _fallDirection.x = Random.Range(-0.8f, 0.8f);
        GetComponent<SpriteRenderer>().flipX = Random.Range(0, 101) <= 50;
        Destroy(gameObject, 5f);
    }

    private void Update()
    {
        transform.Translate(_fallDirection * _fallSpeed * Time.deltaTime);
    }
}