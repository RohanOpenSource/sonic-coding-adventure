using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    [SerializeField] private float minDistance;
    [SerializeField] private float maxDistance;
    [SerializeField] private float smooth;
    private Vector3 dollyDir;
    [SerializeField] private Vector3 dollyDirAdjusted;
    [SerializeField] private float distance;
    private void Awake() {
        dollyDir = transform.localPosition.normalized;
        distance = transform.localPosition.magnitude;
    }

    private void Update(){

        Vector3 desiredCameraPos;
    }
}
