using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuFollow : MonoBehaviour
{
    [SerializeField] private float smooth = 1;
    [SerializeField] private Transform FPS;

    void Awake()
    {
        FPS = Camera.main.transform;
    }

    void Update()
    {
        transform.position = new Vector3(FPS.transform.position.x, FPS.transform.position.y, FPS.transform.position.z);
        Quaternion target = Quaternion.Euler(0.0f, FPS.transform.eulerAngles.y, 0.0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.unscaledDeltaTime * smooth);
    }
}
