using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;

public class MoveBlock_Main : MonoBehaviour
{

    public MoveBlock_Battery moveBlock_Battery_Up;
    public MoveBlock_Battery moveBlock_Battery_Down;


    public List<int> inputList = new List<int>();

    public List<int> obstacleList = new List<int>(){0,3,0,2};
    public List<int> curBlockList = new List<int>(){3,1,2,5};
    public List<int> answerList = new List<int>(){3,0,1,3};

    public Material m_UpLight;
    public Material m_DownLight;
    public Material m_OffLight;

    public List<MeshRenderer> guideLightsList = new List<MeshRenderer>();
    public List<MeshRenderer> moveBlocksList_1 = new List<MeshRenderer>();
    public List<MeshRenderer> moveBlocksList_2 = new List<MeshRenderer>();
    public List<MeshRenderer> moveBlocksList_3 = new List<MeshRenderer>();
    public List<MeshRenderer> moveBlocksList_4 = new List<MeshRenderer>();

    public List<List<MeshRenderer>> moveBlocksList = new List<List<MeshRenderer>>();

    public void Start()
    {
        SetDefaultBlock();
        moveBlocksList.Add(moveBlocksList_1);
        moveBlocksList.Add(moveBlocksList_2);
        moveBlocksList.Add(moveBlocksList_3);
        moveBlocksList.Add(moveBlocksList_4);
    }

    public void SetDefaultBlock(){
        curBlockList = new List<int>(){4,1,2,5};
    }

    public void SetBatteryMaxCharge(int _maxCharge)
    {
        moveBlock_Battery_Up.fMaxClockBattery = _maxCharge;
        moveBlock_Battery_Down.fMaxClockBattery = _maxCharge;

        if(_maxCharge <= 0){
            moveBlock_Battery_Up.clockWork.GetComponent<ClockWork>().canInteract = false;
            moveBlock_Battery_Down.clockWork.GetComponent<ClockWork>().canInteract = false;
        }

    }

    public void SetGuideLight()
    {
        for(int i = 0; i < inputList.Count; i++){
            guideLightsList[i].material = inputList[i] == 0 ? m_UpLight : m_DownLight;
        }

        for(int i = inputList.Count; i < guideLightsList.Count; i++){
            guideLightsList[i].material = m_OffLight;
        }
    }

    public void AddInputList(int _input)
    {
        inputList.Add(_input);
        SetGuideLight();

        if (_input == 0)
            MoveBlock(-1);
        else
            MoveBlock(1);

        if(inputList.Count >= 3)
        {
            CheckAnswer();
        }
    }

    public void CheckAnswer(){
        for(int i = 0; i < curBlockList.Count; i++){
            if(curBlockList[i] != answerList[i]) {
                Debug.Log("오답입니다.");
                StartCoroutine(C_AnswerInCorrectEvent());
                return;
            }
        }
        Debug.Log("정답입니다.");
        StartCoroutine(C_AnswerCorrectEvent());
    }

    IEnumerator C_AnswerCorrectEvent()
    {
        yield return new WaitForSeconds(2.0f);
    }

    IEnumerator C_AnswerInCorrectEvent()
    {
        yield return new WaitForSeconds(2.0f);
        SetDefaultBlock();
        SetMoveBlockLight();
        SetBatteryMaxCharge(3);
        moveBlock_Battery_Up.clockWork.GetComponent<ClockWork>().canInteract = true;
        moveBlock_Battery_Down.clockWork.GetComponent<ClockWork>().canInteract = true;
        inputList.Clear();
        SetGuideLight();
    }
    

    public void SetMoveBlockLight(){
        for(int i = 0; i < moveBlocksList.Count; i++){
            for(int j = 0; j < moveBlocksList[i].Count; j++){
                if(moveBlocksList[i][j] == null) continue;
                if(j == curBlockList[i]) moveBlocksList[i][j].material = m_UpLight;
                else moveBlocksList[i][j].material = m_OffLight;
            }
        }
    }



    public void MoveBlock(int _index){
        for(int i =0; i<curBlockList.Count;i++)
        {
            int nextPos = curBlockList[i] + _index;
            if(nextPos == -1 || nextPos == 6 || nextPos == obstacleList[i]) continue;
            moveBlocksList[i][curBlockList[i]].material = m_OffLight;
            curBlockList[i] = nextPos;
            moveBlocksList[i][nextPos].material = m_UpLight;
        }
    }


}
