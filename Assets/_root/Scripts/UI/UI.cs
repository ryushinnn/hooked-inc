using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UI : MonoBehaviour
{
    public virtual void Open(params object[] prs){}
    public virtual void Close(){}
}
