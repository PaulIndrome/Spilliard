using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Interactible : MonoBehaviour {
    
	[SerializeField, EnumMask] InteractionType possibleInteractionTypes;
    public virtual bool Interact(InteractionType iType){
        if((possibleInteractionTypes & iType) != InteractionType.None){
            switch(iType){
                case InteractionType.Pickup:
                    GetPickedUp();
                    return true;
                case InteractionType.Shove:
                    GetShoved();
                    return true;
            }
        } else {
            return false;
        }
        return false;
    }

	public virtual void GetPickedUp(){
         throw new System.NotImplementedException();
    }

    public virtual void GetShoved(){
         throw new System.NotImplementedException();
    }
}
