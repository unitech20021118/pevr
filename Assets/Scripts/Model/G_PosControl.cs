using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class G_PosControl : MonoBehaviour
{
    
    public GameObject pos;
    public GameObject target_obj;
    private GameObject pos_x;
    private GameObject pos_y;
    private GameObject pos_z;

    private float distance;
    private GameObject xz;
    private GameObject xy;
    private GameObject zy;

    private GameObject pos_center;

    private Ray ray;
    private RaycastHit hit;

    private LayerMask layer_transform;
    private LayerMask layer_xyz;
    private bool is_pos_x;

    private bool is_pos_y;

    private bool is_pos_z;

    private bool is_yz;

    private bool is_xz;

    private bool is_xy;

    private bool is_pos_center;

    private Vector3 oldMousePos = new Vector3(0f, 0f, 0f);

    private Vector3 delta = new Vector3(0f, 0f, 0f);

    private Vector3 dir1;

    private Vector3 originPos;
    Vector3 dir;
    // Use this for initialization
    void Start()
    {
        layer_transform = 1 << LayerMask.NameToLayer("prs");
        layer_xyz = 1 << LayerMask.NameToLayer("xyz");
        pos_x = transform.Find("pos_x").gameObject;
        pos_y = transform.Find("pos_y").gameObject;
        pos_z = transform.Find("pos_z").gameObject;

        xz = transform.Find("xz").gameObject;
        xy = transform.Find("xy").gameObject;
        zy = transform.Find("zy").gameObject;

        pos_center = transform.Find("pos_center").gameObject;

    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        distance = Vector3.Distance(Camera.main.transform.position, pos_center.transform.position);
        pos.transform.rotation = Quaternion.Euler(Vector3.zero);
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 3.40282E+38f, layer_transform) && Input.GetMouseButtonDown(0) && hit.collider.tag != "floor")
        {
            if (hit.collider.name == "pos_x")
            {
                pos_x.GetComponent<Renderer>().material.color = Color.yellow;
                dir = (pos_x.transform.position - pos.transform.position).normalized;
                is_pos_x = true;
            }

            if (hit.collider.name == "pos_y")
            {
                pos_y.GetComponent<Renderer>().material.color = Color.yellow;
                dir = (pos_y.transform.position - pos.transform.position).normalized;
                is_pos_y = true;
            }
            if (hit.collider.name == "pos_z")
            {
                pos_z.GetComponent<Renderer>().material.color = Color.yellow;
                dir = (pos_z.transform.position - pos.transform.position).normalized;
                is_pos_z = true;
            }
            if (hit.collider.name == "xy")
            {
                xy.GetComponent<Renderer>().material.color = new Color(0f, 0f, 1f, 0.65f);
                dir = (xy.transform.position - pos.transform.position).normalized;
                is_xy = true;
            }
            if (hit.collider.name == "zy")
            {
                zy.GetComponent<Renderer>().material.color = new Color(1f, 0f, 0f, 0.65f);
                dir = (zy.transform.position - pos.transform.position).normalized;
                is_yz = true;
            }
            if (hit.collider.name == "xz")
            {
                xz.GetComponent<Renderer>().material.color = new Color(0f, 1f, 0f, 0.65f);
                dir = (xz.transform.position - pos.transform.position).normalized;
                is_xz = true;
            }
            if (hit.collider.name == "pos_center")
            {
                pos_center.GetComponent<Renderer>().material.color = Color.yellow;
                is_pos_center = true;
            }

        }
        if (Input.GetMouseButtonDown(0))
        {
            //originPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            originPos = Input.mousePosition;
        }
        Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitxyz;
        if (Physics.Raycast(ray2, out hitxyz, 3.40282347E+38f, layer_transform))
        {
        }
        if (Input.GetMouseButtonUp(0))
        {
            this.pos_x.GetComponent<Renderer>().material.color = Color.red;
            this.pos_y.GetComponent<Renderer>().material.color = Color.green;
            this.pos_z.GetComponent<Renderer>().material.color = Color.blue;
            this.zy.GetComponent<Renderer>().material.color = new Color(0f, 0f, 1f, 0.4f);
            this.xz.GetComponent<Renderer>().material.color = new Color(1f, 0f, 0f, 0.4f);
            this.xy.GetComponent<Renderer>().material.color = new Color(0f, 1f, 0f, 0.4f);
            this.pos_center.GetComponent<Renderer>().material.color = Color.gray;
            this.pos_center.GetComponent<Renderer>().material.color = Color.gray;
            this.is_pos_x = false;
            this.is_pos_y = false;
            this.is_pos_z = false;
            this.is_pos_center = false;
            this.is_xy = false;
            this.is_yz = false;
            this.is_xz = false;

        }
        if (Input.GetMouseButton(0) && this.target_obj.tag != "Terrain")
        {
            Vector3 vector = Camera.main.WorldToScreenPoint(this.target_obj.transform.position);
            Vector3 vector2 = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, vector.z));

            if (this.is_pos_x || this.is_pos_y || this.is_pos_z)
            {
                dir1 = (vector2 - oldMousePos).normalized;
                float dot = Vector3.Dot(dir, dir1);
                float mouseY = Input.GetAxis("Mouse Y");
                float mouseX = Input.GetAxis("Mouse X");
                Vector3 mouseXY = new Vector3(mouseX, mouseY, 0f);
                float d = Vector3.Magnitude(mouseXY) * 0.07f;
                Vector3 position = this.target_obj.transform.position + this.dir * dot * d*distance;
                //if (position.y < 0f)
                //{
                //    this.target_obj.transform.position = new Vector3(position.x, 0f, position.z);
                //    return;
                //}
                this.target_obj.transform.position = position;
                //oldMousePos = vector2;
            }
            //else if (is_xy || is_xz || is_yz)
            //{
            //    if (this.delta == new Vector3(0f, 0f, 0f))
            //    {
            //        this.delta = hitxyz.point - target_obj.transform.position;
            //    }
            //    Vector3 position2 = hitxyz.point - this.delta;
            //    //if (position2.y < 0f)
            //    //{
            //    //    this.target_obj.transform.position = new Vector3(position2.x, 0f, position2.z);
            //    //    base.transform.position = this.target_obj.transform.position;
            //    //    this.oldMousePos = vector2;
            //    //    return;
            //    //}
            //    this.target_obj.transform.position = position2;
            //}
            if (this.is_pos_center)
            {
                this.target_obj.transform.position = vector2;
            }
            base.transform.position = this.target_obj.transform.position;
            this.oldMousePos = vector2;


            //if (is_pos_x)
            //{
            //    //Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //    //Debug.Log(mousePosition);
            //    //Vector3 a = mousePosition - originPos;
            //    //float offset = Vector3.Dot(dir, Camera.main.ScreenToWorldPoint(Input.mousePosition) - originPos);
            //    //Vector3 temp = target_obj.transform.position + offset * dir;
            //    //target_obj.transform.position = temp;
            //    //originPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //    float offset3 = Input.mousePosition.x - originPos.x;
            //    target_obj.transform.position = new Vector3(target_obj.transform.position.x - offset3, target_obj.transform.position.y, target_obj.transform.position.z);
            //    originPos = Input.mousePosition;
            //}

            //    if (is_pos_y)
            //    {
            //        float offset2 = Input.mousePosition.y - originPos.y;
            //        target_obj.transform.position = new Vector3(target_obj.transform.position.x, target_obj.transform.position.y + offset2, target_obj.transform.position.z);
            //        originPos = Input.mousePosition;
            //    }
            //    if (is_pos_z)
            //    {
            //        float offset3 = Input.mousePosition.x - originPos.x;
            //        target_obj.transform.position = new Vector3(target_obj.transform.position.x, target_obj.transform.position.y, target_obj.transform.position.z + offset3);
            //        originPos = Input.mousePosition;
            //    }
            //    if (is_xy)
            //    {
            //        float offsetY = Input.mousePosition.y - originPos.y;
            //        float offsetX = Input.mousePosition.x - originPos.x;
            //        target_obj.transform.position = new Vector3(target_obj.transform.position.x-offsetX, target_obj.transform.position.y+offsetY, target_obj.transform.position.z);
            //        originPos = Input.mousePosition;
            //    }
            //    if (is_xz)
            //    {
            //        //float offsetY = Input.mousePosition.y - originPos.y;
            //        float offsetX = Input.mousePosition.x - originPos.x;
            //        target_obj.transform.position = new Vector3(target_obj.transform.position.x-offsetX, target_obj.transform.position.y, target_obj.transform.position.z + offsetX);
            //        originPos = Input.mousePosition;
            //    }
            //}


        }

    }
}
