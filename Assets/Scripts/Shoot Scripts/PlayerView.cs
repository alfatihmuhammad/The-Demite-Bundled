using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public float maxRotationDegreePerSecond = 75f;
    public float gyroRotationSpeed = 70f;

    #if UNITY_EDITOR
    public float mouseRotationSpeed = 100f;
    #endif

    [Range(0, 45)]
    public float maxPitchUpAngle = 45f;

    [Range(0, 45)]
    public float maxPitchDownAngle = 5f;

    private Rect lookReact;
    private int lookTouchID = -1;
    private Vector2 touchOrigin;

    private void Start()
    {
        lookReact = new Rect(0, 0, Screen.width / 2, Screen.height * 0.75f);

        #if UNITY_EDITOR
        Cursor.visible = false;
        #endif
    }

    private void Update()
    {
        if(GameManager.instance.gameState == GameState.Running)
        {
            if(Input.gyro.enabled)
            {
                GyroInput();
            }
            else
            {
                TouchInput();
            }

            #if UNITY_EDITOR
            MouseInput();
            #endif
        }
    }

    private void GyroInput()
    {
        Vector3 rotation = Input.gyro.rotationRate * gyroRotationSpeed;

        RotateView(new Vector3(-rotation.x, -rotation.y, 0));
    }

    private void TouchInput()
    {
        if(Input.touchCount > 0)
        {
            //menyimpan inputan touch yang pertama ke lookrect
            if(lookTouchID == -1)
            {
                foreach (Touch touch in Input.touches)
                {
                    //register touch pertama
                    if (touch.phase != TouchPhase.Began)
                        continue;

                    //hanya menggunakan touch di lookrect
                    if (!lookReact.Contains(touch.position))
                        continue;

                    //menyimpan touch dan posisi (loop)
                    lookTouchID = touch.fingerId;
                    touchOrigin = touch.position;
                    break;
                }
            }

            foreach (Touch touch in Input.touches)
            {
                //nenproses touch ID yg disimpan
                if (touch.fingerId != lookTouchID)
                    continue;

                Vector3 touchDistance = touch.position - touchOrigin;

                //membatasi rotasi touch
                Vector3 clampedRotation = Vector3.ClampMagnitude(new Vector3(-touchDistance.y, touchDistance.x), maxRotationDegreePerSecond);

                //view rotasi
                RotateView(clampedRotation);

                //clear touch id tersimpan
                if(touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    lookTouchID = -1;
                }

                //exit the loop
                break;
            }
        }
    }

#if UNITY_EDITOR
    private void MouseInput()
    {
        Vector3 rotation = new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);
        RotateView (rotation * mouseRotationSpeed);
    }
#endif

    private void RotateView(Vector3 rotation)
    {
        //rotate player
        transform.Rotate(rotation * Time.deltaTime);

        if(Input.gyro.enabled)
        {
            //reset rotasi z local
            Vector3 localEuler = transform.localEulerAngles;
            transform.localRotation = Quaternion.Euler(localEuler.x, localEuler.y, 0);
        }

        //limit player rotation pitch
        float playerPitch = LimitPitch();

        //apply clamped pitch and clear roll
        transform.rotation = Quaternion.Euler(playerPitch, transform.eulerAngles.y, 0);
    }

    private float LimitPitch()
    {
        float playerPitch = transform.eulerAngles.x;

        float maxPitchUp = 360 - maxPitchUpAngle;
        float maxPitchDown = maxPitchDownAngle;

        if (playerPitch > 180 && playerPitch < maxPitchUp)
        {
            playerPitch = maxPitchUp;
        }
        else if (playerPitch < 180 && playerPitch > maxPitchDown)
        {
            playerPitch = maxPitchDown;
        }

        return playerPitch;
    }

}
