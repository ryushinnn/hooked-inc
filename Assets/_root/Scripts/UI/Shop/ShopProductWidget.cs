using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopProductWidget : MonoBehaviour {
    [SerializeField] private State _state;
    
    public State GetState() {
        return _state;
    }
    
    public enum State {
        Equipped,
        Owned,
        NotOwned,
    }
}
