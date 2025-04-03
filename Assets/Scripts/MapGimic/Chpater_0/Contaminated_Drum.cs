using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contaminated_Drum : MonoBehaviour
{
    public GameObject[] platforms; // 발판 배열
    public float fChangeTime = 1.5f; // 패턴 변경 주기

    private bool[,] activationPatterns = new bool[,]
    {
        { false,  true,  true  },
        { false,  false, true  },
        { true,   false, false },
        { true,   true,  false },
        { true,   true,  true  }
    };

    private int currentPatternIndex = 0;
    private float timer;

    void Start()
    {
        timer = fChangeTime;
        ApplyPattern();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            NextPattern();
            timer = fChangeTime;
        }
    }

    private void NextPattern()
    {
        currentPatternIndex = (currentPatternIndex + 1) % activationPatterns.GetLength(0);
        ApplyPattern();
    }

    private void ApplyPattern()
    {
        for (int i = 0; i < platforms.Length; i++)
        {
            platforms[i].SetActive(activationPatterns[currentPatternIndex, i]);
        }
    }

}
