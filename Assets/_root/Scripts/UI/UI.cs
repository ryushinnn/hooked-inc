using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UI : MonoBehaviour
{
    public virtual void OnOpen(params object[] prs) {
        gameObject.SetActive(true);
    }

    public virtual void OnClose() {
        gameObject.SetActive(false);
    }
}
