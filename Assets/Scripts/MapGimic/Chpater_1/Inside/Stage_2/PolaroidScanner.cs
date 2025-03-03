using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolaroidScanner : MonoBehaviour
{
    public List<PolaroidFigure> polaroidFigures = new List<PolaroidFigure>();  // 스캐너 위에 올라가 있는 피규어들
    public int iFigureIndex;      // 스캐너 위에 올라가 있는 피규어 넘버

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PolaroidFigure>() != null)
        {
            PolaroidFigure polaroidFigure = other.GetComponent<PolaroidFigure>();

            polaroidFigures.Add(polaroidFigure);

            if(polaroidFigures != null)
            {
                iFigureIndex = polaroidFigures[0].iIndex;
            }
        }

        if (other.GetComponentInParent<Player>() != null)
        {
            iFigureIndex = 4;
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PolaroidFigure>() != null)
        {
            PolaroidFigure polaroidFigure = other.GetComponent<PolaroidFigure>();

            if (polaroidFigures.Contains(polaroidFigure))
            {
                polaroidFigures.Remove(polaroidFigure);
                if (polaroidFigures.Count > 0)
                {
                    iFigureIndex = polaroidFigures[0].iIndex;
                }
                else
                {
                    iFigureIndex = 0;
                }
            }
        }

        if (other.GetComponentInParent<Player>() != null)
        {
            if (polaroidFigures.Count > 0)
            {
                iFigureIndex = polaroidFigures[0].iIndex;
            }
            else
            {
                iFigureIndex = 0;
            }
        }

    }
}
