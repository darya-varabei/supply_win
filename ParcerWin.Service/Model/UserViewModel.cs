using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcerWin.Service.Model
{
    public class UserViewModel : IDataErrorInfo
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public DateTime? Created { get; set; }
        public string? Position { get; set; }
        public string this[string columnName]
        {
            get
            {
                Error = string.Empty;

                switch (columnName)
                {
                    case "FirstName":
                        if (string.IsNullOrWhiteSpace(FirstName))
                            Error = "Имя не может быть пустым";
                        break;
                    case "LastName":
                        if (string.IsNullOrWhiteSpace(LastName))
                            Error = "Фамилия не может быть пустой";
                        break;
                    case "Login":
                        if (string.IsNullOrWhiteSpace(Login))
                            Error = "Логин не может быть пустым";
                        else if (Login.Length < 5)
                            Error = "Длина логина не может быть меньше 5";
                        break;
                    case "Password":
                        if (string.IsNullOrWhiteSpace(Password))
                            Error = "Пароль не может быть пустым";
                        else if (Password.Length < 5)
                            Error = "Длина пароля не может быть меньше 5";
                        break;
                }
                return Error;
            }
        }
        public string Error { get; set; }
    }
}
