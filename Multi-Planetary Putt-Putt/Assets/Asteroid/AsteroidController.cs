using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class AsteroidController : MonoBehaviour
{
    private SplineContainer asteroidSpline;
    [SerializeField] private int numberOfAsteroids = 0;
    [SerializeField] private GameObject asteroid;
    [SerializeField] private float maxSpawnRate = 1.0f;
    [SerializeField] private float speedPerAsteroid = 0.2f;
    private float currentSpawnRate = 0;
    private Dictionary<GameObject, Asteroid> asteroids = new Dictionary<GameObject, Asteroid>();

    private void Awake()
    {
        asteroidSpline = GetComponent<SplineContainer>();
        currentSpawnRate = maxSpawnRate;
    }

    private void Start()
    {
        for (int currentAsteroid = 0; currentAsteroid < numberOfAsteroids; currentAsteroid++)
        {
            GameObject addedAsteroid = Instantiate(asteroid);
            asteroids.Add(addedAsteroid, addedAsteroid.GetComponent<Asteroid>());
            if (asteroids.TryGetValue(addedAsteroid, out Asteroid targetAsteroid))
            {
                targetAsteroid.SetAsteroidSpline(asteroidSpline);
                targetAsteroid.SetSpeed(speedPerAsteroid);
            }
            addedAsteroid.SetActive(false);
        }
    }

    private void Update()
    {
        SpawnAsteroid(Time.deltaTime);
    }

    private void SpawnAsteroid(float deltaTime)
    {
        currentSpawnRate += deltaTime;
        if (currentSpawnRate >= maxSpawnRate)
        {
            currentSpawnRate = 0.0f;
            foreach (GameObject kAsteroid in asteroids.Keys)
            {
                if (!kAsteroid.activeInHierarchy)
                {
                    kAsteroid.SetActive(true);
                    return;
                }
            }
        }
    }

}
