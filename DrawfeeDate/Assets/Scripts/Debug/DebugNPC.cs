using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugNPC : MonoBehaviour {

	private EventManager em;
    private bool active;
    [SerializeField] private string event_file; 

	void Start () {
        em = GetComponent<EventManager>();
		active = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (active) {
            if (Input.GetMouseButtonDown(0)) {
                active = false;
                em.startEvent("/Events/" + event_file);
            }
        }
	}
}
