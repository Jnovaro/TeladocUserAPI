using Newtonsoft.Json;
using System.Text.Json.Nodes;
using TeladocUserAPI.Models;

namespace TeladocUserAPI.Controllers
{
    public class UserController
    {
        const string filePath = "..\\users.json";

        public UserController() { }



        public JsonNode AddUser(UserModel user)
        {
            UserResponseModel response = new(true);
            try
            {
                string error = String.Empty;
                if (user.IsValid(true, ref error))
                {
                    List<UserModel>? userData = GetUserData();
                    userData ??= new List<UserModel>();

                    userData.Add(user);
                    System.IO.File.WriteAllText(filePath, JsonConvert.SerializeObject(userData));
                    response.Message = "User added succesfully.";
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = error;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return ResponseFormatter(response);
        }

        public JsonNode UpdateUser(string email, UserModel user)
        {
            UserResponseModel response = new(true);
            try
            {
                string error = String.Empty;
                List<UserModel>? userData = GetUserData();
                if (userData.Any(x => x.Email == email))
                {
                    if (user.IsValid(false, ref error, email))
                    {
                        UserModel oldUser = userData.First(x => x.Email == email);
                        userData.Remove(oldUser);
                        userData.Add(user);
                        System.IO.File.WriteAllText(filePath, JsonConvert.SerializeObject(userData));
                        response.Message = "The information was updated succesfully.";
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = error;
                    }
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "There's no user registered under that Email Address.";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return ResponseFormatter(response);
        }


        public JsonNode DeleteUser(string email)
        {
            UserResponseModel response = new(true);
            try
            {
                List<UserModel>? userData = GetUserData();
                if (userData.Any(x => x.Email == email))
                {
                    UserModel oldUser = userData.First(x => x.Email == email);
                    userData.Remove(oldUser);
                    System.IO.File.WriteAllText(filePath, JsonConvert.SerializeObject(userData));
                    response.Message = "The user was deleted succesfully.";
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "There's no user registered under that Email Address.";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return ResponseFormatter(response);
        }

        public JsonNode GetUserByEmail(string email)
        {
            UserResponseModel response = new(true);
            try
            {
                List<UserModel>? userData = GetUserData();
                if (userData.Any(x => x.Email == email))
                {
                    List<UserModel> users = new()
                    {
                        userData.First(x => x.Email == email)
                    };
                    response.Users = users;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "There's no user registered under that Email Address.";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return ResponseFormatter(response);
        }

        public JsonNode GetAllUsers()
        {
            UserResponseModel response = new(true);
            try
            {
                response.Users = GetUserData();
                if (response.Users == null || response.Users.Count == 0)
                {
                    response.IsSuccess = false;
                    response.Message = "There are no users registered.";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return ResponseFormatter(response);
        }

        public List<UserModel> GetUserData()
        {
            try
            {
                string text = string.Empty;
                if (System.IO.File.Exists(filePath))
                {
                    text = System.IO.File.ReadAllText(filePath);
                }

                List<UserModel>? userData = JsonConvert.DeserializeObject<List<UserModel>>(text);
                userData ??= new List<UserModel>();
                return userData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static JsonObject ResponseFormatter(UserResponseModel response)
        {
            JsonObject json = (JsonObject)JsonNode.Parse(JsonConvert.SerializeObject(response));
            if (String.IsNullOrEmpty(response.Message))
            {
                json.Remove("Message");
            }
            if (response.Users == null || response.Users.Count == 0)
            {
                json.Remove("Users");
            }
            return json;

        }
    }
}
