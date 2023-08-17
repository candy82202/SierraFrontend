using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIERRA_Server.Models.DTOs.Members;
using SIERRA_Server.Models.EFModels;
using SIERRA_Server.Models.Interfaces;
using SIERRA_Server.Models.Services;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SIERRA_Server.Models.Repository.EFRepository;
using SIERRA_Server.Models.Infra;
using Microsoft.AspNetCore.Authorization;
using System.Xml.Serialization;
using Google.Apis.Auth;
using System.Net;
using System.Text.Json.Nodes;
using System.Reflection;

namespace SIERRA_Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MembersController : ControllerBase
	{
		private readonly AppDbContext _context;
		private readonly MemberEFRepository _repo;
		private readonly IConfiguration _config;
		private readonly HashUtility _hashUtility;
		private readonly EmailHelper _emailHelper;

		public MembersController(AppDbContext context, MemberEFRepository repo, IConfiguration config, HashUtility hashUtility, EmailHelper emailHelper)
		{
			_context = context;
			_repo = repo;
			_config = config;
			_hashUtility = hashUtility;
			_emailHelper = emailHelper;

		}

		[HttpPost("Login")]
		[AllowAnonymous]
		public IActionResult Login(LoginDTO request)
		{
			var service = new MemberService(_repo, _hashUtility, _config);

			// 驗證帳密
			var result = service.ValidLogin(request);

			// 驗證失敗
			if (result.IsFail)
			{
				return BadRequest(result.ErrorMessage);
			}

			var token = service.CreateJwtToken(request.Username);
			return Ok(token);
		}

		[HttpPost("Register")]
		[AllowAnonymous]
		public IActionResult Register(RegisterDTO request)
		{
			var service = new MemberService(_repo, _hashUtility, _emailHelper);
			var result = service.Register(request);

			if (result.IsFail)
			{
				return BadRequest(result.ErrorMessage);
			}

			return Ok("註冊完成,請至信箱收取驗證信");
		}

		[HttpPost("ActiveRegister")]
		[AllowAnonymous]
		public IActionResult ActiveRegister(ActiveRegisterDTO request)
		{
			var service = new MemberService(_repo);
			var result = service.ActiveRegister(request);

			if (result.IsFail)
			{
				return BadRequest(result.ErrorMessage);
			}

			return Ok("驗證成功");
		}

		[HttpPost("ForgotPassword")]
		[AllowAnonymous]
		public IActionResult ForgotPassword(ForgotPasswordDTO request)
		{
			var service = new MemberService(_repo, _hashUtility, _emailHelper);
			var result = service.ProccessResetPassword(request);
			if (result.IsFail)
			{
				return BadRequest(result.ErrorMessage);
			}

			return Ok("已寄發信件，請至信箱收取驗證信");

		}

		[HttpPost("ResetPassword")]
		[AllowAnonymous]
		public IActionResult ResetPassword(ResetPasswordDTO request)
		{
			var service = new MemberService(_repo, _hashUtility);
			var result = service.ProccessChangePassword(request);
			if (result.IsFail)
			{
				return BadRequest(result.ErrorMessage);
			}

			return Ok("已更新您重設的密碼, 以後請用新密碼登入");
		}

		[HttpPut("EditPassword")]
		public IActionResult EditPassword(EditPasswordDTO request)
		{
			var service = new MemberService(_repo, _hashUtility);
			var result = service.EditPassword(request);
			if (result.IsFail)
			{
				return BadRequest(result.ErrorMessage);
			}

			return Ok("修改密碼成功");
		}

		[HttpPost("GoogleLogin")]
		[AllowAnonymous]
		public IActionResult GoogleLogin(string email)
		{
			if (_repo.IsEmailExist(email) == false)
			{
				return BadRequest("請填入使用者名稱和密碼後，成為Sierra會員");
			}

			var memberInDb = _repo.GetMemberByEmail(email);
			var service = new MemberService(_repo, _config);
			var token = service.CreateJwtToken(memberInDb.Username);
			return Ok(token);
		}

		[HttpGet("GetMember")]
		public async Task<ActionResult<EditMemberDTO>> GetMember(int id)
		{
			var service = new MemberService(_repo);
			var member = await service.GetMember(id);
			if (member == null)
			{
				return BadRequest("找不到該用戶");
			}
			return Ok(member);
		}

		[HttpPut("EditMember")]
		public async Task<ActionResult<EditMemberDTO>> EditMember(EditMemberDTO request)
		{
			var service = new MemberService(_repo);
			var result = await service.EditMemberAsync(request);
			if (result.IsFail)
			{
				return BadRequest(result.ErrorMessage);
			}
			return Ok("修改資料成功");
		}


		
		[HttpPost("GoogleRegister")]
		[AllowAnonymous]
		public IActionResult GoogleRegister(RegisterDTO request)
		{
			var service = new MemberService(_repo, _hashUtility, _config);
			var result = service.GoogleRegister(request);
			if (result.IsFail)
			{
				return BadRequest(result.ErrorMessage);
			}
			var token = service.CreateJwtToken(request.Username);
			return Ok(token);
		}
		/*若Google登入，在前端是使用HTML API再看
		//[HttpPost("ValidGoogleLogin")]
		//[AllowAnonymous]
		//public IActionResult ValidGoogleLogin()
		//{

		//	string? formCredential = Request.Form["credential"]; //回傳憑證
		//	string? formToken = Request.Form["g_csrf_token"]; //回傳令牌
		//	string? cookiesToken = Request.Cookies["g_csrf_token"]; //Cookie 令牌

		//	//// 驗證 Google Token
		//	GoogleJsonWebSignature.Payload? payload = VerifyGoogleToken(formCredential, formToken, cookiesToken).Result;
		//	if (payload == null)
		//	{
		//		// 驗證失敗
		//		return BadRequest("驗證 Google 授權失敗");
		//	}
		//	else
		//	{
		//		// 驗證成功，構造回傳的物件
		//		var result = new
		//		{
		//			Msg = "驗證 Google 授權成功",
		//			Email = payload.Email,
		//			Name = payload.Name,
		//			Picture = payload.Picture
		//		};
		//		return Ok(result);
		//		//return Redirect("http://localhost:5501/index.html");
		//	}
		//}
		//private async Task<GoogleJsonWebSignature.Payload?> VerifyGoogleToken(string? formCredential, string? formToken, string? cookiesToken)
		//{
		//	// 檢查空值
		//	if (formCredential == null || formToken == null && cookiesToken == null)
		//	{
		//		return null;
		//	}

		//	GoogleJsonWebSignature.Payload? payload;
		//	try
		//	{
		//		// 驗證 token
		//		if (formToken != cookiesToken)
		//		{
		//			return null;
		//		}

		//		// 驗證憑證
		//		//IConfiguration Config = new ConfigurationBuilder().AddJsonFile("appSettings.json").Build();
		//		string GoogleApiClientId = _config["GoogleAuthentication:ClientId"];
		//		var settings = new GoogleJsonWebSignature.ValidationSettings()
		//		{
		//			Audience = new List<string>() { GoogleApiClientId }
		//		};
		//		payload = await GoogleJsonWebSignature.ValidateAsync(formCredential, settings);
		//		if (!payload.Issuer.Equals("accounts.google.com") && !payload.Issuer.Equals("https://accounts.google.com"))
		//		{
		//			return null;
		//		}
		//		if (payload.ExpirationTimeSeconds == null)
		//		{
		//			return null;
		//		}
		//		else
		//		{
		//			DateTime now = DateTime.Now.ToUniversalTime();
		//			DateTime expiration = DateTimeOffset.FromUnixTimeSeconds((long)payload.ExpirationTimeSeconds).DateTime;
		//			if (now > expiration)
		//			{
		//				return null;
		//			}
		//		}
		//	}
		//	catch
		//	{
		//		return null;
		//	}
		//	return payload;
		//}
		*/



		//// 以下是精靈生成的
		//// GET: api/Members
		//[HttpGet]
		//public async Task<ActionResult<IEnumerable<Member>>> GetMembers()
		//{
		//    if (_context.Members == null)
		//    {
		//        return NotFound();
		//    }
		//    return await _context.Members.ToListAsync();
		//}

		//// GET: api/Members/5
		//[HttpGet("{id}")]
		//public async Task<ActionResult<Member>> GetMember(int id)
		//{
		//    if (_context.Members == null)
		//    {
		//        return NotFound();
		//    }
		//    var member = await _context.Members.FindAsync(id);

		//    if (member == null)
		//    {
		//        return NotFound();
		//    }

		//    return member;
		//}

		//// PUT: api/Members/5
		//// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		//[HttpPut("{id}")]
		//public async Task<IActionResult> PutMember(int id, Member member)
		//{
		//    if (id != member.Id)
		//    {
		//        return BadRequest();
		//    }

		//    _context.Entry(member).State = EntityState.Modified;

		//    try
		//    {
		//        await _context.SaveChangesAsync();
		//    }
		//    catch (DbUpdateConcurrencyException)
		//    {
		//        if (!MemberExists(id))
		//        {
		//            return NotFound();
		//        }
		//        else
		//        {
		//            throw;
		//        }
		//    }

		//    return NoContent();
		//}

		//// POST: api/Members
		//// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		//[HttpPost]
		//public async Task<ActionResult<Member>> PostMember(Member member)
		//{
		//    if (_context.Members == null)
		//    {
		//        return Problem("Entity set 'AppDbContext.Members'  is null.");
		//    }
		//    _context.Members.Add(member);
		//    await _context.SaveChangesAsync();

		//    return CreatedAtAction("GetMember", new { id = member.Id }, member);
		//}

		//// DELETE: api/Members/5
		//[HttpDelete("{id}")]
		//public async Task<IActionResult> DeleteMember(int id)
		//{
		//    if (_context.Members == null)
		//    {
		//        return NotFound();
		//    }
		//    var member = await _context.Members.FindAsync(id);
		//    if (member == null)
		//    {
		//        return NotFound();
		//    }

		//    _context.Members.Remove(member);
		//    await _context.SaveChangesAsync();

		//    return NoContent();
		//}

		//private bool MemberExists(int id)
		//{
		//    return (_context.Members?.Any(e => e.Id == id)).GetValueOrDefault();
		//}
	}
}
