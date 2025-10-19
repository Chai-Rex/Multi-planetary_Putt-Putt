using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static UnityEngine.CullingGroup;
using static UnityEngine.GraphicsBuffer;
using static UnityEditor.FilePathAttribute;

public class AtmosphereManager : MonoBehaviour {

    public static AtmosphereManager Instance { get; private set; }

    public static List<Atmosphere> atmosphereObjects = new List<Atmosphere>();

    private void Awake() {
        Instance = this;
    }


    public static float PredictAtmosphereAtLocation(Vector2 location)
    {
        float totalDrag = 0;

        foreach(Atmosphere atmosphere in atmosphereObjects)
        {
            float distance = ((Vector3)location - atmosphere.gameObject.transform.position).magnitude;
            if(atmosphere.Radius >= distance)
            {
                totalDrag = atmosphere.LinearDrag;
            }
        }

        return totalDrag;
    }

}