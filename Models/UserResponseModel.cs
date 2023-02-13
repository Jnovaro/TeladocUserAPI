namespace TeladocUserAPI.Models
{
    public class UserResponseModel
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public List<UserModel>? Users { get; set; }

        public UserResponseModel(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }

    }
}
