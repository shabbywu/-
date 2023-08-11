using System;
using MoonSharp.Interpreter.Interop.BasicDescriptors;
using MoonSharp.Interpreter.Interop.Converters;

namespace MoonSharp.Interpreter.Interop.StandardDescriptors.HardwiredDescriptors;

public abstract class HardwiredMemberDescriptor : IMemberDescriptor
{
	public Type MemberType { get; private set; }

	public bool IsStatic { get; private set; }

	public string Name { get; private set; }

	public MemberDescriptorAccess MemberAccess { get; private set; }

	protected HardwiredMemberDescriptor(Type memberType, string name, bool isStatic, MemberDescriptorAccess access)
	{
		IsStatic = isStatic;
		Name = name;
		MemberAccess = access;
		MemberType = memberType;
	}

	public DynValue GetValue(Script script, object obj)
	{
		this.CheckAccess(MemberDescriptorAccess.CanRead, obj);
		object valueImpl = GetValueImpl(script, obj);
		return ClrToScriptConversions.ObjectToDynValue(script, valueImpl);
	}

	public void SetValue(Script script, object obj, DynValue value)
	{
		this.CheckAccess(MemberDescriptorAccess.CanWrite, obj);
		object value2 = ScriptToClrConversions.DynValueToObjectOfType(value, MemberType, null, isOptional: false);
		SetValueImpl(script, obj, value2);
	}

	protected virtual object GetValueImpl(Script script, object obj)
	{
		throw new InvalidOperationException("GetValue on write-only hardwired descriptor " + Name);
	}

	protected virtual void SetValueImpl(Script script, object obj, object value)
	{
		throw new InvalidOperationException("SetValue on read-only hardwired descriptor " + Name);
	}
}
