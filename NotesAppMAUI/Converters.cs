using Microsoft.VisualBasic;
using NotesAppAPI.Models;
using NotesAppMAUI.ViewModel.VMO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace NotesAppMAUI
{
    public static class Converters
    {
        #region Converters
        public static SubjectVMO Convert(Tuple<Subject, Permission> model)
        {
            if (model == null) { return null; }
            SubjectVMO vmo = Convert(model.Item1);
            vmo.Permission = Convert(model.Item2);
            return vmo;
        }

        public static Subject Convert(SubjectVMO vmo)
        {
            if (vmo == null) { return null; }
            return new Subject
            {
                ID = vmo.ID,
                Name = vmo.Name,
                Description = vmo.Description,
                Color = vmo.Color,
                Owner = Convert(vmo.Owner)
            };
        }

        public static PermissionVMO Convert(Permission model)
        {
            if (model == null) { return null; }
            return new PermissionVMO
            {
                ID = model.ID,
                Name = model.Name,
                CanCUD = model.CanCUD,
                CanAssign = model.CanAssign,
                CanProgress = model.CanProgress,
                CanShare = model.CanShare,
                IsAdmin = model.IsAdmin,
            };
        }

        public static UserVMO Convert(User model)
        {
            if (model == null) { return null; }
            return new UserVMO
            {
                ID = model.ID,
                Username = model.Username,
                DisplayName = model.DisplayName,
                Email = model.Email,
            };
        }
        public static User Convert(UserVMO vmo)
        {
            if (vmo == null) { return null; }
            return new User
            {
                ID = vmo.ID,
                Username = vmo.Username,
                DisplayName = vmo.DisplayName,
                Email = vmo.Email,
            };
        }

        internal static GoalVMO Convert(Goal model)
        {
            if (model == null) { return null; }
            return new GoalVMO
            {
                ID = model.ID,
                Name = model.Name,
                Text = model.Text,
                DueDate = model.DueDate,
                AssignedUser = Convert(model.AssignedUser),
                Permission = null,
                Progress = model.Progress,
                Subject = Convert(model.Subject)
            };
        }

        internal static Goal Convert(GoalVMO vmo)
        {
            if (vmo == null) { return null; }
            return new Goal
            {
                ID = vmo.ID,
                Name = vmo.Name,
                Text = vmo.Text,
                DueDate = vmo.DueDate,
                AssignedUser = Convert(vmo.AssignedUser),
                Progress = vmo.Progress,
                Subject = Convert(vmo.Subject)
            };
        }

        internal static TaskItem Convert(TaskItemVMO vmo)
        {
            if (vmo == null) { return null; }
            return new TaskItem
            {
                ID = vmo.ID,
                Text = vmo.Text,
                DueDate = vmo.DueDate,
                AssignedUser = Convert(vmo.AssignedUser),
                Goal = Convert(vmo.Goal),
                Progress = vmo.Progress,
            };
        }

        internal static TaskItemVMO Convert(TaskItem model)
        {
            if (model == null) { return null; }
            return new TaskItemVMO
            {
                ID = model.ID,
                Text = model.Text,
                DueDate = model.DueDate,
                AssignedUser = Convert(model.AssignedUser),
                Goal = Convert(model.Goal),
                Progress = model.Progress,
            };
        }

        internal static GoalVMO Convert(Tuple<Goal, Permission> model)
        {
            if (model == null) { return null; }
            GoalVMO vmo = Convert(model.Item1);
            vmo.Permission = Convert(model.Item2);
            return vmo;
        }

        private static SubjectVMO Convert(Subject model)
        {
            if (model == null) { return null; }
            return new SubjectVMO
            {
                ID = model.ID,
                Name = model.Name,
                Description = model.Description,
                Color = model.Color,
                Owner = Convert(model.Owner),
                Permission = null
            };
        }
        #endregion
    }
}
