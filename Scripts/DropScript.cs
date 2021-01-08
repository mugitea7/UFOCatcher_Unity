using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropScript : MonoBehaviour
{

    private GameManager gameManager;
    private PrizesController prizesController;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        prizesController = GameObject.Find("GameManager").GetComponent<PrizesController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(gameManager.state != GameManager.State.Drop)
        {
            gameManager.state = GameManager.State.Drop;
        }
        gameManager.isDrop = true;
        gameManager.dropObject = other.gameObject;
        other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        other.gameObject.transform.rotation = Quaternion.Euler(-10f, 0f, 10f);
        prizesController.Prizes.Remove(other.gameObject);
    }
}
