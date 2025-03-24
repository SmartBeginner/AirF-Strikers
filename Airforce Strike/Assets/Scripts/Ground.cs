using UnityEngine;

public class Ground : MonoBehaviour
{
    private void  OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerShooter player = collision.GetComponent<PlayerShooter>();
            if (player != null)
            {
                player.isGrounded = true;
            }
        }
    }

    private void  OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerShooter player = collision.GetComponent<PlayerShooter>();
            if (player != null)
            {
                player.isGrounded = false;
            }
        }
    }
}
