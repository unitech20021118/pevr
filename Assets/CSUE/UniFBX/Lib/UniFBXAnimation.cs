using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CSUE.UniFBX;

public class UniFBXAnimations {

    public class AnimCurveT {
        public AnimationCurve x = null;
        public AnimationCurve y = null;
        public AnimationCurve z = null;

        public void Init (Vector3 v) {            
            this.x = new AnimationCurve (new Keyframe (0, v.x), new Keyframe (1, v.x));
            this.y = new AnimationCurve (new Keyframe (0, v.y), new Keyframe (1, v.y));
            this.z = new AnimationCurve (new Keyframe (0, v.z), new Keyframe (1, v.z));
        }
    }

    public class AnimCurveR {
        public AnimationCurve x = null;
        public AnimationCurve y = null;
        public AnimationCurve z = null;
        public AnimationCurve w = null;

        public void Init (Vector3 v) {
            Quaternion q = Quaternion.identity;
            if (v.x == 0) {
                //YZX sequence
                q = Quaternion.Euler (Vector3.up * v.y);
                q *= Quaternion.Euler (Vector3.forward * v.z);
                q *= Quaternion.Euler (Vector3.right * v.x);
            }
            else {
                //ZYX sequence
                q = Quaternion.Euler (Vector3.forward * v.z);
                q *= Quaternion.Euler (Vector3.up * v.y);
                q *= Quaternion.Euler (Vector3.right * v.x);
            }

            this.x = new AnimationCurve (new Keyframe (0, q.x), new Keyframe (1, q.x));
            this.y = new AnimationCurve (new Keyframe (0, q.y), new Keyframe (1, q.y));
            this.z = new AnimationCurve (new Keyframe (0, q.z), new Keyframe (1, q.z));
            this.w = new AnimationCurve (new Keyframe (0, q.w), new Keyframe (1, q.w));
        }

        public void ToQuaternion (GameObject o) {
            AnimationCurve cx = new AnimationCurve ();
            AnimationCurve cy = new AnimationCurve ();
            AnimationCurve cz = new AnimationCurve ();
            AnimationCurve cw = new AnimationCurve ();
            for (int i = 0; i < this.x.keys.Length; i++) {
                Vector3 t = Vector3.zero;
                Vector3 v = Vector3.zero;

                if (this.x != null) {
                    if (i < this.x.keys.Length) {
                        t.x = this.x.keys[i].time;
                        v.x = this.x.keys[i].value;
                    }
                }

                if (this.y != null) {
                    if (i < this.y.keys.Length) {
                        t.y = this.y.keys[i].time;
                        v.y = this.y.keys[i].value;
                    }
                }

                if (this.z != null) {
                    if (i < this.z.keys.Length) {
                        t.z = this.z.keys[i].time;
                        v.z = this.z.keys[i].value;
                    }
                }

                Quaternion q = Quaternion.identity;
                if (v.x == 0) {
                    //YZX sequence
                    q = Quaternion.Euler (Vector3.up * v.y);
                    q *= Quaternion.Euler (Vector3.forward * v.z);
                    q *= Quaternion.Euler (Vector3.right * v.x);
                }
                else {
                    //ZYX sequence
                    q = Quaternion.Euler (Vector3.forward * v.z);
                    q *= Quaternion.Euler (Vector3.up * v.y);
                    q *= Quaternion.Euler (Vector3.right * v.x);
                }

                if (o.GetComponent<Light> ()) {
                    if (o.GetComponent<Light> ().type == LightType.Spot) {
                        q *= Quaternion.Euler (90.0f * Vector3.right);
                    }                    
                }                

                //q = Quaternion.Euler (v.x, v.y, v.z);
                cx.AddKey (new Keyframe (t.x, q.x));
                cy.AddKey (new Keyframe (t.y, q.y));
                cz.AddKey (new Keyframe (t.z, q.z));
                cw.AddKey (new Keyframe (t.y, q.w));
            }
            this.x = cx;
            this.y = cy;
            this.z = cz;
            this.w = cw;
        }    
    }

    public class AnimCurveS {
        public AnimationCurve x = null;
        public AnimationCurve y = null;
        public AnimationCurve z = null;

        public void Init (Vector3 v) {
            this.x = new AnimationCurve (new Keyframe (0, v.x), new Keyframe (1, v.x));
            this.y = new AnimationCurve (new Keyframe (0, v.y), new Keyframe (1, v.y));
            this.z = new AnimationCurve (new Keyframe (0, v.z), new Keyframe (1, v.z));
        }
    }

    public class AnimCurve {
        public string id;
        public string name;
        public string relativePath;        
        public string data;
    }

    public class AnimCurveNode {
        public string id;
        public string property;
        public Vector3 value;
        public AnimCurveT curveT = null;
        public AnimCurveR curveR = null;
        public AnimCurveS curveS = null;
    }

    private FBXSetting setting;
    private List<string> list = null;
    private Dictionary<string, AnimCurve> animCurve = null;
    private Dictionary<string, AnimCurveNode> animCurveNode = null;    
    private int foo = -1;
    private int Len = 0;
    private bool _isDone = false;
    public bool IsDone {
        get { return this._isDone; }
        set { this._isDone = value; }
    }
    private bool _isRunning;
    public bool IsRunning {
        get { return this._isRunning; }
        set { this._isRunning = value; }
    }


    public void Init (List<string> list, FBXSetting setting, int connectionsIndex) {
        this.list = list;
        this.setting = setting;
        this.foo = connectionsIndex;

        switch (this.setting.paths.runnningMethode) {
            case RunnningMethode.MainThread: this.Run (); break;
#if (!UNITY_WEBPLAYER && !UNITY_WEBGL)
            case RunnningMethode.AsyncThread: UniFBXThread.RunAsync (this.Run); break;
#endif
            default: this.Run (); break;
        }
    }

    private void Run ( ) {
        this.IsDone = false;
        this.animCurveNode = new Dictionary<string,AnimCurveNode>();
        this.animCurve = new Dictionary<string, AnimCurve> ();

        int f = this.list.FindIndex (x => x.Contains ("\"AnimCurveNode::"));
        if (f == -1) return;

        string[] data = new string[0];
        for (int i = f; i < list.Count; i++) {
            if (list[i].Contains ("\"AnimCurveNode::")) {
                data = list[i].Split (new char[] { ':', ',', '\"' });
                string id = data[1].Trim ();
                string property = data[5].Trim ();
                AnimCurveNode acn = new AnimCurveNode ();
                acn.id = id;
                acn.property = property;

                switch (property) {
                    case "T":
                        for (int j = i + 2; j < (i + 5); j++) {
                            if (list[j].Contains ("d|X")) {
                                data = list[j].Split (',');
                                acn.value.x = -float.Parse (data[data.Length - 1]);
                            }
                            else if (list[j].Contains ("d|Y")) {
                                data = list[j].Split (',');
                                acn.value.y = float.Parse (data[data.Length - 1]);
                            }
                            else if (list[j].Contains ("d|Z")) {
                                data = list[j].Split (',');
                                acn.value.z = float.Parse (data[data.Length - 1]);
                            }
                        }
                        break;

                    case "R":
                        for (int j = i + 2; j < (i + 5); j++) {
                            if (list[j].Contains ("d|X")) {
                                data = list[j].Split (',');
                                acn.value.x = float.Parse (data[data.Length - 1]);
                            }
                            else if (list[j].Contains ("d|Y")) {
                                data = list[j].Split (',');
                                acn.value.y = -float.Parse (data[data.Length - 1]);
                            }
                            else if (list[j].Contains ("d|Z")) {
                                data = list[j].Split (',');
                                acn.value.z = -float.Parse (data[data.Length - 1]);
                            }
                        }
                        break;

                    case "S":
                        for (int j = i + 2; j < (i + 5); j++) {
                            if (list[j].Contains ("d|X")) {
                                data = list[j].Split (',');
                                acn.value.x = float.Parse (data[data.Length - 1]);
                            }
                            else if (list[j].Contains ("d|Y")) {
                                data = list[j].Split (',');
                                acn.value.y = float.Parse (data[data.Length - 1]);
                            }
                            else if (list[j].Contains ("d|Z")) {
                                data = list[j].Split (',');
                                acn.value.z = float.Parse (data[data.Length - 1]);
                            }
                        }
                        break;
                }

                acn.value *= this.setting.meshes.scaleFactor;
                this.animCurveNode.Add (id, acn);
            }
            else if (list[i].Contains ("\"AnimCurve::")) {
                AnimCurve acn = new AnimCurve ();
                data = list[i].Split (new char[] { ':', ',', '\"' });
                acn.id = data[1].Trim ();
                acn.name = data[5].Trim ();

                for (int j = i; j < (i + 10); j++) {
                    if (this.list[j].Contains ("KeyValueFloat: *")) {
                        int dnValue = j + 1;
                        string group = "";
                        list[dnValue] = list[dnValue].Replace ("a:", "").Trim ();
                        for (int v = dnValue; v < list.Count; v++) {
                            if (list[v].Contains ("}")) break;
                            group += list[v];
                        }
                        acn.data = group;
                        break;
                    }
                }
                this.animCurve.Add (acn.id, acn);
                i = i + 20;
            }
            else if (list[i].Contains ("Connections:  {")) {
                if (this.animCurveNode.Count > 0) Connections_Run ();
                this.IsDone = true;
                return;
            }
        }
    }

    private void Connections_Run ( ) {
        for (int i = 2; i < this.list.Count - 3; i = i + 3) {
            int n = foo + i;
            if (n >= (list.Count - 1) || list[n].Contains (";Takes section")) break;

            if (list[n].Contains (";AnimCurve::") && list[n].Contains (", AnimCurveNode::")) {
                #region "AnimCurve To AnimCurveNode"
                string[] data = list[n].Split (new char[] { ':', ',' });
                //string ncc = data[2];
                string npp = data[5];   //Curve T,R,S
                data = list[n + 1].Split (new char[] { ',', '\"' });                
                string animCurveId = data[3];   //AnimCurve ID
                string animCurveNodeId=data[4];//AnimCurveNode ID
                string property = data[6];      //d|X, d|Y, d|Z

                data = this.animCurve[animCurveId].data.Split (',');
                AnimationCurve animCurve = new AnimationCurve ();
                this.Len = data.Length;
                for (float k = 0; k < this.Len; k = k + 2) {
                    float key = k / this.setting.animations.frameRate;
                    float value = float.Parse (data[(int)k]);

                    switch (npp) {
                        case "T":
                            if (property.Contains ("X")) animCurve.AddKey (key, -value * this.setting.meshes.scaleFactor);
                            else animCurve.AddKey (key, value * this.setting.meshes.scaleFactor);
                            break;

                        case "R":
                            if (property.Contains ("X")) animCurve.AddKey (key, value);
                            else if (property.Contains ("Y")) animCurve.AddKey (key, -value);
                            else if (property.Contains ("Z")) animCurve.AddKey (key, -value);                            
                            break;

                        case "S":
                            animCurve.AddKey (key, value);
                            break;
                    }
                }

                switch (npp) {
                    case "T":
                        if (this.animCurveNode[animCurveNodeId].curveT == null) {
                            this.animCurveNode[animCurveNodeId].curveT = new AnimCurveT ();
                            this.animCurveNode[animCurveNodeId].curveT.Init (this.animCurveNode[animCurveNodeId].value);
                        }
                        if (property.Contains ("X")) this.animCurveNode[animCurveNodeId].curveT.x = animCurve;
                        if (property.Contains ("Y")) this.animCurveNode[animCurveNodeId].curveT.y = animCurve;
                        if (property.Contains ("Z")) this.animCurveNode[animCurveNodeId].curveT.z = animCurve;
                        break;

                    case "R":
                        if (this.animCurveNode[animCurveNodeId].curveR == null) {
                            this.animCurveNode[animCurveNodeId].curveR = new AnimCurveR ();
                            this.animCurveNode[animCurveNodeId].curveR.Init (this.animCurveNode[animCurveNodeId].value);
                        }
                        if (property.Contains ("X")) this.animCurveNode[animCurveNodeId].curveR.x = animCurve;
                        if (property.Contains ("Y")) this.animCurveNode[animCurveNodeId].curveR.y = animCurve;
                        if (property.Contains ("Z")) this.animCurveNode[animCurveNodeId].curveR.z = animCurve;
                        
                        break;

                    case "S":
                        if (this.animCurveNode[animCurveNodeId].curveS == null) {
                            this.animCurveNode[animCurveNodeId].curveS = new AnimCurveS ();
                            this.animCurveNode[animCurveNodeId].curveS.Init (this.animCurveNode[animCurveNodeId].value);
                        }
                        if (property.Contains ("X")) this.animCurveNode[animCurveNodeId].curveS.x = animCurve;
                        if (property.Contains ("Y")) this.animCurveNode[animCurveNodeId].curveS.y = animCurve;
                        if (property.Contains ("Z")) this.animCurveNode[animCurveNodeId].curveS.z = animCurve;
                        break;
                }               
                #endregion
            }            
        }    
    }

    public bool AnimationExists ( ) {
        if (this.animCurveNode.Count > 0) {
            return true;
        }
        else {
            return false;
        }
    }

    public void SetCurve (GameObject root, GameObject o, string relativePath, string property, string animCurveNodeID) {
        string id = animCurveNodeID;        

        switch (property) {
            case "T":
            if (this.animCurveNode[id].curveT != null) {
                root.GetComponent<Animation> ().clip.SetCurve (relativePath, typeof (Transform), "localPosition.x", this.animCurveNode[id].curveT.x);
                root.GetComponent<Animation> ().clip.SetCurve (relativePath, typeof (Transform), "localPosition.y", this.animCurveNode[id].curveT.y);
                root.GetComponent<Animation> ().clip.SetCurve (relativePath, typeof (Transform), "localPosition.z", this.animCurveNode[id].curveT.z);
            }
            break;

            case "R":
            if (this.animCurveNode[id].curveR != null) {
                this.animCurveNode[id].curveR.ToQuaternion (o);
                root.GetComponent<Animation> ().clip.SetCurve (relativePath, typeof (Transform), "localRotation.x", this.animCurveNode[id].curveR.x);
                root.GetComponent<Animation> ().clip.SetCurve (relativePath, typeof (Transform), "localRotation.y", this.animCurveNode[id].curveR.y);
                root.GetComponent<Animation> ().clip.SetCurve (relativePath, typeof (Transform), "localRotation.z", this.animCurveNode[id].curveR.z);
                root.GetComponent<Animation> ().clip.SetCurve (relativePath, typeof (Transform), "localRotation.w", this.animCurveNode[id].curveR.w);
            }
            break;

            case "S":
            if (this.animCurveNode[id].curveS != null) {
                root.GetComponent<Animation> ().clip.SetCurve (relativePath, typeof (Transform), "localScale.x", this.animCurveNode[id].curveS.x);
                root.GetComponent<Animation> ().clip.SetCurve (relativePath, typeof (Transform), "localScale.y", this.animCurveNode[id].curveS.y);
                root.GetComponent<Animation> ().clip.SetCurve (relativePath, typeof (Transform), "localScale.z", this.animCurveNode[id].curveS.z);
            }
            break;
        }

    }

    public void Clear ( ) {        
        if (this.animCurveNode != null) this.animCurveNode.Clear ();
        if (this.animCurve != null) this.animCurve.Clear ();        
        this.animCurveNode = null;
        this.animCurve = null;
    }

    public void Finish ( ) {
        if (this.list != null) this.list.Clear ();
        this.list = null;
    }

}