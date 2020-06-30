using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace DWDW_API.Core.Infrastructure
{
    public partial class BaseModel
    {
        public TDestination ToEntity<TDestination>() where TDestination:BaseEntity{
            return AutoMapperConfiguration.GetInstance().Map<TDestination>(this);
        }
    }
}
