using UnityEngine;
using UnityEngine.Splines;

public class Asteroid : MonoBehaviour
{
    private SplineContainer asteroidSpline;
    private float speed = 0.2f;
    private float t = 0;

    private void OnEnable()
    {
        t = 0.0f;
    }

    private void Update()
    {
        t += speed * Time.deltaTime;
        if (t > 1.0f) 
        {
            t = 0.0f;
            gameObject.SetActive(false);
        }

        Vector3 position = asteroidSpline.EvaluatePosition(t);
        transform.position = position;
    }

    public void SetAsteroidSpline(SplineContainer _asteroidSpline)
    {
        asteroidSpline = _asteroidSpline;
    }

    public void SetSpeed(float _speed)
    {
        speed = _speed;
    }
}
