using System;
using UnityEngine;

/// <summary>
/// PropertyAttribute继承自System.Attribute，继承自System.Attribute就可以用作字段属性，通过[]使用，PropertyAttribute是针对Unity的。
/// 这个属性的作用是可以在Inspector中显示并选择某个类及其子类的类型名称
/// </summary>
public class ClassTypeName : PropertyAttribute
{
    public Type BaseType { get; private set; }

    public ClassTypeName(Type baseType)
    {
        BaseType = baseType;
    }
}