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
            decimal dessertDiscountPrice = entity.Discounts.Any(d => d.StartAt < DateTime.Now && d.EndAt > DateTime.Now)
    ? Math.Round((decimal)unitPrice * ((decimal)entity.Discounts.First().DiscountPrice / 100), 0, MidpointRounding.AwayFromZero)
    : (decimal)unitPrice;
            return new DessertsIndexDTO((dessertDiscountPrice))
            {
                DessertId = entity.DessertId,
                DessertImageName = entity.DessertImages.FirstOrDefault().DessertImageName,
                DessertName = entity.DessertName,
                UnitPrice = unitPrice,
                Specification = entity.Specifications.FirstOrDefault(),
                Size = entity.Specifications.FirstOrDefault().Size,
                Flavor = entity.Specifications.FirstOrDefault().Flavor,
             
            };
        }
        public static DessertDiscountDTO ToDDiscountDto(this DiscountGroupItem discountGroupItem)
        {
            var dessertDiscountDTO = new DessertDiscountDTO
            {
                DessertId = 0,
                DessertImageName = "",
                DessertName = "",  // Set the default value, or remove this line if not needed
                UnitPrice = 0,
                DiscountGroupId = discountGroupItem.DiscountGroupId,
                Specification = null
            };

            if (discountGroupItem.Dessert != null)
            {
                dessertDiscountDTO.DessertId = discountGroupItem.Dessert.DessertId;
                dessertDiscountDTO.DessertName = discountGroupItem.Dessert.DessertName;
                dessertDiscountDTO.DessertImageName = discountGroupItem.Dessert.DessertImages.FirstOrDefault()?.DessertImageName;
                dessertDiscountDTO.UnitPrice = discountGroupItem.Dessert.Specifications.FirstOrDefault()?.UnitPrice ?? 0;

                dessertDiscountDTO.Specification = new Specification
                {
                    UnitPrice = discountGroupItem.Dessert.Specifications.FirstOrDefault()?.UnitPrice ?? 0,
                    Flavor = discountGroupItem.Dessert.Specifications.FirstOrDefault()?.Flavor,
                    Size = discountGroupItem.Dessert.Specifications.FirstOrDefault()?.Size
                };
            }

            return dessertDiscountDTO;
        }
        //public static DessertDiscountDTO ToDDiscountDto(this DiscountGroup entity)
        //{
        //    var dessertDiscountDTO = new DessertDiscountDTO
        //    {
        //        DessertId = 0,
        //        DessertImageName = "", 
        //        DessertName = entity.DiscountGroupName, 
        //        UnitPrice = 0, 
        //        DiscountGroupId = entity.DiscountGroupId,
        //        Specification = null 
        //    };

        //    // Assuming there's only one DiscountGroupItem per DiscountGroup for simplicity
        //    //var discountGroupItem = entity.DiscountGroupItems.FirstOrDefault();

        //    //if (discountGroupItem != null && discountGroupItem.Dessert != null)
        //    //{
        //    //    dessertDiscountDTO.DessertId = discountGroupItem.Dessert.DessertId;
        //    //    dessertDiscountDTO.DessertName = discountGroupItem.Dessert.DessertName;
        //    //    dessertDiscountDTO.DessertImageName = discountGroupItem.Dessert.DessertImages.FirstOrDefault()?.DessertImageName;
        //    //    dessertDiscountDTO.UnitPrice = discountGroupItem.Dessert.Specifications.FirstOrDefault()?.UnitPrice ?? 0;

        //    //    dessertDiscountDTO.Specification = new Specification
        //    //    {
        //    //        UnitPrice = discountGroupItem.Dessert.Specifications.FirstOrDefault()?.UnitPrice ?? 0,
        //    //        Flavor = discountGroupItem.Dessert.Specifications.FirstOrDefault()?.Flavor,
        //    //        Size = discountGroupItem.Dessert.Specifications.FirstOrDefault()?.Size
        //    //    };
        //    //}
        //    foreach (var discountGroupItem in entity.DiscountGroupItems)
        //    {
        //        if (discountGroupItem != null && discountGroupItem.Dessert != null)
        //        {
        //            dessertDiscountDTO.DessertId = discountGroupItem.Dessert.DessertId;
        //            dessertDiscountDTO.DessertName = discountGroupItem.Dessert.DessertName;
        //            dessertDiscountDTO.DessertImageName = discountGroupItem.Dessert.DessertImages.FirstOrDefault()?.DessertImageName;
        //            dessertDiscountDTO.UnitPrice = discountGroupItem.Dessert.Specifications.FirstOrDefault()?.UnitPrice ?? 0;

        //            dessertDiscountDTO.Specification = new Specification
        //            {
        //                UnitPrice = discountGroupItem.Dessert.Specifications.FirstOrDefault()?.UnitPrice ?? 0,
        //                Flavor = discountGroupItem.Dessert.Specifications.FirstOrDefault()?.Flavor,
        //                Size = discountGroupItem.Dessert.Specifications.FirstOrDefault()?.Size
        //            };

        //            // Break out of the loop after processing the first DiscountGroupItem
        //            break;
        //        }
        //    }

        //    return dessertDiscountDTO;
        //}

    }
}
