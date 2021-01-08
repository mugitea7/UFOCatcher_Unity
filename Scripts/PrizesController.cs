using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrizesController : MonoBehaviour
{
    public Transform PrizesParent;
    public List<GameObject> Prizes = new List<GameObject>();
    public Dictionary<GameObject, Vector3> PrizePos = new Dictionary<GameObject, Vector3>();
    private GameManager gameManager;
    public Image RelocationButtonImage;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GetComponent<GameManager>();

        foreach(Transform child in PrizesParent)
        {
            Prizes.Add(child.gameObject);
        }

        foreach(GameObject pos in Prizes)
        {
            PrizePos.Add(pos, pos.transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.state == GameManager.State.WaitingMoveLeft)
            ButtonColorChange(Color.red);
        else
            ButtonColorChange(Color.white);
    }

    public void Relocation()
    {
        if (gameManager.state != GameManager.State.WaitingMoveLeft)
        {
            return;
        }

        foreach(GameObject obj in Prizes)
        {
            obj.transform.position = PrizePos[obj];
            obj.transform.rotation = Quaternion.Euler(-90f, Random.Range(-180f,180f), Random.Range(-180f, 180f));
        }
    }

    private void ButtonColorChange(Color color)
    {
        RelocationButtonImage.color = color;
    }

}
