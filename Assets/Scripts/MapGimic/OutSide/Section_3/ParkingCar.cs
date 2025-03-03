using System.Collections; 
using System.Collections.Generic; 
using UnityEditor; 
using UnityEngine; 

public class ParkingCar : ClockBattery
{
    public bool bMoveDirection; // 현재 진행해야 하는 방향 true = 앞 방향 / false = 뒷 방향
    private bool bIsMove;

    public float initialSpeed;  // 처음 속도

    public GameObject CarObj; // 최종적으로 움직임이 적용될 오브젝트
    public Rigidbody rb; // CarObj의 하위 항목에서 찾은 Rigidbody
    public bool bIsWall;


    private Coroutine nowCoroutine;


    public override void TurnOnObj()
    {
        base.TurnOnObj();

        bIsMove = true;

        RotateObject((int)fCurClockBattery);
        nowCoroutine = StartCoroutine(MoveForwardWithAcceleration());
    }
    public override void TurnOffObj()
    {
        base.TurnOffObj();

        Debug.Log("실행");

        if (nowCoroutine != null) StopCoroutine(nowCoroutine);
        if(rb != null) rb.velocity = Vector3.zero;

        if(bIsMove)
        {
            bMoveDirection = !bMoveDirection;
            bIsMove = false;
        }
       
    }



    private void Awake()
    {
        // CarObj의 하위 오브젝트에서 Rigidbody 찾기
        if (CarObj != null && !bIsWall)
        {
            rb = CarObj.GetComponentInChildren<Rigidbody>();
        }

        bIsMove = false;
    }


    private IEnumerator MoveForwardWithAcceleration()
    {
        float currentSpeed = initialSpeed;

        Color rayColor = Color.red;

        while (fCurClockBattery > 0)
        {
            if (fCurClockBattery < fLowClockBatteryPoint)
            {
                currentSpeed = Mathf.Lerp(0, initialSpeed, fCurClockBattery / fLowClockBatteryPoint);
            }
            float moveDirection = bMoveDirection ? -1f : 1f;

            CarObj.transform.position += CarObj.transform.forward * currentSpeed * moveDirection * Time.deltaTime;
            fCurClockBattery -= Time.deltaTime;

            yield return null;
        }


        TurnOffObj();
    }



    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<ParkingCar>() != null && bIsMove)
        {
            Debug.Log("충돌!!!");
            TurnOffObj();
        }
    }





}
