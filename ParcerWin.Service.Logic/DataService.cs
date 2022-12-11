using Newtonsoft.Json;
using ParcerWin.Service.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ParcerWin.Service.Logic
{
    public class DataService : IDataService
    {
        private readonly HttpClient _httpClient;

        public DataService(HttpClient httpClient)
            => _httpClient = httpClient;

        public async Task<int> AddPackage(ExtendedPackageViewModel package)
        {
            HttpResponseMessage response;
            string responseString;
            int packageId;

            _httpClient.DefaultRequestHeaders.Remove("access_token");
            _httpClient.DefaultRequestHeaders.Add("access_token", Global.Token.AccessToken);

            response = await _httpClient.PostAsJsonAsync($"parcer/package/add", package);

            if ((int)response.StatusCode == 401)
            {
                var tokens = await SendRefreshToken(Global.Token);

                if (tokens is null)
                    return -1;

                Global.Token = tokens;

                _httpClient.DefaultRequestHeaders.Remove("access_token");
                _httpClient.DefaultRequestHeaders.Add("access_token", Global.Token.AccessToken);

                response = await _httpClient.PostAsJsonAsync($"parcer/package/add", package);
                responseString = await response.Content.ReadAsStringAsync();
                packageId = JsonConvert.DeserializeObject<int>(responseString);

                return packageId;
            }

            responseString = await response.Content.ReadAsStringAsync();
            packageId = JsonConvert.DeserializeObject<int>(responseString);

            return packageId;
        }

        public async Task<ObservableCollection<string>> GetNumbersOfCertificates()
        {
            HttpResponseMessage response;
            string responseString;
            ObservableCollection<string> numbers;

            _httpClient.DefaultRequestHeaders.Remove("access_token");
            _httpClient.DefaultRequestHeaders.Add("access_token", Global.Token.AccessToken);

            response = await _httpClient.GetAsync("parcer/certificate/numbers");

            if ((int)response.StatusCode == 401)
            {
                var tokens = await SendRefreshToken(Global.Token);

                if (tokens is null)
                    return null;

                Global.Token = tokens;

                _httpClient.DefaultRequestHeaders.Remove("access_token");
                _httpClient.DefaultRequestHeaders.Add("access_token", Global.Token.AccessToken);

                response = await _httpClient.GetAsync("parcer/certificate/numbers");
                responseString = await response.Content.ReadAsStringAsync();
                numbers = JsonConvert.DeserializeObject<ObservableCollection<string>>(responseString);

                return numbers;
            }

            responseString = await response.Content.ReadAsStringAsync();
            numbers = JsonConvert.DeserializeObject<ObservableCollection<string>>(responseString);

            return numbers;
        }

        public async Task<ExtendedPackageViewModel> GetPackage(int packageId)
        {
            HttpResponseMessage response;
            string responseString;
            ExtendedPackageViewModel package;

            _httpClient.DefaultRequestHeaders.Remove("access_token");
            _httpClient.DefaultRequestHeaders.Add("access_token", Global.Token.AccessToken);

            response = await _httpClient.GetAsync($"parcer/package/{packageId}");

            if ((int)response.StatusCode == 401)
            {
                var tokens = await SendRefreshToken(Global.Token);

                if (tokens is null)
                    return null;

                Global.Token = tokens;

                _httpClient.DefaultRequestHeaders.Remove("access_token");
                _httpClient.DefaultRequestHeaders.Add("access_token", Global.Token.AccessToken);

                response = await _httpClient.GetAsync($"parcer/package/{packageId}");
                responseString = await response.Content.ReadAsStringAsync();
                package = JsonConvert.DeserializeObject<ExtendedPackageViewModel>(responseString);

                return package;
            }

            responseString = await response.Content.ReadAsStringAsync();
            package = JsonConvert.DeserializeObject<ExtendedPackageViewModel>(responseString);

            return package;
        }

        public async Task<ObservableCollection<PackageModel>> GetPackages(string status)
        {
            HttpResponseMessage response;
            string responseString;
            ObservableCollection<PackageModel> packages;

            _httpClient.DefaultRequestHeaders.Remove("access_token");
            _httpClient.DefaultRequestHeaders.Add("access_token", Global.Token.AccessToken);

            response = await _httpClient.GetAsync($"parcer/package?status={status}");

            if ((int)response.StatusCode == 401)
            {
                var tokens = await SendRefreshToken(Global.Token);

                if (tokens is null)
                    return null;

                Global.Token = tokens;

                _httpClient.DefaultRequestHeaders.Remove("access_token");
                _httpClient.DefaultRequestHeaders.Add("access_token", Global.Token.AccessToken);

                response = await _httpClient.GetAsync($"parcer/package?status={status}");
                responseString = await response.Content.ReadAsStringAsync();
                packages = JsonConvert.DeserializeObject<ObservableCollection<PackageModel>>(responseString);

                for (var i = 0; i < packages.Count; i++)
                    if (string.IsNullOrWhiteSpace(packages[i].Comment))
                        packages[i].Comment = "-";

                return packages;
            }

            responseString = await response.Content.ReadAsStringAsync();
            packages = JsonConvert.DeserializeObject<ObservableCollection<PackageModel>>(responseString);

            for (var i = 0; i < packages.Count; i++)
                if (string.IsNullOrWhiteSpace(packages[i].Comment))
                    packages[i].Comment = "-";

            return packages;
        }

        public async Task<ObservableCollection<PackageModel>> Search(string searchString, string status)
        {
            HttpResponseMessage response;
            string responseString;
            ObservableCollection<PackageModel> packages;

            _httpClient.DefaultRequestHeaders.Remove("access_token");
            _httpClient.DefaultRequestHeaders.Add("access_token", Global.Token.AccessToken);

            response = await _httpClient.GetAsync($"parcer/package/search?str={searchString}&status={status}");

            if ((int)response.StatusCode == 401)
            {
                var tokens = await SendRefreshToken(Global.Token);

                if (tokens is null)
                    return null;

                Global.Token = tokens;

                _httpClient.DefaultRequestHeaders.Remove("access_token");
                _httpClient.DefaultRequestHeaders.Add("access_token", Global.Token.AccessToken);

                response = await _httpClient.GetAsync($"parcer/package/search?str={searchString}&status={status}");
                responseString = await response.Content.ReadAsStringAsync();
                packages = JsonConvert.DeserializeObject<ObservableCollection<PackageModel>>(responseString);

                for (var i = 0; i < packages.Count; i++)
                    if (string.IsNullOrWhiteSpace(packages[i].Comment))
                        packages[i].Comment = "-";

                return packages;
            }

            responseString = await response.Content.ReadAsStringAsync();
            packages = JsonConvert.DeserializeObject<ObservableCollection<PackageModel>>(responseString);

            for (var i = 0; i < packages.Count; i++)
                if (string.IsNullOrWhiteSpace(packages[i].Comment))
                    packages[i].Comment = "-";

            return packages;
        }

        public async Task<int> UpdatePackage(ExtendedPackageViewModel package)
        {
            HttpResponseMessage response;
            string responseString;
            int packageId;

            _httpClient.DefaultRequestHeaders.Remove("access_token");
            _httpClient.DefaultRequestHeaders.Add("access_token", Global.Token.AccessToken);

            response = await _httpClient.PutAsJsonAsync($"parcer/package/update", package);

            if ((int)response.StatusCode == 401)
            {
                var tokens = await SendRefreshToken(Global.Token);

                if (tokens is null)
                    return -1;

                Global.Token = tokens;

                _httpClient.DefaultRequestHeaders.Remove("access_token");
                _httpClient.DefaultRequestHeaders.Add("access_token", Global.Token.AccessToken);

                response = await _httpClient.PutAsJsonAsync($"parcer/package/update", package);
                responseString = await response.Content.ReadAsStringAsync();
                packageId = JsonConvert.DeserializeObject<int>(responseString);

                return packageId;
            }

            responseString = await response.Content.ReadAsStringAsync();
            packageId = JsonConvert.DeserializeObject<int>(responseString);

            return packageId;
        }

        public async Task<string> GetSupplierByNumberOfCertificate(string number)
        {
            HttpResponseMessage response;
            string responseString;
            string supplier;

            _httpClient.DefaultRequestHeaders.Remove("access_token");
            _httpClient.DefaultRequestHeaders.Add("access_token", Global.Token.AccessToken);

            response = await _httpClient.GetAsync($"parcer/package/supplier?number={number}");

            if ((int)response.StatusCode == 401)
            {
                var tokens = await SendRefreshToken(Global.Token);

                if (tokens is null)
                    return null;

                Global.Token = tokens;

                _httpClient.DefaultRequestHeaders.Remove("access_token");
                _httpClient.DefaultRequestHeaders.Add("access_token", Global.Token.AccessToken);

                response = await _httpClient.GetAsync($"parcer/package/supplier?number={number}");
                responseString = await response.Content.ReadAsStringAsync();
                supplier = JsonConvert.DeserializeObject<string>(responseString);

                return supplier;
            }

            responseString = await response.Content.ReadAsStringAsync();
            supplier = JsonConvert.DeserializeObject<string>(responseString);

            return supplier;
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
    }
}
