using Microsoft.Win32;
using System;
using System.Threading.Tasks;


internal class ShareInfoTemplate : CheckTemplate
{
	private RegistryKey ShareKey;
	private SafeString[] ValueNames;
	private SafeString KeyName;
	/// <summary>
	/// 
	/// </summary>
	/// <param name="args">0 = sharename, 1+ = keys</param>
	internal ShareInfoTemplate(params string[] args)
	{
		if (args.Length < 2)
		{
			Enabled = false;
			return;
		}
		
		try
		{
			ShareKey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\LanmanServer\Shares");
			KeyName = args[0];
			int numValues = args.Length - 1;
			ValueNames = new SafeString[numValues];

			for (int i = 0; i < numValues; i++)
			{
				ValueNames[i] = args[i + 1];
			}
		}

		catch
		{
			Enabled = false;
		}
	}

	/// <summary>
	/// This iterates through all the requested properties for this share and xors the results together. if all the requested keys are the correct value, the final result will be correct too.
	/// </summary>
	/// <returns></returns>
	internal override async Task<byte[]> GetCheckValue()
	{
		byte[] value = new byte[0];

		try
		{
			foreach (string Value in ShareKey?.GetValue(KeyName) as string[] ?? new string[0])
			{
				string[] split = Value.Split('=');
				bool found = false;
				foreach (string s in ValueNames)
				{
					if (s.ToLower().Trim() == split[0].ToLower().Trim())
					{
						found = true;
					}
				}

				if (found)
				{
					value = XORStates(value, PrepareState32(split.Length > 1 ? split[1] : ""));
				}
			}
		}
		catch
		{
				
		}

		return await Task.FromResult(value);
	}

	private static byte[] XORStates(byte[] state0, byte[] state1)
	{
		byte[] state_f = new byte[Math.Max(state0.Length, state1.Length)];
		for(int i = 0; i < state_f.Length; i++)
		{
			byte b_0 = 0x0;
			byte b_1 = 0x0;

			if (i < state0.Length)
				b_0 = state0[state0.Length - 1 - i];

			if (i < state1.Length)
				b_1 = state1[state1.Length - 1 - i];

			state_f[i] = (byte)(b_0 ^ b_1);
		}
		return state_f;
	}
}

