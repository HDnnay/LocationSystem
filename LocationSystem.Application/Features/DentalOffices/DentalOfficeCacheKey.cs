using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.DentalOffices
{
    public static class DentalOfficeCacheKey
    {
        public static string DentalOfficeIdKey = "DentalOffices:Id:";
        public static string DentalOfficeAllKey = "DentalOffices:All";
        public static string DetalOfficePageKey = "DentalOffices:Page:*PageSize:*KeySize:*";
        public static string GetDetalOfficePageKey(int page,int pageSize,string keyWord)
        {
            var key = $"DentalOffices:Page:{page}:PageSize:{pageSize}:KeySize:{keyWord}";
            return key;
        }
    }
}
