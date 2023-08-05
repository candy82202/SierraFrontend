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
        public async Task<List<DessertDiscountDTO>> GetChocoDiscountGroups()
        {
            var dessertDiscountList = new List<DessertDiscountDTO>();

            // Get the connection string from configuration
            var connectionString = _configuration.GetConnectionString("Sierra");

            using (var connection = new SqlConnection(connectionString))
            {
                // Your SQL query goes here (use the one you provided)
                string sqlQuery = @"
                SELECT 
                    D.DessertId,
                    D.DessertName,
                    S.UnitPrice,
                    S.Flavor,
                    S.Size,
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
WHERE Dg.DiscountGroupId=6 
                ORDER BY DG.DiscountGroupId";

                await connection.OpenAsync();

                var queryResult = await connection.QueryAsync(sqlQuery);

                foreach (var row in queryResult)
                {
                    var dessertDiscountDTO = new DessertDiscountDTO
                    {
                        DessertId = row.DessertId,
                        DessertName = row.DessertName,
                        UnitPrice = row.UnitPrice,
                        DessertImageName = row.DessertImageName,
                        DiscountGroupId = row.DiscountGroupId,
                        Specification = new Specification
                        {
                            UnitPrice = row.UnitPrice,
                            Flavor = row.Flavor,
                            Size = row.Size
                        }
                    };
                    dessertDiscountList.Add(dessertDiscountDTO);
                }
            }

            return dessertDiscountList;
        }
    }
}
