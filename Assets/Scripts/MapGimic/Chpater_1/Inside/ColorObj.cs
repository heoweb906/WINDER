using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ColorObj : MonoBehaviour
{ 
    public ColorType colorType;

    public Material _material; // ����� ���׸����� ������ ����

    private void Awake()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer == null)
        {
            Debug.LogError("Renderer ������Ʈ�� �����ϴ�!");
            return;
        }

        // ���׸��� ����
        _material = new Material(renderer.material);

        // ������ ���׸����� ������Ʈ�� ����
        renderer.material = _material;
    }
}
