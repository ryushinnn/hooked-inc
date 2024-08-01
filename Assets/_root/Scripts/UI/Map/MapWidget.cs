using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapWidget : MonoBehaviour {
    [SerializeField] private State _state;
    
    public State GetState() {
        return _state;
    }
    
    public enum State {
       Locked,
       Unlocked
    }
}
