using Unity.Data.Curves.Serializable;
using UnityEngine;

namespace Unity.Data.Curves;

[CreateAssetMenu(menuName = "Curves/Curve Data Storage")]
public class CurveDataStorage : ScriptableObject
{
    public CurveData CurveData;
}