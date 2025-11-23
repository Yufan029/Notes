using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CheckServerMemory.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class LoginController
    {
        [HttpPost]
        public LoginResponse Login(LoginRequest request)
        {
            if (request.UserName == "admin" && request.Password == "123456")
            {
                var processInfos = Process.GetProcesses().Select(x => new ProcessInfo(x.Id, x.ProcessName, x.WorkingSet64)).ToArray();
                return new LoginResponse(true, processInfos);
            }
            else
            {
                return new LoginResponse(false, null);
            }
        }
    }

    public record LoginRequest(string UserName, string Password);
    public record ProcessInfo(int Id, string Name, long workingSet64);
    public record LoginResponse(bool Success, ProcessInfo[]? ProcessInfos);
}
