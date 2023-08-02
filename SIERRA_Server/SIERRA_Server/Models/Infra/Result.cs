namespace SIERRA_Server.Models.Infra
{
	public class Result
	{
		public bool ISuccess { get; set; }
		public bool IsFail => !ISuccess;
		public string? ErrorMessage { get; set; }

		public static Result Success()
			=> new Result() { ISuccess = true, ErrorMessage = null };
		public static Result Fail(string errorMessage)
			=> new Result() { ISuccess = false, ErrorMessage = errorMessage };


	}
}
