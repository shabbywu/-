namespace MoonSharp.Interpreter;

public delegate object ScriptFunctionDelegate(params object[] args);
public delegate T ScriptFunctionDelegate<T>(params object[] args);
