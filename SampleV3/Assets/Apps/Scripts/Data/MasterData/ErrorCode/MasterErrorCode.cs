using System.Collections.Generic;
using Core.Model;

public class MasterErrorCode : MasterModel
{
	public string DescriptionKey { get; set; }

	public bool Handle { get; set; }

	public bool Success { get; set; }

	#region getter
	private bool __IsConverted;
	private int masterErrorCodeId;
	[Ignore]
	public int MasterErrorCodeId { 
		get {
			if (!__IsConverted) { __IsConverted = true; masterErrorCodeId = string.IsNullOrEmpty(Id) ? -1 : Id.TryToConvertInt32(defaultValue: -1); }
			return masterErrorCodeId;
		} 
	}


    #endregion
}


public class MasterErrorCodeEmbed : OriginalModel
{
	public List<MasterErrorCode> data { get; set; }
}