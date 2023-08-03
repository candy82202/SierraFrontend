using SIERRA_Server.Models.EFModels;

namespace SIERRA_Server.Models.DTOs.Desserts
{
    public static class DessertExts
    {
        public static DessertListDTO ToDListDto(this Dessert entity)
        {
            int unitPrice = entity.Specifications.FirstOrDefault()?.UnitPrice ?? 0;
            return new DessertListDTO {            
                Dessert=entity,
                DessertId = entity.DessertId,
                DessertImageName = entity.DessertImages.FirstOrDefault().DessertImageName,
                DessertName = entity.DessertName,
                UnitPrice = unitPrice,
            };
        }

        public static DessertsIndexDTO ToDIndexDto(this Dessert entity)
        {
            int unitPrice = entity.Specifications.FirstOrDefault()?.UnitPrice ?? 0;
            return new DessertsIndexDTO
            {              
                DessertId = entity.DessertId,
                DessertImageName = entity.DessertImages.FirstOrDefault().DessertImageName,
                DessertName = entity.DessertName,
                UnitPrice = unitPrice,
            };
        }
    }
}
