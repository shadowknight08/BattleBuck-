using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Deterministic;

namespace Quantum
{
    public class ProjectileConfig : AssetObject
    {
        [Tooltip("Speed applied to the projectile when spawned")]
        public FP ProjectileInitialSpeed = 15;

        [Tooltip("Time until destroy the projectile")]
        public FP ProjectileTTL = 1;
    }
}
