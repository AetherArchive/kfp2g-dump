using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field)]
public sealed class EnumFlagsAttribute : PropertyAttribute
{
}
