using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagement : MonoBehaviour
{
    enum State
    {
        Test,

        //piecePosition�ɏ����l������
        Init,
        //�}�E�X�ŋ��u���ꏊ��I��
        Selection,
        //�I�����ꂽ�ꏊ�ɋ�u���邩�𔻒肷��
        Judgement,
        //piecePosition�̑I�����ꂽ�ꏊ�ɋ�̏���������,�Ֆʂ̍X�V
        Arrangement,
        //����v���C���[��ύX
        Change,
        //���s���~�߂�
        Result
    }
    State state = State.Init;
    private BoardManagement boardManagement;
    public Vector2Int cellpos;
    private int playerTurn;
    private int blockageCounter = 0;
    private bool callConfirmation;

    // Start is called before the first frame update
    void Start()
    {
        //state = State.Test;
        boardManagement = GetComponent<BoardManagement>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Init:
                Debug.Log(state);
                boardManagement.Init();
                boardManagement.GeneratePiece();
                callConfirmation = true;
                state = State.Selection;
                break;
            case State.Selection:
                if (!boardManagement.BlockageJudgment(playerTurn, blockageCounter))
                {
                    Debug.Log("�l��");
                    state = State.Change;
                    break;
                }
                if (Input.GetMouseButtonUp(0))
                {
                    Debug.Log(state);
                    Vector3? cellPos3 = null;
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
                    {
                        cellPos3 = hit.collider.gameObject.transform.position;
                    }
                    if (cellPos3 is null)
                    {
                        break;
                    }
                    cellpos = FunctionStorage.Vector3ToVector2(cellPos3.Value);
                    Debug.Log((int)(-1 + 0.5f));
                    boardManagement.Intermediary(cellpos);
                    state = State.Judgement;
                }
                break;
            case State.Judgement:
                if (boardManagement.Judge(playerTurn))
                {
                    Debug.Log("�W���b�W�ʂ�����");
                    state = State.Arrangement;
                }
                else
                {
                    Debug.Log("�W���b�W�ʂ��ĂȂ���");
                    state = State.Selection;
                }
                break;
            case State.Arrangement:
                /*
                 * Arrangenment�֐�������Ď��s����
                 */
                boardManagement.Arrangement(playerTurn);
                boardManagement.GeneratePiece();
                state = State.Change;
                break;
            case State.Change:
                if (boardManagement.EndJudge())
                {
                    playerTurn += 1;
                    //boardManagement.BoardPrint();
                    state = State.Selection;
                }
                else
                {
                    state = State.Result;
                }
                break;
            case State.Result:
                if (callConfirmation)
                {
                    SceneManager.LoadScene("Result", LoadSceneMode.Additive);
                    callConfirmation = false;
                }
                break;
        }

    }


}
