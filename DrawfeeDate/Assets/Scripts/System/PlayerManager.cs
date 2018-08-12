using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    //Instance Variable
    public static PlayerManager instance {get; private set;}

    //Settings
    private bool canmove;

	//Init Event
	void Awake () {
        //Init Manager
		if (GameObject.FindGameObjectWithTag("GameController") != null) {
            Destroy(gameObject);
        }
        else {
            gameObject.tag = "GameController";
            instance = gameObject.GetComponent<PlayerManager>();
            DontDestroyOnLoad(gameObject);
        }

        //Settings
        canmove = true;
	}
	
	//Update Event
	void Update () {
		if (canmove) {
            //Select Interactable Object
            bool select = false;
            GameObject[] objects = sortByZAxis(GameObject.FindGameObjectsWithTag("Interact"));
            for (int i = 0; i < objects.Length; i++) {
                objects[i].GetComponent<InteractableBehavior>().selected = false;
                if (select != true) {
                    if (objects[i].GetComponent<InteractableBehavior>().containsPoint(cursorPos)) {
                        select = true;
                        objects[i].GetComponent<InteractableBehavior>().selected = true;

                        if (cursorClick) {
                            objects[i].GetComponent<InteractableBehavior>().action();
                        }
                    }
                }
            }

        }
	}

    //Methods
    public GameObject[] sortByZAxis(GameObject[] array) {
        GameObject temp;
        for (int i = 0; i < array.Length; i++) {
            for (int j = 0; j < array.Length; j++) {
                if (array[i].transform.position.z < array[j].transform.position.z) {
                    temp = array[i];
                    array[i] = array[j];
                    array[j] = temp;
                }
            }
        }
        return array;
    }

    //Getter & Setter Methods
    public Vector2 cursorPos {
        get{
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return pos;
        }
    }

    public bool cursorClick {
        get {
            if (Input.GetMouseButtonDown(0)) {
                return true;
            }
            return false;
        }
    }

    public bool canMove {
        set {
            canmove = value;
        }
    }

}
