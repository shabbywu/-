using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Debugging;

namespace MoonSharp.VsCodeDebugger.DebuggerLogic;

internal interface IAsyncDebuggerClient
{
	void SendStopEvent();

	void OnWatchesUpdated(WatchType watchType);

	void OnSourceCodeChanged(int sourceID);

	void OnExecutionEnded();

	void OnException(ScriptRuntimeException ex);

	void Unbind();
}
