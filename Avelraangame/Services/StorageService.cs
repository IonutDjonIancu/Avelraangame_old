using Avelraangame.Services.SubService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avelraangame.Services
{
    public class StorageService : StorageSubService
    {
        public string GetStorageValueById(Guid storageId)
        {
            return DataService.GetStorageValueById(storageId);
        }





    }
}
