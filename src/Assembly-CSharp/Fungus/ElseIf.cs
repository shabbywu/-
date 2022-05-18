﻿using System;
using UnityEngine;

namespace Fungus
{
	// Token: 0x02001204 RID: 4612
	[CommandInfo("Flow", "Else If", "Marks the start of a command block to be executed when the preceding If statement is False and the test expression is true.", 0)]
	[AddComponentMenu("")]
	public class ElseIf : VariableCondition
	{
		// Token: 0x17000A62 RID: 2658
		// (get) Token: 0x060070DC RID: 28892 RVA: 0x0000A093 File Offset: 0x00008293
		protected override bool IsElseIf
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060070DD RID: 28893 RVA: 0x0000A093 File Offset: 0x00008293
		public override bool OpenBlock()
		{
			return true;
		}

		// Token: 0x060070DE RID: 28894 RVA: 0x0000A093 File Offset: 0x00008293
		public override bool CloseBlock()
		{
			return true;
		}

		// Token: 0x060070DF RID: 28895 RVA: 0x0004C5A3 File Offset: 0x0004A7A3
		public override Color GetButtonColor()
		{
			return new Color32(253, 253, 150, byte.MaxValue);
		}
	}
}
