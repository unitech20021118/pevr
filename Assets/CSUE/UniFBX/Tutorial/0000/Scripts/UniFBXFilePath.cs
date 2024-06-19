using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UniFBXFilePath : MonoBehaviour {

    public InputField pathModels;
    public InputField pathTextures;
    public InputField filename;

    // Use this for initialization
    void Start ( ) {
#if (UNITY_WEBPLAYER || UNITY_WEBGL)
        pathModels.text = "http://localhost:8383/UniFBX/models/";
        pathTextures.text = "http://localhost:8383/UniFBX/models/tex/";
        filename.text = "levelGeometry";
#elif (UNITY_STANDALONE_LINUX || UNITY_STANDALONE_WIN)
        pathModels.text = "file:///E:/models/";
        pathTextures.text = "file:///E:/models/tex/";
        filename.text = "levelGeometry";
#elif UNITY_STANDALONE
        pathModels.text = Application.dataPath + "/models/";
        pathTextures.text = Application.dataPath + "/models/tex/";
        filename.text = "levelGeometry";
#elif UNITY_ANDROID
        pathModels.text = Application.dataPath + "/models/";
        pathTextures.text = Application.dataPath + "/models/tex/";
        filename.text = "levelGeometry";
#elif UNITY_IPHONE
        pathModels.text = Application.dataPath + "/models/";
        pathTextures.text = Application.dataPath + "/models/tex/";
        filename.text = "levelGeometry";
#endif
    }

    // Update is called once per frame
    void Update ( ) {

    }


}
