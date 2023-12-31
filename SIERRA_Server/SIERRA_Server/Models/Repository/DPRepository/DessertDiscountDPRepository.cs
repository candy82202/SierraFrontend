﻿using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SIERRA_Server.Models.DTOs.Desserts;
using SIERRA_Server.Models.EFModels;
using SIERRA_Server.Models.Interfaces;
using System.Drawing;
using System;

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
        //public async Task<List<DessertDiscountDTO>> GetDiscountGroupsByGroupId(int discountGroupId)
        //{
        //    var dessertDiscountList = new List<DessertDiscountDTO>();

        //    // 取得名為Sierra的連接字符串(用於連接到數據庫)
        //    var connectionString = _configuration.GetConnectionString("Sierra");

        //    // 建立數據庫連接
        //    using (var connection = new SqlConnection(connectionString))
        //    {
        //        // 從SQL 查詢裡面使用SQL Query的語法查找出資料
        //        string sqlQuery = @"
        //    SELECT 
        //        D.DessertId,
        //        D.DessertName,
        //        S.UnitPrice,
        //        S.Flavor,
        //        S.Size,
        //        S.SpecificationId,
        //        DG.DiscountGroupId,
        //        DI.DessertImageName
        //    FROM 
        //        DiscountGroups DG
        //    INNER JOIN 
        //        DiscountGroupItems DGI ON DG.DiscountGroupId = DGI.DiscountGroupId
        //    INNER JOIN 
        //        Desserts D ON DGI.DessertId = D.DessertId
        //    LEFT JOIN 
        //        DessertImages DI ON DI.DessertId = D.DessertId
        //    LEFT JOIN 
        //        Specification S ON D.DessertId = S.DessertId 
        //    WHERE DG.DiscountGroupId = @DiscountGroupId
        //    ORDER BY DG.DiscountGroupId";

        //        await connection.OpenAsync();
        //        var queryResult = await connection.QueryAsync<DessertDiscountDTO, Specification, DessertDiscountDTO>(
        //            sqlQuery,
        //            (dessertDiscountDTO, specification) =>
        //            {
        //                dessertDiscountDTO.Specification = specification;
        //                return dessertDiscountDTO;
        //            },
        //            new { DiscountGroupId = discountGroupId },
        //            splitOn: "SpecificationId");

        //        dessertDiscountList.AddRange(queryResult);
        //    }

        //    return dessertDiscountList;
        //}
        //public async Task<List<DessertDiscountDTO>> GetDiscountGroupsByGroupId(int discountGroupId)
        //{

        //    var dessertDiscountList = new List<DessertDiscountDTO>();

        //    // 取得名為Sierra的連接字符串(用於連接到數據庫)
        //    var connectionString = _configuration.GetConnectionString("Sierra");

        //    //建立數據庫連接
        //    using (var connection = new SqlConnection(connectionString))
        //    {
        //        // 從SQL 查詢裡面使用SQL Query的語法查找出資料
        //        string sqlQuery = $@"
        //        SELECT 
        //            D.DessertId,
        //            D.DessertName,
        //            S.UnitPrice,
        //            S.Flavor,
        //            S.Size,S.SpecificationId,

        //            DG.DiscountGroupId,
        //            DI.DessertImageName
        //        FROM 
        //            DiscountGroups DG
        //        INNER JOIN 
        //            DiscountGroupItems DGI ON DG.DiscountGroupId = DGI.DiscountGroupId
        //        INNER JOIN 
        //            Desserts D ON DGI.DessertId = D.DessertId
        //        LEFT JOIN 
        //            DessertImages DI ON DI.DessertId = D.DessertId
        //        LEFT JOIN 
        //            Specification S ON D.DessertId = S.DessertId 
        //        WHERE DG.DiscountGroupId = @DiscountGroupId
        //        ORDER BY DG.DiscountGroupId";

        //        await connection.OpenAsync();

        //        //這裡QueryAsync使用Dapper的方法來非同步執行SQL查詢。
        //        //這裡為了防止SQL注入，使用參數化@DiscountGroupId 對應到 discountGroupId傳遞給方法的參數。
        //        var queryResult = await connection.QueryAsync(sqlQuery, new { DiscountGroupId = discountGroupId });

        //        foreach (var row in queryResult)
        //        {
        //            //然後查詢的結果對應到DTO
        //            var dessertDiscountDTO = new DessertDiscountDTO
        //            {
        //                DessertId = row.DessertId,
        //                DessertName = row.DessertName,
        //                UnitPrice = row.UnitPrice,
        //                DessertImageName = row.DessertImageName,
        //                DiscountGroupId = row.DiscountGroupId,
        //                Specification = new Specification
        //                {
        //                    SpecificationId = row.SpecificationId,
        //                    UnitPrice = row.UnitPrice,
        //                    Flavor = row.Flavor,
        //                    Size = row.Size
        //                }
        //            };
        //            //foreach迴圈找完相對應的結果，放在剛剛創建的DessertDiscountDTO，把這個物件內容加到dessertDiscountList裡面
        //            dessertDiscountList.Add(dessertDiscountDTO);
        //        }
        //    }
        //    //返回剛剛迴圈找出的所有結果
        //    return dessertDiscountList;

        //}

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
        MAX(D.DessertName) AS DessertName,
        MAX(S.Flavor) AS Flavor,
        MAX(S.Size) AS Size,
        MAX(S.UnitPrice) AS UnitPrice,
        DGI.DiscountGroupId,
        MAX(DI.DessertImageName) AS DessertImageName,
        STRING_AGG(CONVERT(VARCHAR, S2.SpecificationId), ', ') WITHIN GROUP(ORDER BY S2.SpecificationId) AS SpecificationIds
                FROM
                     DiscountGroupItems DGI
                INNER JOIN
                    Desserts D ON DGI.DessertId = D.DessertId
                LEFT JOIN
                    DessertImages DI ON DI.DessertId = D.DessertId
                LEFT JOIN
                    Specification S ON D.DessertId = S.DessertId
                LEFT JOIN
                    Specification S2 ON D.DessertId = S2.DessertId
                           WHERE DGI.DiscountGroupId = @DiscountGroupId

                GROUP BY D.DessertId, DGI.DiscountGroupId
                ORDER BY DGI.DiscountGroupId";

                await connection.OpenAsync();

                //這裡QueryAsync使用Dapper的方法來非同步執行SQL查詢。
                //這裡為了防止SQL注入，使用參數化@DiscountGroupId 對應到 discountGroupId傳遞給方法的參數。
                var queryResult = await connection.QueryAsync(sqlQuery, new { DiscountGroupId = discountGroupId });

                foreach (var row in queryResult)
                {
                    var dessertDiscountDTO = new DessertDiscountDTO
                    {
                        DessertId = row.DessertId,
                        DessertName = row.DessertName,
                        UnitPrice = row.UnitPrice,
                        DessertImageName = row.DessertImageName,
                        DiscountGroupId = row.DiscountGroupId,
                        Flavor=row.Flavor,
                        Size = row.Size,
                    };

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
        MAX(D.DessertName) AS DessertName,
        MAX(S.Flavor) AS Flavor,
        MAX(S.Size) AS Size,
        MAX(S.UnitPrice) AS UnitPrice,
        DGI.DiscountGroupId,
        MAX(DI.DessertImageName) AS DessertImageName,
        STRING_AGG(CONVERT(VARCHAR, S2.SpecificationId), ', ') WITHIN GROUP(ORDER BY S2.SpecificationId) AS SpecificationIds
                FROM
                     DiscountGroupItems DGI
                INNER JOIN
                    Desserts D ON DGI.DessertId = D.DessertId
                LEFT JOIN
                    DessertImages DI ON DI.DessertId = D.DessertId
                LEFT JOIN
                    Specification S ON D.DessertId = S.DessertId
                LEFT JOIN
                    Specification S2 ON D.DessertId = S2.DessertId
                           WHERE DGI.DiscountGroupId >5

                GROUP BY D.DessertId, DGI.DiscountGroupId
                ORDER BY DGI.DiscountGroupId";

                await connection.OpenAsync();

                //這裡QueryAsync使用Dapper的方法來非同步執行SQL查詢。
                //這裡為了防止SQL注入，使用參數化@DiscountGroupId 對應到 discountGroupId傳遞給方法的參數。
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
                        Flavor = row.Flavor,
                        Size = row.Size,
                    };

                    dessertDiscountList.Add(dessertDiscountDTO);
                }
            }
            //返回剛剛迴圈找出的所有結果
            return dessertDiscountList;

        }

        //        public async Task<List<DessertDiscountDTO>> GetDiscountGroups()
        //        {

        //            var dessertDiscountList = new List<DessertDiscountDTO>();

        //            // 取得名為Sierra的連接字符串(用於連接到數據庫)
        //            var connectionString = _configuration.GetConnectionString("Sierra");

        //            //建立數據庫連接
        //            using (var connection = new SqlConnection(connectionString))
        //            {
        //                // 從SQL 查詢裡面使用SQL Query的語法查找出資料
        //                string sqlQuery = $@"
        //                SELECT 
        //                    D.DessertId,
        //                    D.DessertName,
        //                    S.UnitPrice,
        //                    S.Flavor,
        //                    S.Size,S.SpecificationId,

        //                    DG.DiscountGroupId,
        //                    DI.DessertImageName
        //                FROM 
        //                    DiscountGroups DG
        //                INNER JOIN 
        //                    DiscountGroupItems DGI ON DG.DiscountGroupId = DGI.DiscountGroupId
        //                INNER JOIN 
        //                    Desserts D ON DGI.DessertId = D.DessertId
        //                LEFT JOIN 
        //                    DessertImages DI ON DI.DessertId = D.DessertId
        //                LEFT JOIN 
        //                    Specification S ON D.DessertId = S.DessertId 
        //  WHERE DGI.DiscountGroupId >5
        //";

        //                await connection.OpenAsync();

        //                //這裡QueryAsync使用Dapper的方法來非同步執行SQL查詢。
        //                //這裡為了防止SQL注入，使用參數化@DiscountGroupId 對應到 discountGroupId傳遞給方法的參數。
        //                var queryResult = await connection.QueryAsync(sqlQuery);

        //                foreach (var row in queryResult)
        //                {
        //                    //然後查詢的結果對應到DTO
        //                    var dessertDiscountDTO = new DessertDiscountDTO
        //                    {
        //                        DessertId = row.DessertId,
        //                        DessertName = row.DessertName,
        //                        UnitPrice = row.UnitPrice,
        //                        DessertImageName = row.DessertImageName,
        //                        DiscountGroupId = row.DiscountGroupId,
        //                        //Specification = new Specification
        //                        //{
        //                            SpecificationId = row.SpecificationId,
        //                            //UnitPrice = row.UnitPrice,
        //                            Flavor = row.Flavor,
        //                            Size = row.Size
        //                        //}
        //                    };
        //                    //foreach迴圈找完相對應的結果，放在剛剛創建的DessertDiscountDTO，把這個物件內容加到dessertDiscountList裡面
        //                    dessertDiscountList.Add(dessertDiscountDTO);
        //                }
        //            }
        //            //返回剛剛迴圈找出的所有結果
        //            return dessertDiscountList;

        //        }
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
        MAX(D.DessertName) AS DessertName,
        MAX(S.Flavor) AS Flavor,
        MAX(S.Size) AS Size,
        MAX(S.UnitPrice) AS UnitPrice,
        DGI.DiscountGroupId,
        MAX(DI.DessertImageName) AS DessertImageName,
        STRING_AGG(CONVERT(VARCHAR, S2.SpecificationId), ', ') WITHIN GROUP(ORDER BY S2.SpecificationId) AS SpecificationIds
                FROM
                     DiscountGroupItems DGI
                INNER JOIN
                    Desserts D ON DGI.DessertId = D.DessertId
                LEFT JOIN
                    DessertImages DI ON DI.DessertId = D.DessertId
                LEFT JOIN
                    Specification S ON D.DessertId = S.DessertId
                LEFT JOIN
                    Specification S2 ON D.DessertId = S2.DessertId
                           WHERE DGI.DiscountGroupId <6

                GROUP BY D.DessertId, DGI.DiscountGroupId
                ORDER BY DGI.DiscountGroupId";

                await connection.OpenAsync();

                //這裡QueryAsync使用Dapper的方法來非同步執行SQL查詢。
                //這裡為了防止SQL注入，使用參數化@DiscountGroupId 對應到 discountGroupId傳遞給方法的參數。
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
                        Flavor = row.Flavor,
                        Size = row.Size,
                    };

                    dessertDiscountList.Add(dessertDiscountDTO);
                }
            }
            //返回剛剛迴圈找出的所有結果
            return dessertDiscountList;

        }
        //        public async Task<List<DessertDiscountDTO>> GetCategory()
        //        {

        //            var dessertDiscountList = new List<DessertDiscountDTO>();

        //            // 取得名為Sierra的連接字符串(用於連接到數據庫)
        //            var connectionString = _configuration.GetConnectionString("Sierra");

        //            //建立數據庫連接
        //            using (var connection = new SqlConnection(connectionString))
        //            {
        //                // 從SQL 查詢裡面使用SQL Query的語法查找出資料
        //                string sqlQuery = $@"
        //                SELECT 
        //                    D.DessertId,
        //                    D.DessertName,
        //                    S.UnitPrice,
        //                    S.Flavor,
        //                    S.Size,S.SpecificationId,

        //                    DG.DiscountGroupId,
        //                    DI.DessertImageName
        //                FROM 
        //                    DiscountGroups DG
        //                INNER JOIN 
        //                    DiscountGroupItems DGI ON DG.DiscountGroupId = DGI.DiscountGroupId
        //                INNER JOIN 
        //                    Desserts D ON DGI.DessertId = D.DessertId
        //                LEFT JOIN 
        //                    DessertImages DI ON DI.DessertId = D.DessertId
        //                LEFT JOIN 
        //                    Specification S ON D.DessertId = S.DessertId 
        //  WHERE DGI.DiscountGroupId <6
        //";

        //                await connection.OpenAsync();

        //                //這裡QueryAsync使用Dapper的方法來非同步執行SQL查詢。
        //                //這裡為了防止SQL注入，使用參數化@DiscountGroupId 對應到 discountGroupId傳遞給方法的參數。
        //                var queryResult = await connection.QueryAsync(sqlQuery);

        //                foreach (var row in queryResult)
        //                {
        //                    //然後查詢的結果對應到DTO
        //                    var dessertDiscountDTO = new DessertDiscountDTO
        //                    {
        //                        DessertId = row.DessertId,
        //                        DessertName = row.DessertName,
        //                        UnitPrice = row.UnitPrice,
        //                        DessertImageName = row.DessertImageName,
        //                        DiscountGroupId = row.DiscountGroupId,
        //                        //Specification = new Specification
        //                        //{
        //                            SpecificationId = row.SpecificationId,
        //                            //UnitPrice = row.UnitPrice,
        //                            Flavor = row.Flavor,
        //                            Size = row.Size
        //                        //}
        //                    };
        //                    //foreach迴圈找完相對應的結果，放在剛剛創建的DessertDiscountDTO，把這個物件內容加到dessertDiscountList裡面
        //                    dessertDiscountList.Add(dessertDiscountDTO);
        //                }
        //            }
        //            //返回剛剛迴圈找出的所有結果
        //            return dessertDiscountList;

        //        }
        public async Task<List<DessertDiscountDTO>> GetTopSales()
        {
            var dessertDiscountList = new List<DessertDiscountDTO>();

            // 取得名為Sierra的連接字符串(用於連接到數據庫)
            var connectionString = _configuration.GetConnectionString("Sierra");

            //建立數據庫連接
            using (var connection = new SqlConnection(connectionString))
            {
                // 從SQL 查詢裡面使用SQL Query的語法查找出資料
                string sqlQuery = $@"             
SELECT Top 6
    D.DessertId,
    MAX(D.DessertName) AS DessertName,
    MAX(S.Flavor) AS Flavor,
    MAX(S.Size) AS Size,
    MAX(S.UnitPrice) AS UnitPrice,
    MAX(DGI.DiscountGroupId) AS DiscountGroupId,
    SUM(DOD.Quantity) AS TotalQuantity,
    MAX(DI.DessertImageName) AS DessertImageName,
    MAX(S2.SpecificationId) AS SpecificationId
FROM
    DiscountGroupItems DGI
INNER JOIN
    Desserts D ON DGI.DessertId = D.DessertId
LEFT JOIN
    DessertImages DI ON DI.DessertId = D.DessertId
LEFT JOIN
    Specification S ON D.DessertId = S.DessertId
LEFT JOIN
    Specification S2 ON D.DessertId = S2.DessertId
LEFT JOIN
    DessertOrderDetails DOD ON D.DessertId = DOD.DessertId
	WHERE DGI.DiscountGroupId <6
GROUP BY D.DessertId, DGI.DiscountGroupId
ORDER BY TotalQuantity DESC;;
";

                await connection.OpenAsync();

                //這裡QueryAsync使用Dapper的方法來非同步執行SQL查詢。
                //這裡為了防止SQL注入，使用參數化@DiscountGroupId 對應到 discountGroupId傳遞給方法的參數。
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
                        Flavor = row.Flavor,
                        Size = row.Size,
                    };

                    dessertDiscountList.Add(dessertDiscountDTO);
                }
            }
            //返回剛剛迴圈找出的所有結果
            return dessertDiscountList;
        }
    }
}
