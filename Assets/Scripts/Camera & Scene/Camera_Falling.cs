using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Falling : CameraObj
{
    private Transform player;
    public Vector3 offset;
    public float smoothSpeed = 0.125f;

    private Coroutine dropShakeCoroutine;

    public bool vPlayerFollow = true;

    private void Start()
    {
        player = GameAssistManager.Instance.player.transform;
    }

    private void FixedUpdate()
    {
        if (!GameAssistManager.Instance.GetBoolPlayerDie() && vPlayerFollow)
        {
            Vector3 targetPosition = player.position + offset;
            Vector3 smoothed = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
            transform.position = smoothed;
        }
    }



    public void DropShake(float dropAmount)
    {
        TogglePlayerFollowTemporarily();
        smoothSpeed = 0.5f;

        if (dropShakeCoroutine != null)
            StopCoroutine(dropShakeCoroutine);
        dropShakeCoroutine = StartCoroutine(DropShakeRoutine(dropAmount));
    }

    private IEnumerator DropShakeRoutine(float dropAmount)
    {
        Vector3 originalOffset = offset;
        Vector3 targetOffset = new Vector3(offset.x + dropAmount, offset.y, offset.z);

        float duration = 1.2f;
        float timer = 0f;
        while (timer < duration)
        {
            offset = Vector3.Lerp(originalOffset, targetOffset, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        offset = targetOffset;
        dropShakeCoroutine = null;
    }




    public void TogglePlayerFollowTemporarily(float delay = 0.2f)
    {
        // StartCoroutine(ToggleFollowRoutine(delay));
    }
    private IEnumerator ToggleFollowRoutine(float delay)
    {
        vPlayerFollow = false;
        yield return new WaitForSeconds(delay);
        vPlayerFollow = true;
    }
}
