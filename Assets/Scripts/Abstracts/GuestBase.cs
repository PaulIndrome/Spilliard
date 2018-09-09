using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public abstract class GuestBase : MonoBehaviour, IPickupable, IShovable {
    [SerializeField] protected float forceToTopple = 1f, squirmTime = 10f, maxSquirmSpeed = 5f;
    protected Rigidbody rb;
    protected Collider col;
    protected Coroutine squirmRoutine;
    [HideInInspector] public bool shoved = false;

    public virtual void Start(){
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    public virtual void GetPickedUp(){
        squirmRoutine = StartCoroutine(SquirmTimer());
        throw new System.NotImplementedException();
    }

    public virtual void GetThrown(Vector3 velocity){
        transform.parent = null;
        ToggleKinematic(false);
        rb.AddForce(velocity * rb.mass, ForceMode.Impulse);
    }

    public virtual void JumpOff(){
        PlayerPickup.GuestJumpOff();
        transform.parent = null;
        ToggleKinematic(false);
        //throw new System.NotImplementedException();
    }

    public virtual void FlagShoved(){
        shoved = true;
    }

    public virtual void GetShoved(float force, Vector3 direction){
        if(!shoved){
            rb.AddForce(direction.normalized * force, ForceMode.Impulse);
            FlagShoved();
        }
    }

    public virtual void ToggleKinematic(bool onOff){
        rb.isKinematic = onOff;
        col.enabled = !rb.isKinematic;
    }

    public virtual IEnumerator SquirmTimer(){
        Vector3 localCenter = transform.InverseTransformPoint(col.bounds.center);
        for(float f = 0f; f < squirmTime; f = f + Time.deltaTime){
            if(PlayerThrow.throwing) {
                f = squirmTime;
                squirmRoutine = null;
                yield break;
            }
            else{
                transform.RotateAround(localCenter, Random.insideUnitSphere, (f/squirmTime) * maxSquirmSpeed * Time.deltaTime);
                yield return null;
            }
        }
        if(PlayerPickup.HoldingGuest) JumpOff();
        yield return null;
    }

}