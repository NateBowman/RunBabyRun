//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="Chunk.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace Terrain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using NateTools.Attributes;
    using NateTools.Utils;

    using UnityEngine;

    using Random = UnityEngine.Random;

#if UNITY_EDITOR
#endif

    /// <summary>
    ///     A chunk of generated sections
    /// </summary>
    public class Chunk : TerrainArea
    {
        /// <summary>
        ///     List of all Non-Terminal symbols
        /// </summary>
        public static List<string> NonTerminal = new List<string>() { "A", "B", "E" };

        /// <summary>
        ///     List of production grammars that govern the creation of terrain
        /// </summary>
        private static readonly List<Grammar> Grammars = new List<Grammar>
                                                             {
                                                                 // A = Start
                                                                 // B = up + down
                                                                 // 
                                                                 // a = incline / ramp up
                                                                 // b = ramp down
                                                                 // 
                                                                 // c = stairs up
                                                                 // d = stairs down
                                                                 // 
                                                                 // e = jump up
                                                                 // f = jump down
                                                                 // 
                                                                 // g = hopscotch(intense jumps)
                                                                 // 
                                                                 // x = end of section
                                                                 // 
                                                                 // i = Flat
                                                                 // 
                                                                 // --Generation of Obstacles--
                                                                 // A = i + (a | b | c | d | e | f | g | B) + (A)
                                                                 // 
                                                                 // A = E[when length reached]
                                                                 // 
                                                                 // B = (a | c | e) + (b | d | f)
                                                                 // 
                                                                 // E = x
                                                                 new Grammar("A", "i+a+A", 1f),
                                                                 new Grammar("A", "i+b+A", 1f),
                                                                 new Grammar("A", "i+c+A", 1f),
                                                                 new Grammar("A", "i+d+A", 1f),
                                                                 new Grammar("A", "i+e+A", 1f),
                                                                 new Grammar("A", "i+f+A", 1f),
                                                                 new Grammar("A", "i+g+A", 1f),
                                                                 new Grammar("A", "i+B+A", 1f),
                                                                 new Grammar("B", "a+b", 1f),
                                                                 new Grammar("B", "a+d", 1f),
                                                                 new Grammar("B", "a+f", 1f),
                                                                 new Grammar("B", "c+b", 1f),
                                                                 new Grammar("B", "c+d", 1f),
                                                                 new Grammar("B", "c+f", 1f),
                                                                 new Grammar("B", "e+b", 1f),
                                                                 new Grammar("B", "e+d", 1f),
                                                                 new Grammar("B", "e+f", 1f)
                                                             };

        /// <summary>
        ///     A list of all Terrain section prefabs to be populated at runtime;
        /// </summary>
        private static List<GameObject> prefabs = new List<GameObject>();

        /// <summary>
        ///     The length of the Chunk in sections
        /// </summary>
        public int SectionLength;

        /// <summary>
        ///     The slope of the line of best fit through the terrain
        /// </summary>
        public float Slope;

        /// <summary>
        ///     A measure of how straight (not bumpy) the terrain is
        /// </summary>
        public float Straightness;

        /// <summary>
        ///     The Length of the chunk in Units
        /// </summary>
        public int UnitLength;

        /// <summary>
        ///     Generate a string from production grammars for the chunk
        /// </summary>
        /// <param name="length">The required <paramref name="length" /> in Sections of the chunk</param>
        /// <returns>Procedural string</returns>
        public static string GenerateProductionString(int length)
        {
            var currentLength = 0;

            var lookup = new Dictionary<string, List<Grammar>>();

            // All generation starts with the first non-Terminal symbol
            var currentProduction = "A";

            // create a lookup list of the grammars so we don't have to do it for every replacement
            foreach (var grammar in Grammars)
            {
                if (!lookup.ContainsKey(grammar.Lhs))
                {
                    lookup.Add(grammar.Lhs, new List<Grammar>());
                }

                lookup[grammar.Lhs].Add(grammar);
            }

            while (currentLength < length)
            {
                var tempProduction = "";
                for (var index = 0; index < currentProduction.Length; index++)
                {
                    var chr = currentProduction[index].ToString();
                    if (lookup.ContainsKey(chr))
                    {
                        // Process the selected non-terminal grammar
                        Grammar selectedGrammar = null;

                        // probability test iteration counter
                        for (var counter = 100; counter > 0; counter--)
                        {
                            // Select a random element from the grammar table
                            var element = lookup[chr][Random.Range(0, lookup[chr].Count - 1)];

                            // Random test against probability prop
                            if (!(Random.value < element.Probability))
                            {
                                // if the test fails, run try again
                                continue;
                            }

                            // Assign the selected grammar and exit the selection loop
                            selectedGrammar = element;

                            // Reduce the probability of the same section appearing again
                            // TODO: This should be replaced with a more considered method 
                            selectedGrammar.Probability = selectedGrammar.Probability / 2f;
                            break;
                        }

                        // If the random test failed, then just select a random grammar
                        // TODO: There should be a fall back that is not just random
                        if (selectedGrammar == null)
                        {
                            selectedGrammar = lookup[chr][Random.Range(0, lookup[chr].Count - 1)];
                        }

                        // Add the string to the generated string
                        tempProduction += selectedGrammar.Rhs;

                        // Add the rest of the remaining string
                        if (index < currentProduction.Length)
                        {
                            tempProduction += currentProduction.Substring(index + 1);
                        }

                        // break out of the loop and evaluate again from the start
                        break;
                    }
                    else
                    {
                        // skip over the terminal or operator string
                        tempProduction += chr;
                    }
                }

                // count the terminal symbols as the number of sections
                currentLength = tempProduction.Count(c => char.IsLower(c));
                currentProduction = tempProduction;
            }

            // Dirty way to add the final terminator and remove the trailing A
            if (currentProduction[currentProduction.Length - 1] == 'A')
            {
                currentProduction = currentProduction.Substring(0, currentProduction.Length - 1) + "x";
            }

            return currentProduction.Replace("[", "").Replace("]", "");
        }

        public static void Init()
        {
            // The prefabs list is shared among instances, we only need to populate it once
            if (prefabs.Count == 0)
            {
                // clear prefab list
                prefabs = new List<GameObject>();

                var p = Resources.LoadAll("TemplateSections");

                // find all prefabs in the section template folder and add them to the prefab list
                //foreach (var s in AssetDatabase.FindAssets("t:prefab", new[] { "Assets/Prefabs/Terrain/TemplateSections" }))
                //{
                //    prefabs.Add(AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(s)));
                //}
                prefabs = p.Cast<GameObject>().ToList();
            }
        }

        /// <summary>
        ///     Create all sections of the chink from a string
        /// </summary>
        /// <param name="inputString">A string that adheres to the production grammars</param>
        public void CreateTerrain(string inputString)
        {
            var currentLocation = Vector3.zero;

            // iterate over the grammar, select a section for each terminal and spawn the prefab at end location of the last section
            foreach (var symbol in inputString)
            {
                switch (symbol)
                {
                    case 'a':
                        currentLocation = SpawnSectionOfType(SectionType.RampUp, currentLocation);
                        break;
                    case 'b':
                        currentLocation = SpawnSectionOfType(SectionType.RampDown, currentLocation);

                        break;
                    case 'c':
                        currentLocation = SpawnSectionOfType(SectionType.StairsUp, currentLocation);
                        break;
                    case 'd':
                        currentLocation = SpawnSectionOfType(SectionType.StairsDown, currentLocation);
                        break;
                    case 'e':
                        currentLocation = SpawnSectionOfType(SectionType.JumpUp, currentLocation);
                        break;
                    case 'f':
                        currentLocation = SpawnSectionOfType(SectionType.JumpDown, currentLocation);
                        break;
                    case 'g':
                        currentLocation = SpawnSectionOfType(SectionType.Hopscotch, currentLocation);
                        break;
                    case 'i':
                        currentLocation = SpawnSectionOfType(SectionType.Flat, currentLocation);
                        break;
                    case 'x':
                        // TODO: Add a terminator section?
                        break;
                }
            }

            // Set the start and end locations of the chunk
            StartLocation = transform.GetChild(0).GetComponent<Section>().StartLocation;
            EndLocation = transform.GetChild(transform.childCount - 1).GetComponent<Section>().EndLocation;
        }

#if UNITY_EDITOR
        /// <summary>
        ///     A method for testing generation inside the editor
        /// </summary>
        [Button]
        public void TestGeneration()
        {
            Start();
            gameObject.DestroyAllChildrenImmediate();
            var productionGrammar = GenerateProductionString(20);
            SectionLength = productionGrammar.Count(c => char.IsLower(c));
            CreateTerrain(productionGrammar);
        }

#endif

        /// <summary>
        ///     Create a section of a given type.
        /// </summary>
        /// <param name="sectionType">The type of section to create</param>
        /// <param name="currentLocation">The location to place the section</param>
        /// <returns>The end position of the section</returns>
        private Vector3 SpawnSectionOfType(SectionType sectionType, Vector3 currentLocation)
        {
            if (prefabs.Count == 0)
            {
                Init();
            }
            var selectedPrefabs = prefabs.Where(o => o.GetComponent<Section>().SectionType == sectionType).ToList();

            Debug.Log(string.Format("Trying to spawn a {0} : {1} found", Enum.GetName(typeof(SectionType), sectionType), selectedPrefabs.Count));

            if (selectedPrefabs.Count > 0)
            {
                // spawn one
                var go = (Instantiate(selectedPrefabs[Random.Range(0, selectedPrefabs.Count - 1)], currentLocation, Quaternion.identity) as GameObject).SetParent(gameObject);
                var section = go.GetComponent<Section>();
                section.PositionStartAt(currentLocation);
                return section.EndLocation.EndPosition;
            }

            return currentLocation;
        }

        private void Start()
        {
        }

        /// <summary>
        ///     An implementation of a simple production grammar
        /// </summary>
        internal class Grammar
        {
            /// <summary>
            ///     The left hand side of a grammar
            /// </summary>
            public string Lhs;

            /// <summary>
            ///     The probability of the symbol occurring
            /// </summary>
            public float Probability;

            /// <summary>
            ///     The right hand side of the grammar
            /// </summary>
            public string Rhs;

            /// <summary>
            ///     Initializes a new instance of the <see cref="Grammar" /> class.
            /// </summary>
            /// <param name="lhs">Left hand Side rule</param>
            /// <param name="rhs">Right hand side production</param>
            /// <param name="probability">Chance of the rule being used</param>
            public Grammar(string lhs, string rhs, float probability)
            {
                Lhs = lhs;
                Rhs = rhs;
                Probability = probability;
            }
        }
    }
}