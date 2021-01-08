using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightsController : MonoBehaviour
{

    public Light LeftButtonLight;
    public Light FrontButtonLight;

    public Image LeftMoveButton;
    public Image FrontMoveButton;

    public GameObject Lights;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.state == GameManager.State.WaitingMoveLeft || gameManager.state == GameManager.State.LeftMove)
        {
            ButtonFlash(true, false);
            SetButtonActive(true, false);
        }
        else if (gameManager.state == GameManager.State.WaitingMoveFront || gameManager.state == GameManager.State.FrontMove)
        {
            ButtonFlash(false, true);
            SetButtonActive(false, true);
        }
        else
        {
            ButtonFlash(false, false);
            SetButtonActive(false, false);
        }
    }

    private void ButtonFlash(bool isLeftButtonFlash,bool isFrontButtonFlash)
    {
        LeftButtonLight.gameObject.SetActive(isLeftButtonFlash);
        FrontButtonLight.gameObject.SetActive(isFrontButtonFlash);
    }

    private void SetButtonActive(bool isLeftButtonFlash, bool isFrontButtonFlash)
    {
        LeftMoveButton.gameObject.SetActive(isLeftButtonFlash);
        FrontMoveButton.gameObject.SetActive(isFrontButtonFlash);
    }
}
