﻿using System.ComponentModel;
namespace Bankify.Domain.Models.Shared
{
    public enum UserStatusAction
    {
        [Description("Mark As Active")]
        MarkAsActive = 1,
        [Description("Mark As In Active")]
        MarkAsInActive = 2
    }

    public enum RecordStatus
    {
        InActive = -1,
        Active = 1,
        Deleted = 0,
        Closed = 2
    }

    public enum ActionType
    {
        Create=1,
        Update=2, 
        Delete=3,
        ChangeStatus=4,
        Assign=5,
        Other=6
    }
  
    
   
   
  
}
