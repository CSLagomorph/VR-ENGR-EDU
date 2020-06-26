using UnityEngine;
using System;
using System.Data;
using System.Text;

using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

using MySql.Data;
using MySql.Data.MySqlClient;
using UnityEngine.UI;
using Random = System.Random;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.IO;
using System.Threading;


public class DatabaseHandler : MonoBehaviour
{
    public string host, database, user, password;
    public bool pooling = true;

    private string connectionString;
    private MySqlConnection con = null;
    private MySqlCommand cmd = null;
    private MySqlDataReader rdr = null;

    private MD5 md5Hash;

    string tempPasswordSymbols = "abcdefghigklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ123456780!@#$%^&*()-_=+"; //characters for a temporary password

    //create an account
    public string theName;
    public string email;
    public string username;
    public string userPassword;
    public string occupation;
    public GameObject inputFieldName;
    public GameObject inputFieldEmail;
    public GameObject inputFieldUsername;
    public GameObject inputFieldPassword;
    public GameObject inputFieldOccupation;

    //login
    public string usernameForLogin;
    public string passwordForLogin;
    public GameObject inputFieldUserLogin;
    public GameObject inputFieldPasswordLogin;

    //delete a user
    public string usernameToDelete;
    public string emailToDelete;
    public GameObject inputFieldUserDelete;
    public GameObject inputFieldEmailDelete;

    //add Problem
    public string courseName;
    public string problemName;
    public string problemText;
    public string topic;
    public string chapter;
    public string lvlOfDifficulty;
    public GameObject inputFieldCourseName;
    public GameObject inputFieldProblemName;
    public GameObject inputFieldProblemText;
    public GameObject inputFieldTopic;
    public GameObject inputFieldChapter;
    public GameObject inputFieldLevelOfDifficulty;

    //add Solution
    public string problemId;
    public string solutionDescription;
    public string fileName;
    public GameObject inputFieldProblemID;
    public GameObject inputFieldSolutionDescription;
    public GameObject inputFieldFileName;

    //Change permissions
    public string changePermissionsUsername;
    public string changePermissionsOccupation;
    public GameObject inputFieldCPUsername;
    public GameObject inputFieldCPOccupation;

    //Add answer
    public string addAnswerProblemID;
    public string addAnswerDescription;
    public GameObject inputFieldAnsPrID;
    public GameObject inputFieldAnsDesc;

    //change password
    public string cPasswordUsername;
    public string cPasswordOldP;
    public string cPasswordNewP;
    public GameObject inputFieldCPasswordUsername;
    public GameObject inputFieldCPasswordOldP;
    public GameObject inputFieldCPasswordNewP;

    //change Public Name
    public string cNameUsername;
    public string cNameNewName;
    public GameObject inputFieldCNUsername;
    public GameObject inputFieldNewName;

    //forgot password
    public string forgotPasswordUsername;
    public GameObject inputFieldFPUsername;

    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        connectionString = "Server=" + host + ";Database=" + database + ";User=" + user + ";Password=" + password + ";Pooling=";
        Debug.Log(connectionString);
        if (pooling)
        {
            connectionString += "True";
        }
        else
        {
            connectionString += "False";
        }
        try
        {
            con = new MySqlConnection(connectionString);
            con.Open();
            Debug.Log("Mysql state: " + con.State);

            string sql = "SELECT * FROM UserRole";
            cmd = new MySqlCommand(sql, con);
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Debug.Log(rdr[0] + " -- " + rdr[1]);
            }
            rdr.Close();

        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
    void onApplicationQuit()
    {
        if (con != null)
        {
            if (con.State.ToString() != "Closed")
            {
                con.Close();
                Debug.Log("Mysql connection closed");
            }
            con.Dispose();
        }
    }

    //a method that checks the credentials that a user types in (Log In) 
    public bool authenticate(string username, string password)
    {
        try
        {
            if (con == null)
            {
                Awake();
            }

            string sql = "SELECT Password FROM User Where Username = @user";
            MySqlParameter param = new MySqlParameter();
            param.ParameterName = "@user";
            param.Value = username;

            cmd = new MySqlCommand(sql, con);
            cmd.Parameters.Add(param);

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                if (rdr[0].Equals(password.GetHashCode().ToString()))
                {
                    Debug.Log("You're successfully signed in");
                    return true;
                }
            }
            rdr.Close();

        }
        catch (Exception e)
        {
            Debug.Log(e);
            return false;
        }
        return false;
    }

    //a method that creates an account for a user. It sets occupation and permission levels to "student" by default
    public bool createAnAccount(string name, string email, string username, string password, string occupation)
    {
        try
        {
            if (con == null)
            {
                Awake();
            }

            string select = "SELECT Id FROM UserRole WHERE Role = @role";
            MySqlParameter param = new MySqlParameter();
            param.ParameterName = "@role";
            param.Value = occupation;

            int id = 0;
            cmd = new MySqlCommand(select, con);
            cmd.Parameters.Add(param);

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Debug.Log("rdr: " + rdr[0]);
                id = (int)rdr[0];
            }

            rdr.Close();

            string sql = "INSERT INTO User (Email, Username, Password, PublicName, UserRoleID) VALUES (@email, @usr, @pswrd, @publicN, @role)";
            MySqlCommand command = new MySqlCommand(sql, con);

            MySqlParameter param1 = new MySqlParameter();
            param1.ParameterName = "@email";
            param1.Value = email;

            MySqlParameter param2 = new MySqlParameter();
            param2.ParameterName = "@usr";
            param2.Value = username;

            MySqlParameter param3 = new MySqlParameter();
            param3.ParameterName = "@pswrd";
            param3.Value = password.GetHashCode();  //encryption

            MySqlParameter param4 = new MySqlParameter();
            param4.ParameterName = "@publicN";
            param4.Value = name;

            MySqlParameter param5 = new MySqlParameter();
            param5.ParameterName = "@role";
            param5.Value = id;

            command.Parameters.Add(param1);
            command.Parameters.Add(param2);
            command.Parameters.Add(param3);
            command.Parameters.Add(param4);
            command.Parameters.Add(param5);


            int rows = command.ExecuteNonQuery();
            Debug.Log("rows inserted: " + rows);

            select = "SELECT * FROM User";

            cmd = new MySqlCommand(select, con);
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Debug.Log("rdr: " + rdr[2]);
            }

            rdr.Close();

            return true;
        }

        catch (Exception e)
        {
            Debug.Log(e);
            return false;
        }
    }

    //a method that is used to delete a user. It is intended for admin use only
    public bool deleteUser(string username, string email)
    {
        try
        {
            if (con == null)
            {
                Awake();
            }

            string sql = "DELETE FROM User WHERE Username = @usr";
            MySqlCommand command = new MySqlCommand(sql, con);

            MySqlParameter param1 = new MySqlParameter();
            param1.ParameterName = "@usr";
            param1.Value = username;

            command.Parameters.Add(param1);

            int rows = command.ExecuteNonQuery();
            Debug.Log("rows deleted: " + rows);

            return true;
        }
        catch (Exception e)
        {
            Debug.Log(e);
            return false;
        }
    }

    //a method that is used to load the problem from the database when a user clicks
    //to open a problem.
    public Problem retrieveProblem(string id)
    {
        try
        {
            if (con == null)
            {
                Awake();
            }

            string sql = "SELECT Name FROM Problem Where ID = @id";
            MySqlParameter param = new MySqlParameter();
            param.ParameterName = "@id";
            param.Value = id;

            Problem pr = null;

            cmd = new MySqlCommand(sql, con);
            cmd.Parameters.Add(param);

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Debug.Log("rdr: " + rdr[0]);
                pr.problemName = (string)rdr[0];
            }

            rdr.Close();

            return pr;
        }
        catch (Exception e)
        {
            Debug.Log(e);
            return null;
        }
    }

    //a method that is used to load a solution for a problem user has selected
    public Solution retrieveSolution(string id)
    {
        try
        {
            if (con == null)
            {
                Awake();
            }

            string sql = "SELECT Image FROM Solution Where ID = @id";
            MySqlParameter param = new MySqlParameter();
            param.ParameterName = "@id";
            param.Value = id;

            Solution solution = null;

            cmd = new MySqlCommand(sql, con);
            cmd.Parameters.Add(param);

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Debug.Log("rdr: " + rdr[0]);
                solution.id = (string)rdr[0];
            }

            rdr.Close();

            return solution;
        }
        catch (Exception e)
        {
            Debug.Log(e);
            return null;
        }
    }

    //a method to add a problem. It is intended for faculty use only.
    public bool addProblem(string courseName, string name, string problemText, string topic, int chapter, string levelOfDifficulty)
    {
        try
        {
            if (con == null)
            {
                Awake();
            }

            string sqlCourse = "INSERT INTO Course (Name) VALUES (@courseName)";
            MySqlCommand commandCourse = new MySqlCommand(sqlCourse, con);

            MySqlParameter paramName = new MySqlParameter();
            paramName.ParameterName = "@courseName";
            paramName.Value = courseName;

            commandCourse.Parameters.Add(paramName);

            int rows = commandCourse.ExecuteNonQuery();
            Debug.Log("Course rows inserted: " + rows);

            string select = "SELECT Id FROM Course WHERE Name = @name";
            MySqlParameter param = new MySqlParameter();
            param.ParameterName = "@name";
            param.Value = courseName;

            int idCourse = 0;
            cmd = new MySqlCommand(select, con);
            cmd.Parameters.Add(param);

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Debug.Log("rdr: " + rdr[0]);
                idCourse = (int)rdr[0];
            }

            rdr.Close();

            string selectD = "SELECT Id FROM LevelOfDifficulty WHERE Description = @desc";
            MySqlParameter paramD = new MySqlParameter();
            paramD.ParameterName = "@desc";
            paramD.Value = levelOfDifficulty;

            int idDifficulty = 0;
            cmd = new MySqlCommand(selectD, con);
            cmd.Parameters.Add(paramD);

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Debug.Log("rdr: " + rdr[0]);
                idDifficulty = (int)rdr[0];
            }

            rdr.Close();

            string sql = "INSERT INTO Chapter (Number, DifficultyID, CourseID) VALUES (@num, @diff, @course)";
            MySqlCommand commandChapter = new MySqlCommand(sql, con);

            MySqlParameter param1 = new MySqlParameter();
            param1.ParameterName = "@num";
            param1.Value = chapter;

            MySqlParameter param2 = new MySqlParameter();
            param2.ParameterName = "@diff";
            param2.Value = idDifficulty;

            MySqlParameter param3 = new MySqlParameter();
            param3.ParameterName = "@course";
            param3.Value = idCourse;

            commandChapter.Parameters.Add(param1);
            commandChapter.Parameters.Add(param2);
            commandChapter.Parameters.Add(param3);

            rows = commandChapter.ExecuteNonQuery();
            Debug.Log("Chapter rows inserted: " + rows);

            string selectC = "SELECT Id FROM Chapter WHERE Number = @number";
            MySqlParameter paramC = new MySqlParameter();
            paramC.ParameterName = "@number";
            paramC.Value = chapter;

            int idChapter = 0;
            cmd = new MySqlCommand(selectC, con);
            cmd.Parameters.Add(paramC);

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Debug.Log("rdr: " + rdr[0]);
                idChapter = (int)rdr[0];
            }

            rdr.Close();

            string sqlTopic = "INSERT INTO Topic (Name, ChapterID) VALUES (@name, @chapter)";
            MySqlCommand commandTopic = new MySqlCommand(sqlTopic, con);

            MySqlParameter paramT1 = new MySqlParameter();
            paramT1.ParameterName = "@name";
            paramT1.Value = topic;

            MySqlParameter paramT2 = new MySqlParameter();
            paramT2.ParameterName = "@chapter";
            paramT2.Value = idChapter;

            commandTopic.Parameters.Add(paramT1);
            commandTopic.Parameters.Add(paramT2);

            rows = commandTopic.ExecuteNonQuery();
            Debug.Log("Topic rows inserted: " + rows);

            string selectT = "SELECT Id FROM Topic WHERE Name = @name";
            MySqlParameter paramT = new MySqlParameter();
            paramT.ParameterName = "@name";
            paramT.Value = topic;

            int idTopic = 0;
            cmd = new MySqlCommand(selectT, con);
            cmd.Parameters.Add(paramT);

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Debug.Log("rdr: " + rdr[0]);
                idTopic = (int)rdr[0];
            }

            rdr.Close();

            string sqlProblem = "INSERT INTO Problem (Name, ProblemText, TopicID) VALUES (@name, @ptext, @topicId)";
            MySqlCommand commandProblem = new MySqlCommand(sqlProblem, con);

            MySqlParameter paramP1 = new MySqlParameter();
            paramP1.ParameterName = "@name";
            paramP1.Value = name;

            MySqlParameter paramP2 = new MySqlParameter();
            paramP2.ParameterName = "@ptext";
            paramP2.Value = problemText;

            MySqlParameter paramP3 = new MySqlParameter();
            paramP3.ParameterName = "@topicId";
            paramP3.Value = idTopic;

            commandProblem.Parameters.Add(paramP1);
            commandProblem.Parameters.Add(paramP2);
            commandProblem.Parameters.Add(paramP3);

            rows = commandProblem.ExecuteNonQuery();
            Debug.Log("Problem rows inserted: " + rows);

            select = "SELECT * FROM Problem";

            cmd = new MySqlCommand(select, con);
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Debug.Log("rdr: " + rdr[2]);
            }

            rdr.Close();

            return true;
        }

        catch (Exception e)
        {
            Debug.Log(e);
            return false;
        }
    }

    //A method to add a solution to a selected problem. It is intended for faculty use only.
    public bool addSolution(int problemID, string description, string fileName)
    {
        try
        {
            if (con == null)
            {
                Awake();
            }

            string sqlSolution = "INSERT INTO Solution (Description, Image) VALUES (@description, @file)";
            MySqlCommand commandDesc = new MySqlCommand(sqlSolution, con);

            MySqlParameter paramDesc = new MySqlParameter();
            paramDesc.ParameterName = "@description";
            paramDesc.Value = description;

            MySqlParameter paramImage = new MySqlParameter();
            paramImage.ParameterName = "@file";
            paramImage.Value = null; //read file

            commandDesc.Parameters.Add(paramDesc);
            commandDesc.Parameters.Add(paramImage);

            int rows = commandDesc.ExecuteNonQuery();
            Debug.Log("Course rows inserted: " + rows);

            string select = "SELECT Id FROM Solution WHERE Description = @desc";
            MySqlParameter param = new MySqlParameter();
            param.ParameterName = "@desc";
            param.Value = description;

            int idDesc = 0;
            cmd = new MySqlCommand(select, con);
            cmd.Parameters.Add(param);

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Debug.Log("rdr: " + rdr[0]);
                idDesc = (int)rdr[0];
            }

            rdr.Close();

            string sqlUpdate = "UPDATE Problem SET SolutionID = @id WHERE Problem.Id = @prID";
            MySqlCommand commandUpdate = new MySqlCommand(sqlUpdate, con);

            MySqlParameter solId = new MySqlParameter();
            solId.ParameterName = "@id";
            solId.Value = idDesc;

            MySqlParameter probId = new MySqlParameter();
            probId.ParameterName = "@prID";
            probId.Value = problemID;

            commandUpdate.Parameters.Add(solId);
            commandUpdate.Parameters.Add(probId);

            rows = commandUpdate.ExecuteNonQuery();
            Debug.Log("Problem rows inserted: " + rows);

            select = "SELECT * FROM Problem";

            cmd = new MySqlCommand(select, con);
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Debug.Log("rdr: " + rdr[2]);
            }

            rdr.Close();

            return true;
        }

        catch (Exception e)
        {
            Debug.Log(e);
            return false;
        }
    }

    // A method to change permissions of a user by changing the occupation
    // This method is intended for Admin use only. 
    public bool changePermissions(string username, string occupation)
    {
        try
        {
            if (con == null)
            {
                Awake();
            }

            string select = "SELECT Id FROM UserRole WHERE Role = @role";
            MySqlParameter param = new MySqlParameter();
            param.ParameterName = "@role";
            param.Value = occupation;

            int id = 0;
            cmd = new MySqlCommand(select, con);
            cmd.Parameters.Add(param);

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Debug.Log("rdr: " + rdr[0]);
                id = (int)rdr[0];
            }

            rdr.Close();

            string sql = "SELECT Id FROM User Where Username = @user";
            MySqlParameter param1 = new MySqlParameter();
            param1.ParameterName = "@user";
            param1.Value = username;

            int idUsername = 0;
            cmd = new MySqlCommand(sql, con);
            cmd.Parameters.Add(param1);

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Debug.Log("rdr: " + rdr[0]);
                idUsername = (int)rdr[0];
            }

            rdr.Close();

            string sqlUpdate = "UPDATE User SET UserRoleID = @id WHERE ID = @userId";
            MySqlCommand commandUpdate = new MySqlCommand(sqlUpdate, con);

            MySqlParameter userRoleID = new MySqlParameter();
            userRoleID.ParameterName = "@id";
            userRoleID.Value = id;

            MySqlParameter userId = new MySqlParameter();
            userId.ParameterName = "@userId";
            userId.Value = idUsername;

            commandUpdate.Parameters.Add(userRoleID);
            commandUpdate.Parameters.Add(userId);

            int rows = commandUpdate.ExecuteNonQuery();
            Debug.Log("Problem rows inserted: " + rows);

            return true;

        }
        catch (Exception e)
        {
            Debug.Log(e);
            return false;
        }
    }

    //A method to add an answer to a selected problem. It is intended for faculty use only.
    public bool addAnswer(int problemID, string answer)
    {
        try
        {
            if (con == null)
            {
                Awake();
            }

            string sqlAnswer = "INSERT INTO Answer (Description) VALUES (@description)";
            MySqlCommand commandDesc = new MySqlCommand(sqlAnswer, con);

            MySqlParameter paramDesc = new MySqlParameter();
            paramDesc.ParameterName = "@description";
            paramDesc.Value = answer;

            commandDesc.Parameters.Add(paramDesc);

            int rows = commandDesc.ExecuteNonQuery();
            Debug.Log("Course rows inserted: " + rows);

            string select = "SELECT Id FROM Answer WHERE Description = @desc";
            MySqlParameter param = new MySqlParameter();
            param.ParameterName = "@desc";
            param.Value = answer;

            int idAns = 0;
            cmd = new MySqlCommand(select, con);
            cmd.Parameters.Add(param);

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Debug.Log("rdr: " + rdr[0]);
                idAns = (int)rdr[0];
            }

            rdr.Close();

            string sqlUpdate = "UPDATE Problem SET AnswerID = @id WHERE Problem.Id = @prID";
            MySqlCommand commandUpdate = new MySqlCommand(sqlUpdate, con);

            MySqlParameter ansID = new MySqlParameter();
            ansID.ParameterName = "@id";
            ansID.Value = idAns;

            MySqlParameter probId = new MySqlParameter();
            probId.ParameterName = "@prID";
            probId.Value = problemID;

            commandUpdate.Parameters.Add(ansID);
            commandUpdate.Parameters.Add(probId);

            rows = commandUpdate.ExecuteNonQuery();
            Debug.Log("Problem rows inserted: " + rows);

            select = "SELECT * FROM Problem";

            cmd = new MySqlCommand(select, con);
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Debug.Log("rdr: " + rdr[2]);
            }

            rdr.Close();

            return true;
        }

        catch (Exception e)
        {
            Debug.Log(e);
            return false;
        }
    }

    //A method to change a password. 
    public bool changePassword(string username, string oldPassword, string newPassword)
    {
        try
        {
            if (con == null)
            {
                Awake();
            }

            bool match = false;

            //check if the user knows the old password
            string sql = "SELECT Password FROM User Where Username = @user";
            MySqlParameter param = new MySqlParameter();
            param.ParameterName = "@user";
            param.Value = username;

            cmd = new MySqlCommand(sql, con);
            cmd.Parameters.Add(param);

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                if (rdr[0].Equals(oldPassword.GetHashCode().ToString()))
                {
                    Debug.Log("You're successfully signed in");
                    match = true;
                }
            }
            rdr.Close();

            if (match)
            {
                string sqlUpdate = "UPDATE User SET Password = @password WHERE Username = @username";
                MySqlCommand commandUpdate = new MySqlCommand(sqlUpdate, con);

                MySqlParameter passw = new MySqlParameter();
                passw.ParameterName = "@password";
                passw.Value = newPassword.GetHashCode();

                MySqlParameter uName = new MySqlParameter();
                uName.ParameterName = "@username";
                uName.Value = username;

                commandUpdate.Parameters.Add(passw);
                commandUpdate.Parameters.Add(uName);

                int rows = commandUpdate.ExecuteNonQuery();
                Debug.Log("Problem rows inserted: " + rows);

                return true;
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
            return false;
        }
        return false;
    }

    //a method to change a name. Can be used by everyone
    public bool changeName(string username, string newName)
    {
        try
        {
            if (con == null)
            {
                Awake();
            }

            string sqlUpdate = "UPDATE User SET PublicName = @newName WHERE Username = @user";
            MySqlCommand commandUpdate = new MySqlCommand(sqlUpdate, con);

            MySqlParameter usernameParam = new MySqlParameter();
            usernameParam.ParameterName = "@user";
            usernameParam.Value = username;

            MySqlParameter newNameParam = new MySqlParameter();
            newNameParam.ParameterName = "@newName";
            newNameParam.Value = newName;

            commandUpdate.Parameters.Add(usernameParam);
            commandUpdate.Parameters.Add(newNameParam);

            int rows = commandUpdate.ExecuteNonQuery();
            Debug.Log("Problem rows inserted: " + rows);

            return true;

        }
        catch (Exception e)
        {
            Debug.Log(e);
            return false;
        }
    }

    public List<Problem> getProblemList()
    {
        try
        {
            if (con == null)
            {
                Awake();
            }

            string sql = "SELECT * FROM Problem";

            cmd = new MySqlCommand(sql, con);

            rdr = cmd.ExecuteReader();

            List<Problem> returnList = new List<Problem>();

            while (rdr.Read())
            {
                returnList.Add(createProblemInstance(rdr));
            }
            rdr.Close();
            return returnList;

        }
        catch (Exception e)
        {
            Debug.Log(e);
            return null;
        }
    }

    private Problem createProblemInstance(MySqlDataReader rdr)
    {
        Problem problem = new Problem();
        problem.id = (int)rdr[0];
        problem.problemName = (string)rdr[1];
        problem.problemText = (string)rdr[2];
        problem.answerId = (int)rdr[3];
        problem.solutionId = (int)rdr[4];
        problem.topicId = (int)rdr[5];
        return problem;
    }

    public bool forgotPassword(string username)
    {
        //Generate a temporaty passowrd
        string TempPassword = "";
        for (int i = 0; i < 8; i++)
        {
            Random rnd = new Random();
            int random = rnd.Next(1, tempPasswordSymbols.Length);
            TempPassword += tempPasswordSymbols.Substring(random, 1);
        }

        try
        {
            if (con == null)
            {
                Awake();
            }

            string sqlUpdate = "UPDATE User SET Password = @password WHERE Username = @username";
            MySqlCommand commandUpdate = new MySqlCommand(sqlUpdate, con);

            MySqlParameter passw = new MySqlParameter();
            passw.ParameterName = "@password";
            passw.Value = TempPassword.GetHashCode();

            MySqlParameter uName = new MySqlParameter();
            uName.ParameterName = "@username";
            uName.Value = username;

            commandUpdate.Parameters.Add(passw);
            commandUpdate.Parameters.Add(uName);

            int rows = commandUpdate.ExecuteNonQuery();
            Debug.Log("Problem rows inserted: " + rows);


            string select = "SELECT Email FROM User WHERE Username = @user";
            MySqlParameter param = new MySqlParameter();
            param.ParameterName = "@user";
            param.Value = username;

            string emailAddress = null;
            cmd = new MySqlCommand(select, con);
            cmd.Parameters.Add(param);

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Debug.Log("rdr: " + rdr[0]);
                emailAddress = (string)rdr[0];
            }

            rdr.Close();

            //send the email
            sendEmail(emailAddress, TempPassword);

            return true;

        }
        catch (Exception e)
        {
            Debug.Log(e);
            return false;
        }
    }

    //a method that is used by forgotPassword method. It is using the Google API for now,
    //so make sure that you credentials.json in the program folder. 
    public void sendEmail(string email, string tempPassword)
    {
        UserCredential credential;
        string[] Scopes = { GmailService.Scope.GmailSend };
        string ApplicationName = "VR Engineering Education";

        using (var stream =
            new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
        {
            // The file token.json stores the user's access and refresh tokens, and is created
            // automatically when the authorization flow completes for the first time.
            string credPath = "token.json";
            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.Load(stream).Secrets,
                Scopes,
                "user",
                CancellationToken.None,
                new FileDataStore(credPath, true)).Result;
            Console.WriteLine("Credential file saved to: " + credPath);
        }

        // Create Gmail API service.
        var service = new GmailService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = ApplicationName
        });

        // Define parameters of request.           
        string plainText = "To:" + email + "\r\n" +
                         "Subject: Forgot Password Request\r\n" +
                         "Content-Type: text/html; charset=us-ascii\r\n\r\n" +
                         "<p>Dear User, " + "<br>" + 
                         "<p>Your new temporary password is " + tempPassword + "<p>" +
                         "<p>Sincerely,<p>" + "<p>VR Engineering Education<p>"
                         ;

        var newMsg = new Google.Apis.Gmail.v1.Data.Message();
        newMsg.Raw = DatabaseHandler.Base64UrlEncode(plainText.ToString());
        service.Users.Messages.Send(newMsg, "me").Execute();
        Console.Read();


    }

    private static string Base64UrlEncode(string input)
    {
        var inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
        // Special "url-safe" base64 encode.
        return Convert.ToBase64String(inputBytes)
          .Replace('+', '-')
          .Replace('/', '_')
          .Replace("=", "");
    }

    //The following methods are used to pass variables to Unity and call methods
    public void StoreCreateAccount()
    {
        theName = inputFieldName.GetComponent<Text>().text;
        email = inputFieldEmail.GetComponent<Text>().text;
        username = inputFieldUsername.GetComponent<Text>().text;
        userPassword = inputFieldPassword.GetComponent<Text>().text;
        occupation = inputFieldOccupation.GetComponent<Text>().text;
        createAnAccount(theName, email, username, userPassword, occupation);
    }

    public void storeAuthenticate()
    {
        usernameForLogin = inputFieldUserLogin.GetComponent<Text>().text;
        passwordForLogin = inputFieldPasswordLogin.GetComponent<Text>().text;
        authenticate(usernameForLogin, passwordForLogin);
    }

    public void storeDeleteUser()
    {
        usernameToDelete = inputFieldUserDelete.GetComponent<Text>().text;
        emailToDelete = inputFieldEmailDelete.GetComponent<Text>().text;
        deleteUser(usernameToDelete, emailToDelete);
    }

    public void storeAddProblem()
    {
        courseName = inputFieldCourseName.GetComponent<Text>().text;
        problemName = inputFieldProblemName.GetComponent<Text>().text;
        problemText = inputFieldProblemText.GetComponent<Text>().text;
        topic = inputFieldTopic.GetComponent<Text>().text;
        chapter = inputFieldChapter.GetComponent<Text>().text;
        int chapter1 = Int32.Parse(chapter);  //need to conver to int
        lvlOfDifficulty = inputFieldLevelOfDifficulty.GetComponent<Text>().text;
        addProblem(courseName, problemName, problemText, topic, chapter1, lvlOfDifficulty);
    }

    public void storeAddSolution()
    {
        problemId = inputFieldProblemID.GetComponent<Text>().text;
        int problemIdInt = Int32.Parse(problemId);
        solutionDescription = inputFieldSolutionDescription.GetComponent<Text>().text;
        fileName = inputFieldFileName.GetComponent<Text>().text;
        addSolution(problemIdInt, solutionDescription, fileName);
    }

    public void storeChangepermissions()
    {
        changePermissionsUsername = inputFieldCPUsername.GetComponent<Text>().text;
        changePermissionsOccupation = inputFieldCPOccupation.GetComponent<Text>().text;
        changePermissions(changePermissionsUsername, changePermissionsOccupation);
    }

    public void storeAddAnswer()
    {
        addAnswerProblemID = inputFieldAnsPrID.GetComponent<Text>().text;
        int probID = Int32.Parse(addAnswerProblemID);
        addAnswerDescription = inputFieldAnsDesc.GetComponent<Text>().text;
        addAnswer(probID, addAnswerDescription);
    }

    public void storeChangepassword()
    {
        cPasswordUsername = inputFieldCPasswordUsername.GetComponent<Text>().text;
        cPasswordOldP = inputFieldCPasswordOldP.GetComponent<Text>().text;
        cPasswordNewP = inputFieldCPasswordNewP.GetComponent<Text>().text;
        changePassword(cPasswordUsername, cPasswordOldP, cPasswordNewP);
    }

    public void storeChangeName()
    {
        cNameUsername = inputFieldCNUsername.GetComponent<Text>().text;
        cNameNewName = inputFieldNewName.GetComponent<Text>().text;
        changeName(cNameUsername, cNameNewName);
    }

    public void storeResetPassword()
    {
        forgotPasswordUsername = inputFieldFPUsername.GetComponent<Text>().text;
        forgotPassword(forgotPasswordUsername);
    }

    public void getInput(string input)
    {
        Debug.Log("you entered " + input);
    }
    public string GetConnectionState()
    {
        return con.State.ToString();
    }
}