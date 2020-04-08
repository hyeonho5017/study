using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MoveAgent : MonoBehaviour {

    public List<Transform> wayPoints;
    public int nextIdx;

    private NavMeshAgent agent;

    //변하지 않는 변수에 대해서 readonly를 붙여서 메모리를 효율적으로 쓸수 있게 해주자.
    private readonly float patrolSpeed = 1.5f;
    private readonly float traceSpeed = 2.5f;

    private bool _patrolling;
    public bool patrolling
    {
        get
        {
            return _patrolling;
        }
        set
        {
            _patrolling = value;
            if(_patrolling)
            {
                agent.speed = patrolSpeed;
                MoveWayPoint();
            }
        }
    }

    private Vector3 _traceTarget;
    public Vector3 traceTarget
    {
        get { return _traceTarget;}
        set
        {
            _traceTarget = value;
            agent.speed = traceSpeed;
            TraceTarget(_traceTarget);
        }
    }

    void TraceTarget(Vector3 pos)
    {
        if (agent.isPathStale) return;

        agent.destination = pos;
        agent.isStopped = false;
    }

    public void Stop()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        _patrolling = false;
    }

    // Use this for initialization
    void Start () {
        var group = GameObject.Find("WayPointGroup");//이름으로 GameObject를 찾음!
        if(group)
        {
            group.GetComponentsInChildren<Transform>(wayPoints);//오버로드 목록중에 List자체를 받는 것이 있음.
            wayPoints.RemoveAt(0);
        }
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        agent.speed = patrolSpeed;

        MoveWayPoint();
    }

    void MoveWayPoint()
    {
        if (agent.isPathStale) return;//현재 갖고 있는 경로가 오래된 건지 아닌지

        agent.destination = wayPoints[nextIdx].position;
        agent.isStopped = false;
    }
	
	// Update is called once per frame
	void Update () {

        if (_patrolling == false) return;

		if(agent.velocity.sqrMagnitude >= 0.2f * 0.2f && agent.remainingDistance <= 0.5f)//매그니튜드는 벡터의 크기, 스퀘어는 제곱연산, 루트 연산이 느리니까 그냥 제곱값 씀.
        {
            nextIdx = ++nextIdx % wayPoints.Count;
            MoveWayPoint();
        }
	}
}
