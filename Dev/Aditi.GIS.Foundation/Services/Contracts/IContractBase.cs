using System;
using Aditi.GIS.Foundation.Shared;

namespace Aditi.GIS.Foundation.Services.Contracts
{
    public interface IContractBase: IDto
    {
        void SetDefaultLinks();
        void SetSelfLink();
        // long Id { get; set; }
        string ETag { get; set; }
    }
}
