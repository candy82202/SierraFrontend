
using Dapper;
using Microsoft.Data.SqlClient;
using SIERRA_Server.Models.DTOs.Members;
using SIERRA_Server.Models.EFModels;
using SIERRA_Server.Models.Interfaces;
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
			string query = "SELECT * FROM Members WHERE Id = Id";
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
			throw new NotImplementedException();
		}

		public bool IsAccountExist(int memberId)
		{
			throw new NotImplementedException();
		}

		public bool IsEmailExist(string email)
		{
			throw new NotImplementedException();
		}

		public void PostMember(RegisterDTO dto)
		{
			throw new NotImplementedException();
		}

		public void SaveChanges()
		{
			throw new NotImplementedException();
		}

		public Task SaveChangesAsync()
		{
			throw new NotImplementedException();
		}
	}
}
