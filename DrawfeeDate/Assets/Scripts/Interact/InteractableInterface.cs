using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableInterface : MonoBehaviour {

    protected abstract void init();
    protected abstract void step();
    public abstract void action();

}