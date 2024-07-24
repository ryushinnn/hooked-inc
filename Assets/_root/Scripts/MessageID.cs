using UnityEngine;

public static class MessageID {
    public delegate void OnMoneyUpdated(float amount);
    public delegate void OnFloatingTextRequested(string value, Vector3 worldPosition);
}
