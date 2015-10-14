using System;
using System.Collections.Generic;
using Aditi.GIS.Foundation.Shared;

namespace Aditi.GIS.Foundation.Mapper
{
    public interface IMap<TFrom, TTo> 
        where TFrom : IDto
        where TTo : IDto, new()
    {
        TTo Map(TFrom from);
        IEnumerable<TTo> MapCollection(IEnumerable<TFrom> from);
    }
}
