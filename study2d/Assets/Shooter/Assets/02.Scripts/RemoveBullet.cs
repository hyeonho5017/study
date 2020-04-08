using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour {

    public GameObject sparkEffect;

    private void OnCollisionEnter(Collision coll)
    {
        if(coll.collider.CompareTag("Bullet"))
        {
            ShowEffect(coll);
            Destroy(coll.gameObject);//,2.0f 초 지정
        }
    }

    private void ShowEffect(Collision coll)
    {
        ContactPoint contact = coll.contacts[0];//충돌 했을때의 충돌 정보를 받아옴. 부딪힌 좌표 위치중에 첫번째를 가져다 쓰겠다.
        Quaternion rot = Quaternion.FromToRotation(Vector3.forward, -contact.normal);

        GameObject go = Instantiate(sparkEffect, contact.point - contact.normal * 0.3f, rot);
        go.transform.SetParent(transform);
    }
}
