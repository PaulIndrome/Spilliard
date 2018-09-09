using UnityEngine;
public interface IPickupable {
    Transform transform { get; }
    void GetPickedUp();
    void GetThrown(Vector3 velocity);
    void JumpOff();
    void ToggleKinematic(bool onOff);
}