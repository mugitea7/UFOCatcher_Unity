using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOController : MonoBehaviour
{
    public GameObject Stick;
    public GameObject OriginStick;
    public GameObject UFOSphere;
    public GameObject LeftArm;
    public GameObject RightArm;

    public GameObject LeftArm1;
    public GameObject LeftArm2;
    public GameObject LeftArm3;

    public GameObject RightArm1;
    public GameObject RightArm2;
    public GameObject RightArm3;

    private List<GameObject> ArmList = new List<GameObject>();
    private Dictionary<GameObject, Vector3> ArmPos = new Dictionary<GameObject, Vector3>();

    private ArmController LeftArm1Controller;
    private ArmController LeftArm2Controller;
    private ArmController LeftArm3Controller;
    private ArmController RightArm1Controller;
    private ArmController RightArm2Controller;
    private ArmController RightArm3Controller;

    private readonly float moveSpeed = 1.5f;       //上下
    private readonly float moveSpeed2 = 2f;         //左右
    private readonly float moveSpeed3 = 1f;         //前後
    private readonly float maxDownDirection = -1.8f;
    private readonly float armRotateSpeed = 1f;

    private readonly float maxMoveToX = 3f;
    private readonly float maxMoveToZ = 2f;

    private Vector3 Origin;
    private Vector3 SphereOriginVec;
    private Vector3 StickOriginVec;

    private GameManager gameManager;

    private int count = 0;

    private bool readyToChangeStateToWaitingMoveLeft = true;

    // Start is called before the first frame update
    void Start()
    {
        Origin = transform.position;
        SphereOriginVec = UFOSphere.transform.position;
        StickOriginVec = Stick.transform.position;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        LeftArm1Controller = LeftArm1.GetComponent<ArmController>();
        LeftArm2Controller = LeftArm2.GetComponent<ArmController>();
        LeftArm3Controller = LeftArm3.GetComponent<ArmController>();

        RightArm1Controller = RightArm1.GetComponent<ArmController>();
        RightArm2Controller = RightArm2.GetComponent<ArmController>();
        RightArm3Controller = RightArm3.GetComponent<ArmController>();

        ArmList.Add(GameObject.Find("L1").gameObject);
        ArmList.Add(GameObject.Find("L2").gameObject);
        ArmList.Add(GameObject.Find("L3").gameObject);
        ArmList.Add(GameObject.Find("R1").gameObject);
        ArmList.Add(GameObject.Find("R2").gameObject);
        ArmList.Add(GameObject.Find("R3").gameObject);

        foreach(GameObject obj in ArmList)
        {
            ArmPos.Add(obj, obj.transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (gameManager.state)
        {
            case GameManager.State.Idle:
                if (gameManager.isStateTransitionToWaitingMoveLeft && readyToChangeStateToWaitingMoveLeft)
                    gameManager.state = GameManager.State.WaitingMoveLeft;
                break;

            case GameManager.State.WaitingMoveLeft:
                SetArmPos();
                transform.position = Origin;
                UFOSphere.transform.position = SphereOriginVec;
                Stick.transform.position = StickOriginVec;
                readyToChangeStateToWaitingMoveLeft = false;
                break;

            case GameManager.State.LeftMove:
                LeftMoveUFO(moveSpeed2);
                if(transform.position.x <= -maxMoveToX)
                {
                    gameManager.state = GameManager.State.WaitingMoveFront;
                }
                break;

            case GameManager.State.WaitingMoveFront:
                break;

            case GameManager.State.FrontMove:
                FrontMoveUFO(moveSpeed3);
                if(transform.position.z >= maxMoveToZ)
                {
                    gameManager.state = GameManager.State.OpenArm;
                }
                break;

            case GameManager.State.OpenArm:

                ArmSetPar();

                LeftArmOpen(armRotateSpeed);
                RightArmOpen(armRotateSpeed);
                if (LeftArm.transform.rotation.eulerAngles.z <= 315f)
                {
                    gameManager.state = GameManager.State.DownUFO;
                }
                break;

            case GameManager.State.DownUFO:
                DownUFO(moveSpeed);
                if(Stick.transform.localPosition.z <= maxDownDirection)
                {
                    gameManager.state = GameManager.State.CloseArm;
                }
                break;

            case GameManager.State.CloseArm:
                LeftArmClose(armRotateSpeed);
                RightArmClose(armRotateSpeed);
                if (LeftArm.transform.rotation.eulerAngles.z <= 314f)
                {
                    gameManager.state = GameManager.State.UpUFO;
                }
                break;

            case GameManager.State.UpUFO:
                UpUFO(moveSpeed);
                if (Stick.transform.localPosition.z >= -0.114f)
                {
                    ArmRemovePar();
                    gameManager.state = GameManager.State.BackMove;
                }
                break;

            case GameManager.State.BackMove:
                BackMoveUFO(moveSpeed3);
                Stick.transform.localPosition = new Vector3(0f, 0f, -0.114f);
                if (transform.position.z <= Origin.z)
                {
                    gameManager.state = GameManager.State.RightMove;
                }
                break;

            case GameManager.State.RightMove:
                RightMoveUFO(moveSpeed2);
                if (transform.position.x >= Origin.x)
                {
                    gameManager.state = GameManager.State.Drop;
                }
                break;

            case GameManager.State.Drop:
                StartCoroutine("DropArm");
                break;

            case GameManager.State.DoNothing:
                break;

            default:
                break;
        }
    }

    void LeftArmOpen(float angle)
    {
        LeftArm.transform.Rotate(new Vector3(0f, 0f, -angle));
    }

    void LeftArmClose(float angle)
    {
        LeftArm.transform.Rotate(new Vector3(0f, 0f, angle));
    }

    void RightArmOpen(float angle)
    {
        RightArm.transform.Rotate(new Vector3(0f, 0f, angle));
    }

    void RightArmClose(float angle)
    {
        RightArm.transform.Rotate(new Vector3(0f, 0f, -angle));
    }

    void DownUFO(float speed)
    {
        Stick.GetComponent<Rigidbody>().MovePosition(Stick.transform.position + new Vector3(0f, -1f, 0f) * speed * Time.deltaTime);
    }

    void UpUFO(float speed)
    {
        Stick.GetComponent<Rigidbody>().MovePosition(Stick.transform.position + new Vector3(0f, 1f, 0f) * speed * Time.deltaTime);
    }

    void LeftMoveUFO(float speed)
    {
        OriginStick.GetComponent<Rigidbody>().MovePosition(transform.position + new Vector3(-1f, 0f, 0f) * speed * Time.deltaTime);
        LeftArm1Controller.MoveLeft(speed);
        LeftArm2Controller.MoveLeft(speed);
        LeftArm3Controller.MoveLeft(speed);
        RightArm1Controller.MoveLeft(speed);
        RightArm2Controller.MoveLeft(speed);
        RightArm3Controller.MoveLeft(speed);
    }

    void RightMoveUFO(float speed)
    {
        OriginStick.GetComponent<Rigidbody>().MovePosition(transform.position + new Vector3(1f, 0f, 0f) * speed * Time.deltaTime);
        LeftArm1Controller.MoveRight(speed);
        LeftArm2Controller.MoveRight(speed);
        LeftArm3Controller.MoveRight(speed);
        RightArm1Controller.MoveRight(speed);
        RightArm2Controller.MoveRight(speed);
        RightArm3Controller.MoveRight(speed);
    }

    void FrontMoveUFO(float speed)
    {
        OriginStick.GetComponent<Rigidbody>().MovePosition(transform.position + new Vector3(0f, 0f, 1f) * speed * Time.deltaTime);
        LeftArm1Controller.MoveFront(speed);
        LeftArm2Controller.MoveFront(speed);
        LeftArm3Controller.MoveFront(speed);
        RightArm1Controller.MoveFront(speed);
        RightArm2Controller.MoveFront(speed);
        RightArm3Controller.MoveFront(speed);
    }

    void BackMoveUFO(float speed)
    {
        OriginStick.GetComponent<Rigidbody>().MovePosition(transform.position + new Vector3(0f, 0f, -1f) * speed * Time.deltaTime);
        LeftArm1Controller.MoveBack(speed);
        LeftArm2Controller.MoveBack(speed);
        LeftArm3Controller.MoveBack(speed);
        RightArm1Controller.MoveBack(speed);
        RightArm2Controller.MoveBack(speed);
        RightArm3Controller.MoveBack(speed);
    }

    private IEnumerator DropArm()
    {
        count++;
        ArmSetPar();
        LeftArmOpen(armRotateSpeed);
        RightArmOpen(armRotateSpeed);
        if (LeftArm.transform.rotation.eulerAngles.z <= 315f)
        {
            gameManager.state = GameManager.State.DoNothing;
        }

        yield return new WaitForSeconds(2f);

        LeftArmClose(armRotateSpeed);
        RightArmClose(armRotateSpeed);
        if (LeftArm.transform.rotation.eulerAngles.z <= 314f)
        {
            ArmRemovePar();
        }

        //コルーチンが重複して呼び出されているため、1回のみ以下の命令をするための応急処置
        count--;
        if (count == 0)
        {
            readyToChangeStateToWaitingMoveLeft = true;
            gameManager.state = GameManager.State.Idle;
        }
    }

    private void ArmSetPar()
    {
        LeftArm1Controller.SetPar(LeftArm.transform);
        LeftArm2Controller.SetPar(LeftArm.transform);
        LeftArm3Controller.SetPar(LeftArm.transform);

        RightArm1Controller.SetPar(RightArm.transform);
        RightArm2Controller.SetPar(RightArm.transform);
        RightArm3Controller.SetPar(RightArm.transform);
    }

    private void ArmRemovePar()
    {
        LeftArm1Controller.RemovePar();
        LeftArm2Controller.RemovePar();
        LeftArm3Controller.RemovePar();

        RightArm1Controller.RemovePar();
        RightArm2Controller.RemovePar();
        RightArm3Controller.RemovePar();
    }

    private void SetArmPos()
    {
        foreach(GameObject obj in ArmList)
        {
            obj.transform.position = ArmPos[obj];
        }
    }
}
