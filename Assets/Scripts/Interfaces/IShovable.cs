using UnityEngine;
public interface IShovable {
    Transform transform { get; }
    void GetShoved(float force, Vector3 direction);

    void FlagShoved();
}