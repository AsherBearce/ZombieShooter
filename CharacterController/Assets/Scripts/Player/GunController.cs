using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AnimationController))]
public class GunController : MonoBehaviour
{
    public GameObject starter;
    public GameObject currentlyEquipped;
    public float range;
    public Camera mainCamera;
    private Quaternion startingRotation;
    private Vector3 startingPosition;
    private Coroutine gunLerp;

    public void equipWeapon(GameObject weaponPrefab)
    {
        if (currentlyEquipped != null)
        {
            Destroy(currentlyEquipped);
        }

        currentlyEquipped = Instantiate(weaponPrefab);
        AnimationController anim = transform.GetComponent<AnimationController>();
        currentlyEquipped.transform.SetParent(anim.head.transform.Find("HeadMesh").transform);
        currentlyEquipped.transform.localScale = new Vector3(1, 1, 1);
        currentlyEquipped.transform.localPosition = Vector3.zero;
        startingRotation = currentlyEquipped.transform.Find("Gun").transform.localRotation;
        startingPosition = currentlyEquipped.transform.Find("Gun").transform.localPosition;
        anim.leftArmTracker = currentlyEquipped.transform.Find("Gun").Find("HandLTarget").gameObject;
        anim.rightArmTracker = currentlyEquipped.transform.Find("Gun").Find("HandRTarget").gameObject;
    }

    public void pointGun(GameObject gun, Vector3 target)
    {

        Vector3 startFrom = gun.transform.InverseTransformDirection(-gun.transform.up);

        Vector3 localSpace = gun.transform.InverseTransformPoint(target);
        Vector3 axis = Vector3.Cross(localSpace, -startFrom);
        float angle = Vector3.Angle(localSpace, startFrom);

        gun.transform.Rotate(axis, angle);
    }

    public void shoot()
    {


        if (gunLerp != null)
        {
            StopCoroutine(gunLerp);
        }

        gunLerp = StartCoroutine(gunAnimation(currentlyEquipped.transform.Find("Gun").gameObject));
            

    }

    IEnumerator lerp(GameObject gunMesh, Quaternion startR, Vector3 startPos, Quaternion endR, Vector3 endPos, float t = 1)
    {
        float startTime = Time.time;

        while (Time.time <= startTime + t)
        {
            float percent = (Time.time - startTime) / t;

            gunMesh.transform.localRotation = Quaternion.Lerp(startR, endR, percent);
            gunMesh.transform.localPosition = Vector3.Lerp(startPos, endPos, percent);

            yield return null;
        }
    }

    IEnumerator gunAnimation(GameObject gunMesh)
    {
        Quaternion midRot = startingRotation * Quaternion.AngleAxis(-45.0f, Vector3.right);
        Vector3 midPosition = startingPosition + Vector3.up * 0.01f;

        yield return lerp(gunMesh, startingRotation, startingPosition, midRot, midPosition, 0.1f);
        yield return lerp(gunMesh, midRot, midPosition, startingRotation, startingPosition, 0.1f);
        
    }

    // Start is called before the first frame update
    void Start()
    {
        equipWeapon(starter);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentlyEquipped != null)
        {
            GameObject gunMesh = currentlyEquipped.transform.Find("Gun").gameObject;
            gunMesh.transform.localRotation = startingRotation;
            Vector3 shootPos = mainCamera.transform.position + mainCamera.transform.forward * range;
            RaycastHit hit;
            bool didHit = Physics.Raycast(gunMesh.transform.position, shootPos - gunMesh.transform.position, out hit);

            if (didHit)
            {
                Debug.DrawLine(gunMesh.transform.position, hit.point, Color.red);
                
                pointGun(gunMesh, hit.point);
            }

            if (Input.GetMouseButtonDown(0))
            {
                shoot();
            }
        }
    }
}
