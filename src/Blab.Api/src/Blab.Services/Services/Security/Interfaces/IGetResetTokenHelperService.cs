namespace Blab.Services.Services.Security.Interfaces;

public interface IGetResetTokenHelperService
{
    public Task<string> ExecuteAsync(int id, string role);
}
