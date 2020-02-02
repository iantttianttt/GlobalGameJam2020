// Blender To Unity Importer v1.4
// Cogumelo Softworks 2020

using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using UnityEditor;
using System.IO;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

// B2U Inspector UI

public class B2U_Importer : EditorWindow {

    // Global parameters
    public static string metaDataPath = "B2U/Editor/Data/";
    public static string prefixPrefab = "_";
    Object metaDataFolder;

    // Scene Importer parameters
    Object sceneFile;

    // ------------------------------------------------------------------------------------------
    // Importer Panel UI
    [MenuItem("Window/B2U Importer")]
    static void Init() {

        B2U_Importer window = (B2U_Importer)EditorWindow.GetWindow(typeof(B2U_Importer));
        GUIContent title = new GUIContent();
        title.text = "B2U Importer";
        window.titleContent = title;
        window.Show();
    }

    void Awake() {
        prefixPrefab = EditorPrefs.GetString("B2U_prefixPrefab", prefixPrefab);
    }

    void OnGUI() {

        GUILayout.Label(" B2U Importer:", EditorStyles.boldLabel);
        prefixPrefab = EditorGUILayout.TextField("Prefix Identification", prefixPrefab);

        // Save in EditorPlayerPrefs
        EditorPrefs.SetString("B2U_prefixPrefab", prefixPrefab);
    }
}

// B2U Importer System

class B2U_Postprocessor : AssetPostprocessor {

    public static List<B2U_MatPath> MatList = new List<B2U_MatPath>();

    //Setting Objects and Material Paths --------------------
    void OnPreprocessModel() {

        // Clean the list of materials
        MatList = new List<B2U_MatPath>(); 

        if (assetPath.Contains(B2U_Importer.prefixPrefab)) {

            ModelImporter modelImporter = assetImporter as ModelImporter;
            modelImporter.materialLocation = ModelImporterMaterialLocation.External;
            modelImporter.materialName = ModelImporterMaterialName.BasedOnMaterialName;
            modelImporter.materialSearch = ModelImporterMaterialSearch.Everywhere;

            string b2upref = Path.ChangeExtension(assetPath, "b2up");
            string correct_nameprefab = Path.GetFileName(b2upref);
            string prefab_path = "Assets/" + B2U_Importer.metaDataPath + "Prefabs/" + correct_nameprefab;

            // Keep Materials and correct paths in a list
            XmlDocument doc = new XmlDocument();
            doc.Load(prefab_path);
            XmlNode root = doc.DocumentElement;
            XmlNodeList Materials = root.SelectSingleNode("Materials").ChildNodes;

            // UV2 Generation
            string uv2 = root.SelectSingleNode("UV2").InnerText;
            if (uv2 == "True") {
                modelImporter.generateSecondaryUV = true;
                modelImporter.secondaryUVPackMargin = 10.0f;
            }
            else {
                modelImporter.generateSecondaryUV = false;
            }

            for (int i = 0; i < Materials.Count; i++) {
                string path = Materials[i].SelectSingleNode("Path").InnerText;
                string name = Materials[i].SelectSingleNode("Name").InnerText;

                // Creates a Generic material
                Material mat = new Material(Shader.Find("Standard"));
                mat.name = name;

                B2U_MatPath matpath = new B2U_MatPath(mat, path);
                MatList.Add(matpath);
            }
        }
    }

    // Import and Set Materials -----------------------------
    Material OnAssignMaterialModel(Material material, Renderer renderer) {
        // Find this material
        string path_mat = "";
        string path_mat_xml = "";
        for (int i = 0; i < MatList.Count; i++) {
            B2U_MatPath Mat = MatList[i];
            if (Mat == null) { // Previne erros quando a variável possui lixo ou foi removido pela Garbage Collector
                MatList.RemoveAt(i);
            }
            else {
                if (Mat._Mat.name == material.name) {
                    path_mat = Mat._Path + "/" + material.name + ".mat";
                    path_mat_xml = "Assets/" + B2U_Importer.metaDataPath + "Materials/" + material.name + ".b2mat";
                }
            }
        }
        // Create/Reimport B2U Material
        if (path_mat != "") {
            // Configure the Material based on XML file
            XmlDocument mat_xml = new XmlDocument();
            mat_xml.Load(path_mat_xml);
            XmlNode mat_xml_root = mat_xml.DocumentElement;
            Shader shader_name = Shader.Find(mat_xml_root.SelectSingleNode("Shader").InnerText);
            //XmlNode mode = mat_xml_root.SelectSingleNode("MaterialMode");

            // If it's a not valid shader use the B2U error material
            if (shader_name == null) {
                Debug.LogWarning(mat_xml_root.SelectSingleNode("Shader").InnerText);
                Debug.LogWarning(shader_name);
                Debug.LogWarning("B2U Process: The material " + path_mat + " has not a valid shader name. The Default Error Material will be used to your convenience");
                Material errorMat = (Material)AssetDatabase.LoadAssetAtPath("Assets/B2U/Editor/UI/Error.mat", typeof(Material));
                return errorMat;
            }


            bool rewrite = ((mat_xml_root.SelectSingleNode("Rewrite").InnerText == "True") ? true : false);
            Material mat;

            bool updateProperties = false;

            // Se tiver um material com esse nome
            if (AssetDatabase.LoadAssetAtPath(path_mat, typeof(Material))) {

                // Se estiver marcado para ser atualizado
                if (rewrite) {
                    // Marca para atualizar
                    updateProperties = true;
                    mat = AssetDatabase.LoadAssetAtPath(path_mat, typeof(Material)) as Material;
                    mat.shader = shader_name;
                }

                // Tem o material, mas não está marcado para atualizar, só usa o mesmo
                else {
                    return AssetDatabase.LoadAssetAtPath(path_mat, typeof(Material)) as Material;
                }

            }

            // O material ainda não existe
            else {
                updateProperties = true;
                material.shader = shader_name;
                AssetDatabase.CreateAsset(material, path_mat);
                mat = material;
            }


            if (updateProperties) {
                
                // Set Keywords
                mat.EnableKeyword("_NORMALMAP");
                mat.EnableKeyword("_EMISSION");
                mat.EnableKeyword("_METALLICGLOSSMAP");
                mat.EnableKeyword("_SPECGLOSSMAP");

                // Get Data from Channels when available

                XmlNode channels = mat_xml_root.SelectSingleNode("Channels");

                // Base Color
                if (B2U_Utils.getXMLChannel(channels, "_Color") != "null") {
                    Color baseColorFromData = B2U_Utils.parseColorChannel(channels.SelectSingleNode("_Color").InnerText);
                    mat.SetColor("_Color", baseColorFromData);
                    mat.SetColor("_BaseColor", baseColorFromData); // URP Unlit
                    mat.SetColor("_UnlitColor", baseColorFromData); // HDRP Unlit
                    
                }

                // Albedo
                if (B2U_Utils.getXMLChannel(channels, "_MainTex") != "null") {
                    Texture baseAlbedoFromData = B2U_Utils.parseTextureChannel(channels.SelectSingleNode("_MainTex").InnerText);
                    if (baseAlbedoFromData) {
                        mat.SetTexture("_MainTex", baseAlbedoFromData);
                        mat.SetTexture("_BaseMap", baseAlbedoFromData); // URP Unlit
                        mat.SetTexture("_UnlitColorMap", baseAlbedoFromData); //HDRP Unlit
                        mat.SetFloat("_UseColorMap", 1.0f); // URP Autodesk

                    }
                    else {
                        mat.SetTexture("_MainTex", null);
                        mat.SetTexture("_BaseMap", null); // URP Unlit
                        mat.SetTexture("_UnlitColorMap", null); // HDRP Unlit
                        mat.SetFloat("_UseColorMap", 0.0f); // URP Autodesk
                    }
                }

                // Metallic
                if (B2U_Utils.getXMLChannel(channels, "_MetallicGlossMap") != "null" &&
                B2U_Utils.getXMLChannel(channels, "_Metallic") != "null") {
                    Texture baseMetallicFromData = B2U_Utils.parseTextureChannel(channels.SelectSingleNode("_MetallicGlossMap").InnerText);
                    if (baseMetallicFromData) {
                        mat.SetTexture("_MetallicGlossMap", baseMetallicFromData);
                        mat.SetFloat("_UseMetalicMap", 1.0f); // URP Autodesk 
                    }
                    else {
                        mat.SetTexture("_MetallicGlossMap", null);
                        float baseMetallicFloatFromData = B2U_Utils.parseFloatChannel(channels.SelectSingleNode("_Metallic").InnerText);
                        mat.SetFloat("_Metallic", baseMetallicFloatFromData);
                        mat.SetFloat("_UseMetalicMap", 0.0f); // URP Autodesk 
                    }
                }

                // Roughness
                Texture baseRoughnessFromData = B2U_Utils.parseTextureChannel(channels.SelectSingleNode("_SpecGlossMap").InnerText);
                if (baseRoughnessFromData) {
                    mat.SetTexture("_SpecGlossMap", baseRoughnessFromData);
                    mat.SetFloat("_UseRoughnessMap", 1.0f); // URP Autodesk 
                }
                else {
                    mat.SetTexture("_SpecGlossMap", null);
                    float baseRoughnessFloatFromData = B2U_Utils.parseFloatChannel(channels.SelectSingleNode("_Glossiness").InnerText);
                    mat.SetFloat("_Glossiness", baseRoughnessFloatFromData);
                    mat.SetFloat("_UseRoughnessMap", 0.0f); // URP Autodesk 
                }

                // Normal Map
                Texture baseNormalFromData = B2U_Utils.parseTextureChannel(channels.SelectSingleNode("_BumpMap").InnerText);
                if (baseNormalFromData) {
                    mat.SetTexture("_BumpMap", baseNormalFromData);
                }
                else {
                    mat.SetTexture("_BumpMap", null);
                }


                // Emission
                Texture baseEmissionFromData = B2U_Utils.parseTextureChannel(channels.SelectSingleNode("_EmissionMap").InnerText);
                if (baseEmissionFromData) {
                    mat.SetColor("_EmissionColor", new Color(1, 1, 1));
                    mat.SetTexture("_EmissionMap", baseEmissionFromData);
                }
                else {
                    mat.SetTexture("_EmissionMap", null);
                    Color baseEmissionColorFromData = B2U_Utils.parseColorChannel(channels.SelectSingleNode("_EmissionColor").InnerText);
                    mat.SetColor("_EmissionColor", baseEmissionColorFromData);
                }

                // Transparent
                string value = channels.SelectSingleNode("_Transparent").InnerText;
                if(value == "OPAQUE") {mat.SetFloat("_Mode", 0.0f); }
                if (value == "CLIP") { mat.SetFloat("_Mode", 1.0f); }
                if (value == "BLEND") { mat.SetFloat("_Mode", 2.0f); }

            }
            return mat;
        }
        else {
            // Default Importer
            return null;
        }
    }

    // Add Properties and Correct the Object Transform ------
    void OnPostprocessModel(GameObject obj) {
        if (obj.name.Contains(B2U_Importer.prefixPrefab)) {
            obj.transform.rotation = Quaternion.identity;
            obj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

            string b2upref = Path.ChangeExtension(assetPath, "b2up");
            string correct_nameprefab = Path.GetFileName(b2upref);
            string prefab_path = "Assets/" + B2U_Importer.metaDataPath + "Prefabs/" + correct_nameprefab;

            XmlDocument doc = new XmlDocument();
            doc.Load(prefab_path);
            XmlNode root = doc.DocumentElement;
            string Tag = root.SelectSingleNode("Tag").InnerText;
            string Layer = root.SelectSingleNode("Layer").InnerText;
            string Static = root.SelectSingleNode("Static").InnerText;
            string _Collider = root.SelectSingleNode("Collider").InnerText;

            // Check if Layer is Valid and Set
            int layeridx = LayerMask.NameToLayer(Layer);
            obj.layer = ((layeridx >= 0) ? layeridx : 0);

            // Check if Tag is Valid and Set
            for (int i = 0; i < UnityEditorInternal.InternalEditorUtility.tags.Length; i++) {
                if (UnityEditorInternal.InternalEditorUtility.tags[i].Contains(Tag)) {
                    obj.tag = Tag;
                    break;
                }
            }

            // Set Object to Static or Dynamic
            obj.isStatic = ((Static == "Static") ? true : false);

            // Set Collider
            switch (_Collider) {
                case "None":
                    break;
                case "Box":
                    obj.AddComponent<BoxCollider>();
                    break;
                case "Sphere":
                    obj.AddComponent<SphereCollider>();
                    break;
                case "Mesh":
                    obj.AddComponent<MeshCollider>();
                    break;
                case "Capsule":
                    obj.AddComponent<CapsuleCollider>();
                    break;
                default:
                    return;
            }
            Debug.Log("B2U Process: " + obj.name + " Impoted");
        }
        
    }

    // Handle all Group to Prefab imports, also will handle MetaData cleanups for models in future
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) {
        foreach (string str in importedAssets) {
            // Auto Import Groups
            if (str.Contains(".b2ug")) {
                B2U_Utils.GroupToPrefab(str);
                
            }

            if (str.Contains(".b2us")) {
                B2U_Utils.SceneToPrefab(str);
            }
        }
    }
}

// Utility Functions

public class B2U_Utils {
    // Parse Functions ---------------------------------------

    public static string getXMLChannel(XmlNode data,string channel) {
        string channelData = "null";
        try {
            channelData = data.SelectSingleNode(channel).InnerText;
        }
        catch {}
        return channelData;
    }

    public static Texture parseTextureChannel(string sourceString) {
        Texture outTex = null;
        string[] splitString = new string[3];
        splitString = sourceString.Split(","[0]);
        if (splitString.Length > 1) {

            string type = splitString[1];
            string path = splitString[2];
            if (path != null) {
                // Default importer
                TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
                if (textureImporter != null) {
                    textureImporter.textureType = TextureImporterType.Default;
                    if (type == "NORMAL") {

                        textureImporter.textureType = TextureImporterType.NormalMap;
                    }
                    AssetDatabase.ImportAsset(path);
                    outTex = AssetDatabase.LoadAssetAtPath(path, typeof(Texture)) as Texture;
                }
            }
        }
        return outTex;
    }

    public static float parseFloatChannel(string sourceString) {
        float outfloat = 0.0f;
        string[] splitString = new string[2];
        splitString = sourceString.Split(","[0]);
        if(splitString.Length > 1) {
            outfloat = float.Parse(splitString[1].Replace(".", ","));
        }
        return outfloat;
    }

    public static Color parseColorChannel(string sourceString) {
        Color outColor = new Color();
        
        string[] splitString = new string[4];
        splitString = sourceString.Split(","[0]);
        if (splitString.Length > 1) {
            string hex = splitString[1];
            ColorUtility.TryParseHtmlString(hex, out outColor);
        }
        return outColor;
    }

    public static Vector3 parseVector3(string sourceString) {

        string outString;
        Vector3 outVector3;
        string[] splitString = new string[3];

        outString = sourceString.Substring(1, sourceString.Length - 2);
        splitString = outString.Split(","[0]);
        outVector3.x = float.Parse(splitString[0].Replace(".", ","));
        outVector3.y = float.Parse(splitString[1].Replace(".", ","));
        outVector3.z = float.Parse(splitString[2].Replace(".", ","));

        return outVector3;
    }

    public static Quaternion parseQuart(string sourceString) {

        string outString;
        Quaternion outQuart;
        string[] splitString = new string[3];

        outString = sourceString.Substring(1, sourceString.Length - 2);
        splitString = outString.Split(","[0]);

        outQuart.x = float.Parse(splitString[0].Replace(".", ","));
        outQuart.y = float.Parse(splitString[1].Replace(".", ","));
        outQuart.z = float.Parse(splitString[2].Replace(".", ","));
        outQuart.w = float.Parse(splitString[3].Replace(".", ","));

        return outQuart;
    }

    public static void SceneToPrefab(string path) {

        XmlDocument doc = new XmlDocument();
        try {
            doc.Load(path);
        }
        catch {
            Debug.Log("B2U Process: Scene is not Valid");
        }
        XmlNode root = doc.DocumentElement;

        XmlNode Parameters = root.SelectSingleNode("Parameters");
        
        // Cria a cena
        GameObject ScenePivot = new GameObject();
        ScenePivot.name = Parameters.SelectSingleNode("Name").InnerText;

        // Objetos da Cena e seu parentesco
        List<B2U_ParentList> pList = new List<B2U_ParentList>();

        // Importa Objetos
        XmlNodeList Objects = root.SelectSingleNode("Objects").ChildNodes;
        foreach (XmlNode Obj in Objects) {

            string prefab_path = Obj.SelectSingleNode("Prefab").InnerText;
            string loc = Obj.SelectSingleNode("Position").InnerText;
            string rot = Obj.SelectSingleNode("Rotation").InnerText;
            string sca = Obj.SelectSingleNode("Scale").InnerText;
            string nameObj = Obj.SelectSingleNode("Name").InnerText;

            // Overwrited per Object Settings		
            string sta = Obj.SelectSingleNode("Static").InnerText;
            string tag = Obj.SelectSingleNode("Tag").InnerText;
            string layer = Obj.SelectSingleNode("Layer").InnerText;

            GameObject element = AssetDatabase.LoadAssetAtPath<GameObject>(prefab_path);
            GameObject objTemp = PrefabUtility.InstantiatePrefab(element) as GameObject;

            objTemp.name = nameObj;
            objTemp.transform.parent = ScenePivot.transform;

            objTemp.transform.position = parseVector3(loc);
            objTemp.transform.localScale = parseVector3(sca);
            Vector3 FRot = parseVector3(rot);

            objTemp.transform.rotation = new Quaternion();
            objTemp.transform.Rotate(new Vector3(FRot[0] * -1, 0, 0), Space.World);
            objTemp.transform.Rotate(new Vector3(0, FRot[2] * -1, 0), Space.World);
            objTemp.transform.Rotate(new Vector3(0, 0, FRot[1] * -1), Space.World);

            if (sta != "Keep") {
                if (sta == "Static")
                    objTemp.isStatic = true;
                else
                    objTemp.isStatic = false;
            }

            // Check if Layer is Valid and Set
            if (layer != "") {
                int layeridx = LayerMask.NameToLayer(layer);
                objTemp.layer = ((layeridx >= 0) ? layeridx : 0);

                // Check if Tag is Valid and Set
                if (tag != "") {
                    for (int j = 0; j < UnityEditorInternal.InternalEditorUtility.tags.Length; j++) {
                        if (UnityEditorInternal.InternalEditorUtility.tags[j].Contains(tag)) {
                            objTemp.tag = tag;
                            break;
                        }
                    }
                }
            }

            B2U_ParentList TempDataObj = new B2U_ParentList();
            TempDataObj.obj = objTemp;
            TempDataObj.parentName = Obj.SelectSingleNode("Parent").InnerText;
            pList.Add(TempDataObj);

        }

        // Importa Lampadas
        XmlNodeList Lamps = root.SelectSingleNode("Lamps").ChildNodes;
        foreach (XmlNode Lamp in Lamps) {
            string nameLamp = Lamp.SelectSingleNode("Name").InnerText;
            string locLamp = Lamp.SelectSingleNode("Position").InnerText;
            string rotLamp = Lamp.SelectSingleNode("Rotation").InnerText;
            string colorLamp = Lamp.SelectSingleNode("Color").InnerText;
            string powerLamp = Lamp.SelectSingleNode("Power").InnerText;
            string typeLamp = Lamp.SelectSingleNode("Type").InnerText;

            string objNameLamp = B2U_Importer.prefixPrefab + nameLamp;
            GameObject objLamp = new GameObject();
            objLamp.AddComponent<Light>();
            objLamp.name = objNameLamp;
            objLamp.transform.parent = ScenePivot.transform;

            objLamp.transform.position = parseVector3(locLamp);
            Vector3 FRotLamp = parseVector3(rotLamp);

            objLamp.transform.rotation = Quaternion.identity;
            //Fix Light Default Rotation in Unity 
            objLamp.transform.Rotate(new Vector3(90, 0, 0), Space.World);

            objLamp.transform.Rotate(new Vector3(FRotLamp[0] * -1, 0, 0), Space.World);
            objLamp.transform.Rotate(new Vector3(0, FRotLamp[2] * -1, 0), Space.World);
            objLamp.transform.Rotate(new Vector3(0, 0, FRotLamp[1] * -1), Space.World);

            Color Col;
            ColorUtility.TryParseHtmlString(colorLamp, out Col);
            objLamp.GetComponent<Light>().color = Col;
            float power = float.Parse(powerLamp.Replace(".", ","));
            objLamp.GetComponent<Light>().intensity = power;
            objLamp.GetComponent<Light>().range = Mathf.Sqrt(1/power);

            if (typeLamp == "POINT") {
                objLamp.GetComponent<Light>().type = LightType.Point;
            }
            if (typeLamp == "SPOT") {
                string SpotSize = Lamp.SelectSingleNode("SpotSize").InnerText;
                objLamp.GetComponent<Light>().spotAngle = float.Parse(SpotSize.Replace(".", ","));
                objLamp.GetComponent<Light>().type = LightType.Spot;
            }
            if (typeLamp == "SUN") {
                objLamp.GetComponent<Light>().type = LightType.Directional;
            }

            B2U_ParentList TempDataObj = new B2U_ParentList();
            TempDataObj.obj = objLamp;
            TempDataObj.parentName = Lamp.SelectSingleNode("Parent").InnerText;
            pList.Add(TempDataObj);
        }

        // Importa Cameras
        XmlNodeList Cams = root.SelectSingleNode("Cameras").ChildNodes;
        foreach (XmlNode Cam in Cams) {
            string nameCam = Cam.SelectSingleNode("Name").InnerText;
            string locCam = Cam.SelectSingleNode("Position").InnerText;
            string rotCam = Cam.SelectSingleNode("Rotation").InnerText;
            string projCam = Cam.SelectSingleNode("Projection").InnerText;
            string fovCam = Cam.SelectSingleNode("Fov").InnerText;
            string nearCam = Cam.SelectSingleNode("Near").InnerText;
            string farCam = Cam.SelectSingleNode("Far").InnerText;
            string sizeCam = Cam.SelectSingleNode("Size").InnerText;

            string objNameCam = B2U_Importer.prefixPrefab + nameCam;
            GameObject objCam = new GameObject();
            objCam.AddComponent<Camera>();
            objCam.name = objNameCam;

            objCam.transform.parent = ScenePivot.transform;
            objCam.transform.position = parseVector3(locCam);
            Vector3 FrotCam = parseVector3(rotCam);

            objCam.transform.rotation = new Quaternion(); //Quaternion.EulerRotation(0, 0, 0);
            objCam.transform.rotation = Quaternion.Euler(90 - FrotCam[0], FrotCam[2] * -1, FrotCam[1] * -1);

            float vFov = Mathf.Atan(Mathf.Tan(Mathf.Deg2Rad * float.Parse(fovCam.Replace(".", ",")) / 2) / objCam.GetComponent<Camera>().aspect) * 2;
            vFov *= Mathf.Rad2Deg;

            objCam.GetComponent<Camera>().fieldOfView = vFov;
            objCam.GetComponent<Camera>().nearClipPlane = float.Parse(nearCam.Replace(".", ","));
            objCam.GetComponent<Camera>().farClipPlane = float.Parse(farCam.Replace(".", ","));
            objCam.GetComponent<Camera>().orthographicSize = float.Parse(sizeCam.Replace(".", ",")) * 0.28f;

            if (projCam == "PERSP") {
                objCam.GetComponent<Camera>().orthographic = false;
            }
            else {
                objCam.GetComponent<Camera>().orthographic = true;
            }

            B2U_ParentList TempDataObj = new B2U_ParentList();
            TempDataObj.obj = objCam;
            TempDataObj.parentName = Cam.SelectSingleNode("Parent").InnerText;
            pList.Add(TempDataObj);
        }

        // Configure Parents
        for (int k = 0; k < pList.Count; k++) {
            B2U_ParentList Data = pList[k];
            GameObject ObjSource = Data.obj;
            string dest = Data.parentName;
            List<GameObject> staticList = new List<GameObject>();

            foreach (B2U_ParentList obj in pList) {
                if (Data.parentName == obj.obj.name) {
                    ObjSource.transform.parent = obj.obj.transform;
                }
            }
        }

        // Save in Project
        string dir = Path.GetDirectoryName(path);
        string nameExt = Path.GetFileName(path);
        int fileExtPos = nameExt.LastIndexOf(".");
        string name = nameExt.Substring(0, fileExtPos);

        string prefabPath = dir + "/" + name + ".prefab";

        PrefabUtility.SaveAsPrefabAsset(ScenePivot, prefabPath);
        GameObject.DestroyImmediate(ScenePivot);
    }

    public static void GroupToPrefab(string path) {
        XmlDocument doc = new XmlDocument();
        try {
            doc.Load(path);
        }
        catch { }

        GameObject tempPivot = new GameObject();
        List<B2U_ParentList> pList = new List<B2U_ParentList>();

        XmlNode root = doc.DocumentElement;
        XmlNodeList Objects = root.SelectNodes("Object");
        string prefabPath = root.SelectSingleNode("Path").InnerText;
        foreach (XmlNode Obj in Objects) {
            string prefab_path = Obj.SelectSingleNode("Prefab").InnerText;
            string loc = Obj.SelectSingleNode("Position").InnerText;
            string rot = Obj.SelectSingleNode("Rotation").InnerText;
            string sca = Obj.SelectSingleNode("Scale").InnerText;
            string name = Obj.SelectSingleNode("Name").InnerText;
            GameObject element = AssetDatabase.LoadAssetAtPath<GameObject>(prefab_path);
            GameObject objTemp = PrefabUtility.InstantiatePrefab(element) as GameObject;
            
            objTemp.name = name;
            objTemp.transform.parent = tempPivot.transform;

            objTemp.transform.position = parseVector3(loc);
            objTemp.transform.localScale = parseVector3(sca);
            Vector3 FRot = parseVector3(rot);

            objTemp.transform.rotation = new Quaternion();
            objTemp.transform.Rotate(new Vector3(FRot[0] * -1, 0, 0), Space.World);
            objTemp.transform.Rotate(new Vector3(0, FRot[2] * -1, 0), Space.World);
            objTemp.transform.Rotate(new Vector3(0, 0, FRot[1] * -1), Space.World);

            B2U_ParentList TempDataObj = new B2U_ParentList();
            TempDataObj.obj = objTemp;
            TempDataObj.parentName = Obj.SelectSingleNode("Parent").InnerText;
            pList.Add(TempDataObj);
            
        }

        // Configure Parents
        for (int k = 0; k < pList.Count; k++) {
            B2U_ParentList Data = pList[k];
            GameObject ObjSource = Data.obj;
            string dest = Data.parentName;
            List<GameObject> staticList = new List<GameObject>();

            foreach (B2U_ParentList obj in pList) {
                if (Data.parentName == obj.obj.name) {
                    ObjSource.transform.parent = obj.obj.transform;
                }
            }
        }

        // Save in Project
        PrefabUtility.SaveAsPrefabAsset(tempPivot, prefabPath);
        GameObject.DestroyImmediate(tempPivot);
    }

}

public class B2U_MatPath {
    public Material _Mat;
    public string _Path;

    public B2U_MatPath(Material _mat, string _path) {
        _Mat = _mat;
        _Path = _path;
    }
}

public class B2U_ParentList {
    public GameObject obj;
    public string parentName;
}

