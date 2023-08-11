using System;

namespace MoonSharp.Interpreter.Loaders;

internal class InvalidScriptLoader : IScriptLoader
{
	private string m_Error;

	internal InvalidScriptLoader(string frameworkname)
	{
		m_Error = $"Loading scripts from files is not automatically supported on {frameworkname}. \r\nPlease implement your own IScriptLoader (possibly, extending ScriptLoaderBase for easier implementation),\r\nuse a preexisting loader like EmbeddedResourcesScriptLoader or UnityAssetsScriptLoader or load scripts from strings.";
	}

	public object LoadFile(string file, Table globalContext)
	{
		throw new PlatformNotSupportedException(m_Error);
	}

	public string ResolveFileName(string filename, Table globalContext)
	{
		return filename;
	}

	public string ResolveModuleName(string modname, Table globalContext)
	{
		throw new PlatformNotSupportedException(m_Error);
	}
}
