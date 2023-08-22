
using Dapper;
using Microsoft.Data.SqlClient;
using SIERRA_Server.Models.DTOs.Members;
using SIERRA_Server.Models.EFModels;
using SIERRA_Server.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace SIERRA_Server.Models.Repository.DPRepository
{
	public class MemberDPRepository : IMemberRepository
	{
		private readonly AppDbContext _db;
		private readonly IConfiguration _config;
		private readonly string _connStr;
		public MemberDPRepository(AppDbContext db, IConfiguration config)
		{
			_db = db;
			_config = config;
			_connStr = _config.GetConnectionString("Sierra");
		}
		public Member GetMemberByEmail(string email)
		{
			using var conn = new SqlConnection(_connStr);
			var query = "SELECT * FROM Members WHERE Email = @Email";
			return conn.QueryFirstOrDefault<Member>(query, new { Email = email });

		}

		public Member GetMemberById(int memberId)
		{
			using var conn = new SqlConnection(_connStr);
			var query = "SELECT * FROM Members WHERE Id = @Id";
			return conn.QueryFirstOrDefault<Member>(query, new { Id = memberId });
		}

		public async Task<Member> GetMemberByIdAsync(int memberId)
		{
			using var conn = new SqlConnection(_connStr);
			await conn.OpenAsync();
			string query = "SELECT * FROM Members WHERE Id = @Id";
			return await conn.QueryFirstOrDefaultAsync<Member>(query, new { Id = memberId });
		}

		public Member GetMemberByUsername(string username)
		{
			using var conn = new SqlConnection(_connStr);
			var query = "SELECT * FROM Members WHERE Username = @Username";
			return conn.QueryFirstOrDefault<Member>(query, new { Username = username });
		}

		public int GetMemberIdByUsername(string username)
		{
			// try "ExecuteScalar" , may occur errors
			using var conn = new SqlConnection(_connStr);
			string query = "SELECT Id FROM Members WHERE Username = @Username";
			return conn.ExecuteScalar<int?>(query, new { Username = username }) ?? -1;
		}

		public bool IsAccountExist(string username)
		{
			using var conn = new SqlConnection(_connStr);
			string query = "SELECT TOP 1 1 FROM Members WHERE Username=@Username";
			return conn.ExecuteScalar<bool>(query, new { Username = username });
		}

		public bool IsAccountExist(int memberId)
		{
			using var conn = new SqlConnection(_connStr);
			string query = "SELECT TOP 1 1 FROM Members WHERE Id=@Id";
			return conn.ExecuteScalar<bool>(query, new { Id = memberId });
		}

		public bool IsEmailExist(string email)
		{
			using var conn = new SqlConnection(_connStr);
			string query = "SELECT TOP 1 1 FROM Members WHERE Email=@Email";
			return conn.ExecuteScalar<bool>(query, new { Email = email });
		}

		public void PostMember(RegisterDTO dto)
		{
			using var conn = new SqlConnection(_connStr);

			string query = @"
            INSERT INTO Members (Username, Email, EncryptedPassword, ImageName, IsConfirmed, ConfirmCode)
            VALUES (@Username, @Email, @EncryptedPassword, @ImageName, @IsConfirmed, @ConfirmCode)";

			var parameters = new
			{
				dto.Username,
				dto.Email,
				dto.EncryptedPassword,
				dto.ImageName,
				dto.IsConfirmed,
				dto.ConfirmCode
			};

			conn.Execute(query, parameters);
		}

		public void ActiveRegister(Member member)
		{
			using var conn = new SqlConnection(_connStr);

			string query = @"
UPDATE Members 
SET IsConfirmed = @IsConfirmed, ConfirmCode = @ConfirmCode
WHERE Id = @Id
";

			// 參數名稱如果跟資料庫欄位名稱相同，可把等號左邊部分去掉
			var parameters = new
			{
				IsConfirmed = member.IsConfirmed,
				ConfirmCode = member.ConfirmCode,
				Id = member.Id
			};

			conn.Execute(query, parameters);
		}

		public void ForgotPassword(Member member)
		{
			using var conn = new SqlConnection(_connStr);

			string query = @"
UPDATE Members 
SET ConfirmCode = @ConfirmCode
WHERE Id = @Id
";
			
			var parameters = new
			{
				member.ConfirmCode,
				member.Id
			};

			conn.Execute(query, parameters);
		}
		public void ResetPassword(Member member)
		{
			using var conn = new SqlConnection(_connStr);

			string query = @"
UPDATE Members 
SET EncryptedPassword = @EncryptedPassword, ConfirmCode = @ConfirmCode
WHERE Id = @Id
";
			
			var parameters = new
			{
				member.EncryptedPassword,
				member.ConfirmCode,
				member.Id
			};
			conn.Execute(query, parameters);
		}

		public void EditPassword(Member member)
		{
			using var conn = new SqlConnection(_connStr);

			string query = @"
UPDATE Members 
SET EncryptedPassword = @EncryptedPassword
WHERE Id = @Id
";
			
			var parameters = new
			{
				member.EncryptedPassword,
				member.Id
			};
			conn.Execute(query, parameters);
		}

		public async Task EditMemberAsync(Member member)
		{
			using var conn = new SqlConnection(_connStr);

			string query = @"
UPDATE Members 
SET Address = @Address, Phone = @Phone, Birth = @Birth, Gender = @Gender
WHERE Id = @Id
";
			
			var parameters = new
			{
				member.Address,
				member.Phone,
				member.Birth,
				member.Gender,
				member.Id
			};
			await conn.ExecuteAsync(query, parameters);
		}

		public async Task EditMemberImageAsync(Member member)
		{
			using var conn = new SqlConnection(_connStr);

			string query = @"
UPDATE Members 
SET ImageName = @ImageName
WHERE Id = @Id
";
			
			var parameters = new
			{
				member.ImageName,
				member.Id
			};
			await conn.ExecuteAsync(query, parameters);
		}

	}
}
