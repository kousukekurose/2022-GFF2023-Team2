using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutShot : MonoBehaviour
{
    [SerializeField]
    GameObject shotPrefab = null;

    [SerializeField]
    private float shotSpeed = 500;

    // Start is called before the first frame update
    public void PutShotItem()
    {
        GameObject shot = Instantiate(shotPrefab, transform.position,transform.rotation);
        var shotRigidbody = shot.GetComponent<Rigidbody>();
        shotRigidbody.AddForce(transform.forward * -shotSpeed);
    }
}
