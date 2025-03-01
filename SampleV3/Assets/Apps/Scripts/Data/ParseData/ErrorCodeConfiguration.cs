using System.Collections.Generic;
using System.Linq;

public class ErrorCodeConfiguration : IErrorCodeConfiguration
{
    public static ErrorCodeConfiguration Initialize(MasterErrorCode[] masterErrorCodes)
    {
        return new ErrorCodeConfiguration
        {
            SuccessCode = masterErrorCodes.First(x => x.Success).MasterErrorCodeId,
            HandleCode = masterErrorCodes.Where(x => x.Handle == true)
                                                 .Select(x => x.MasterErrorCodeId)
                                                 .ToArray(),
            ErrorCodeMessage = masterErrorCodes.ToDictionary(x => x.MasterErrorCodeId, x => x.DescriptionKey)
        };
    }
    public int SuccessCode { get; private set; }
    public int[] HandleCode { get; private set; }
    public IReadOnlyDictionary<int, string> ErrorCodeMessage { get; private set; }
}
