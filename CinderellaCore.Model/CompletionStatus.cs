using System;
using System.ComponentModel.DataAnnotations;

namespace CinderellaCore.Model
{
    public enum CompletionStatus
    {
        [Display(Name = "Not Started")] NotStarted,
        [Display(Name = "In Progress")] InProgress,
        Completed
    }
}
