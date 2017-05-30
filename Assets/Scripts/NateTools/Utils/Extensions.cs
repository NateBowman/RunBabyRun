//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="Extensions.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace NateTools.Utils
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using UnityEngine;

    /// <summary>
    ///     Extensions to Unity built in types.
    ///     Many are to support method chaining
    ///     Some are just helper methods such as checking if an animator has a parameter
    /// </summary>
    public static class Extensions
    {
        public static Vector3 Add(this Vector2 xy, Vector3 xyz)
        {
            return new Vector3(xy.x + xyz.x, xy.y + xyz.y, xyz.z);
        }

        public static Vector2 Add(this Vector2 xy, float x, float y)
        {
            return new Vector2(xy.x + x, xy.y + y);
        }

        public static Vector2 Add(this Vector2 xy, float x)
        {
            return xy.Add(x, 0);
        }

        public static Vector3 Add(this Vector3 xyz, float x, float y, float z)
        {
            return new Vector3(xyz.x + x, xyz.y + y, xyz.z + z);
        }

        public static Vector3 Add(this Vector3 xyz, float x, float y)
        {
            return xyz.Add(x, y, 0);
        }

        public static Vector3 Add(this Vector3 xyz, float x)
        {
            return xyz.Add(x, 0, 0);
        }

        public static Vector3 Add(this Vector3 xyz, Vector2 xy)
        {
            return new Vector3(xy.x + xyz.x, xy.y + xyz.y, xyz.z);
        }

        public static Vector3 AsVector3(this Vector2 xy)
        {
            return (Vector3)xy;
        }

        public static List<Transform> ChildrenToList(this Transform transform)
        {
            var children = new List<Transform>();
            foreach (Transform child in transform)
            {
                children.Add(child);
            }

            return children;
        }

        public static Vector3 CreateVector3(this float xyz)
        {
            return new Vector3(xyz, xyz, xyz);
        }

        public static void DestroyAllChildren(this GameObject go, string nameRestriction = "")
        {
            {
                for (var i = go.transform.childCount - 1; i >= 0; i--)
                {
                    var gObj = go.transform.GetChild(i).gameObject;

                    if (gObj.name.StartsWith(nameRestriction) || gObj.name.StartsWith("Generated -"))
                    {
                        Object.Destroy(gObj);
                    }
                }
            }
        }

        public static void DestroyAllChildrenImmediate(this GameObject go, string nameRestriction = "")
        {
            {
                for (var i = go.transform.childCount - 1; i >= 0; i--)
                {
                    var gObj = go.transform.GetChild(i).gameObject;

                    if (gObj.name.StartsWith(nameRestriction) || gObj.name.StartsWith("Generated -"))
                    {
                        Object.DestroyImmediate(gObj);
                    }
                }
            }
        }

        public static bool HasParameter(this Animator anim, string parameterName)
        {
            return anim.parameters.Any(acp => acp.name == parameterName);
        }

        public static bool IsAutoProperty(this PropertyInfo prop)
        {
            if (!prop.CanWrite || !prop.CanRead)
            {
                return false;
            }

            return (prop.DeclaringType != null) && prop.DeclaringType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance).Any(f => f.Name.Contains("<" + prop.Name + ">"));
        }

        public static GameObject SetLocalPosition(this GameObject gameObject, Vector3 position)
        {
            gameObject.transform.localPosition = position;
            return gameObject;
        }

        public static GameObject SetName(this GameObject gameObject, string name)
        {
            gameObject.name = name;
            return gameObject;
        }

        public static GameObject SetParent(this GameObject gameObject, Transform parentTransform)
        {
            gameObject.transform.parent = parentTransform;
            return gameObject;
        }

        public static GameObject SetParent(this GameObject gameObject, GameObject parentGameObject)
        {
            gameObject.SetParent(parentGameObject.transform);
            return gameObject;
        }

        public static GameObject SetPosition(this GameObject gameObject, Vector3 position)
        {
            gameObject.transform.position = position;
            return gameObject;
        }

        public static GameObject SetRotation(this GameObject gameObject, Quaternion rotation)
        {
            gameObject.transform.rotation = rotation;
            return gameObject;
        }

        public static Vector2 ToVector2(this float x, float y)
        {
            return new Vector2(x, y);
        }

        public static Vector2 ToVector2(this int x, int y)
        {
            return new Vector2(x, y);
        }

        public static Vector3 ToVector3(this Vector2 xy, float z)
        {
            return new Vector3(xy.x, xy.y, z);
        }
    }
}