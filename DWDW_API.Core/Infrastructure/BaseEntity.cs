using System;
using System.Collections.Generic;
using System.Text;

namespace DWDW_API.Core.Infrastructure
{
    public class BaseEntity
    {
        public TDestination ToViewModel<TDestination>() where TDestination : BaseModel
        {
            return AutoMapperConfiguration.GetInstance().Map<TDestination>(this);
        }
    }
}
