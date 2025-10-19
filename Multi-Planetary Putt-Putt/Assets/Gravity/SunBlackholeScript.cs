using AudioSystem;
using UnityEngine;

public class SunBlackholeScript : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ball")
        {
            Ball ball = collision.gameObject.GetComponent<Ball>();
            if(ball != null)
            {
                ball.ForceReset = true;
            }
        }
    }
}
