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


    private void Awake()
    {
        path = GetComponent<CinemachineSmoothPath>();   
    }


    private void Update()
    {
        fTimer += Time.deltaTime;

        float randomInterval = fCreateInterval + Random.Range(-fIntervalVariance, fIntervalVariance);

        if (fTimer >= randomInterval)
        {
            CreateCarAtPath();
            fTimer = 0f;
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
        int rand = Random.Range(0, 100); // 0 ~ 99 사이 난수 생성

        if (rand < 60) return Cars[0]; 
        else if (rand < 85) return Cars[1]; 
        else return Cars[2]; 
    }
}
