using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Scriptable Object that holds the different variables that can be generated.
/// </summary>
[CreateAssetMenu(fileName = "Generator Table", menuName = "Custom/Generator Table", order = 2)]
public class GeneratorTable : ScriptableObject {

    public List<GeneratorVariable> listVars = new List<GeneratorVariable>();
}
