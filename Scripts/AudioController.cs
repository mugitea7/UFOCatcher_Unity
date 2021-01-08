using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{

    private AudioSource UFOUpDownSE;
    private AudioSource UFOMoveSE;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        AudioSource[] audio = GetComponents<AudioSource>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        UFOUpDownSE = audio[1];
        UFOMoveSE = audio[2];
    }

    // Update is called once per frame
    void Update()
    {
        switch (gameManager.state)
        {
            case GameManager.State.OpenArm:
                break;

            case GameManager.State.DownUFO:
                if (!UFOUpDownSE.isPlaying)
                    UFOUpDownSE.PlayOneShot(UFOUpDownSE.clip);
                break;

            case GameManager.State.CloseArm:
                UFOUpDownSE.Stop();
                break;

            case GameManager.State.UpUFO:
                if (!UFOUpDownSE.isPlaying)
                    UFOUpDownSE.PlayOneShot(UFOUpDownSE.clip);
                break;


            case GameManager.State.LeftMove:
                if (!UFOMoveSE.isPlaying)
                {
                    UFOMoveSE.PlayOneShot(UFOMoveSE.clip);
                }
                break;

            case GameManager.State.RightMove:
                if (!UFOMoveSE.isPlaying)
                {
                    UFOMoveSE.PlayOneShot(UFOMoveSE.clip);
                }
                break;

            case GameManager.State.FrontMove:
                if (!UFOMoveSE.isPlaying)
                {
                    UFOMoveSE.PlayOneShot(UFOMoveSE.clip);
                }
                break;

            case GameManager.State.BackMove:
                UFOUpDownSE.Stop();
                if (!UFOMoveSE.isPlaying)
                {
                    UFOMoveSE.PlayOneShot(UFOMoveSE.clip);
                }
                break;
        }
    }
}
