using ParcerWin.Service.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcerWin.Service
{
    public interface IAuthService
    {
        public Task<Tokens> Login(User user);
        public Task<bool> CheckKey();
        public Task<bool> AddUser(User user);
        public Task<ObservableCollection<UserViewModel>> GetUsers();
    }
}
