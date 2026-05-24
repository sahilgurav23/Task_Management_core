using System.ComponentModel;

namespace Data.Enums
{
   public enum Status
    {
        [Description("To Do")]
        ToDo = 1,

        [Description("In Progress")]
        InProgress = 2,
        Review = 3,
        Done = 4,
    }
}
