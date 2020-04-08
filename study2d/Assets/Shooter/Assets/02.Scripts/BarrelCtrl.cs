using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]//여기 붙은 컴포넌트에 audio sorce가 없으면 자동으로 붙여줌
public class BarrelCtrl : MonoBehaviour {


    public GameObject expEffect;
    private int hitCount = 0;
    private Rigidbody rb;

    public Mesh[] meshes;
    private MeshFilter meshFilter;

    public Texture[] textures;
    private MeshRenderer _renderer;

    public float expRadius = 10.0f;

    private AudioSource _audio;
    public AudioClip expSfx;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        meshFilter = GetComponent<MeshFilter>();

        _renderer = GetComponent<MeshRenderer>();

        _renderer.material.mainTexture = textures[Random.Range(0, textures.Length)];//매시의 텍스쳐가 렌덤하게 입혀짐.

        _audio = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update () {
		
	}

    void OnCollisionEnter(Collision coll)
    {
        if(coll.collider.CompareTag("Bullet"))
        {
            if(++hitCount ==3)
            {
                Explosion();
            }
        }
    }

    void Explosion()
    {
        GameObject go = Instantiate(expEffect, transform.position, Quaternion.identity);

        //rb.mass = 1.0f;
        //rb.AddForce(Vector3.up * 1000.0f); //기본 윗방향 삭제
        IndirectDamage(transform.position);

        Destroy(go, 2.0f);

        int idx = Random.Range(0, meshes.Length);
        meshFilter.sharedMesh = meshes[idx];
        GetComponent<MeshCollider>().sharedMesh = meshes[idx];

        //폭발 소리 추가
        _audio.PlayOneShot(expSfx,1.0f);
    }

    void IndirectDamage(Vector3 pos)
    {
        Collider[] colls = Physics.OverlapSphere(pos, expRadius,1 << 9);//겹치는 영역이 있는지 구체로 판단하기(9번 레이어 쓴다는 의미) //| 1<< 8 

        //auto랑 똑같은 방식
        foreach (var coll in colls)
        {
            var _rb = coll.GetComponent<Rigidbody>();
            _rb.mass = 1.0f;
            _rb.AddExplosionForce(1000.0f,pos,expRadius,1000.0f);
        }

    }
}
