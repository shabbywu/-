﻿using System;
using System.Collections.Generic;

namespace KBEngine
{
	// Token: 0x02000FC3 RID: 4035
	public class Message_Loginapp_reqAccountResetPassword : Message
	{
		// Token: 0x06005F73 RID: 24435 RVA: 0x00042600 File Offset: 0x00040800
		public Message_Loginapp_reqAccountResetPassword(ushort msgid, string msgname, short length, sbyte argstype, List<byte> msgargtypes) : base(msgid, msgname, length, argstype, msgargtypes)
		{
		}

		// Token: 0x06005F74 RID: 24436 RVA: 0x000042DD File Offset: 0x000024DD
		public override void handleMessage(MemoryStream msgstream)
		{
		}
	}
}
