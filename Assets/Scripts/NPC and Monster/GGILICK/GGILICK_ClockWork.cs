using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GGILICK_ClockWork : InteractableObject
{
    public CineCameraChager cineChager;
    public Transform transformTeleport_Inside;
    public GameObject gamObejct;
    

    private void Start()
    {
        type = InteractableType.SingleEvent;
    }



    public override void ActiveEvent()
    {
        canInteract = false;
        GameAssistManager.Instance.FadeOutInEffect(5f);
        GameAssistManager.Instance.StartCoroutine(ChangeMap());
        // StartCoroutine();
    }

    // #. 맵 변경 함수
    IEnumerator ChangeMap()
    {
        Rigidbody rigid = GameAssistManager.Instance.player.GetComponent<Rigidbody>();
        rigid.constraints = RigidbodyConstraints.FreezePositionY;

        yield return new WaitForSeconds(1.2f);

        cineChager.CameraChange();

        yield return new WaitForSeconds(3.2f);


        Vector3 teleportPosition = transformTeleport_Inside.position;

        // gamObject의 상대 위치를 계산
        Vector3 playerPosition = GameAssistManager.Instance.GetPlayer().transform.position;
        Vector3 offset = gamObejct.transform.position - playerPosition; // 플레이어와 gamObject 간의 상대적 위치

        // 두 객체를 동시에 순간이동
        gamObejct.transform.position = teleportPosition + offset; // gamObject를 새로운 위치에 배치
        yield return new WaitForEndOfFrame();
        GameAssistManager.Instance.GetPlayer().transform.position = teleportPosition; // 플레이어를 순간이동

        yield return new WaitForSeconds(0.2f);

        rigid.constraints = RigidbodyConstraints.None; // FreezePosition X, Y, Z 모두 false

        // Freeze Rotation 설정
        rigid.constraints = RigidbodyConstraints.FreezeRotationX |
                            RigidbodyConstraints.FreezeRotationY |
                            RigidbodyConstraints.FreezeRotationZ;


        yield return new WaitForSeconds(6f);
        

        InsideAssist_GGILICK.Instance.bCarCreating = true;
    }


}
