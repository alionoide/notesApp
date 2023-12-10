using MySqlConnector;
using NotesAppAPI.Models;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NotesAppAPI
{
    public class NotesAPI : INotesAPI
    {
        MySqlConnectionStringBuilder builder;

        public NotesAPI()
        {
            builder = new MySqlConnectionStringBuilder();

            //builder.Server = "houserhouse.com";
            builder.Server = "209.159.220.14";
            builder.UserID = "app";
            builder.Password = Environment.GetEnvironmentVariable("NOTES_APP_DB_PASSWORD");
            builder.Database = "notesapp";
            builder.TreatTinyAsBoolean = true;
        }

        public int AddGoal(Goal goal)
        {
            int id = -1;
            using (MySqlConnection cnn = new MySqlConnection(builder.ConnectionString))
            {
                cnn.Open();

                MySqlCommand command = new MySqlCommand("insert into Goal (subjectID, name, text, progress, dueDate, assignedUserID) value (?, ?, ?, ?, ?, ?);", cnn);
                command.Parameters.Add(new MySqlParameter("subjectID", goal.Subject.ID));
                command.Parameters.Add(new MySqlParameter("name", goal.Name));
                command.Parameters.Add(new MySqlParameter("text", goal.Text));
                command.Parameters.Add(new MySqlParameter("progress", goal.Progress));
                command.Parameters.Add(new MySqlParameter("dueDate", goal.DueDate));
                command.Parameters.Add(new MySqlParameter("assignedUserID", goal.AssignedUser?.ID));

                var reader = command.ExecuteReader();

                reader.Close();
                command.Dispose();


                MySqlCommand getIDCommand = new MySqlCommand("select goalID from Goal order by goalID desc;", cnn);

                reader = getIDCommand.ExecuteReader();
                if (reader.Read())
                {
                    id = reader.GetInt32(0);
                }

                reader.Close();
                getIDCommand.Dispose();
                cnn.Close();
            }
            return id;
        }

        public int AddSubject(Subject subject)
        {
            int id = -1;
            using (MySqlConnection cnn = new MySqlConnection(builder.ConnectionString))
            {
                cnn.Open();

                MySqlCommand command = new MySqlCommand("insert into Subject (ownerID, name, description, color) value (?, ?, ?, ?);", cnn);
                command.Parameters.Add(new MySqlParameter("ownerID", subject.Owner.ID));
                command.Parameters.Add(new MySqlParameter("name", subject.Name));
                command.Parameters.Add(new MySqlParameter("description", subject.Description));
                command.Parameters.Add(new MySqlParameter("color", subject.Color));

                var reader = command.ExecuteReader();

                reader.Close();
                command.Dispose();


                MySqlCommand getIDCommand = new MySqlCommand("select subjectID from Subject order by subjectID desc;", cnn);

                reader = getIDCommand.ExecuteReader();
                if (reader.Read())
                {
                    id = reader.GetInt32(0);
                }

                reader.Close();
                getIDCommand.Dispose();
                cnn.Close();
            }
            return id;
        }

        public int AddTask(TaskItem task)
        {
            int id = -1;
            using (MySqlConnection cnn = new MySqlConnection(builder.ConnectionString))
            {
                cnn.Open();

                MySqlCommand command = new MySqlCommand("insert into Task (goalID, assignedUserID, text, progress, dueDate) value (?, ?, ?, ?, ?);", cnn);
                command.Parameters.Add(new MySqlParameter("goalID", task.Goal.ID));
                command.Parameters.Add(new MySqlParameter("assignedUserID", task.AssignedUser?.ID));
                command.Parameters.Add(new MySqlParameter("text", task.Text));
                command.Parameters.Add(new MySqlParameter("progress", task.Progress));
                command.Parameters.Add(new MySqlParameter("dueDate", task.DueDate));

                var reader = command.ExecuteReader();

                reader.Close();
                command.Dispose();


                MySqlCommand getIDCommand = new MySqlCommand("select taskID from Task order by taskID desc;", cnn);

                reader = getIDCommand.ExecuteReader();
                if (reader.Read())
                {
                    id = reader.GetInt32(0);
                }

                reader.Close();
                getIDCommand.Dispose();
                cnn.Close();
            }
            return id;
        }

        public void AddUser(User user, string password)
        {
            using (MySqlConnection cnn = new MySqlConnection(builder.ConnectionString))
            {
                cnn.Open();

                string hash = Hasher.Hash(password);

                MySqlCommand command = new MySqlCommand("insert into User (uName, email, displayName, password, salt) " +
                                                        "value (?, ?, ?, ?, ?);", cnn);
                command.Parameters.Add(new MySqlParameter("uName", user.Username));
                command.Parameters.Add(new MySqlParameter("email", user.Email));
                command.Parameters.Add(new MySqlParameter("displayName", user.DisplayName));
                command.Parameters.Add(new MySqlParameter("password", hash));
                command.Parameters.Add(new MySqlParameter("salt", hash));

                var reader = command.ExecuteReader();

                reader.Close();
                command.Dispose();
                cnn.Close();
            }
        }

        public bool ChangePassword(int userID, string password, string newPassword)
        {
            using (MySqlConnection cnn = new MySqlConnection(builder.ConnectionString))
            {
                string storedHash;

                cnn.Open();

                MySqlCommand command = new MySqlCommand("select password from User where userID = ?;", cnn);
                command.Parameters.Add(new MySqlParameter("uName", userID));

                var reader = command.ExecuteReader();

                reader.Read();

                storedHash = reader.GetString(0);
                if (Hasher.Verify(password, storedHash))
                {
                    reader.Close();
                    command.Dispose();

                    var newHash = Hasher.Hash(newPassword);

                    MySqlCommand addCommand = new MySqlCommand("update User set password = ?, salt = ? where userID = ?;", cnn);
                    addCommand.Parameters.Add(new MySqlParameter("password", newHash));
                    addCommand.Parameters.Add(new MySqlParameter("salt", newHash));
                    addCommand.Parameters.Add(new MySqlParameter("userID", userID));

                    reader = addCommand.ExecuteReader();

                    reader.Close();
                    command.Dispose();
                    cnn.Close();

                    return true;
                }

                reader.Close();
                command.Dispose();
                cnn.Close();

                return false;
            }
        }

        public void DeleteGoal(Goal goal)
        {
            using (MySqlConnection cnn = new MySqlConnection(builder.ConnectionString))
            {
                cnn.Open();

                MySqlCommand command = new MySqlCommand("delete from Goal where goalID = ?;", cnn);
                command.Parameters.Add(new MySqlParameter("goalID", goal.ID));

                var reader = command.ExecuteReader();

                reader.Close();
                command.Dispose();
                cnn.Close();
            }
        }

        public void DeleteSubject(Subject subject)
        {
            using (MySqlConnection cnn = new MySqlConnection(builder.ConnectionString))
            {
                cnn.Open();

                MySqlCommand command = new MySqlCommand("delete from Subject where subjectID = ?;", cnn);
                command.Parameters.Add(new MySqlParameter("subjectID", subject.ID));

                var reader = command.ExecuteReader();

                reader.Close();
                command.Dispose();
                cnn.Close();
            }
        }

        public void DeleteTask(TaskItem task)
        {
            using (MySqlConnection cnn = new MySqlConnection(builder.ConnectionString))
            {
                cnn.Open();

                MySqlCommand command = new MySqlCommand("delete from Task where taskID = ?;", cnn);
                command.Parameters.Add(new MySqlParameter("taskID", task.ID));

                var reader = command.ExecuteReader();

                reader.Close();
                command.Dispose();
                cnn.Close();
            }
        }

        public IEnumerable<User> GetAllUsersNotCurrent(int currentUserID)
        {
            using (MySqlConnection cnn = new MySqlConnection(builder.ConnectionString))
            {
                List<User> list = new List<User>();

                cnn.Open();

                MySqlCommand command = new MySqlCommand("select userID, uName, email, displayName " +
                                                        "from User " +
                                                        "where userID <> ?;", cnn);
                command.Parameters.Add(new MySqlParameter("currentUserID", currentUserID));

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new User
                    {
                        ID = reader.GetInt32(0),
                        Username = reader.GetString(1),
                        Email = reader.GetString(2),
                        DisplayName = reader.GetValue(3) == DBNull.Value ? null : (string)reader.GetValue(3),
                    });
                }

                reader.Close();
                command.Dispose();
                cnn.Close();

                return list;
            }
        }

        public IEnumerable<User> GetAvaliableUsers(int goalID)
        {
            using (MySqlConnection cnn = new MySqlConnection(builder.ConnectionString))
            {
                List<User> list = new List<User>();

                cnn.Open();

                MySqlCommand command = new MySqlCommand("select u.userID, u.uName, u.email, u.displayName " +
                                                        "from User u, Goal g, Subject s " +
                                                        "where u.userID = s.ownerID and s.subjectID = g.subjectID and g.goalId = ? " +
                                                        "union " +
                                                        "select u.userID, u.uName, u.email, u.displayName " +
                                                        "from User u, SubjectShare ss, Goal g " +
                                                        "where u.userID = ss.userID and ss.subjectID = g.subjectID and g.goalId = ? " +
                                                        "union " +
                                                        "select u.userID, u.uName, u.email, u.displayName " +
                                                        "from User u, GoalShare gs, Goal g " +
                                                        "where u.userID = gs.userID and gs.goalId = ?;", cnn);
                command.Parameters.Add(new MySqlParameter("goalID", goalID));
                command.Parameters.Add(new MySqlParameter("goalID2", goalID));
                command.Parameters.Add(new MySqlParameter("goalID3", goalID));

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new User
                        {
                            ID = reader.GetInt32(0),
                            Username = reader.GetString(1),
                            Email = reader.GetString(2),
                            DisplayName = reader.GetValue(3) == DBNull.Value ? null : (string)reader.GetValue(3),
                        });
                }

                reader.Close();
                command.Dispose();
                cnn.Close();

                return list;
            }
        }

        public IEnumerable<Goal> GetGoals(int subjectID)
        {
            using (MySqlConnection cnn = new MySqlConnection(builder.ConnectionString))
            {
                List<Goal> list = new List<Goal>();

                cnn.Open();

                MySqlCommand command = new MySqlCommand("select g.goalID, g.subjectID, g.name, g.text, g.progress, g.dueDate, " +
                                                               "u.userID, u.uName, u.email, u.displayName " +
                                                        "from Goal g " +
                                                        "left join User u on g.assignedUserID = u.userID " +
                                                        "where g.subjectID = ?;", cnn);
                command.Parameters.Add(new MySqlParameter("subjectID", subjectID));

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new Goal
                    {
                        ID = reader.GetInt32(0),
                        Subject = new Subject
                        {
                            ID = reader.GetInt32(1),
                        },
                        Name = reader.GetValue(2) == DBNull.Value ? null : reader.GetString(2),
                        Text = reader.GetValue(3) == DBNull.Value ? null : reader.GetString(3),
                        Progress = reader.GetDouble(4),
                        DueDate = reader.GetValue(5) == DBNull.Value ? null : reader.GetDateTime(5),
                        AssignedUser = reader.GetValue(6) == DBNull.Value ? null : new User
                        {
                            ID = reader.GetInt32(6),
                            Username = reader.GetString(7),
                            Email = reader.GetString(8),
                            DisplayName = reader.GetValue(9) == DBNull.Value ? null : (string)reader.GetValue(9),
                        },
                    });
                }

                reader.Close();
                command.Dispose();
                cnn.Close();

                return list;
            }
        }

        public IEnumerable<Permission> GetPermissions()
        {
            using (MySqlConnection cnn = new MySqlConnection(builder.ConnectionString))
            {
                cnn.Open();

                MySqlCommand command = new MySqlCommand("select * from Permission", cnn);

                var reader = command.ExecuteReader();
                List<Permission> permissions = new List<Permission>();

                while (reader.Read())
                {
                    permissions.Add(new Permission
                    {
                        ID = (int)reader.GetValue(0),
                        Name = (string)reader.GetValue(1),
                        IsAdmin = ((bool)reader.GetValue(2)),
                        CanCUD = ((bool)reader.GetValue(3)),
                        CanShare = ((bool)reader.GetValue(4)),
                        CanAssign = ((bool)reader.GetValue(5)),
                        CanProgress = ((bool)reader.GetValue(6))
                    });
                }

                cnn.Close();

                return permissions;
            }
        }

        public IEnumerable<Tuple<Goal, Permission>> GetSharedGoals(int userID)
        {
            using (MySqlConnection cnn = new MySqlConnection(builder.ConnectionString))
            {
                List<Tuple<Goal, Permission>> list = new List<Tuple<Goal, Permission>>();

                cnn.Open();

                MySqlCommand command = new MySqlCommand("select g.goalID, g.subjectID, g.name, g.text, g.progress, g.dueDate, " +
                                                               "u.userID, u.uName, u.email, u.displayName, p.* " +
                                                        "from GoalShare ss " +
                                                        "join Permission p on ss.permissionID = p.permissionID " +
                                                        "join Goal g on ss.goalId = g.goalID " +
                                                        "left join User u on g.assignedUserID = u.userID " +
                                                        "where ss.userID = 1;", cnn);
                command.Parameters.Add(new MySqlParameter("userID", userID));

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new Tuple<Goal, Permission>(
                        new Goal
                        {
                            ID = reader.GetInt32(0),
                            Subject = new Subject
                            {
                                ID = reader.GetInt32(1),
                            },
                            Name = reader.GetString(2),
                            Text = reader.GetString(3),
                            Progress = reader.GetDouble(4),
                            DueDate = reader.GetDateTime(5),
                            AssignedUser = reader.GetValue(6) == DBNull.Value ? null : new User
                            {
                                ID = reader.GetInt32(6),
                                Username = reader.GetString(7),
                                Email = reader.GetString(8),
                                DisplayName = reader.GetValue(9) == DBNull.Value ? null : (string)reader.GetValue(9),
                            },
                        },
                        new Permission
                        {
                            ID = reader.GetInt32(10),
                            Name = reader.GetString(11),
                            IsAdmin = reader.GetBoolean(12),
                            CanCUD = reader.GetBoolean(13),
                            CanAssign = reader.GetBoolean(14),
                            CanProgress = reader.GetBoolean(15),
                            CanShare = reader.GetBoolean(16),
                        }));
                }

                reader.Close();
                command.Dispose();
                cnn.Close();

                return list;
            }
        }

        public IEnumerable<Tuple<Subject, Permission>> GetSharedSubjects(int userID)
        {
            using (MySqlConnection cnn = new MySqlConnection(builder.ConnectionString))
            {
                List<Tuple<Subject, Permission>> list = new List<Tuple<Subject, Permission>>();

                cnn.Open();

                MySqlCommand command = new MySqlCommand("select s.subjectID, s.name, s.description, s.color, u.userID, u.uName, u.email, u.displayName, p.* " +
                                                        "from SubjectShare ss " +
                                                        "join Permission p on ss.permissionID = p.permissionID " +
                                                        "join Subject s on ss.subjectID = s.subjectID " +
                                                        "join User u on s.ownerID = u.userID " +
                                                        "where ss.userID = ?;", cnn);
                command.Parameters.Add(new MySqlParameter("ownerID", userID));

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new Tuple<Subject, Permission>
                        (
                        new Subject
                        {
                            ID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Description = reader.GetString(2),
                            Color = reader.GetValue(3) == DBNull.Value ? null : (string)reader.GetValue(3),
                            Owner = new User
                            {
                                ID = reader.GetInt32(4),
                                Username = reader.GetString(5),
                                Email = reader.GetString(6),
                                DisplayName = reader.GetValue(7) == DBNull.Value ? null : (string)reader.GetValue(7),
                            }
                        },
                        new Permission
                        {
                            ID = reader.GetInt32(8),
                            Name = reader.GetString(9),
                            IsAdmin = reader.GetBoolean(10),
                            CanCUD = reader.GetBoolean(11),
                            CanAssign = reader.GetBoolean(12),
                            CanProgress = reader.GetBoolean(13),
                            CanShare = reader.GetBoolean(14),
                        }
                        ));
                }

                reader.Close();
                command.Dispose();
                cnn.Close();

                return list;
            };
        }

        public IEnumerable<Tuple<Subject, Permission>> GetSubjects(int userID)
        {
            using (MySqlConnection cnn = new MySqlConnection(builder.ConnectionString))
            {
                List<Tuple<Subject, Permission>> list = new List<Tuple<Subject, Permission>>();
                
                cnn.Open();

                MySqlCommand command = new MySqlCommand("select s.subjectID, s.name, s.description, s.color, u.userID, u.uName, u.email, u.displayName " +
                                                        "from Subject s " +
                                                        "join User u on s.ownerID = u.userID " +
                                                        "where s.ownerID = ?", cnn);
                command.Parameters.Add(new MySqlParameter("ownerID", userID));

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new Tuple<Subject, Permission>
                        (
                        new Subject
                        {
                            ID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Description = reader.GetString(2),
                            Color = reader.GetValue(3) == DBNull.Value ? null : (string)reader.GetValue(3),
                            Owner = new User
                            {
                                ID = reader.GetInt32(4),
                                Username = reader.GetString(5),
                                Email = reader.GetString(6),
                                DisplayName = reader.GetValue(7) == DBNull.Value ? null : (string)reader.GetValue(7),
                            }
                        },
                        new Permission
                        {
                            ID = 0,
                            Name = "Owner",
                            IsAdmin = true,
                            CanCUD = true,
                            CanAssign = true,
                            CanProgress = true,
                            CanShare = true,
                        }
                        ));
                }

                reader.Close();
                command.Dispose();
                cnn.Close();

                return list;
            }
        }

        public IEnumerable<TaskItem> GetTasks(int goalID)
        {
            using (MySqlConnection cnn = new MySqlConnection(builder.ConnectionString))
            {
                List<TaskItem> list = new List<TaskItem>();

                cnn.Open();

                MySqlCommand command = new MySqlCommand("select t.taskID, t.text, t.progress, t.dueDate, u.userID, u.uName, u.email, u.displayName, t.goalID " +
                                                        "from Task t " +
                                                        "left join User u on t.assignedUserID = u.userID " +
                                                        "where t.goalID = ?;", cnn);
                command.Parameters.Add(new MySqlParameter("goalID", goalID));

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new TaskItem
                        {
                            ID = reader.GetInt32(0),
                            Text = reader.GetString(1),
                            Progress = reader.GetDouble(2),
                            DueDate = reader.GetValue(3) == DBNull.Value ? null : reader.GetDateTime(3),
                            AssignedUser = reader.GetValue(4) == DBNull.Value ? null : new User
                            {
                                ID = reader.GetInt32(4),
                                Username = reader.GetString(5),
                                Email = reader.GetString(6),
                                DisplayName = reader.GetValue(7) == DBNull.Value ? null : (string)reader.GetValue(7),
                            },
                            Goal = new Goal
                            {
                                ID = reader.GetInt32(8),
                            },
                        });
                }

                reader.Close();
                command.Dispose();
                cnn.Close();

                return list;
            }
        }

        public User Login(string username, string password)
        {
            using (MySqlConnection cnn = new MySqlConnection(builder.ConnectionString))
            {
                User user = null;
                string storedHash;

                cnn.Open();

                MySqlCommand command = new MySqlCommand("select userID, uName, email, displayName, password from User where uName = ?;", cnn);
                command.Parameters.Add(new MySqlParameter("uName", username));

                var reader = command.ExecuteReader();

                while (user == null && reader.Read())
                {
                    storedHash = reader.GetString(4);
                    if (Hasher.Verify(password, storedHash))
                    {
                        user = new User
                        {
                            ID = reader.GetInt32(0),
                            Username = reader.GetString(1),
                            Email = reader.GetString(2),
                            DisplayName = reader.GetValue(3) == DBNull.Value ? null : (string)reader.GetValue(3),
                        };
                    }
                }

                reader.Close();
                command.Dispose();
                cnn.Close();

                return user;
            }
        }

        public void ShareGoal(int goalID, int userID, int permissionID, int sharerID)
        {
            using (MySqlConnection cnn = new MySqlConnection(builder.ConnectionString))
            {
                cnn.Open();

                MySqlCommand command = new MySqlCommand("insert into GoalShare (userID, goalID, permissionID, sharerID) value (?, ?, ?, ?);", cnn);
                command.Parameters.Add(new MySqlParameter("userID", userID));
                command.Parameters.Add(new MySqlParameter("goalID", goalID));
                command.Parameters.Add(new MySqlParameter("permissionID", permissionID));
                command.Parameters.Add(new MySqlParameter("sharerID", sharerID));

                var reader = command.ExecuteReader();

                reader.Close();
                command.Dispose();
                cnn.Close();
            }
        }

        public void ShareSubject(int subjectID, int userID, int permissionID, int sharerID)
        {
            using (MySqlConnection cnn = new MySqlConnection(builder.ConnectionString))
            {
                cnn.Open();

                MySqlCommand command = new MySqlCommand("insert into SubjectShare (userID, subjectID, permissionID, sharerID) value (?, ?, ?, ?);", cnn);
                command.Parameters.Add(new MySqlParameter("userID", userID));
                command.Parameters.Add(new MySqlParameter("subjectID", subjectID));
                command.Parameters.Add(new MySqlParameter("permissionID", permissionID));
                command.Parameters.Add(new MySqlParameter("sharerID", sharerID));

                var reader = command.ExecuteReader();

                reader.Close();
                command.Dispose();
                cnn.Close();
            }
        }

        public void UpdateGoal(Goal goal)
        {
            using (MySqlConnection cnn = new MySqlConnection(builder.ConnectionString))
            {
                cnn.Open();

                MySqlCommand command = new MySqlCommand("update Goal set name = ?, text = ?, progress = ?, dueDate = ?, assignedUserID = ? where goalID = ?;", cnn);
                command.Parameters.Add(new MySqlParameter("name", goal.Name));
                command.Parameters.Add(new MySqlParameter("text", goal.Text));
                command.Parameters.Add(new MySqlParameter("progress", goal.Progress));
                command.Parameters.Add(new MySqlParameter("dueDate", goal.DueDate));
                command.Parameters.Add(new MySqlParameter("assignedUserID", goal.AssignedUser?.ID));
                command.Parameters.Add(new MySqlParameter("goalID", goal.ID));

                var reader = command.ExecuteReader();

                reader.Close();
                command.Dispose();
                cnn.Close();
            }
        }

        public void UpdateSubject(Subject subject)
        {
            using (MySqlConnection cnn = new MySqlConnection(builder.ConnectionString))
            {
                cnn.Open();

                MySqlCommand command = new MySqlCommand("update Subject set name = ?, description = ?, color = ? where subjectID = ?;", cnn);
                command.Parameters.Add(new MySqlParameter("name", subject.Name));
                command.Parameters.Add(new MySqlParameter("description", subject.Description));
                command.Parameters.Add(new MySqlParameter("color", subject.Color));
                command.Parameters.Add(new MySqlParameter("subjectID", subject.ID));

                var reader = command.ExecuteReader();

                reader.Close();
                command.Dispose();
                cnn.Close();
            }
        }

        public void UpdateTask(TaskItem task)
        {
            using (MySqlConnection cnn = new MySqlConnection(builder.ConnectionString))
            {
                cnn.Open();

                MySqlCommand command = new MySqlCommand("update Task set assignedUserID = ?, text = ?, progress = ?, dueDate = ? where taskID = ?;", cnn);
                command.Parameters.Add(new MySqlParameter("assignedUserID", task.AssignedUser?.ID));
                command.Parameters.Add(new MySqlParameter("text", task.Text));
                command.Parameters.Add(new MySqlParameter("progress", task.Progress)); 
                command.Parameters.Add(new MySqlParameter("dueDate", task.DueDate));
                command.Parameters.Add(new MySqlParameter("taskID", task.ID));

                var reader = command.ExecuteReader();

                reader.Close();
                command.Dispose();
                cnn.Close();
            }
        }

        public void UpdateUser(User user)
        {
            using (MySqlConnection cnn = new MySqlConnection(builder.ConnectionString))
            {
                cnn.Open();

                MySqlCommand command = new MySqlCommand("update User set email = ?, displayName = ? where userID = ?;", cnn);
                command.Parameters.Add(new MySqlParameter("email", user.Email));
                command.Parameters.Add(new MySqlParameter("displayName", user.DisplayName));
                command.Parameters.Add(new MySqlParameter("userID", user.ID));

                var reader = command.ExecuteReader();

                reader.Close();
                command.Dispose();
                cnn.Close();
            }
        }
    }
}