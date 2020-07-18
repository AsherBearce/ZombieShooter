using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MouseLook))]
public class AnimationController : MonoBehaviour
{
    public GameObject Head;
    public GameObject Torso;
    private MouseLook mouseLook;

    private void Start()
    {
        mouseLook = transform.GetComponent<MouseLook>();
    }

    // Update is called once per frame
    void Update()
    {
        Head.transform.localRotation = Quaternion.Euler(mouseLook.pitch / 2, 0, 0);
        Torso.transform.localRotation = Quaternion.Euler(mouseLook.pitch / 2 + 180f, -90f, 0);
    }
}
