using System.Collections;
using UnityEngine;

namespace Battle.State_Machine
{
    public class Opening : State
    {
        public Opening(BattleManager bm) : base(bm)
        {
        }

        public override IEnumerator EnterState()
        {
            Debug.Log("It has begun.");
            yield break;
        }
    }
}