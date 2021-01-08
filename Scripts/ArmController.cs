using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmController : MonoBehaviour
{
    Rigidbody rig;
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody>();  
    }

    public void SetPar(Transform Par)
    {
        transform.SetParent(Par);
    }

    public void RemovePar()
    {
        transform.parent = null;
    }

    public void MoveRight(float speed)
    {
        rig.MovePosition(transform.position + new Vector3(1f, 0f, 0f) * speed * Time.deltaTime);
    }

    public void MoveLeft(float speed)
    {
        rig.MovePosition(transform.position + new Vector3(-1f, 0f, 0f) * speed * Time.deltaTime);
    }

    public void MoveFront(float speed)
    {
        rig.MovePosition(transform.position + new Vector3(0f, 0f, 1f) * speed * Time.deltaTime);
    }

    public void MoveBack(float speed)
    {
        rig.MovePosition(transform.position + new Vector3(0f, 0f, -1f) * speed * Time.deltaTime);
    }
}
