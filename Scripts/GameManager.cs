using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Camera cam;
    private Vector3 CamOriginVec = new Vector3(0f, 12f, -10f);
    private Quaternion CamOriginRot = new Quaternion();

    private Vector3 DropItemCamPos = new Vector3(0f, 5f, -10f);
    private Quaternion DropItemCamRot = new Quaternion();

    private Vector3 LeftCameraPos = new Vector3(-11f, 12f, 0f);
    private Quaternion LeftCameraRot = new Quaternion();

    private Vector3 RightCameraPos = new Vector3(11f, 12f, 0f);
    private Quaternion RightCameraRot = new Quaternion();

    private PrizesController prizesController;

    //状態
    public enum State
    {
        Idle,
        WaitingMoveLeft,
        LeftMove,
        WaitingMoveFront,
        FrontMove,
        OpenArm,
        DownUFO,
        CloseArm,
        UpUFO,
        BackMove,
        RightMove,
        Drop,
        DoNothing
    };

    public enum CamState
    {
        Front,
        Left,
        Right,
        Drop
    };

    public State state;
    public CamState camState;

    public bool isDrop;

    //落ちたアイテムの変数
    public GameObject cover;
    public GameObject dropObject;
    private Vector3 ItemPos = new Vector3(0f, 4f, -6f);

    //UI
    public GameObject MoveUIs;  //動作中に表示するUI
    public GameObject GetUIs;   //景品入手時に表示するUI
    public GameObject ClearUIs; //クリア時に表示するUI

    public bool isStateTransitionToWaitingMoveLeft = false;

    // Start is called before the first frame update
    void Start()
    {
        //State
        state = State.WaitingMoveLeft;
        camState = CamState.Front;

        //VectorsAndRotations
        CamOriginRot = Quaternion.Euler(12f, 0f, 0f);
        DropItemCamRot = Quaternion.Euler(12f, 0f, 0f);
        LeftCameraRot = Quaternion.Euler(12f, 90f, 0f);
        RightCameraRot = Quaternion.Euler(12f, -90f, 0f);

        //CameraPosition
        cam.transform.position = CamOriginVec;
        cam.transform.rotation = CamOriginRot;

        prizesController = GetComponent<PrizesController>();

        MoveUIsActive();
    }

    // Update is called once per frame
    void Update()
    {
        Ray();

        //カメラの位置と回転
        if (camState == CamState.Front)
            SetCamPosAndRot(CamOriginVec, CamOriginRot);
        else if (camState == CamState.Left)
            SetCamPosAndRot(LeftCameraPos, LeftCameraRot);
        else if (camState == CamState.Right)
            SetCamPosAndRot(RightCameraPos, RightCameraRot);
        else if (camState == CamState.Drop)
            SetCamPosAndRot(DropItemCamPos, DropItemCamRot);
        else
            SetCamPosAndRot(CamOriginVec, CamOriginRot);

        //ボタンを左クリックから離したとき間
        if (Input.GetMouseButtonUp(0))
        {
            if (state == State.LeftMove)
                state = State.WaitingMoveFront;
            else if (state == State.FrontMove)
                state = State.OpenArm;
        }

        //モノが落ちたら
        if (isDrop)
        {
            GetUIsActive();
            DropDown();
        }
        else
        {
            cover.transform.rotation = Quaternion.Lerp(cover.transform.rotation, Quaternion.Euler(-90f, 0f, 0f), Time.deltaTime);
        }
    }

    void Ray()
    {
        Ray mouseRay = cam.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButton(0))
        {
            //ボタンを左クリックしたときの処理
            if (Physics.Raycast(mouseRay, out RaycastHit hit))
            {
                if (state == State.WaitingMoveLeft && hit.collider.gameObject.tag == "LeftButton")
                    state = State.LeftMove;
                else if (state == State.WaitingMoveFront && hit.collider.gameObject.tag == "FrontButton")
                    state = State.FrontMove;
            }
        }
    }

    //景品が落ちたとき
    private void DropDown()
    {
        camState = CamState.Drop;
        cover.transform.rotation = Quaternion.Lerp(cover.transform.rotation, Quaternion.Euler(0f, 0f, 0f), Time.deltaTime);
        dropObject.transform.position = Vector3.Lerp(dropObject.transform.position, ItemPos, Time.deltaTime);
        dropObject.transform.Rotate(new Vector3(0f, 45f, 0f) * Time.deltaTime);
    }

    //景品落下後から元に戻す
    public void DropUp()
    {
        if (prizesController.Prizes.Count > 0)
        {
            MoveUIsActive();
            camState = CamState.Front;
            isDrop = false;
            Destroy(dropObject);
        }
        else
        {
            camState = CamState.Front;
            Destroy(dropObject);
            isDrop = false;
            ClearUIsActive();
        }
    }

    //カメラの操作
    private void SetCamPosAndRot(Vector3 targetPos, Quaternion targetRot)
    {
        cam.transform.position = Vector3.Lerp(cam.transform.position, targetPos, Time.deltaTime);
        cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, targetRot, Time.deltaTime);
    }

    //UIの操作
    private void GetUIsActive()
    {
        isStateTransitionToWaitingMoveLeft = false;
        if (state == State.DoNothing)
            state = State.Idle;
        MoveUIs.SetActive(false);
        GetUIs.SetActive(true);
    }

    private void MoveUIsActive()
    {
        MoveUIs.SetActive(true);
        GetUIs.SetActive(false);
        ClearUIs.SetActive(false);
        isStateTransitionToWaitingMoveLeft = true;
        state = State.Idle;
    }

    private void ClearUIsActive()
    {
        MoveUIs.SetActive(false);
        GetUIs.SetActive(false);
        ClearUIs.SetActive(true);
        state = State.DoNothing;
    }

    //↑←ボタン
    public void OnPushMoveLeft()
    {
        if (state == State.WaitingMoveLeft)
            state = State.LeftMove;
    }

    public void OnPushMoveFront()
    {
        if (state == State.WaitingMoveFront)
            state = State.FrontMove;
    }

    public void CameraStateChangeToLeft()
    {
        switch (camState)
        {
            case CamState.Right:
                camState = CamState.Front;
                break;

            case CamState.Front:
                camState = CamState.Left;
                break;

            case CamState.Left:
                camState = CamState.Right;
                break;

            default:
                break;
        }
    }

    public void CameraStateChangeToRight()
    {
        switch (camState)
        {
            case CamState.Left:
                camState = CamState.Front;
                break;

            case CamState.Front:
                camState = CamState.Right;
                break;

            case CamState.Right:
                camState = CamState.Left;
                break;

            default:
                break;
        }
    }

    public void GameReset()
    {
        SceneManager.LoadScene("GameScene");
    }
}
