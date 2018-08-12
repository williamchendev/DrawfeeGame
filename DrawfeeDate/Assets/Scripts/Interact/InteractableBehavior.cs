using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBehavior : InteractableInterface {

	//Components
    protected OutlineObject outline;
    protected SpriteRenderer sr;
    protected Collider2D col;

    //Setting
    public bool selected;

    protected bool outline_active;
    protected float outline_alpha;

    //Init Event
    protected void Awake() {
        gameObject.tag = "Interact";
        outline = gameObject.AddComponent<OutlineObject>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        if (gameObject.GetComponent<Collider2D>() != null) {
            col = gameObject.GetComponent<PolygonCollider2D>();
        }
        else {
            col = gameObject.AddComponent<PolygonCollider2D>();
        }

        init();
    }

    //Update Event
    protected void Update() {
        step();
    }

    //Inherited Methods
    protected override void init() {
        selected = false;

        outline_active = true;
        outline_alpha = 0;

        outline.outlineMaterial = Resources.Load<Material>("Materials/Sprites-Outline");
        outline.outlineSize = 10;
        outline.outlineBlur = 6;
        outline.outlineColor = Color.white;
        outline.Regenerate();
    }

    protected override void step() {
        if (selected) {
            outline_alpha = Mathf.Lerp(outline_alpha, 0.8f, Time.deltaTime * 5);
        }
        else {
            outline_alpha = Mathf.Lerp(outline_alpha, 0, Time.deltaTime * 5);
        }

        if (outline_active) {
            outline.outlineColor = new Color(1f, 1f, 1f, outline_alpha);
            outline.Regenerate();
        }
        else {
            outline.Clear();
        }
    }

    public override void action() {
        selected = false;
    }

    //Methods
    public bool containsPoint(Vector2 point) {
        if (col.bounds.Contains(new Vector3(point.x, point.y, transform.position.z))) {
            return true;
        }
        return false;
    }

}