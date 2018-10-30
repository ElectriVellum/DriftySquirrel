using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "TreeHolder")
        {
            PlayControllerScript.Instance.Score(1);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Tree")
        {
            PlayControllerScript.Instance.Die();
        }
    }
}