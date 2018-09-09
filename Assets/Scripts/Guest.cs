using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guest : GuestBase {

    ParticleSystem spill;
    [SerializeField, Range(0f,1f)] float uprightThreshold = 0.7f;
    [SerializeField] float timeToCheckForUpright = 5f, uprightCheckInterval = 0.2f;
    //new CapsuleCollider col;

    public override void Start(){
        base.Start();
        spill = GetComponentInChildren<ParticleSystem>();
        //col = GetComponent<CapsuleCollider>();
    }

    void OnCollisionEnter(Collision col){
        if(!shoved && col.impulse.magnitude > forceToTopple){
            //Debug.Log("CollisionEnter with enough force of " + col.impulse.magnitude + " on " + gameObject.name);
            FlagShoved();
        } else {
            StartCoroutine(CheckFallenOver());
        }
    }

    public override void FlagShoved(){
        spill.Play();
        shoved = true;
    }

    IEnumerator CheckFallenOver(){
        bool fallenOver = false;
        float checkTime = 0f;
        while(!fallenOver && !shoved && checkTime < timeToCheckForUpright){
            if(transform.up.y < uprightThreshold){
                fallenOver = true;
                FlagShoved();
            }
            checkTime += Time.deltaTime + uprightCheckInterval;
            yield return new WaitForSeconds(uprightCheckInterval);
        }
        yield return null;
    }

    void OnDestroy(){
        StopAllCoroutines();
    }

}