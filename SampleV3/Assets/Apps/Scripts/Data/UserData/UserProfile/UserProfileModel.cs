using System;

using Core.Model;
[System.Serializable]
public class UserProfileModel : BaseModel
{
    public double Money { get; set; }
    public string UserName { get; set; }
    public int Energy { get; set; }
    public string Token { get; set; }
    
    public TypeOfAccount AccountType {get;set;}

    public TypeOfAvatar AvatarType { get; set; }
    public string AvatarID { get; set; }

    public UserProfileModel Clone()
    {
        return (UserProfileModel)this.MemberwiseClone();
    }

    
}
