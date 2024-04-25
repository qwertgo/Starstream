using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;
using NaughtyAttributes;

enum RotationMode {Off, RotateOwnAxis, RotateOrbit}
enum MovementMode {Off, Yoyo, YoyoExtend}
enum OrbitAxis {X, Y, Z}
public class StaticObjectTransformation : MonoBehaviour
{
    [SerializeField]RotationMode rotationMode = RotationMode.Off;
    [HideIf("rotationMode", RotationMode.Off)][SerializeField]float rotSpeedX = 5;
    [HideIf("rotationMode", RotationMode.Off)][SerializeField]float rotSpeedY = 10;
    [HideIf("rotationMode", RotationMode.Off)][SerializeField]float rotSpeedZ = 7;

    [ShowIf("rotationMode", RotationMode.RotateOrbit)][SerializeField]OrbitAxis orbitAxis;
    [ShowIf("rotationMode", RotationMode.RotateOrbit)][SerializeField]Transform orbitTarget;
    [ShowIf("rotationMode", RotationMode.RotateOrbit)][SerializeField]float orbitDuration = 1;


    [SerializeField]MovementMode movementMode = MovementMode.Off;
    [HideIf("movementMode", MovementMode.Off)][SerializeField]Ease ease = Ease.InOutSine;
    [HideIf("movementMode", MovementMode.Off)][SerializeField]float duration = 1;
    [HideIf("movementMode", MovementMode.Off)][SerializeField]float moveDistanceX = 1;
    [HideIf("movementMode", MovementMode.Off)][SerializeField]float moveDistanceY = 0;
    [HideIf("movementMode", MovementMode.Off)][SerializeField]float moveDistanceZ = 1;

    Vector3 originPosition;
    Tween tween;
    Sequence sequence;

    private void Start() 
    {
        originPosition = transform.position;
        if(rotationMode != RotationMode.Off)
            SetRotation();
        if(movementMode != MovementMode.Off)
            SetMovement();
    }
    [Button]
    void SetRotation()
    {
        switch (rotationMode)
        {
            case RotationMode.Off:
                break;
            case RotationMode.RotateOwnAxis:
                RotateOwnAxis();
                break;
            case RotationMode.RotateOrbit:
                RotateOrbit();
                break;
            default:
                break;
        };
    }
    Vector3 UpdateRotation(float x, float y, float z) => new Vector3(x, y, z);

    void RotateOwnAxis()
    {
        transform.DORotate(UpdateRotation(rotSpeedX, rotSpeedY, rotSpeedZ), 1f, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Incremental)
            .SetEase(Ease.Linear);
    }

    void RotateOrbit()
    {
        GameObject rotateParent = new GameObject("runtimeOrbitParent");
        rotateParent.transform.position = orbitTarget.position;
        rotateParent.transform.rotation = orbitTarget.rotation;
        transform.SetParent(rotateParent.transform);

        Vector3 orbitRotateAxis = new();
        switch (orbitAxis)
        {
            case OrbitAxis.X:
                orbitRotateAxis = new Vector3(90,0,0);
                break;
            case OrbitAxis.Y:
                orbitRotateAxis = new Vector3(0,90,0);
                break;
            case OrbitAxis.Z:
                orbitRotateAxis = new Vector3(0,0,90);
                break;
        }

        rotateParent.transform.DORotate(transform.position + orbitRotateAxis, orbitDuration, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Incremental)
            .SetEase(Ease.Linear);
    }

    [Button]
    void SetMovement()
    {
        transform.position = originPosition;
        tween.Kill();
        sequence.Kill();
        switch (movementMode)
        {
            case MovementMode.Off:
                break;
            case MovementMode.Yoyo:
                YoyoMovement();
                break;
            case MovementMode.YoyoExtend:
                YoyoExtendMovement();
                break;
        }
    }
    Vector3 UpdateMovement(float x, float y, float z) => new Vector3(x, y, z);
    void YoyoMovement()
    {
        tween = transform.DOMove(transform.position + UpdateMovement(moveDistanceX, moveDistanceY, moveDistanceZ),duration)
            .SetLoops(-1,LoopType.Yoyo)
            .SetEase(ease);
    }
    void YoyoExtendMovement()
    {
        Vector3 endPosition = UpdateMovement(moveDistanceX, moveDistanceY, moveDistanceZ);
        Vector3 reverseEndPosition = -endPosition;

        tween = transform.DOMove(transform.position + endPosition, duration/2).SetEase(ease).OnComplete(() =>
        {
            Sequence yoyoExtendSequence = DOTween.Sequence();
            sequence = yoyoExtendSequence;
            yoyoExtendSequence.Append(transform.DOMove(reverseEndPosition, duration).SetEase(ease));
            yoyoExtendSequence.Append(transform.DOMove(endPosition, duration).SetEase(ease));
            yoyoExtendSequence.SetLoops(-1, LoopType.Restart);
        });
    }
}