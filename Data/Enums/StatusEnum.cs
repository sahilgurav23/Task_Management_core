using System.ComponentModel;

namespace Data.Enums
{
    /// <summary>
    /// Represents the current progression state of a task.
    /// </summary>
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
