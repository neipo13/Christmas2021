using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockToTransform : MonoBehaviour
{
    public Transform LockToThisObjectsPosition;
    public Vector3 Offset;

    void Update()
    {
        this.transform.position = LockToThisObjectsPosition.position + Offset;
    }
}