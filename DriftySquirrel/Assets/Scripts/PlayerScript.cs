using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Tree")
        {
            PlayControllerScript.Instance.Score(1);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Branch")
        {
            PlayControllerScript.Instance.Die();
        }
    }
}