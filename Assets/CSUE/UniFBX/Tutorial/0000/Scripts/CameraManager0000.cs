//using UnityEngine;
using UnityEngine;
using System.Collections;

public class CameraManager0000 : MonoBehaviour {

    public UniFBXImport ufbx;
    public float cameraSensitivity = 90;
    public float climbSpeed = 4;
    public float normalMoveSpeed = 10;
    public float slowMoveFactor = 0.25f;
    public float fastMoveFactor = 3;
    private float rotationX = 0.0f;
    private float rotationY = 0.0f;
    private Vector3 position;
    private Quaternion rotation;
    private float z = 0.0f;


    void Start ( ) {
        //position = this.transform.position;
        //rotation = this.transform.localRotation;
        //Screen.lockCursor = true;
    }

    void Update ( ) {
        if (Input.GetMouseButton (1)) {
            Cursor.lockState = CursorLockMode.Locked;
            //Screen.lockCursor = true;
            rotationX += Input.GetAxis ("Mouse X") * cameraSensitivity * Time.deltaTime;
            rotationY += Input.GetAxis ("Mouse Y") * cameraSensitivity * Time.deltaTime;
            rotationY = Mathf.Clamp (rotationY, -90, 90);

            rotation = Quaternion.AngleAxis (rotationX, Vector3.up);
            rotation *= Quaternion.AngleAxis (rotationY, Vector3.left);

            if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) {
                position += transform.forward * (normalMoveSpeed * fastMoveFactor) * Input.GetAxis ("Vertical") * Time.deltaTime;
                position += transform.right * (normalMoveSpeed * fastMoveFactor) * Input.GetAxis ("Horizontal") * Time.deltaTime;
            }
            else if (Input.GetKey (KeyCode.LeftControl) || Input.GetKey (KeyCode.RightControl)) {
                position += transform.forward * (normalMoveSpeed * slowMoveFactor) * Input.GetAxis ("Vertical") * Time.deltaTime;
                position += transform.right * (normalMoveSpeed * slowMoveFactor) * Input.GetAxis ("Horizontal") * Time.deltaTime;
            }
            else {
                position += transform.forward * normalMoveSpeed * Input.GetAxis ("Vertical") * Time.deltaTime;
                position += transform.right * normalMoveSpeed * Input.GetAxis ("Horizontal") * Time.deltaTime;
            }


            if (Input.GetKey (KeyCode.Q)) { position += transform.up * climbSpeed * Time.deltaTime; }
            if (Input.GetKey (KeyCode.E)) { position -= transform.up * climbSpeed * Time.deltaTime; }

            //if (Input.GetKeyDown (KeyCode.End)) {
            //    Screen.lockCursor = (Screen.lockCursor == false) ? true : false;
            //}
        }
        else {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (ufbx.IsDone) {
            var dz = -100.0f * Input.mouseScrollDelta.y * Time.deltaTime;
            z = Mathf.Lerp (z, dz, 5.0f * Time.deltaTime);
            position += this.transform.forward * z;
            this.transform.position = Vector3.Lerp (this.transform.position, position, 5.0f * Time.deltaTime);
            this.transform.localRotation = Quaternion.Slerp (this.transform.localRotation, rotation, 5.0f * Time.deltaTime);
        }
    }

}