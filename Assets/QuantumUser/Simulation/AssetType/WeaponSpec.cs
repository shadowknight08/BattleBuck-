using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Deterministic;
namespace Quantum
{
    public class WeaponSpec :AssetObject
    {
        public AssetRef<EntityPrototype> bulletPrototype;

        public FP ShotOffset = 1;
        public FP FireInterval = FP._0_10;

    }
}
