using UnityEngine;
using UnityEditor;

public class AddGameObjectsToPrefabs : EditorWindow
{

    [UnityEditor.MenuItem("Tools/Add GameObjects to Prefabs")]
    private static void ShowWindow()
    {
        GetWindow<AddGameObjectsToPrefabs>("Add GameObjects to Prefabs");
    }

    private void OnGUI()
    {
        GUILayout.Label("Add GameObject to Selected Prefabs", EditorStyles.boldLabel);

        EditorGUILayout.LabelField("Large Ships", EditorStyles.boldLabel);

        EditorGUILayout.LabelField("Cast Position", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("One"))
        {
            AddCastPoint();
        }

        GUILayout.EndHorizontal();

        EditorGUILayout.LabelField("Smallship Docking Points", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("One"))
        {
            AddSmallShipDockingPointsToPrefabs(1);
        }

        if (GUILayout.Button("Two"))
        {
            AddSmallShipDockingPointsToPrefabs(2);
        }

        if (GUILayout.Button("Three"))
        {
            AddSmallShipDockingPointsToPrefabs(3);
        }

        if (GUILayout.Button("Four"))
        {
            AddSmallShipDockingPointsToPrefabs(4);
        }

        GUILayout.EndHorizontal();

        EditorGUILayout.LabelField("Largeship Docking Points", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("One"))
        {
            AddLargeShipDockingPointsToPrefabs(1);
        }

        if (GUILayout.Button("Two"))
        {
            AddLargeShipDockingPointsToPrefabs(2);
        }

        if (GUILayout.Button("Three"))
        {
            AddLargeShipDockingPointsToPrefabs(3);
        }

        if (GUILayout.Button("Four"))
        {
            AddLargeShipDockingPointsToPrefabs(4);
        }

        GUILayout.EndHorizontal();

        EditorGUILayout.LabelField("Turret Points Small", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Four"))
        {
            AddSmallTurrets(4);
        }

        if (GUILayout.Button("Eight"))
        {
            AddSmallTurrets(8);
        }

        if (GUILayout.Button("Twelve"))
        {
            AddSmallTurrets(12);
        }

        if (GUILayout.Button("Sixteen"))
        {
            AddSmallTurrets(16);
        }

        GUILayout.EndHorizontal();

        EditorGUILayout.LabelField("Turret Points Large", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Two"))
        {
            AddLargeTurrets(2);
        }

        if (GUILayout.Button("Four"))
        {
            AddLargeTurrets(4);
        }

        if (GUILayout.Button("Six"))
        {
            AddLargeTurrets(6);
        }

        if (GUILayout.Button("Eight"))
        {
            AddLargeTurrets(8);
        }

        GUILayout.EndHorizontal();

        EditorGUILayout.LabelField("Hangar Launch", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("One"))
        {
            AddHangarLaunchToPrefab(1);
        }

        if (GUILayout.Button("Two"))
        {
            AddHangarLaunchToPrefab(2);
        }

        if (GUILayout.Button("Three"))
        {
            AddHangarLaunchToPrefab(3);
        }

        if (GUILayout.Button("Four"))
        {
            AddHangarLaunchToPrefab(4);
        }

        GUILayout.EndHorizontal();

        EditorGUILayout.LabelField("Small Ships", EditorStyles.boldLabel);

        EditorGUILayout.LabelField("Smallship Docking Points", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("One"))
        {
            AddSmallShipDockingPointsToPrefabs(1);
        }

        GUILayout.EndHorizontal();

        EditorGUILayout.LabelField("Camera Positions", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("One"))
        {
            AddCameraPosition();
        }

        GUILayout.EndHorizontal();

        EditorGUILayout.LabelField("Gun Points", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("One"))
        {
            AddGunsToPrefabs(1);
        }

        if (GUILayout.Button("Two"))
        {
            AddGunsToPrefabs(2);
        }

        if (GUILayout.Button("Three"))
        {
            AddGunsToPrefabs(3);
        }

        if (GUILayout.Button("Four"))
        {
            AddGunsToPrefabs(4);
        }

        GUILayout.EndHorizontal();

        EditorGUILayout.LabelField("IonPoints", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("One"))
        {
            AddIonsToPrefabs(1);
        }

        if (GUILayout.Button("Two"))
        {
            AddIonsToPrefabs(2);
        }

        if (GUILayout.Button("Three"))
        {
            AddIonsToPrefabs(3);
        }

        if (GUILayout.Button("Four"))
        {
            AddIonsToPrefabs(4);
        }

        GUILayout.EndHorizontal();

        EditorGUILayout.LabelField("Missile Points", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("One"))
        {
            AddMissilesToPrefabs(1);
        }

        if (GUILayout.Button("Two"))
        {
            AddMissilesToPrefabs(2);
        }

        if (GUILayout.Button("Three"))
        {
            AddMissilesToPrefabs(3);
        }

        if (GUILayout.Button("Four"))
        {
            AddMissilesToPrefabs(4);
        }

        GUILayout.EndHorizontal();

        EditorGUILayout.LabelField("Engine Glow", EditorStyles.boldLabel);

        EditorGUILayout.LabelField("Circle", EditorStyles.label);

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Blue"))
        {
            AddEngineGlow("engineglow_circle_blue");
        }

        if (GUILayout.Button("Red"))
        {
            AddEngineGlow("engineglow_circle_red");
        }

        if (GUILayout.Button("RedIon"))
        {
            AddEngineGlow("engineglow_circle_redion");
        }

        if (GUILayout.Button("Orange"))
        {
            AddEngineGlow("engineglow_circle_orange");
        }

        if (GUILayout.Button("Purple"))
        {
            AddEngineGlow("engineglow_circle_purple");
        }

        GUILayout.EndHorizontal();

        EditorGUILayout.LabelField("Rounded", EditorStyles.label);

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Blue"))
        {
            AddEngineGlow("engineglow_rounded_blue");
        }

        if (GUILayout.Button("Red"))
        {
            AddEngineGlow("engineglow_rounded_red");
        }

        if (GUILayout.Button("RedIon"))
        {
            AddEngineGlow("engineglow_rounded_redion");
        }

        if (GUILayout.Button("Orange"))
        {
            AddEngineGlow("engineglow_rounded_orange");
        }

        if (GUILayout.Button("Purple"))
        {
            AddEngineGlow("engineglow_rounded_purple");
        }

        GUILayout.EndHorizontal();

        EditorGUILayout.LabelField("Square", EditorStyles.label);

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Blue"))
        {
            AddEngineGlow("engineglow_square_blue");
        }

        if (GUILayout.Button("Red"))
        {
            AddEngineGlow("engineglow_square_red");
        }

        if (GUILayout.Button("RedIon"))
        {
            AddEngineGlow("engineglow_square_redion");
        }

        if (GUILayout.Button("Orange"))
        {
            AddEngineGlow("engineglow_square_orange");
        }

        if (GUILayout.Button("Purple"))
        {
            AddEngineGlow("engineglow_square_purple");
        }

        GUILayout.EndHorizontal();
    }

    private void AddSmallShipDockingPointsToPrefabs(int number)
    {
        Object[] selectedObjects = Selection.objects;

        foreach (Object obj in selectedObjects)
        {
            string path = AssetDatabase.GetAssetPath(obj);

            if (string.IsNullOrEmpty(path) || !path.EndsWith(".prefab"))
            {
                Debug.LogWarning($"Skipped {obj.name}. Not a valid prefab.");
                continue;
            }

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null)
            {
                Debug.LogWarning($"Could not load prefab at path: {path}");
                continue;
            }

            GameObject prefabInstance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            if (prefabInstance == null)
            {
                Debug.LogWarning($"Could not instantiate prefab: {prefab.name}");
                continue;
            }


            if (number > 0)
            {
                GameObject dockingPoint01 = new GameObject("dockingpoint01");
                dockingPoint01.transform.parent = prefabInstance.transform;
                dockingPoint01.transform.localPosition = new Vector3(0, 0, 0);

                if (number > 1)
                {
                    GameObject dockingPoint02 = new GameObject("dockingpoint02");
                    dockingPoint02.transform.parent = prefabInstance.transform;
                    dockingPoint02.transform.localPosition = new Vector3(0, 0, 0);
                }

                if (number > 2)
                {
                    GameObject dockingPoint03 = new GameObject("dockingpoint03");
                    dockingPoint03.transform.parent = prefabInstance.transform;
                    dockingPoint03.transform.localPosition = new Vector3(0, 0, 0);
                }

                if (number > 3)
                {
                    GameObject dockingPoint04 = new GameObject("dockingpoint04");
                    dockingPoint04.transform.parent = prefabInstance.transform;
                    dockingPoint04.transform.localPosition = new Vector3(0, 0, 0);
                }
            }

            PrefabUtility.SaveAsPrefabAsset(prefabInstance, path);

            DestroyImmediate(prefabInstance);

            Debug.Log($"Added smallship gameObjects to prefab: {prefab.name}");
        }

        AssetDatabase.Refresh();
    }

    private void AddLargeShipDockingPointsToPrefabs(int number)
    {
        Object[] selectedObjects = Selection.objects;

        foreach (Object obj in selectedObjects)
        {
            string path = AssetDatabase.GetAssetPath(obj);

            if (string.IsNullOrEmpty(path) || !path.EndsWith(".prefab"))
            {
                Debug.LogWarning($"Skipped {obj.name}. Not a valid prefab.");
                continue;
            }

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null)
            {
                Debug.LogWarning($"Could not load prefab at path: {path}");
                continue;
            }

            GameObject prefabInstance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            if (prefabInstance == null)
            {
                Debug.LogWarning($"Could not instantiate prefab: {prefab.name}");
                continue;
            }


            if (number > 0)
            {
                GameObject dockingPoint01 = new GameObject("dockingpoint01_ls");
                dockingPoint01.transform.parent = prefabInstance.transform;
                dockingPoint01.transform.localPosition = new Vector3(0, 0, 0);

                if (number > 1)
                {
                    GameObject dockingPoint02 = new GameObject("dockingpoint02_ls");
                    dockingPoint02.transform.parent = prefabInstance.transform;
                    dockingPoint02.transform.localPosition = new Vector3(0, 0, 0);
                }

                if (number > 2)
                {
                    GameObject dockingPoint03 = new GameObject("dockingpoint03_ls");
                    dockingPoint03.transform.parent = prefabInstance.transform;
                    dockingPoint03.transform.localPosition = new Vector3(0, 0, 0);
                }

                if (number > 3)
                {
                    GameObject dockingPoint04 = new GameObject("dockingpoint04_ls");
                    dockingPoint04.transform.parent = prefabInstance.transform;
                    dockingPoint04.transform.localPosition = new Vector3(0, 0, 0);
                }
            }

            PrefabUtility.SaveAsPrefabAsset(prefabInstance, path);

            DestroyImmediate(prefabInstance);

            Debug.Log($"Added smallship gameObjects to prefab: {prefab.name}");
        }

        AssetDatabase.Refresh();
    }

    private void AddSmallTurrets(int number)
    {
        Object[] selectedObjects = Selection.objects;

        foreach (Object obj in selectedObjects)
        {
            string path = AssetDatabase.GetAssetPath(obj);

            if (string.IsNullOrEmpty(path) || !path.EndsWith(".prefab"))
            {
                Debug.LogWarning($"Skipped {obj.name}. Not a valid prefab.");
                continue;
            }

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null)
            {
                Debug.LogWarning($"Could not load prefab at path: {path}");
                continue;
            }

            GameObject prefabInstance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            if (prefabInstance == null)
            {
                Debug.LogWarning($"Could not instantiate prefab: {prefab.name}");
                continue;
            }

            if (number > 0)
            {
                GameObject turrettransforms = null;
                Transform tempTranform = prefabInstance.transform.Find("turrettransforms");

                if (tempTranform != null)
                {
                    turrettransforms = tempTranform.gameObject;
                }

                if (turrettransforms == null)
                {
                    turrettransforms = new GameObject("turrettransforms");
                    turrettransforms.transform.parent = prefabInstance.transform;
                    turrettransforms.transform.localPosition = new Vector3(0, 0, 0);
                }
                
                for (int i = 0; i < number; i++)
                {
                    GameObject smallTurret = new GameObject("turretsmall");
                    smallTurret.transform.parent = turrettransforms.transform;
                    smallTurret.transform.localPosition = new Vector3(0, 0, 0);
                }
            }

            PrefabUtility.SaveAsPrefabAsset(prefabInstance, path);

            DestroyImmediate(prefabInstance);

            Debug.Log($"Added smallship gameObjects to prefab: {prefab.name}");
        }

        AssetDatabase.Refresh();
    }

    private void AddLargeTurrets(int number)
    {
        Object[] selectedObjects = Selection.objects;

        foreach (Object obj in selectedObjects)
        {
            string path = AssetDatabase.GetAssetPath(obj);

            if (string.IsNullOrEmpty(path) || !path.EndsWith(".prefab"))
            {
                Debug.LogWarning($"Skipped {obj.name}. Not a valid prefab.");
                continue;
            }

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null)
            {
                Debug.LogWarning($"Could not load prefab at path: {path}");
                continue;
            }

            GameObject prefabInstance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            if (prefabInstance == null)
            {
                Debug.LogWarning($"Could not instantiate prefab: {prefab.name}");
                continue;
            }

            if (number > 0)
            {
                GameObject turrettransforms = null;
                Transform tempTranform = prefabInstance.transform.Find("turrettransforms");

                if (tempTranform != null)
                {
                    turrettransforms = tempTranform.gameObject;
                }

                if (turrettransforms == null)
                {
                    turrettransforms = new GameObject("turrettransforms");
                    turrettransforms.transform.parent = prefabInstance.transform;
                    turrettransforms.transform.localPosition = new Vector3(0, 0, 0);
                }

                for (int i = 0; i < number; i++)
                {
                    GameObject smallTurret = new GameObject("turretlarge");
                    smallTurret.transform.parent = turrettransforms.transform;
                    smallTurret.transform.localPosition = new Vector3(0, 0, 0);
                }
            }

            PrefabUtility.SaveAsPrefabAsset(prefabInstance, path);

            DestroyImmediate(prefabInstance);

            Debug.Log($"Added smallship gameObjects to prefab: {prefab.name}");
        }

        AssetDatabase.Refresh();
    }

    private void AddGunsToPrefabs(int number)
    {
        Object[] selectedObjects = Selection.objects;

        foreach (Object obj in selectedObjects)
        {
            string path = AssetDatabase.GetAssetPath(obj);

            if (string.IsNullOrEmpty(path) || !path.EndsWith(".prefab"))
            {
                Debug.LogWarning($"Skipped {obj.name}. Not a valid prefab.");
                continue;
            }

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null)
            {
                Debug.LogWarning($"Could not load prefab at path: {path}");
                continue;
            }

            GameObject prefabInstance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            if (prefabInstance == null)
            {
                Debug.LogWarning($"Could not instantiate prefab: {prefab.name}");
                continue;
            }

            if (number > 0)
            {
                GameObject gunbank01 = new GameObject("gunbank01");
                gunbank01.transform.parent = prefabInstance.transform;
                gunbank01.transform.localPosition = new Vector3(0, 0, 0);

                GameObject gunbank01_01 = new GameObject("gunbank01-01");
                gunbank01_01.transform.parent = gunbank01.transform;
                gunbank01_01.transform.localPosition = new Vector3(0, 0, 0);

                if (number > 1)
                {
                    GameObject gunbank01_02 = new GameObject("gunbank01-02");
                    gunbank01_02.transform.parent = gunbank01.transform;
                    gunbank01_02.transform.localPosition = new Vector3(0, 0, 0);
                }

                if (number > 2)
                {
                    GameObject gunbank01_03 = new GameObject("gunbank01-03");
                    gunbank01_03.transform.parent = gunbank01.transform;
                    gunbank01_03.transform.localPosition = new Vector3(0, 0, 0);
                }

                if (number > 3)
                {
                    GameObject gunbank01_04 = new GameObject("gunbank01-04");
                    gunbank01_04.transform.parent = gunbank01.transform;
                    gunbank01_04.transform.localPosition = new Vector3(0, 0, 0);
                }
            }

            PrefabUtility.SaveAsPrefabAsset(prefabInstance, path);

            DestroyImmediate(prefabInstance);

            Debug.Log($"Added smallship gameObjects to prefab: {prefab.name}");
        }

        AssetDatabase.Refresh();
    }

    private void AddIonsToPrefabs(int number)
    {
        Object[] selectedObjects = Selection.objects;

        foreach (Object obj in selectedObjects)
        {
            string path = AssetDatabase.GetAssetPath(obj);

            if (string.IsNullOrEmpty(path) || !path.EndsWith(".prefab"))
            {
                Debug.LogWarning($"Skipped {obj.name}. Not a valid prefab.");
                continue;
            }

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null)
            {
                Debug.LogWarning($"Could not load prefab at path: {path}");
                continue;
            }

            GameObject prefabInstance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            if (prefabInstance == null)
            {
                Debug.LogWarning($"Could not instantiate prefab: {prefab.name}");
                continue;
            }

            if (number > 0)
            {
                GameObject gunbank01 = new GameObject("ionbank01");
                gunbank01.transform.parent = prefabInstance.transform;
                gunbank01.transform.localPosition = new Vector3(0, 0, 0);

                GameObject gunbank01_01 = new GameObject("ionbank01-01");
                gunbank01_01.transform.parent = gunbank01.transform;
                gunbank01_01.transform.localPosition = new Vector3(0, 0, 0);

                if (number > 1)
                {
                    GameObject gunbank01_02 = new GameObject("ionbank01-02");
                    gunbank01_02.transform.parent = gunbank01.transform;
                    gunbank01_02.transform.localPosition = new Vector3(0, 0, 0);
                }

                if (number > 2)
                {
                    GameObject gunbank01_03 = new GameObject("ionbank01-03");
                    gunbank01_03.transform.parent = gunbank01.transform;
                    gunbank01_03.transform.localPosition = new Vector3(0, 0, 0);
                }

                if (number > 3)
                {
                    GameObject gunbank01_04 = new GameObject("ionbank01-04");
                    gunbank01_04.transform.parent = gunbank01.transform;
                    gunbank01_04.transform.localPosition = new Vector3(0, 0, 0);
                }
            }

            PrefabUtility.SaveAsPrefabAsset(prefabInstance, path);

            DestroyImmediate(prefabInstance);

            Debug.Log($"Added smallship gameObjects to prefab: {prefab.name}");
        }

        AssetDatabase.Refresh();
    }

    private void AddMissilesToPrefabs(int number)
    {
        Object[] selectedObjects = Selection.objects;

        foreach (Object obj in selectedObjects)
        {
            string path = AssetDatabase.GetAssetPath(obj);

            if (string.IsNullOrEmpty(path) || !path.EndsWith(".prefab"))
            {
                Debug.LogWarning($"Skipped {obj.name}. Not a valid prefab.");
                continue;
            }

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null)
            {
                Debug.LogWarning($"Could not load prefab at path: {path}");
                continue;
            }

            GameObject prefabInstance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            if (prefabInstance == null)
            {
                Debug.LogWarning($"Could not instantiate prefab: {prefab.name}");
                continue;
            }

            if (number > 0)
            {
                GameObject missilebank01 = new GameObject("missilebank01");
                missilebank01.transform.parent = prefabInstance.transform;
                missilebank01.transform.localPosition = new Vector3(0, 0, 0);

                GameObject missilebank01_01 = new GameObject("missilebank01-01");
                missilebank01_01.transform.parent = missilebank01.transform;
                missilebank01_01.transform.localPosition = new Vector3(0, 0, 0);

                if (number > 1)
                {
                    GameObject missilebank01_02 = new GameObject("missilebank01-02");
                    missilebank01_02.transform.parent = missilebank01.transform;
                    missilebank01_02.transform.localPosition = new Vector3(0, 0, 0);
                }

                if (number > 2)
                {
                    GameObject missilebank01_03 = new GameObject("missilebank01-03");
                    missilebank01_03.transform.parent = missilebank01.transform;
                    missilebank01_03.transform.localPosition = new Vector3(0, 0, 0);
                }

                if (number > 3)
                {
                    GameObject missilebank01_04 = new GameObject("missilebank01-04");
                    missilebank01_04.transform.parent = missilebank01.transform;
                    missilebank01_04.transform.localPosition = new Vector3(0, 0, 0);
                }
            }

            PrefabUtility.SaveAsPrefabAsset(prefabInstance, path);

            DestroyImmediate(prefabInstance);

            Debug.Log($"Added smallship gameObjects to prefab: {prefab.name}");
        }

        AssetDatabase.Refresh();
    }

    private void AddHangarLaunchToPrefab(int number)
    {
        Object[] selectedObjects = Selection.objects;

        foreach (Object obj in selectedObjects)
        {
            string path = AssetDatabase.GetAssetPath(obj);

            if (string.IsNullOrEmpty(path) || !path.EndsWith(".prefab"))
            {
                Debug.LogWarning($"Skipped {obj.name}. Not a valid prefab.");
                continue;
            }

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null)
            {
                Debug.LogWarning($"Could not load prefab at path: {path}");
                continue;
            }

            GameObject prefabInstance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            if (prefabInstance == null)
            {
                Debug.LogWarning($"Could not instantiate prefab: {prefab.name}");
                continue;
            }


            if (number > 0)
            {
                GameObject dockingPoint01 = new GameObject("hangarlaunch");
                dockingPoint01.transform.parent = prefabInstance.transform;
                dockingPoint01.transform.localPosition = new Vector3(0, 0, 0);

                if (number > 1)
                {
                    GameObject dockingPoint02 = new GameObject("hangarlaunch");
                    dockingPoint02.transform.parent = prefabInstance.transform;
                    dockingPoint02.transform.localPosition = new Vector3(0, 0, 0);
                }

                if (number > 2)
                {
                    GameObject dockingPoint03 = new GameObject("hangarlaunch");
                    dockingPoint03.transform.parent = prefabInstance.transform;
                    dockingPoint03.transform.localPosition = new Vector3(0, 0, 0);
                }

                if (number > 3)
                {
                    GameObject dockingPoint04 = new GameObject("hangarlaunch");
                    dockingPoint04.transform.parent = prefabInstance.transform;
                    dockingPoint04.transform.localPosition = new Vector3(0, 0, 0);
                }
            }

            PrefabUtility.SaveAsPrefabAsset(prefabInstance, path);

            DestroyImmediate(prefabInstance);

            Debug.Log($"Added smallship gameObjects to prefab: {prefab.name}");
        }

        AssetDatabase.Refresh();
    }

    private void AddCameraPosition()
    {
        Object[] selectedObjects = Selection.objects;

        foreach (Object obj in selectedObjects)
        {
            string path = AssetDatabase.GetAssetPath(obj);

            if (string.IsNullOrEmpty(path) || !path.EndsWith(".prefab"))
            {
                Debug.LogWarning($"Skipped {obj.name}. Not a valid prefab.");
                continue;
            }

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null)
            {
                Debug.LogWarning($"Could not load prefab at path: {path}");
                continue;
            }

            GameObject prefabInstance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            if (prefabInstance == null)
            {
                Debug.LogWarning($"Could not instantiate prefab: {prefab.name}");
                continue;
            }

            GameObject cameraGO = new GameObject("camera");
            cameraGO.transform.parent = prefabInstance.transform;
            cameraGO.transform.localPosition = new Vector3(0, 0, 0);

            GameObject followcameraGO = new GameObject("followcamera");
            followcameraGO.transform.parent = prefabInstance.transform;
            followcameraGO.transform.localPosition = new Vector3(0, 0, 0);

            GameObject focuscameraGO = new GameObject("focuscamera");
            focuscameraGO.transform.parent = prefabInstance.transform;
            focuscameraGO.transform.localPosition = new Vector3(0, 0, 0);

            PrefabUtility.SaveAsPrefabAsset(prefabInstance, path);

            DestroyImmediate(prefabInstance);

            Debug.Log($"Added smallship gameObjects to prefab: {prefab.name}");
        }

        AssetDatabase.Refresh();
    }

    private void AddSecondaryCameraPosition()
    {
        Object[] selectedObjects = Selection.objects;

        foreach (Object obj in selectedObjects)
        {
            string path = AssetDatabase.GetAssetPath(obj);

            if (string.IsNullOrEmpty(path) || !path.EndsWith(".prefab"))
            {
                Debug.LogWarning($"Skipped {obj.name}. Not a valid prefab.");
                continue;
            }

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null)
            {
                Debug.LogWarning($"Could not load prefab at path: {path}");
                continue;
            }

            GameObject prefabInstance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            if (prefabInstance == null)
            {
                Debug.LogWarning($"Could not instantiate prefab: {prefab.name}");
                continue;
            }

            GameObject cameraGO = new GameObject("secondarycamera");
            cameraGO.transform.parent = prefabInstance.transform;
            cameraGO.transform.localPosition = new Vector3(0, 0, 0);

            PrefabUtility.SaveAsPrefabAsset(prefabInstance, path);

            DestroyImmediate(prefabInstance);

            Debug.Log($"Added smallship gameObjects to prefab: {prefab.name}");
        }

        AssetDatabase.Refresh();
    }

    private void AddCastPoint()
    {
        Object[] selectedObjects = Selection.objects;

        foreach (Object obj in selectedObjects)
        {
            string path = AssetDatabase.GetAssetPath(obj);

            if (string.IsNullOrEmpty(path) || !path.EndsWith(".prefab"))
            {
                Debug.LogWarning($"Skipped {obj.name}. Not a valid prefab.");
                continue;
            }

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null)
            {
                Debug.LogWarning($"Could not load prefab at path: {path}");
                continue;
            }

            GameObject prefabInstance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            if (prefabInstance == null)
            {
                Debug.LogWarning($"Could not instantiate prefab: {prefab.name}");
                continue;
            }

            GameObject castPoint = new GameObject("castPoint");
            castPoint.transform.parent = prefabInstance.transform;
            castPoint.transform.localPosition = new Vector3(0, 0, 0);

            PrefabUtility.SaveAsPrefabAsset(prefabInstance, path);

            DestroyImmediate(prefabInstance);

            Debug.Log($"Added smallship gameObjects to prefab: {prefab.name}");
        }

        AssetDatabase.Refresh();
    }

    private void AddEngineGlow(string name)
    {
        Object[] selectedObjects = Selection.objects;

        foreach (Object obj in selectedObjects)
        {
            string path = AssetDatabase.GetAssetPath(obj);

            if (string.IsNullOrEmpty(path) || !path.EndsWith(".prefab"))
            {
                Debug.LogWarning($"Skipped {obj.name}. Not a valid prefab.");
                continue;
            }

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null)
            {
                Debug.LogWarning($"Could not load prefab at path: {path}");
                continue;
            }

            GameObject prefabInstance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

            if (prefabInstance == null)
            {
                Debug.LogWarning($"Could not instantiate prefab: {prefab.name}");
                continue;
            }

            Object engineGlowPrefab = Resources.Load("particles/engineglows/" + name, typeof(GameObject));
            GameObject engineGlow = GameObject.Instantiate(engineGlowPrefab) as GameObject;
            engineGlow.transform.SetParent(prefabInstance.transform);
            engineGlow.transform.localPosition = new Vector3(0, 0, 0);

            PrefabUtility.SaveAsPrefabAsset(prefabInstance, path);

            DestroyImmediate(prefabInstance);

            Debug.Log($"Added smallship gameObjects to prefab: {prefab.name}");
        }

        AssetDatabase.Refresh();
    }

}