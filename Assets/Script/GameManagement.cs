using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagement : MonoBehaviour
{
    enum State
    {
        //piecePosition�ɏ����l������
        Init,
        //�}�E�X�ŋ��u���ꏊ��I��
        Selection,
        //�I�����ꂽ�ꏊ�ɋ�u���邩�𔻒肷��
        Judgement,
        //piecePosition�̑I�����ꂽ�ꏊ�ɋ�̏���������,�Ֆʂ̍X�V
        Arrangement,
        //����v���C���[��ύX
        Change
    }
    State state = State.Init;
    private BoardManagement boardManagement;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(state);
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Init:
                if (Input.GetMouseButtonUp(0))
                {
                    Debug.Log(state);
                    boardManagement.Init();
                    state = State.Selection;
                }
                break;
            case State.Selection:
                if (Input.GetMouseButtonUp(0))
                {
                    Debug.Log(state);
                    state = State.Judgement;
                }
                break;
            case State.Judgement:
                if (Input.GetMouseButtonUp(0))
                {
                    Debug.Log(state);
                    state = State.Arrangement;
                }
                break;
            case State.Arrangement:
                if (Input.GetMouseButtonUp(0))
                {
                    Debug.Log(state);
                    state = State.Change;
                }
                break;
            case State.Change:
                if (Input.GetMouseButtonUp(0))
                {
                    Debug.Log(state);
                    state = State.Selection;
                }
                break;
        }

    }

    
}
