using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundCollectorScript : MonoBehaviour
{
    private GameObject[] _backgrounds;
    private GameObject[] _grounds;

    private float _lastBackgroundX;
    private float _lastGroundX;

    public BackgroundCollectorScript()
    {
        _backgrounds = null;
        _grounds = null;

        _lastBackgroundX = 0f;
        _lastGroundX = 0f;
    }

    private void Awake()
    {
        _backgrounds = GameObject.FindGameObjectsWithTag("Background");
        _grounds = GameObject.FindGameObjectsWithTag("Ground");

        _lastBackgroundX = _backgrounds[0].transform.position.x;
        _lastGroundX = _grounds[0].transform.position.x;

        foreach (var item in _backgrounds)
        {
            if (item.transform.position.x > _lastBackgroundX)
            {
                _lastBackgroundX = item.transform.position.x;
            }
        }

        foreach (var item in _grounds)
        {
            if (item.transform.position.x > _lastGroundX) 
            {
                _lastGroundX = item.transform.position.x;
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
        else if (collision.tag == "Ground")
        {
            var temp = collision.transform.position;
            var width = ((BoxCollider2D)collision).size.x * collision.transform.localScale.x;
            temp.x = _lastGroundX + width;
            collision.transform.position = temp;
            _lastGroundX = temp.x;
        }
        else if (collision.tag == "Tree")
        {
            Destroy(collision.gameObject);
        }
    }
}