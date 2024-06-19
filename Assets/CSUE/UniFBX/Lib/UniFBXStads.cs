using UnityEngine;
using System.Collections.Generic;
using CSUE.UniFBX;
using System.Diagnostics;

public class UniFBXStads : MonoBehaviour {

    public UniFBXImport uimport;
    public Texture uniFBXTexture;
    private static Stopwatch sw = null;
    private static long objectCount = 0;
    private static long meshCount = 0;
    private static long vertexCount = 0;
    private static long polygoneCount = 0;
    private static long materialCount = 0;
    private static long textureCount = 0;    
    
    private static uint samples = 0;
    private static uint acumFPS = 0;
    private static uint avgFPS = 0;
    private static float porcentage = 0.0f;
    private static float progress = 0.0f;

    Rect rect = new Rect (Screen.width - 210, 10, 210, 180);
    //Rect rectCapture = new Rect ();
    Vector2 rectTarget = new Vector2 (Screen.width - 210, 10);
    Vector2 rectOffset = Vector2.zero;
    bool isPressed = false;

    void Update ( ) {
        var f = (1.0f) / Time.deltaTime;
        acumFPS += (uint)f;
        if (++samples == 8) {
            avgFPS = acumFPS >> 3;
            samples = 0;
            acumFPS = 0;
        }
        
        progress = Mathf.Lerp (progress, UniFBXStads.porcentage, 2.0f * Time.deltaTime);

        //this.DragAndDrop ();
        //rectTarget.x = Mathf.Clamp (rectTarget.x, 0, Screen.width - 200);
        //rectTarget.y = Mathf.Clamp (rectTarget.y, 0, Screen.height - 140);
        //rect.x = Mathf.Lerp (rect.x, rectTarget.x, 5.0f * Time.deltaTime);
        //rect.y = Mathf.Lerp (rect.y, rectTarget.y, 5.0f * Time.deltaTime);
    }

    private void DragAndDrop ( ) {
        Vector3 m = Input.mousePosition;
        m.y = Screen.height - m.y;

        if (Input.GetMouseButton (0) && isPressed == false) {
            if (rect.Contains (m)) {
                isPressed = true;
                rectOffset.x = -rect.x + m.x;
                rectOffset.y = -rect.y + m.y;
            }
        }
        else if (Input.GetMouseButton (0) && isPressed) {
            rectTarget.x = m.x - rectOffset.x;
            rectTarget.y = m.y - rectOffset.y;
        }
        else {
            isPressed = false;            
        }
    }

    public static void Init ( ) {
        sw = null;
        sw = new Stopwatch ();
        sw.Stop ();
        sw.Reset ();
        objectCount = 0;
        meshCount = 0;
        vertexCount = 0;
        polygoneCount = 0;
        materialCount = 0;
        textureCount = 0;
        samples = 0;
        acumFPS = 0;
        avgFPS = 0;
    }

    public static void TimerStart ( ) {
        sw.Reset ();
        sw.Start ();
    }

    public static void TimerStop ( ) {
        sw.Stop ();        
    }

    public static float GetLoadTime ( ) {
        return (float)sw.ElapsedMilliseconds / 1000.0f;
    }

    public static void SetPorcentage (float p ) {
        UniFBXStads.porcentage = p;
    }

    public static void AddObject () {
        objectCount += 1;
    }

    public static void AddMesh () {
        meshCount += 1;
    }

    public static void AddVertices (int Length) {
        vertexCount += Length;
    }

    public static void AddTriangles (int Length) {
        polygoneCount += Length;
    }

    public static void AddMaterial () {
        materialCount += 1;
    }

    public static void AddTexture () {
        textureCount += 1;
    }

    public string GetStads () {
        string s = "";            
        s += "Objects: " + UniFBXStads.objectCount.ToString () + "\n";
        s += "Meshes: " + UniFBXStads.meshCount.ToString () + "\n";
        s += "Vertices: " + UniFBXStads.vertexCount.ToString () + "\n";
        s += "Polygones: " + UniFBXStads.polygoneCount.ToString () + "\n";
        s += "Materials: " + UniFBXStads.materialCount.ToString () + "\n";
        s += "Textures: " + UniFBXStads.textureCount.ToString () + "\n";

        if (uimport) s += "Status: " + uimport.setting.Status.ToString () + "\n";
        s += "Loaded in " + UniFBXStads.GetLoadTime ().ToString ("0.0") + " seconds\n";
        s += "Completed: " + UniFBXStads.progress.ToString ("0") + "%";
        return s;
    }

    void OnGUI ( ) {
        rect = new Rect (Screen.width - 210, 10, 210, 180);
        GUI.BeginGroup (rect);        
        //*************************************************************
        GUI.Box (new Rect (0, 0, 200, 160), "");        
        GUI.Label (new Rect (35, 0, 160, 20), "Stadistics");
        string fps = UniFBXStads.avgFPS.ToString () + " fps";        
        GUI.Label (new Rect (155, 0, 50, 20), fps);
        GUI.color = new Color (1, 1, 1, 0.6f);
        GUI.Label (new Rect (35, 20, 200, 150), GetStads ());        
        GUI.DrawTexture (new Rect (2, 10, 28, 140), uniFBXTexture);
        //*************************************************************
        GUI.EndGroup ();
    }

}