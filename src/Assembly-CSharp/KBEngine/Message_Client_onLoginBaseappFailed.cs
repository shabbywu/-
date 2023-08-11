using System.Collections.Generic;

namespace KBEngine;

public class Message_Client_onLoginBaseappFailed : Message
{
	public Message_Client_onLoginBaseappFailed(ushort msgid, string msgname, short length, sbyte argstype, List<byte> msgargtypes)
		: base(msgid, msgname, length, argstype, msgargtypes)
	{
	}

	public override void handleMessage(MemoryStream msgstream)
	{
		ushort failedcode = msgstream.readUint16();
		KBEngineApp.app.Client_onLoginBaseappFailed(failedcode);
	}
}
