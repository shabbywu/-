using System;
using System.Reflection;

namespace MoonSharp.Interpreter.Compatibility.Frameworks;

internal abstract class FrameworkClrBase : FrameworkReflectionBase
{
	private BindingFlags BINDINGFLAGS_MEMBER = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

	private BindingFlags BINDINGFLAGS_INNERCLASS = BindingFlags.Public | BindingFlags.NonPublic;

	public override Type GetTypeInfoFromType(Type t)
	{
		return t;
	}

	public override MethodInfo GetAddMethod(EventInfo ei)
	{
		return ei.GetAddMethod(nonPublic: true);
	}

	public override ConstructorInfo[] GetConstructors(Type type)
	{
		return type.GetConstructors(BINDINGFLAGS_MEMBER);
	}

	public override EventInfo[] GetEvents(Type type)
	{
		return type.GetEvents(BINDINGFLAGS_MEMBER);
	}

	public override FieldInfo[] GetFields(Type type)
	{
		return type.GetFields(BINDINGFLAGS_MEMBER);
	}

	public override Type[] GetGenericArguments(Type type)
	{
		return type.GetGenericArguments();
	}

	public override MethodInfo GetGetMethod(PropertyInfo pi)
	{
		return pi.GetGetMethod(nonPublic: true);
	}

	public override Type[] GetInterfaces(Type t)
	{
		return t.GetInterfaces();
	}

	public override MethodInfo GetMethod(Type type, string name)
	{
		return type.GetMethod(name);
	}

	public override MethodInfo[] GetMethods(Type type)
	{
		return type.GetMethods(BINDINGFLAGS_MEMBER);
	}

	public override Type[] GetNestedTypes(Type type)
	{
		return type.GetNestedTypes(BINDINGFLAGS_INNERCLASS);
	}

	public override PropertyInfo[] GetProperties(Type type)
	{
		return type.GetProperties(BINDINGFLAGS_MEMBER);
	}

	public override PropertyInfo GetProperty(Type type, string name)
	{
		return type.GetProperty(name);
	}

	public override MethodInfo GetRemoveMethod(EventInfo ei)
	{
		return ei.GetRemoveMethod(nonPublic: true);
	}

	public override MethodInfo GetSetMethod(PropertyInfo pi)
	{
		return pi.GetSetMethod(nonPublic: true);
	}

	public override bool IsAssignableFrom(Type current, Type toCompare)
	{
		return current.IsAssignableFrom(toCompare);
	}

	public override bool IsInstanceOfType(Type t, object o)
	{
		return t.IsInstanceOfType(o);
	}

	public override MethodInfo GetMethod(Type resourcesType, string name, Type[] types)
	{
		return resourcesType.GetMethod(name, types);
	}

	public override Type[] GetAssemblyTypes(Assembly asm)
	{
		return asm.GetTypes();
	}
}
