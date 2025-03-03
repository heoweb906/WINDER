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

    public float rotationSpeed = 20.0f; // ȸ�� �ӵ�
    public float moveSpeed = 10.0f; // ��ġ �̵� �ӵ�

    void Update()
    {
        // ī�޶� ��ġ ����
        transform.localPosition = new Vector3(xPos, yPos, zPos);

        // �θ� ������Ʈ�� �÷��̾� ��ġ�� ����ȭ
        parentObj.transform.position = player.transform.position;

        // J, L, I, K Ű�� �����¿� ȸ��
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

        // T, G, F, H Ű�� ī�޶� ��ġ ����
        if (Input.GetKey(KeyCode.N))
        {
            yPos += moveSpeed * Time.deltaTime; // ���� �̵�
        }
        else if (Input.GetKey(KeyCode.M))
        {
            yPos -= moveSpeed * Time.deltaTime; // �Ʒ��� �̵�
        }

        if (Input.GetKey(KeyCode.F))
        {
            xPos -= moveSpeed * Time.deltaTime; // �������� �̵�
        }
        else if (Input.GetKey(KeyCode.H))
        {
            xPos += moveSpeed * Time.deltaTime; // ���������� �̵�
        }

        // N, M Ű�� �յ� �̵�
        if (Input.GetKey(KeyCode.T))
        {
            zPos += moveSpeed * Time.deltaTime; // ������ �̵�
        }
        else if (Input.GetKey(KeyCode.G))
        {
            zPos -= moveSpeed * Time.deltaTime; // �ڷ� �̵�
        }
    }
}
