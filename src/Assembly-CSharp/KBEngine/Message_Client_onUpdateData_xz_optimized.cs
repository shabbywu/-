﻿using System;
using System.Collections.Generic;

namespace KBEngine
{
	// Token: 0x02000F8B RID: 3979
	public class Message_Client_onUpdateData_xz_optimized : Message
	{
		// Token: 0x06005F03 RID: 24323 RVA: 0x00042600 File Offset: 0x00040800
		public Message_Client_onUpdateData_xz_optimized(ushort msgid, string msgname, short length, sbyte argstype, List<byte> msgargtypes) : base(msgid, msgname, length, argstype, msgargtypes)
		{
		}

		// Token: 0x06005F04 RID: 24324 RVA: 0x000427E3 File Offset: 0x000409E3
		public override void handleMessage(MemoryStream msgstream)
		{
			KBEngineApp.app.Client_onUpdateData_xz_optimized(msgstream);
		}
	}
}
