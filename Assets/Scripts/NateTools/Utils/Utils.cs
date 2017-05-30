//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="Utils.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace NateTools.Utils
{
    using UnityEngine;

    /// <summary>
    ///     Utility Methods
    /// </summary>
    public static class Utils
    {
        /// <summary>
        ///     Checks if a LayerMask contains a specific layer value
        /// </summary>
        /// <param name="layerMaskInt">The integer value of the layer to check for</param>
        /// <returns>True if the mask contains the layer</returns>
        public static bool Contains(this LayerMask val, int layerMaskInt)
        {
            return (val >> layerMaskInt) == 1;
        }

        /// <summary>
        ///     Create a Vector2 where both axes are the same
        /// </summary>
        /// <param name="xyz">axis value</param>
        /// <returns></returns>
        public static Vector2 Vector2XY(float xyz)
        {
            return new Vector2(xyz, xyz);
        }

        /// <summary>
        ///     Create a Vector2 where both axes are the same
        /// </summary>
        /// <param name="xyz">axis value</param>
        /// <returns></returns>
        public static Vector2 Vector2XY(int xyz)
        {
            return new Vector2(xyz, xyz);
        }

        /// <summary>
        ///     Create a Vector3 where all 3 axes are the same
        /// </summary>
        /// <param name="xyz">axis value</param>
        /// <returns></returns>
        public static Vector3 Vector3XYZ(float xyz)
        {
            return new Vector3(xyz, xyz, xyz);
        }

        /// <summary>
        ///     Create a Vector3 where all 3 axes are the same
        /// </summary>
        /// <param name="xyz">axis value</param>
        /// <returns></returns>
        public static Vector3 Vector3XYZ(int xyz)
        {
            return new Vector3(xyz, xyz, xyz);
        }
    }
}