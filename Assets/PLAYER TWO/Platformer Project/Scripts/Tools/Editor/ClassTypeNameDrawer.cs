using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

/// <summary>
/// PropertyDrawer提供OnGUI绘制接口。
/// CustomPropertyDrawer指定这个PropertyDrawer是用来绘制哪个属性的，这样在Inspector中遇到这个属性时就会使用这个PropertyDrawer来绘制
/// </summary>
[CustomPropertyDrawer(typeof(ClassTypeName))]
public class ClassTypeNameDrawer : PropertyDrawer
{
    private ClassTypeName classTypeName;
    // 子类的包含命名空间的类型名
    private List<string> subclassFullNames;
    // 格式化后的子类类型名(不包含明明空间，且根据驼峰命名法添加空格)
    private List<string> subclassFormattedNames;

    private bool initialized = false;

    /// <summary>
    /// Unity调用入口
    /// </summary>
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (!initialized)
        {
            initialized = true;
            Initialize();
        }

        InitializeProperty(property);
        HandleGUI(position, property, label);
    }

    private void Initialize()
    {
        classTypeName = attribute as ClassTypeName;

        var subclasses = System.AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsSubclassOf(classTypeName.BaseType));
        
        subclassFullNames = subclasses
            .Select(type => type.ToString())
            .ToList();

        subclassFormattedNames = subclasses
            .Select(type => type.Name)
            .Select(name => Regex.Replace(name, "(\\B[A-Z])", " $1"))   // ()捕获分组，后面$1引用。\B表示非单词边界，[A-Z]表示大写字母
            .ToList();
    }

    /// <summary>
    /// 初始化属性值
    /// 如果属性为空字符串，则默认选择列表中的第一个子类
    /// </summary>
    /// <param name="property">SerializedProperty 对象</param>
    private void InitializeProperty(SerializedProperty property)
    {
        if (property.stringValue.Length == 0)
        {
            property.stringValue = subclassFullNames[0];
        }
    }

    /// <summary>
    /// 绘制 Inspector 下拉列表 GUI。
    /// 属性值用带命名空间的全名，实际显示用格式化的名字
    /// </summary>
    /// <param name="position">绘制区域</param>
    /// <param name="property">当前属性</param>
    /// <param name="label">显示标签</param>
    private void HandleGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (!subclassFullNames.Contains(property.stringValue)) return;

        int currentIndex = subclassFullNames.IndexOf(property.stringValue);
        position = EditorGUI.PrefixLabel(position, label);
        // 绘制下拉列表，返回选择的索引
        int selectedIndex = EditorGUI.Popup(position, currentIndex, subclassFormattedNames.ToArray());
        property.stringValue = subclassFullNames[selectedIndex];
    }
}