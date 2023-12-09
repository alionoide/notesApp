using NotesAppAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesAppAPI
{
    public interface INotesAPI
    {
        User Login(string username, string password);
        void UpdateUser(User user);
        bool ChangePassword(int userID, string password, string newPassword);
        IEnumerable<Tuple<Subject, Permission>> GetSubjects(int userID);
        int AddSubject(Subject subject);
        void UpdateSubject(Subject subject);
        void DeleteSubject(Subject subject);
        void ShareSubject(int subjectID, int userID, int permissionID, int sharerID);
        IEnumerable<Goal> GetGoals(int subjectID);
        int AddGoal(Goal goal);
        void UpdateGoal(Goal goal); 
        void DeleteGoal(Goal goal);
        void ShareGoal(int goalID, int userID, int permissionID, int sharerID);
        IEnumerable<TaskItem> GetTasks(int goalID);
        int AddTask(TaskItem task);
        void UpdateTask(TaskItem task);
        void DeleteTask(TaskItem task);
        IEnumerable<Tuple<Subject, Permission>> GetSharedSubjects(int userID);
        IEnumerable<Tuple<Goal, Permission>> GetSharedGoals(int userID);
    }
}
