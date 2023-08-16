using Dapper;
using Microsoft.Data.SqlClient;
using SIERRA_Server.Models.DTOs.Desserts;
using SIERRA_Server.Models.EFModels;
using SIERRA_Server.Models.Interfaces;

namespace SIERRA_Server.Models.Repository.DPRepository
{
    public class DessertDiscountDPRepository : IDessertDiscountRepository
    {
        private readonly AppDbContext _context;

        private readonly IConfiguration _configuration;

        public DessertDiscountDPRepository(AppDbContext db,  IConfiguration config)
        {
            _context = db;
            _configuration= config;
        }
        public async Task<List<DessertDiscountDTO>> GetDiscountGroupsByGroupId(int discountGroupId)
        {

            var dessertDiscountList = new List<DessertDiscountDTO>();

            // 取得名為Sierra的連接字符串(用於連接到數據庫)
            var connectionString = _configuration.GetConnectionString("Sierra");

            //建立數據庫連接
            using (var connection = new SqlConnection(connectionString))
            {
                // 從SQL 查詢裡面使用SQL Query的語法查找出資料
                string sqlQuery = $@"
                SELECT 
                    D.DessertId,
                    D.DessertName,
                    S.UnitPrice,
                    S.Flavor,
                    S.Size,S.SpecificationId,

                    DG.DiscountGroupId,
                    DI.DessertImageName
                FROM 
                    DiscountGroups DG
                INNER JOIN 
                    DiscountGroupItems DGI ON DG.DiscountGroupId = DGI.DiscountGroupId
                INNER JOIN 
                    Desserts D ON DGI.DessertId = D.DessertId
                LEFT JOIN 
                    DessertImages DI ON DI.DessertId = D.DessertId
                LEFT JOIN 
                    Specification S ON D.DessertId = S.DessertId 
                WHERE DG.DiscountGroupId = @DiscountGroupId
                ORDER BY DG.DiscountGroupId";

                await connection.OpenAsync();

                //這裡QueryAsync使用Dapper的方法來非同步執行SQL查詢。
                //這裡為了防止SQL注入，使用參數化@DiscountGroupId 對應到 discountGroupId傳遞給方法的參數。
                var queryResult = await connection.QueryAsync(sqlQuery, new { DiscountGroupId = discountGroupId });

                foreach (var row in queryResult)
                {
                    //然後查詢的結果對應到DTO
                    var dessertDiscountDTO = new DessertDiscountDTO
                    {
                        DessertId = row.DessertId,
                        DessertName = row.DessertName,
                        UnitPrice = row.UnitPrice,
                        DessertImageName = row.DessertImageName,
                        DiscountGroupId = row.DiscountGroupId,
                        //Specification = new Specification
                        //{
                            SpecificationId=row.SpecificationId,
                            //UnitPrice = row.UnitPrice,
                            Flavor = row.Flavor,
                            Size = row.Size
                        //}
                    };
                    //foreach迴圈找完相對應的結果，放在剛剛創建的DessertDiscountDTO，把這個物件內容加到dessertDiscountList裡面
                    dessertDiscountList.Add(dessertDiscountDTO);
                }
            }
            //返回剛剛迴圈找出的所有結果
            return dessertDiscountList;
        
    }
        public async Task<List<DessertDiscountDTO>> GetDiscountGroups()
        {

            var dessertDiscountList = new List<DessertDiscountDTO>();

            // 取得名為Sierra的連接字符串(用於連接到數據庫)
            var connectionString = _configuration.GetConnectionString("Sierra");

            //建立數據庫連接
            using (var connection = new SqlConnection(connectionString))
            {
                // 從SQL 查詢裡面使用SQL Query的語法查找出資料
                string sqlQuery = $@"
                SELECT 
                    D.DessertId,
                    D.DessertName,
                    S.UnitPrice,
                    S.Flavor,
                    S.Size,S.SpecificationId,

                    DG.DiscountGroupId,
                    DI.DessertImageName
                FROM 
                    DiscountGroups DG
                INNER JOIN 
                    DiscountGroupItems DGI ON DG.DiscountGroupId = DGI.DiscountGroupId
                INNER JOIN 
                    Desserts D ON DGI.DessertId = D.DessertId
                LEFT JOIN 
                    DessertImages DI ON DI.DessertId = D.DessertId
                LEFT JOIN 
                    Specification S ON D.DessertId = S.DessertId 
  WHERE DGI.DiscountGroupId >5
";

                await connection.OpenAsync();

                //這裡QueryAsync使用Dapper的方法來非同步執行SQL查詢。
                //這裡為了防止SQL注入，使用參數化@DiscountGroupId 對應到 discountGroupId傳遞給方法的參數。
                var queryResult = await connection.QueryAsync(sqlQuery);

                foreach (var row in queryResult)
                {
                    //然後查詢的結果對應到DTO
                    var dessertDiscountDTO = new DessertDiscountDTO
                    {
                        DessertId = row.DessertId,
                        DessertName = row.DessertName,
                        UnitPrice = row.UnitPrice,
                        DessertImageName = row.DessertImageName,
                        DiscountGroupId = row.DiscountGroupId,
                        //Specification = new Specification
                        //{
                            SpecificationId = row.SpecificationId,
                            //UnitPrice = row.UnitPrice,
                            Flavor = row.Flavor,
                            Size = row.Size
                        //}
                    };
                    //foreach迴圈找完相對應的結果，放在剛剛創建的DessertDiscountDTO，把這個物件內容加到dessertDiscountList裡面
                    dessertDiscountList.Add(dessertDiscountDTO);
                }
            }
            //返回剛剛迴圈找出的所有結果
            return dessertDiscountList;

        }

        public async Task<List<DessertDiscountDTO>> GetCategory()
        {

            var dessertDiscountList = new List<DessertDiscountDTO>();

            // 取得名為Sierra的連接字符串(用於連接到數據庫)
            var connectionString = _configuration.GetConnectionString("Sierra");

            //建立數據庫連接
            using (var connection = new SqlConnection(connectionString))
            {
                // 從SQL 查詢裡面使用SQL Query的語法查找出資料
                string sqlQuery = $@"
                SELECT 
                    D.DessertId,
                    D.DessertName,
                    S.UnitPrice,
                    S.Flavor,
                    S.Size,S.SpecificationId,

                    DG.DiscountGroupId,
                    DI.DessertImageName
                FROM 
                    DiscountGroups DG
                INNER JOIN 
                    DiscountGroupItems DGI ON DG.DiscountGroupId = DGI.DiscountGroupId
                INNER JOIN 
                    Desserts D ON DGI.DessertId = D.DessertId
                LEFT JOIN 
                    DessertImages DI ON DI.DessertId = D.DessertId
                LEFT JOIN 
                    Specification S ON D.DessertId = S.DessertId 
  WHERE DGI.DiscountGroupId <6
";

                await connection.OpenAsync();

                //這裡QueryAsync使用Dapper的方法來非同步執行SQL查詢。
                //這裡為了防止SQL注入，使用參數化@DiscountGroupId 對應到 discountGroupId傳遞給方法的參數。
                var queryResult = await connection.QueryAsync(sqlQuery);

                foreach (var row in queryResult)
                {
                    //然後查詢的結果對應到DTO
                    var dessertDiscountDTO = new DessertDiscountDTO
                    {
                        DessertId = row.DessertId,
                        DessertName = row.DessertName,
                        UnitPrice = row.UnitPrice,
                        DessertImageName = row.DessertImageName,
                        DiscountGroupId = row.DiscountGroupId,
                        //Specification = new Specification
                        //{
                            SpecificationId = row.SpecificationId,
                            //UnitPrice = row.UnitPrice,
                            Flavor = row.Flavor,
                            Size = row.Size
                        //}
                    };
                    //foreach迴圈找完相對應的結果，放在剛剛創建的DessertDiscountDTO，把這個物件內容加到dessertDiscountList裡面
                    dessertDiscountList.Add(dessertDiscountDTO);
                }
            }
            //返回剛剛迴圈找出的所有結果
            return dessertDiscountList;

        }
        public async Task<List<DessertDiscountDTO>> GetTopSales()
        {
            var dessertTopSalesList = new List<DessertDiscountDTO>();

            // 取得名為Sierra的連接字符串(用於連接到數據庫)
            var connectionString = _configuration.GetConnectionString("Sierra");

            //建立數據庫連接
            using (var connection = new SqlConnection(connectionString))
            {
                // 從SQL 查詢裡面使用SQL Query的語法查找出資料
                string sqlQuery = $@"
                  SELECT TOP 3 D.DessertId, D.DessertName,s.UnitPrice  , Di.DessertImageName, SUM(Do.Quantity) AS TotalQuantity
FROM DessertOrderDetails AS Do
JOIN Desserts AS D ON D.DessertId = Do.DessertId
Left join Specification as s on s.DessertId = Do.DessertId
Left join DessertImages as Di on Di.DessertId = Do.DessertId
GROUP BY D.DessertId, D.DessertName,s.UnitPrice , Di.DessertImageName
ORDER BY TotalQuantity DESC;
";

                await connection.OpenAsync();

                //這裡QueryAsync使用Dapper的方法來非同步執行SQL查詢。
                //這裡為了防止SQL注入，使用參數化@DiscountGroupId 對應到 discountGroupId傳遞給方法的參數。
                var queryResult = await connection.QueryAsync(sqlQuery);

                foreach (var row in queryResult)
                {
                    //然後查詢的結果對應到DTO
                    var dessertDiscountDTO = new DessertDiscountDTO
                    {
                        DessertId = row.DessertId,
                        DessertName = row.DessertName,
                        UnitPrice = row.UnitPrice,
                        DessertImageName = row.DessertImageName,

                        //Specification = new Specification
                        //{
                            SpecificationId = row.SpecificationId,
                            //UnitPrice = row.UnitPrice,
                            Flavor = row.Flavor,
                            Size = row.Size
                        //}
                    };
                    //foreach迴圈找完相對應的結果，放在剛剛創建的DessertDiscountDTO，把這個物件內容加到dessertDiscountList裡面
                    dessertTopSalesList.Add(dessertDiscountDTO);
                }
            }
            //返回剛剛迴圈找出的所有結果
            return dessertTopSalesList;
        }
    }
}
