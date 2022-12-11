using ParcerWin.Service.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcerWin.Service
{
    public interface IDataService
    {
        public Task<ObservableCollection<PackageModel>> GetPackages(string status);
        public Task<ExtendedPackageViewModel> GetPackage(int packageId);
        public Task<int> UpdatePackage(ExtendedPackageViewModel package);
        public Task<int> AddPackage(ExtendedPackageViewModel package);
        public Task<ObservableCollection<string>> GetNumbersOfCertificates();
        public Task<ObservableCollection<PackageModel>> Search(string searchString, string status);
        public Task<string> GetSupplierByNumberOfCertificate(string number);
    }
}
