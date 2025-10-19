using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Indicator : MonoBehaviour {
    [Header("Rotation")]
    [SerializeField] private float rotationMaxSpeed = 100f;
    [SerializeField] private float rotationTime = 1f;
    [SerializeField] private AnimationCurve rotationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Scale")]
    [SerializeField] private float scaleMaxSpeed = 1f;
    [SerializeField] private float scaleTime = 1f;
    [SerializeField] private AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private float minScale = 0.5f;
    [SerializeField] private float maxScale = 3f;

    [SerializeField] private int maxVisualPredictionIterations = 15;
    [SerializeField] private float physicsPredictionInterval = 0.02f;
    [SerializeField] private float visualPredictionInterval = 0.5f;
    [SerializeField] private GameObject predictionObject = null;

    private bool isMoving = false;
    private float rotationElapsedTime = 0f;
    private float scaleElapsedTime = 0f;

    private Ball targetBall;
    public Ball TargetBall { set { targetBall = value; } }

    private List<GameObject> predictionObjects = new List<GameObject>();

    private void Start()
    {
        //gravityPredictionFactor = 2;// physicsPredictionInterval / Time.fixedDeltaTime;
        physicsPredictionInterval = Time.fixedDeltaTime;

        for(int i = 0; i < maxVisualPredictionIterations; i++)
        {
            predictionObjects.Add(Instantiate(predictionObject));
            predictionObjects[i].SetActive(false);
        }

        UpdatePredictionLine();
    }

    private void OnEnable() {
        InputManager.Instance.MoveAction.started += MoveAction_started;
        InputManager.Instance.MoveAction.canceled += MoveAction_canceled;
    }

    private void OnDisable() {
        InputManager.Instance.MoveAction.started -= MoveAction_started;
        InputManager.Instance.MoveAction.canceled -= MoveAction_canceled;
        isMoving = false;
    }

    private void MoveAction_started(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        if (isMoving) return;
        isMoving = true;
        UpdateAsync();
    }

    private void MoveAction_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        isMoving = false;
        rotationElapsedTime = 0f;
        scaleElapsedTime = 0f;
    }

    private async void UpdateAsync() {
        while (isMoving && this != null) {
            Vector2 input = InputManager.Instance.MoveAction.ReadValue<Vector2>();

            // Handle rotation with curve
            if (input.x != 0) {
                rotationElapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(rotationElapsedTime / rotationTime);
                float curveValue = rotationCurve.Evaluate(t);
                float rotationAmount = -input.x * rotationMaxSpeed * curveValue * Time.deltaTime;
                transform.Rotate(0f, 0f, rotationAmount);
            } else {
                rotationElapsedTime = 0f;
            }

            // Handle scale with curve
            if (input.y != 0) {
                scaleElapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(scaleElapsedTime / scaleTime);
                float curveValue = scaleCurve.Evaluate(t);
                float scaleAmount = input.y * scaleMaxSpeed * curveValue * Time.deltaTime;

                Vector3 newScale = transform.localScale + new Vector3(0f, scaleAmount, 0f);
                newScale.y = Mathf.Clamp(newScale.y, minScale, maxScale);
                transform.localScale = newScale;
            } else {
                scaleElapsedTime = 0f;
            }

            UpdatePredictionLine();

            await Task.Yield();
        }
    }

    public void SetPredictionLineVisible(bool visible)
    {
        if (!visible)
        {
            for (int i = 0; i < maxVisualPredictionIterations; i++)
            {
                predictionObjects[i].SetActive(false);
            }
        }
        else
        {
            UpdatePredictionLine();
        }
    }

    private void UpdatePredictionLine()
    {
        if (targetBall == null || predictionObjects.Count == 0) return;

        Vector2 appliedForce = transform.up * transform.localScale.y * targetBall.LaunchForce;
        
        Vector2 predictedLocation = targetBall.RB.position;
        Vector2 predictedVelocity = (appliedForce + GravityManager.PredictGravityForceAtLocation(predictedLocation, targetBall.RB.mass) ) / targetBall.RB.mass;
        Vector2 nextLocation;
        Vector2 nextVelocity;

        float visualPredictionTime = 0;
        int visualIteration = 0;
        Vector2 gravityForce;
        float atmosphereDrag;
        while (visualIteration < maxVisualPredictionIterations && predictedVelocity.magnitude > targetBall.StopVelocity)
        {
            gravityForce = GravityManager.PredictGravityForceAtLocation(predictedLocation, targetBall.RB.mass);
            atmosphereDrag = AtmosphereManager.PredictAtmosphereAtLocation(predictedLocation);
            //atmosphereDrag = atmosphereDrag * 0.5f;
            PredictLocation(predictedLocation, predictedVelocity, gravityForce / targetBall.RB.mass, atmosphereDrag, physicsPredictionInterval, out nextLocation, out nextVelocity);

            predictedLocation = nextLocation;
            predictedVelocity = nextVelocity;
            visualPredictionTime += physicsPredictionInterval;

            if (visualPredictionTime >= visualPredictionInterval)
            {
                predictionObjects[visualIteration].transform.position = predictedLocation;
                predictionObjects[visualIteration].SetActive(true);
                visualPredictionTime -= visualPredictionInterval;
                visualIteration++;
            }

            // If hit something stop the prediction line
            if (Physics2D.Raycast(predictedLocation, predictedVelocity.normalized, predictedVelocity.magnitude * physicsPredictionInterval, LayerMask.GetMask("PlanetCollider")))
            {
                break;
            }

        }

        for(; visualIteration < maxVisualPredictionIterations; visualIteration++)
        {
            predictionObjects[visualIteration].SetActive(false);
        }
    }

    private void PredictLocation(Vector2 startLocation, Vector2 startVelocity, Vector2 acceleration, float damping, float timeStep, out Vector2 resultLocation, out Vector2 resultVelocity)
    {
        float dampFactor = Mathf.Max(1f - damping * timeStep, 0);

        resultVelocity = startVelocity + acceleration * timeStep;
        Vector2 dragForce = (startVelocity - (resultVelocity * dampFactor)) / timeStep;

        resultLocation = startLocation + (startVelocity + resultVelocity) * dampFactor * 0.5f * timeStep + 0.5f * (acceleration - dragForce)  * Mathf.Pow(timeStep, 2f);

        resultVelocity *= dampFactor;
    }

}