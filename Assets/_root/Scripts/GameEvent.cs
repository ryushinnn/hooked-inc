using UnityEngine;

public static class GameEvent {
    public delegate void OnAvatarChanged(Sprite spr);
    public delegate void OnXpChanged(int lv, int curXp, int nextXp);
    public delegate void OnHookChanged(float hook);
    public delegate void OnMoneyChanged(float money);
    public delegate void OnFloatingTextRequested(string value, Vector3 worldPosition);
}
