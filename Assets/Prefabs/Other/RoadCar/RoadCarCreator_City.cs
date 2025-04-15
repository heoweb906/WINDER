using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RoadCarCreator_City : MonoBehaviour
{
    private CinemachineSmoothPath path;
    private float fTimer;

    public float fCreateInterval;
    public float fIntervalVariance;
    public GameObject[] Cars;
    public bool bCarCreate;


    private void Awake()
    {
        path = GetComponent<CinemachineSmoothPath>();   
    }


    private void Update()
    {
        if(bCarCreate)
        {
            fTimer += Time.deltaTime;

            float randomInterval = fCreateInterval + Random.Range(-fIntervalVariance, fIntervalVariance);

            if (fTimer >= randomInterval)
            {
                CreateCarAtPath();
                fTimer = 0f;
            }
        }

    
    }


    private void CreateCarAtPath()
    {
        if (Cars.Length < 3 || path == null) return;

        Vector3 spawnPosition = path.EvaluatePosition(0f);
        GameObject selectedCar = GetRandomCarByWeight();
        
        RoadCarOnTrack roadCar = Instantiate(selectedCar, spawnPosition, Quaternion.identity).GetComponent<RoadCarOnTrack>();

        roadCar.transform.SetParent(transform);

        roadCar.m_Path = path;
    }

    private GameObject GetRandomCarByWeight()
    {
        if (Cars == null || Cars.Length == 0) return null;

        int randomIndex = Random.Range(0, Cars.Length);
        return Cars[randomIndex];
    }
}
