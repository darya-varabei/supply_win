using Newtonsoft.Json;
using ParcerWin.Service.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Json;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;
using System.IO;

namespace ParcerWin.Service.Logic
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
            => _httpClient = httpClient;

        public async Task<bool> CheckKey()
        {
            var tokens = GetTokens();

            if (tokens is null)
            {
                await _httpClient.GetAsync("parcer/certificate/numbers");
                return false;
            }

            _httpClient.DefaultRequestHeaders.Remove("access_token");
            _httpClient.DefaultRequestHeaders.Add("access_token", tokens.AccessToken);

            var response = await _httpClient.GetAsync("authentication/check");

            if ((int)response.StatusCode == 401)
            {
                tokens = await SendRefreshToken(tokens);

                if (tokens is null || tokens.AccessToken is null)
                    return false;

                var secToken1 = (new JwtSecurityTokenHandler().ReadToken(tokens.AccessToken)) as JwtSecurityToken;
                var claims1 = secToken1.Claims;
                var roleClaim1 = claims1.FirstOrDefault(cl => cl.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
                var role1 = roleClaim1.Value;

                Global.User = new User()
                {
                    UserInfo = new UserInfo()
                    {
                        Position = role1
                    }
                };

                Global.Token = tokens;
                return true;
            }

            var secToken = (new JwtSecurityTokenHandler().ReadToken(tokens.AccessToken)) as JwtSecurityToken;
            var claims = secToken.Claims;
            var roleClaim = claims.FirstOrDefault(cl => cl.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
            var role = roleClaim.Value;

            Global.User = new User()
            {
                UserInfo = new UserInfo()
                {
                    Position = role
                }
            };

            Global.Token = tokens;
            return true;
        }

        public async Task<Tokens> Login(User user)
        {
            var response = await _httpClient.PostAsJsonAsync("authentication/login", user);

            if (response.IsSuccessStatusCode)
            {
                var tokensString = await response.Content.ReadAsStringAsync();
                var tokens = JsonConvert.DeserializeObject<Tokens>(tokensString);

                if (!CheckToken(tokens))
                    return null;

                SetTokens(tokens);

                return tokens;
            }

            return null;
        }

        private bool CheckToken(Tokens tokens)
        {
            var secToken = (new JwtSecurityTokenHandler().ReadToken(tokens.AccessToken)) as JwtSecurityToken;
            var claims = secToken.Claims;
            var roleClaim = claims.FirstOrDefault(cl => cl.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
            var role = roleClaim.Value;

            Global.User = new User()
            {
                UserInfo = new UserInfo()
                {
                    Position = role
                }
            };

            return role == "manager" || role == "admin";
        }

        private void SetTokens(Tokens tokens)
        {
            using var binWriter = new BinaryWriter(File.Open("auth.bin", FileMode.Create));

            binWriter.Write(tokens.AccessToken);
            binWriter.Write(tokens.RefreshToken);
        }

        private Tokens GetTokens()
        {
            using var binReader = new BinaryReader(File.Open("auth.bin", FileMode.OpenOrCreate));

            if (binReader.BaseStream.Length != 0)
            {
                var tokens = new Tokens()
                {
                    AccessToken = binReader.ReadString(),
                    RefreshToken = binReader.ReadString()
                };

                return tokens;
            }

            return null;
        }

        private async Task<Tokens> SendRefreshToken(Tokens tokens)
        {
            var response = await _httpClient.PostAsJsonAsync("token/refresh", tokens);

            if ((int)response.StatusCode == 404)
                return null;

            var respString = await response.Content.ReadAsStringAsync();
            tokens = JsonConvert.DeserializeObject<Tokens>(respString);

            return tokens;
        }

        public async Task<bool> AddUser(User user)
        {
            HttpResponseMessage response;
            int statusCode;

            _httpClient.DefaultRequestHeaders.Remove("access_token");
            _httpClient.DefaultRequestHeaders.Add("access_token", Global.Token.AccessToken);

            response = await _httpClient.PostAsJsonAsync($"authentication", user);
            statusCode = (int)response.StatusCode;

            if (statusCode == 401)
            {
                var tokens = await SendRefreshToken(Global.Token);

                if (tokens is null)
                    return false;

                Global.Token = tokens;

                _httpClient.DefaultRequestHeaders.Remove("access_token");
                _httpClient.DefaultRequestHeaders.Add("access_token", Global.Token.AccessToken);

                response = await _httpClient.PostAsJsonAsync($"authentication", user);
                statusCode = (int)response.StatusCode;

                return statusCode == 200;
            }

            return statusCode == 200;
        }

        public async Task<ObservableCollection<UserViewModel>> GetUsers()
        {
            HttpResponseMessage response;
            string responseString;
            ObservableCollection<UserViewModel> users;

            _httpClient.DefaultRequestHeaders.Remove("access_token");
            _httpClient.DefaultRequestHeaders.Add("access_token", Global.Token.AccessToken);

            response = await _httpClient.GetAsync("authentication/users");

            if ((int)response.StatusCode == 401)
            {
                var tokens = await SendRefreshToken(Global.Token);

                if (tokens is null)
                    return null;

                Global.Token = tokens;

                _httpClient.DefaultRequestHeaders.Remove("access_token");
                _httpClient.DefaultRequestHeaders.Add("access_token", Global.Token.AccessToken);

                response = await _httpClient.GetAsync($"authentication/users");
                responseString = await response.Content.ReadAsStringAsync();
                users = JsonConvert.DeserializeObject<ObservableCollection<UserViewModel>>(responseString);

                return users;
            }

            responseString = await response.Content.ReadAsStringAsync();
            users = JsonConvert.DeserializeObject<ObservableCollection<UserViewModel>>(responseString);

            return users;
        }
    }
}
