using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempCameraPos : MonoBehaviour
{

    public GameObject player;
    public GameObject parentObj;

    [Range(-50, 50)]
    public float xPos;
    [Range(-50, 50)]
    public float yPos;
    [Range(-50, 50)]
    public float zPos;

    public float rotationSpeed = 20.0f; // 회전 속도
    public float moveSpeed = 10.0f; // 위치 이동 속도

    void Update()
    {
        // 카메라 위치 조정
        transform.localPosition = new Vector3(xPos, yPos, zPos);

        // 부모 오브젝트를 플레이어 위치에 동기화
        parentObj.transform.position = player.transform.position;

        // J, L, I, K 키로 상하좌우 회전
        if (Input.GetKey(KeyCode.J))
        {
            parentObj.transform.rotation = Quaternion.Euler(new Vector3(parentObj.transform.eulerAngles.x, parentObj.transform.eulerAngles.y - rotationSpeed * Time.deltaTime, parentObj.transform.eulerAngles.z));
        }
        else if (Input.GetKey(KeyCode.L))
        {
            parentObj.transform.rotation = Quaternion.Euler(new Vector3(parentObj.transform.eulerAngles.x, parentObj.transform.eulerAngles.y + rotationSpeed * Time.deltaTime, parentObj.transform.eulerAngles.z));
        }

        if (Input.GetKey(KeyCode.I))
        {
            parentObj.transform.rotation = Quaternion.Euler(new Vector3(parentObj.transform.eulerAngles.x - rotationSpeed * Time.deltaTime, parentObj.transform.eulerAngles.y, parentObj.transform.eulerAngles.z));
        }
        else if (Input.GetKey(KeyCode.K))
        {
            parentObj.transform.rotation = Quaternion.Euler(new Vector3(parentObj.transform.eulerAngles.x + rotationSpeed * Time.deltaTime, parentObj.transform.eulerAngles.y, parentObj.transform.eulerAngles.z));
        }

        // T, G, F, H 키로 카메라 위치 조정
        if (Input.GetKey(KeyCode.N))
        {
            yPos += moveSpeed * Time.deltaTime; // 위로 이동
        }
        else if (Input.GetKey(KeyCode.M))
        {
            yPos -= moveSpeed * Time.deltaTime; // 아래로 이동
        }

        if (Input.GetKey(KeyCode.F))
        {
            xPos -= moveSpeed * Time.deltaTime; // 왼쪽으로 이동
        }
        else if (Input.GetKey(KeyCode.H))
        {
            xPos += moveSpeed * Time.deltaTime; // 오른쪽으로 이동
        }

        // N, M 키로 앞뒤 이동
        if (Input.GetKey(KeyCode.T))
        {
            zPos += moveSpeed * Time.deltaTime; // 앞으로 이동
        }
        else if (Input.GetKey(KeyCode.G))
        {
            zPos -= moveSpeed * Time.deltaTime; // 뒤로 이동
        }
    }
}
