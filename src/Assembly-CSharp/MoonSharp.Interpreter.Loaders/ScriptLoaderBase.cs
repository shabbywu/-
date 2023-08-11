using System;
using System.Linq;

namespace MoonSharp.Interpreter.Loaders;

public abstract class ScriptLoaderBase : IScriptLoader
{
	public string[] ModulePaths { get; set; }

	public bool IgnoreLuaPathGlobal { get; set; }

	public abstract bool ScriptFileExists(string name);

	public abstract object LoadFile(string file, Table globalContext);

	protected virtual string ResolveModuleName(string modname, string[] paths)
	{
		if (paths == null)
		{
			return null;
		}
		modname = modname.Replace('.', '/');
		for (int i = 0; i < paths.Length; i++)
		{
			string text = paths[i].Replace("?", modname);
			if (ScriptFileExists(text))
			{
				return text;
			}
		}
		return null;
	}

	public virtual string ResolveModuleName(string modname, Table globalContext)
	{
		if (!IgnoreLuaPathGlobal)
		{
			DynValue dynValue = globalContext.RawGet("LUA_PATH");
			if (dynValue != null && dynValue.Type == DataType.String)
			{
				return ResolveModuleName(modname, UnpackStringPaths(dynValue.String));
			}
		}
		return ResolveModuleName(modname, ModulePaths);
	}

	public static string[] UnpackStringPaths(string str)
	{
		return (from s in str.Split(new char[1] { ';' }, StringSplitOptions.RemoveEmptyEntries)
			select s.Trim() into s
			where !string.IsNullOrEmpty(s)
			select s).ToArray();
	}

	public static string[] GetDefaultEnvironmentPaths()
	{
		string[] array = null;
		if (array == null)
		{
			string environmentVariable = Script.GlobalOptions.Platform.GetEnvironmentVariable("MOONSHARP_PATH");
			if (!string.IsNullOrEmpty(environmentVariable))
			{
				array = UnpackStringPaths(environmentVariable);
			}
			if (array == null)
			{
				environmentVariable = Script.GlobalOptions.Platform.GetEnvironmentVariable("LUA_PATH");
				if (!string.IsNullOrEmpty(environmentVariable))
				{
					array = UnpackStringPaths(environmentVariable);
				}
			}
			if (array == null)
			{
				array = UnpackStringPaths("?;?.lua");
			}
		}
		return array;
	}

	public virtual string ResolveFileName(string filename, Table globalContext)
	{
		return filename;
	}
}
