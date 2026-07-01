#region Libraries
using UnityEngine;
#endregion

namespace LunaGames.Extentions.Probability
{
    [CreateAssetMenu(fileName = "Probability Table", menuName = "New Probability Table")]
    public class ProbabilityEngine: ScriptableObject
    {
        #region Properties
        public ProbabilityTable<Object> ProbabilityTable = new ProbabilityTable<Object>();
        #endregion
        
        public T GetValue<T>() where T : Object
        {
            return ProbabilityTable.GetValue() as T;
        }
    }
}
