//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="HealthBar.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace UI
{
    using Events;

    using UnityEngine;

    public class HealthBar : MonoBehaviour
    {
        public GameObject HealthIconPrefab;

        private void HealthChanged(GameEvent arg0)
        {
            var healthChanged = arg0 as HealthChanged;
            if (healthChanged != null)
            {
                var difference = healthChanged.Value - transform.childCount;
                if (difference > 0)
                {
                    SpawnHealth(difference);
                }
                else if (difference < 0)
                {
                    RemoveHealth(Mathf.Abs(difference));
                }
            }
        }

        private void OnDisable()
        {
            EventManager.RemoveListener<HealthChanged>(HealthChanged);
        }

        private void OnEnable()
        {
            EventManager.AddListener<HealthChanged>(HealthChanged);
        }

        private void RemoveHealth(int number)
        {
            for (var i = 0; i < number; i++)
            {
                if (transform.childCount > 0)
                {
                    var t = transform.GetChild(0).gameObject;
                    Destroy(t);
                }
            }
        }

        private void SpawnHealth(int number)
        {
            for (var i = 0; i < number; i++)
            {
                Instantiate(HealthIconPrefab).GetComponent<RectTransform>().SetParent(transform);
            }
        }
    }
}