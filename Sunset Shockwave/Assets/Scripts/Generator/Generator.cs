using UnityEngine;

/// <summary>
/// Used as a toolset for generations.
/// </summary>
public class Generator : MonoBehaviour {

    /// <summary>
    /// Makes a new generation and picks a variable of any given table.
    /// </summary>
    public static GameObject Generation(GeneratorTable genTable)
    {
        // Defines local variables for this calculation.
        int totalChanceSum = 0;
        int[] intervals = new int[genTable.listVars.Count + 1];

        #region Weight Sums
        // Gets the weight of each variable and get's the interval of each object.
        for (int i = 0; i < genTable.listVars.Count; i++)
        {
            totalChanceSum += genTable.listVars[i].chance;
            intervals[i] = totalChanceSum;
        }
        #endregion

        // Generates a random value from all the weight limit.
        int randomValue = Random.Range(0, totalChanceSum + 1);

        #region Interval Calculations
        // Checks if the value is in any interval.
        for (int i = 0; i < intervals.Length - 1; i++)
        {
            // Assings the upper and lower limit of the random check.
            int lowerLimit = 0;
            int upperLimit = 0;

            // Fail-safe in case this is the first object being checked, so it doesnt give a non-existing
            // memory error.
            if (i == 0)
            {
                lowerLimit = 0;
            }
            else
            {
                lowerLimit = intervals[i - 1];
            }

            // Assigns the upper limit.
            upperLimit = intervals[i];

            // Checks if the random value belong to this limit.
            if (randomValue >= lowerLimit && randomValue <= upperLimit)
            {
                return genTable.listVars[i].variable;
            }
        }
        #endregion

        /* Returns null in worse case scenario. This only happens if we do something stupid.
        // Let's not do something stupid.
        // Please. */
        return null;
    }
}
