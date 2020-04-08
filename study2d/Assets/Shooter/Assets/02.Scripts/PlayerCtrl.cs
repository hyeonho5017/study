using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour {

    [System.Serializable]
    public class PlayerAnim
    {
        public AnimationClip idle;
        public AnimationClip runF;
        public AnimationClip runB;
        public AnimationClip runR;
        public AnimationClip runL;
    }

    public PlayerAnim playerAnim;
    public Animation anim;

    public float moveSpeed = 10.0f;
    public float rotSpeed = 110.0f;

    private float h = 0.0f;
    private float v = 0.0f;
    private float r = 0.0f;

    public Transform firePos;
    public GameObject bullet;
    public ParticleSystem cartrige;
    public ParticleSystem muzzle;

    private AudioSource _audio;
    [System.Serializable]
    public struct PlayerSfx
    {
        public AudioClip[] fire;
        public AudioClip[] reload;
    }

    public enum Weapontype
    {
        RIFLE, SHOTGUN
    }

    public Weapontype currWeapon = Weapontype.RIFLE;
    public PlayerSfx playerSfx;

    void Awake()
    {

    }

	// Use this for initialization
	void Start ()
    {
        anim = GetComponent<Animation>();
        anim.clip = playerAnim.idle;
        anim.Play();

        //차일드 중에서 맨 상단에서 부터 찾는다. ParticleSystem인 자료형인 놈들을 찾음.
        muzzle = firePos.GetComponentInChildren<ParticleSystem>();

        _audio = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        r = Input.GetAxis("Mouse X");
        Vector3 pos = new Vector3(h, 0, v);
        //Debug.Log("pos = " + pos.ToString());

        transform.Translate(pos.normalized * moveSpeed * Time.deltaTime);

        transform.Rotate(Vector3.up * r * rotSpeed * Time.deltaTime);

        if ( Input.GetMouseButtonDown(0))
        {
            Fire();
        }
        if(v >= 0.1f)
        {
            anim.CrossFade(playerAnim.runF.name, 0.3f);
        }
        else if(v <= -0.1f)
        {
            anim.CrossFade(playerAnim.runB.name, 0.3f);
        }
        else if (h >= 0.1f)
        {
            anim.CrossFade(playerAnim.runR.name, 0.3f);
        }
        else if (h <= -0.1f)
        {
            anim.CrossFade(playerAnim.runL.name, 0.3f);
        }
        else
        {
            anim.CrossFade(playerAnim.idle.name, 0.3f);
        }
    }

    void LateUpdate()
    {

    }

    void FixedUpdate()
    {

    }

    void Fire()
    {
        GameObject go = Instantiate(bullet,firePos.position, firePos.rotation);

        muzzle.Play();
        cartrige.Play();//눌렀을 때 재생하기
        //go.GetComponent<Rigidbody>().AddForce(firePos.forward * 700.0f);

        FireSfx();//총쏘기 소리 추가
    }
    void FireSfx()
    {
        var _sfx = playerSfx.fire[(int)currWeapon];
        _audio.PlayOneShot(_sfx, 1.0f);
    }
}
