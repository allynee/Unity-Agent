// THIS IS AN ARCHIVE

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Enums;


public class SceneAPI : MonoBehaviour
{
    public List<Object3D> allObjectsInScene;
    private List<string> prefabNames = new();
    
    void Start()
    {
        prefabNames.AddRange(Resources.LoadAll("Prefabs/").Cast<GameObject>().Select(obj => obj.name));
    }

    void Update()
    {
        
    }

    public virtual void PerformAction()
    {

    }


    public List<Object3D> GetAllObject3DsInScene() { return allObjectsInScene; }
    public Object3D FindObject3DByName(string objName) { return GetObject3DByGameObject(GameObject.Find(objName)); }
    public bool IsObject3DInFieldOfView(Object3D obj) { return GeometryUtility.TestPlanesAABB(GetCamFrustum(), obj.collider.bounds) ? true : false;  }
    public bool IsCustomObjectNameValid(string customObjName) { return prefabNames.Contains(customObjName); }
    public Vector3D GetUserPosition() { return (Vector3D)Camera.main.transform.position; }
    public List<string> GetAllValidCustomObjectNames() { return prefabNames; }
    public Object3D CreateObject(string newObjName, ObjectType newObjType, string customObjName = null, Vector3D position = null, Vector3D direction = null, Object3D parent = null)
    {
        Vector3 pos = (position == null) ? Vector3.zero : (Vector3)position;
        print(pos);
        Vector3 dir = (direction == null) ? Vector3.zero : (Vector3)direction;
        print(dir);
        GameObject par = (parent == null ) ? gameObject : parent.gameObject;
        print(par.name);

        Object3D newObj = newObjType switch
        {
            ObjectType.Custom => CreateNewPrefab(newObjName, customObjName, par.transform, pos, Quaternion.Euler(dir)),
            ObjectType.Capsule => CreateNewPrimitive(newObjName, PrimitiveType.Capsule),
            ObjectType.Sphere => CreateNewPrimitive(newObjName, PrimitiveType.Sphere),
            ObjectType.Cuboid => CreateNewPrimitive(newObjName, PrimitiveType.Cube),
            ObjectType.Cylinder => CreateNewPrimitive(newObjName, PrimitiveType.Cylinder),
            _ => throw new System.Exception("Invalid object type. " + newObjType.ToString())
        };
        allObjectsInScene.Add(newObj);
        return newObj;

    }

    public List<Object3D> GetAllObject3DsInFieldOfView()
    {
        Plane[] camFrustum = GetCamFrustum();
        List<Object3D> objectsInView = new();
        foreach (Object3D obj in allObjectsInScene)
            if (GeometryUtility.TestPlanesAABB(camFrustum, obj.collider.bounds))
                objectsInView.Add(obj);

        return objectsInView;
    }

    public void SetObjectOn(Object3D baseObject, Object3D objectToBePlaced, SurfaceMode surfaceMode = SurfaceMode.Top)
    {
        Vector3 planePos = baseObject.GetSurfacePosition(surfaceMode);
        Vector3 planeDir = baseObject.GetSurfaceDirection(surfaceMode);
        float halfSize = objectToBePlaced.GetSize().y / 2;
        Vector3 posOffset = halfSize * planeDir;
        objectToBePlaced.SetRotation(baseObject.GetRotation());
        if (planeDir.y == 1)
            objectToBePlaced.Levitate(false);
        else
            objectToBePlaced.Levitate(true);
        objectToBePlaced.SetPosition((Vector3D)(planePos + posOffset));
    }

    private Object3D GetObject3DByGameObject(GameObject obj)
    {
        return obj != null ? obj.GetComponent<Object3D>() : throw new System.Exception("No such object exists!");
    }

    private Object3D CreateNewPrefab(string newObjectName, string prefabName, Transform parent = null, Vector3 position = default, Quaternion rotation = default)
    {
        print("we're in create new prefav");
        if (parent == null) parent = transform;

        Object prefab = Resources.Load("Prefabs/" + prefabName);    
        if (prefab == null) throw new System.Exception($"There is no custom object with the name '{prefabName}'.");
        GameObject obj = (GameObject)Instantiate(prefab, position, rotation, parent);        
        obj.name = newObjectName;
        return GetObject3DByGameObject(obj);
    }
    private Object3D CreateNewPrimitive(string newObjectName, PrimitiveType primitiveType)
    {
        GameObject obj = GameObject.CreatePrimitive(primitiveType);
        obj.name = newObjectName;
        return GetObject3DByGameObject(obj);
    }
/*    private void CloneObject(string newObjectName, GameObject originalObj, Transform parent = null, Vector3 position = default, Quaternion rotation = default)
    {
        if (!parent)
            parent = transform;

        GameObject obj = Instantiate(originalObj, position, rotation, parent);
        obj.name = newObjectName;
    }*/
    private Plane[] GetCamFrustum()
    {
        return GeometryUtility.CalculateFrustumPlanes(Camera.main);
    }


}
