using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

    public enum State
    {
        PATROL,TRACE, ATTACK,DIE
    }

    public State state = State.PATROL;
    private Transform playerTf;
    private Transform enemyTf;

    public float attackDist = 3.0f;
    public float traceDist = 6.0f;

    public bool isDie = false;
    private WaitForSeconds ws;
    private MoveAgent moveAgent;

    //제일제일 맨 처음 모든 객체 초기화용도
    void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if(player)
        {
            playerTf = player.GetComponent<Transform>();
        }
        enemyTf = GetComponent<Transform>();
        ws = new WaitForSeconds(0.3f);//0.3초 만큼 지연시키는 기능

        moveAgent = GetComponent<MoveAgent>();
    }
    //활성화된 오브젝트에 실행
    //코루틴 찾아보기
    void OnEnable()
    {
        StartCoroutine(CheckState());
        StartCoroutine(Action());
    }

    IEnumerator CheckState()
    {
        while(isDie == false)
        {
            if (state == State.DIE) yield break; //Die됬을때 빠져나가고, 아니면 계속 돌고

            float dist = Vector3.Distance(playerTf.position, enemyTf.position);

            if(dist <= attackDist)
            {
                state = State.ATTACK;
            }
            else if(dist <= traceDist)
            {
                state = State.TRACE;
            }
            else
            {
                state = State.PATROL;
            }
            yield return ws;//프로세스 권한 양도해주기 while만 돌리면 안되니까
        }
    }

    IEnumerator Action()
    {
        while (isDie == false)
        {
            yield return ws;

            switch(state)
            {
                case State.PATROL:
                    moveAgent.patrolling = true;
                    break;
                case State.TRACE:
                    moveAgent.traceTarget = playerTf.position;
                    break;
                case State.ATTACK:
                    moveAgent.Stop();
                    break;
                case State.DIE:
                    moveAgent.Stop();
                    break;
            }
        }
    }

    //스타트 전에 onenable이 실행
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
