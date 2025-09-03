using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Transform player;
    Vector3 target, mousePos, refVel, shakeOffset;
    float cameraDist = 6f;
    float smoothTime = 0.2f, zStart;
    float shakeMag, shakeTimeEnd;
    Vector3 shakeVector;
    bool shaking;
    bool cameraLocked = false;
    float lockSmoothTime = 0.3f;

    void Start()
    {
        target = player.position;
        zStart = transform.position.z;
    }

    void Update()
    {
        shakeOffset = UpdateShake();

        if (cameraLocked)
        {
            target = player.position + shakeOffset;
            target.z = zStart;
        }
        else
        {
            mousePos = CaptureMousePos();
            target = UpdateTargetPos();
        }

        UpdateCameraPosition();
    }

    Vector3 CaptureMousePos()
    {
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector2 ret = Camera.main.ScreenToViewportPoint(mouseScreenPos);
        ret *= 2;
        ret -= Vector2.one;
        float max = 0.9f;
        if (Mathf.Abs(ret.x) > max || Mathf.Abs(ret.y) > max)
        {
            ret = ret.normalized;
        }
        return ret;
    }

    Vector3 UpdateTargetPos()
    {
        Vector3 mouseOffset = mousePos * cameraDist;
        Vector3 ret = player.position + mouseOffset;
        ret += shakeOffset;
        ret.z = zStart;
        return ret;
    }

    Vector3 UpdateShake()
    {
        if (!shaking || Time.time > shakeTimeEnd)
        {
            shaking = false;
            return Vector3.zero;
        }
        Vector3 tempOffset = shakeVector;
        tempOffset *= shakeMag;
        return tempOffset;
    }

    void UpdateCameraPosition()
    {
        float currentSmooth = cameraLocked ? lockSmoothTime : smoothTime;
        Vector3 tempPos = Vector3.SmoothDamp(transform.position, target, ref refVel, currentSmooth);
        transform.position = tempPos;
    }

    public void Shake(Vector3 direction, float magnitude, float length)
    {
        shaking = true;
        shakeVector = direction;
        shakeMag = magnitude;
        shakeTimeEnd = Time.time + length;
    }

    public void SetCameraLock(bool state)
    {
        cameraLocked = state;
    }

    public void LockCameraToPlayer()
    {
        cameraLocked = true;
    }

    public void UnlockCamera()
    {
        cameraLocked = false;
    }
}
