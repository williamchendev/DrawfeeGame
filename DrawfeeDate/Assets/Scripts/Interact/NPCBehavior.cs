using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBehavior : InteractableBehavior {

    //Components
    protected EventManager em;

    //Settings
    [SerializeField] protected bool loop;
    [SerializeField] protected string[] events;

    //Text Variables
    protected int path_num;
    protected string[] paths;

    //Inherited Methods
    protected override void init() {
        em = gameObject.AddComponent<EventManager>();
        path_num = 0;
        paths = events;

        base.init();
    }

    protected override void step() {
        base.step();
    }

    public override void action() {
        if (!em.event_active) {
            if (paths.Length != 0) {
                em.startEvent("/Events/" + paths[path_num]);
                path_num++;
                if (path_num >= paths.Length) {
                    path_num--;
                    if (loop) {
                        path_num = 0;
                    }
                }
            }
        }

        base.action();
    }

}
