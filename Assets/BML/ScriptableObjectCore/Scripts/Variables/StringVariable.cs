using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BML.ScriptableObjectCore.Scripts.Variables
{
    [Required]
    [CreateAssetMenu(fileName = "StringVariable", menuName = "BML/Variables/StringVariable", order = 0)]
    public class StringVariable : Variable<string> { }
}