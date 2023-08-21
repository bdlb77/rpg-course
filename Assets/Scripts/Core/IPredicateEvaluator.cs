using UnityEngine;

namespace RPG.Core
{
    public interface IPredicateEvaluator
    {
        bool? Evaluate(string predicateFuncName, string[] parameters);
    }
}