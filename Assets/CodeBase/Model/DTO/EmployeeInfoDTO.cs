using System;

namespace Model.DTO
{
    [Serializable]
    public class EmployeeInfoDTO
    {
        public int id;
        public string first_name;
        public string last_name;
        public string email;
        public string ip_address;
        public string gender;
    }
}