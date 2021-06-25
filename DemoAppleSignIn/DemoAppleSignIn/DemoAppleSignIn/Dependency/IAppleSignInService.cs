using System.Threading.Tasks;
using DemoAppleSignIn.Model;

namespace DemoAppleSignIn.Dependency
{
    public interface IAppleSignInService
    {
        bool IsAvailable { get; }

        Task<AppleSignInCredentialState> GetCredentialStateAsync(string userId);

        Task<AppleAccount> SignInAsync();
    }
}
