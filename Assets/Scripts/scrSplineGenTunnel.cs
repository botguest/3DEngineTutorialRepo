using UnityEngine;
using UnityEngine.Splines;

// please attach this script to you desired spline.
public class scrSplineGenTunnel : MonoBehaviour
{
    public GameObject[] prefabs;
    public int start_knot;
    public int knot_count_after_start;
    
    void Start(){
        generateTunnuel(start_knot,knot_count_after_start);
    }

    private void generateTunnuel(int start_knot, int knot_count_after_start)
    {
        // creating a new spline game object...
        var go   = new GameObject("splineGenedTunnel");
        
        //aligning spline go transform to the go where this script is attached to...
        go.transform.SetParent(this.transform.parent, worldPositionStays: true);
        go.transform.position = this.transform.position;
        go.transform.rotation = this.transform.rotation;
        
        // adding splineContainer script...
        var sc   = go.AddComponent<SplineContainer>();  
        sc.Spline = new Spline(getSplineSliceFromSpline(start_knot, knot_count_after_start)); 
        
        // adding splineInstantiate script...
        var inst              = go.AddComponent<SplineInstantiate>();
        inst.Container        = sc;                   
        inst.InstantiateMethod = SplineInstantiate.Method.LinearDistance;
        inst.MinSpacing       = 2f;                    
        inst.MinPositionOffset = new Vector3(0f, 2.5f, 0f); // adjust this if generating a different arch / arch prefab modified.
        inst.MaxPositionOffset = new Vector3(0f, 2.5f, 0f);
        inst.PositionSpace = SplineInstantiate.OffsetSpace.Spline;
        inst.itemsToInstantiate = convertPrefabsToInstantiable(prefabs);
        
        // updating splineInstantiate...
        inst.UpdateInstances();   
    }

    // gets the spline slice from the spline component that's attached to the same game object as this script.
    private SplineSlice<Spline> getSplineSliceFromSpline(int start_knot, int knot_count_after_start)
    {
        return new SplineSlice<Spline>(this.GetComponent<SplineContainer>().Spline,
            new SplineRange(start_knot, knot_count_after_start, SliceDirection.Forward));
    }

    private SplineInstantiate.InstantiableItem[] convertPrefabsToInstantiable(GameObject[] prefabs)
    {
        var items = new SplineInstantiate.InstantiableItem[prefabs.Length];
        for (int i = 0; i < prefabs.Length; i++)
        {
            items[i] = new SplineInstantiate.InstantiableItem { Prefab = prefabs[i], Probability = 1f };
        }

        return items;
    }
}
