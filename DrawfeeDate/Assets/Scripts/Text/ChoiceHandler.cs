using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceHandler : MonoBehaviour {

	//Settings
    [SerializeField] private int choice_num_data;
    [SerializeField] private string choice_data;
    [SerializeField] private string[] choices_data;
    [SerializeField] private int[] skips_data;

    private EventManager em;

    //Variables
    private GameObject choice_obj;
    private GameObject[] choices_obj;

    private int player_choice;
    private int timer;

    //Init Event
    void Awake() {
        gameObject.transform.position = new Vector3(0f, 0f, 0f);

        choice_obj = new GameObject("Choice Text");
        choice_obj.transform.parent = transform;
        choice_obj.transform.position = new Vector3(0f, 0f, transform.position.z);
        choice_obj.AddComponent<TextHandler>().choice = true;
    }

    void Start() {
        choice_obj.GetComponent<TextHandler>().text = choice_data;

        choices_obj = new GameObject[choice_num_data];
        for (int i = 0; i < choice_num_data; i++) {
            choices_obj[i] = new GameObject("Choice Option " + (i + 1));
            choices_obj[i].transform.parent = transform;
            choices_obj[i].transform.position = new Vector3(0f, 0f, transform.position.z);
            choices_obj[i].AddComponent<ChoiceOptionHandler>().Text = choices_data[i];
        }

        if (choice_num_data == 4) {
            choices_obj[0].GetComponent<ChoiceOptionHandler>().Angle = 180;
            choices_obj[1].GetComponent<ChoiceOptionHandler>().Angle = 90;
            choices_obj[2].GetComponent<ChoiceOptionHandler>().Angle = 0;
            choices_obj[3].GetComponent<ChoiceOptionHandler>().Angle = 270;
            choices_obj[0].GetComponent<TextMesh>().anchor = TextAnchor.MiddleRight;
            choices_obj[0].GetComponent<TextMesh>().alignment = TextAlignment.Right;
            choices_obj[1].GetComponent<TextMesh>().anchor = TextAnchor.MiddleCenter;
            choices_obj[1].GetComponent<TextMesh>().alignment = TextAlignment.Center;
            choices_obj[2].GetComponent<TextMesh>().anchor = TextAnchor.MiddleLeft;
            choices_obj[2].GetComponent<TextMesh>().alignment = TextAlignment.Left;
            choices_obj[3].GetComponent<TextMesh>().anchor = TextAnchor.MiddleCenter;
            choices_obj[3].GetComponent<TextMesh>().alignment = TextAlignment.Center;
        }
        else if (choice_num_data == 3) {
            choices_obj[0].GetComponent<ChoiceOptionHandler>().Angle = 90;
            choices_obj[1].GetComponent<ChoiceOptionHandler>().Angle = 210;
            choices_obj[2].GetComponent<ChoiceOptionHandler>().Angle = 330;
            choices_obj[0].GetComponent<TextMesh>().anchor = TextAnchor.LowerCenter;
            choices_obj[0].GetComponent<TextMesh>().alignment = TextAlignment.Center;
            choices_obj[1].GetComponent<TextMesh>().anchor = TextAnchor.MiddleRight;
            choices_obj[1].GetComponent<TextMesh>().alignment = TextAlignment.Right;
            choices_obj[2].GetComponent<TextMesh>().anchor = TextAnchor.MiddleLeft;
            choices_obj[2].GetComponent<TextMesh>().alignment = TextAlignment.Left;
        }
        else {
            choices_obj[0].GetComponent<ChoiceOptionHandler>().Angle = 180;
            choices_obj[1].GetComponent<ChoiceOptionHandler>().Angle = 0;
            choices_obj[0].GetComponent<TextMesh>().anchor = TextAnchor.MiddleRight;
            choices_obj[1].GetComponent<TextMesh>().anchor = TextAnchor.MiddleLeft;
        }

        player_choice = -1;
        timer = 0;
    }

    //Check Clicked
    void Update() {
        bool clicked = false;
        if (PlayerManager.instance.cursorClick) {
            clicked = true;
        }

        if (player_choice != -1) {
            timer++;
            if (timer > 60) {
                em.setSkip(player_choice);
                em.continueEvent();
                Destroy(gameObject);
            }
        }
        else if (clicked) {
            Vector2 mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            int choice_clicked = -1;
            for (int i = 0; i < choice_num_data; i++) {
                if (choices_obj[i].GetComponent<Collider2D>().bounds.Contains(new Vector3(mouse_pos.x, mouse_pos.y, choices_obj[i].transform.position.z))) {
                    choice_clicked = i;
                    break;
                }
            }
            if (choice_clicked != -1) {
                player_choice = choice_clicked;
                for (int k = 0; k < choice_num_data; k++) {
                    choices_obj[k].GetComponent<ChoiceOptionHandler>().Active = false;
                }
            }
        }
    }

    //Setter Metheods
    public void setData(int num, string choice_text, string[] choice_text_array, int[] skips_array) {
        choice_num = num;
        choice = choice_text;
        choices = choice_text_array;
        skips = skips_array;
    }

    public string id {
        set {
            choice_obj.GetComponent<TextHandler>().id = value;
        }
    }

    public int choice_num {
        set {
            choice_num_data = value;
        }
    }

    public string choice {
        set {
            choice_data = value;
        }
    }

    public string[] choices {
        set {
            choices_data = value;
        }
    }

    public int[] skips {
        set {
            skips_data = value;
        }
    }

    public EventManager event_manager {
        set {
            em = value;
            choice_obj.GetComponent<TextHandler>().event_manager = value;
        }
    }
}

public class ChoiceOptionHandler : MonoBehaviour {

    //Settings
    private TextMesh mesh;
    private TextOutline outline;
    private BoxCollider2D col;

    //Variables
    private float alpha;
    private bool active;

    //Draw Settings
    private float angle;
    private float length_val;
    private float length;

    private float spd;
    private float radius;
    private float sin_val;
    private float rot_val;

    private float y_offset;
    private float size;
    private float big_size;
    private float small_size;
    private float rand_size_offset;

    //Init Event
    void Awake() {
        //Settings
        angle = UnityEngine.Random.Range(0f, 1f) * (2 * Mathf.PI);
        length_val = 0;
        length = 1.5f;

        spd = UnityEngine.Random.Range(0.8f, 1.2f) * 0.01f;
        radius = 0.14f;
        sin_val = UnityEngine.Random.Range(0f, 1f);
        rot_val = UnityEngine.Random.Range(0f, 1f);

        //Variables
        active = true;
        alpha = 0;

        y_offset = 1.4f;
        size = 0.08f;
        small_size = 0.08f;
        big_size = 0.122f;
        rand_size_offset = UnityEngine.Random.Range(0f, 1f);

        gameObject.transform.parent = transform;
        gameObject.transform.position = new Vector3(0, 0, transform.position.z);
        gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        mesh = gameObject.AddComponent<TextMesh>();
        mesh.font = Resources.Load<Font>("Fonts/KaoriGel");
        mesh.GetComponent<MeshRenderer>().material = Resources.Load<Font>("Fonts/KaoriGel").material;
        mesh.anchor = TextAnchor.MiddleCenter;
        mesh.alignment = TextAlignment.Center;
        mesh.fontSize = 64;
        mesh.color = new Color(1f, 1f, 1f, 0f);

        outline = gameObject.AddComponent<TextOutline>();
        outline.outlineColor = new Color(1f, 1f, 1f, 0f);
    }

    //Add Collider
    void Start() {
        wordWrap(mesh.text);
        col = gameObject.AddComponent<BoxCollider2D>();
    }

    //Update Event
    void Update() {
        //Debug
        if (Input.GetKeyDown(KeyCode.Space)) {
            alpha = 0;
            length_val = 0;
        }

        //Radial Draw Values
        sin_val += 0.004f;
        if (sin_val >= 1){
            sin_val = 0;
        }
        float draw_sin = Mathf.Sin(sin_val * 2 * Mathf.PI);
        rot_val += spd;
        if (rot_val > (2 * Mathf.PI)){
            rot_val = 0;
        }

        if (active) {
            length_val = Mathf.Lerp(length_val, length, Time.deltaTime * 1f);
            alpha = Mathf.SmoothStep(alpha, 1, Time.deltaTime * 5f);
        }
        else {
            length_val = Mathf.Lerp(length_val, 0, Time.deltaTime * 1.8f);
            alpha = Mathf.SmoothStep(alpha, 0, Time.deltaTime * 7f);
        }

        length_val = Mathf.Clamp(length_val, 0, length);
        alpha = Mathf.Clamp(alpha, 0, 1);
        Vector2 temp_position = new Vector2(transform.parent.transform.position.x + (Mathf.Cos(angle) * length_val), (transform.parent.transform.position.y + y_offset) + (Mathf.Sin(angle) * length_val));

        //Mouse Hover
        Vector2 mouse_pos = PlayerManager.instance.cursorPos;

        float temp_size = size;
        if (active) {
            if (gameObject.GetComponent<Collider2D>().bounds.Contains(new Vector3(mouse_pos.x, mouse_pos.y, transform.position.z))) {
                float size_sin = (Mathf.Sin((sin_val * 2 * Mathf.PI) + rand_size_offset) / 2.0f) + 1;
                temp_size = (size_sin * 0.01f) + big_size;
            }
            else {
                temp_size = small_size;
            }
        }
        else {
            temp_size = small_size;
        }
        size = Mathf.Lerp(size, temp_size, Time.deltaTime * 5f);
        transform.localScale = new Vector3(size, size, 1f);

        //Update Settings
        mesh.color = new Color(1f, 1f, 1f, alpha * alpha);
        outline.outlineColor = new Color(0f, 0f, 0f, alpha * alpha);
        transform.position = new Vector3(temp_position.x + (Mathf.Cos(rot_val) * (radius * draw_sin)), temp_position.y + (Mathf.Sin(rot_val) * (radius * draw_sin)), -5f);
        outline.transform.position = new Vector3(temp_position.x + (Mathf.Cos(rot_val) * (radius * draw_sin)), temp_position.y + (Mathf.Sin(rot_val) * (radius * draw_sin)), -4.99f);
        transform.rotation = Quaternion.Euler(20 * Mathf.Sin(Time.time * 5), 0f, 0f);
    }

    //Word Wrap
    protected void wordWrap(string val) {
        float rowLimit = 3f;
     
        string[] parts = val.Split(' ');
        string tmp = "";
        mesh.text = "";
        for (int i = 0; i < parts.Length; i++) {
            tmp = mesh.text;
       
            mesh.text += parts[i] + " ";
            if (GetComponent<MeshRenderer>().bounds.extents.x > rowLimit) {
                tmp += Environment.NewLine;
                tmp += parts[i] + " ";
                mesh.text = tmp;
            }
        }
    }

    //Getter & Setter Methods
    public string Text {
        set {
            mesh.text = value;
        }
    }

    public float Angle { 
        set {
            angle = Mathf.Deg2Rad * value;
        }
    }

    public bool Active {
        set {
            active = value;
        }
    }

}

 public class TextOutline : MonoBehaviour {
 
     public float pixelSize = 1;
     public Color outlineColor = Color.black;
     public bool resolutionDependant = false;
     public int doubleResolution = 1024;
 
     private TextMesh textMesh;
     private MeshRenderer meshRenderer;
 
     void Start() {
         textMesh = GetComponent<TextMesh>();    
         meshRenderer = GetComponent<MeshRenderer>();
 
         for (int i = 0; i < 8; i++) {
             GameObject outline = new GameObject("outline", typeof(TextMesh));
             outline.transform.parent = transform;
             outline.transform.localScale = new Vector3(1, 1, 1);
 
             MeshRenderer otherMeshRenderer = outline.GetComponent<MeshRenderer>();
             otherMeshRenderer.material = new Material(meshRenderer.material);
             otherMeshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
             otherMeshRenderer.receiveShadows = false;
             otherMeshRenderer.sortingLayerID = meshRenderer.sortingLayerID;
             otherMeshRenderer.sortingLayerName = meshRenderer.sortingLayerName;
         }
     }
     
     void LateUpdate() {
         Vector3 screenPoint = Camera.main.WorldToScreenPoint(transform.position);
 
         outlineColor.a = textMesh.color.a * textMesh.color.a;
 
         // copy attributes
         for (int i = 0; i < transform.childCount; i++) {
 
             TextMesh other = transform.GetChild(i).GetComponent<TextMesh>();
             other.color = outlineColor;
             other.text = textMesh.text;
             other.alignment = textMesh.alignment;
             other.anchor = textMesh.anchor;
             other.characterSize = textMesh.characterSize;
             other.font = textMesh.font;
             other.fontSize = textMesh.fontSize;
             other.fontStyle = textMesh.fontStyle;
             other.richText = textMesh.richText;
             other.tabSize = textMesh.tabSize;
             other.lineSpacing = textMesh.lineSpacing;
             other.offsetZ = textMesh.offsetZ;
 
             bool doublePixel = resolutionDependant && (Screen.width > doubleResolution || Screen.height > doubleResolution);
             Vector3 pixelOffset = GetOffset(i) * (doublePixel ? 2.0f * pixelSize : pixelSize);
             Vector3 worldPoint = Camera.main.ScreenToWorldPoint(screenPoint + pixelOffset);
             other.transform.position = worldPoint;
 
             MeshRenderer otherMeshRenderer = transform.GetChild(i).GetComponent<MeshRenderer>();
             otherMeshRenderer.sortingLayerID = meshRenderer.sortingLayerID;
             otherMeshRenderer.sortingLayerName = meshRenderer.sortingLayerName;
         }
     }
 
     Vector3 GetOffset(int i) {
         switch (i % 8) {
         case 0: return new Vector3(0, 1, 0);
         case 1: return new Vector3(1, 1, 0);
         case 2: return new Vector3(1, 0, 0);
         case 3: return new Vector3(1, -1, 0);
         case 4: return new Vector3(0, -1, 0);
         case 5: return new Vector3(-1, -1, 0);
         case 6: return new Vector3(-1, 0, 0);
         case 7: return new Vector3(-1, 1, 0);
         default: return Vector3.zero;
         }
     }
 }

/*
public class ChoiceOptionHandler : MonoBehaviour {

    //Settings
    private string text;
    private TextMesh mesh;

    private float offset_spd;
    private float offset_angle_spd;
    private float offset_radius;

    //Variables
    private float alpha;
    private float timer;

    private Vector2 pos;
    private Vector2 offset_dis;
    private float offset_angle;
    private float target_angle;

    //Init Event
    void Start() {
        //Settings
        offset_spd = 0.006f;
        offset_angle_spd = Mathf.PI * 0.5f;
        offset_radius = 0.5f;

        //Variables
        alpha = 0;

        gameObject.transform.parent = transform;
        gameObject.transform.position = new Vector3(0, 0, transform.position.z);
        gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        mesh = gameObject.AddComponent<TextMesh>();
        mesh.font = Resources.Load<Font>("Fonts/KaoriGel");
        mesh.GetComponent<MeshRenderer>().material = Resources.Load<Font>("Fonts/KaoriGel").material;
        mesh.anchor = TextAnchor.MiddleCenter;
        mesh.alignment = TextAlignment.Center;
        mesh.fontSize = 64;
        mesh.color = Color.white;
        mesh.text = text;
    }

    //Update Event
    void Update() {
        timer += 0.037f;
        if (timer > 1) {
            timer = 0;
        }

        if (timer == 0) {
            int rand_sign = 1;
            if (Random.Range(0, 10) < 5) {
                rand_sign = -1;
            }

            target_angle = offset_angle + (rand_sign * (offset_angle_spd * Random.Range(0.75f, 1.00f)));
        }
        offset_angle = Mathf.SmoothStep(offset_angle, target_angle, timer);
        float temp_spd = offset_spd * Random.Range(0.75f, 1.00f);
        Vector2 temp_dis = new Vector2(temp_spd * Mathf.Cos(offset_angle), temp_spd * Mathf.Sin(offset_angle));
        offset_dis += temp_dis;

        transform.position = new Vector3(pos.x + offset_dis.x, pos.y + offset_dis.y, transform.position.z);
    }

    //Setter Methods
    public string Text {
        set {
            text = value;
        }
    }


}
*/
