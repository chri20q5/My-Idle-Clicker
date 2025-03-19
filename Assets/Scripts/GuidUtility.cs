
    using System;
    using UnityEngine;

    public class GuidUtility : MonoBehaviour
    {
        public static string GenerateGuid()
        {
            return Guid.NewGuid().ToString();
        }
    }
