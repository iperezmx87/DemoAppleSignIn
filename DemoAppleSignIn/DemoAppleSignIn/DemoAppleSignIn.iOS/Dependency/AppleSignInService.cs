using System.Threading.Tasks;
using AuthenticationServices;
using DemoAppleSignIn.Dependency;
using DemoAppleSignIn.iOS.Dependency;
using DemoAppleSignIn.Model;
using Foundation;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(AppleSignInService))]
namespace DemoAppleSignIn.iOS.Dependency
{
    public class AppleSignInService : NSObject, IAppleSignInService, IASAuthorizationControllerDelegate
                                    , IASAuthorizationControllerPresentationContextProviding
    {
        #region IAppleSignInService

        public bool IsAvailable => UIDevice.CurrentDevice.CheckSystemVersion(13, 0);

        TaskCompletionSource<ASAuthorizationAppleIdCredential> tcsCredential;

        public async Task<AppleSignInCredentialState> GetCredentialStateAsync(string userId)
        {
            var appleIdProvider = new ASAuthorizationAppleIdProvider();
            var credentialState = await appleIdProvider.GetCredentialStateAsync(userId);

            switch (credentialState)
            {
                case ASAuthorizationAppleIdProviderCredentialState.Authorized:
                    return AppleSignInCredentialState.Authorized;
                case ASAuthorizationAppleIdProviderCredentialState.Revoked:
                    return AppleSignInCredentialState.Revoked;
                case ASAuthorizationAppleIdProviderCredentialState.NotFound:
                    return AppleSignInCredentialState.NotFound;
                default:
                    return AppleSignInCredentialState.Unknown;
            }
        }


        public async Task<AppleAccount> SignInAsync()
        {
            var appleIdProvider = new ASAuthorizationAppleIdProvider();
            var request = appleIdProvider.CreateRequest();

            request.RequestedScopes = new[] { ASAuthorizationScope.Email, ASAuthorizationScope.FullName };

            var authorizationController = new ASAuthorizationController(new[] { request });
            authorizationController.Delegate = this;
            authorizationController.PresentationContextProvider = this;
            authorizationController.PerformRequests();

            tcsCredential = new TaskCompletionSource<ASAuthorizationAppleIdCredential>();

            var creds = await tcsCredential.Task;

            if (creds == null)
                return null;

            var appleAccount = new AppleAccount
            {
                Email = creds.Email,
                Name = NSPersonNameComponentsFormatter.GetLocalizedString(creds.FullName,
                                                NSPersonNameComponentsFormatterStyle.Default,
                                                NSPersonNameComponentsFormatterOptions.Phonetic),
                RealUserStatus = creds.RealUserStatus.ToString(),
                Token = new NSString(creds.IdentityToken, NSStringEncoding.UTF8).ToString(),
                UserId = creds.User
            };

            return appleAccount;
        }

        #endregion

        #region IASAuthorizationControllerDelegate

        [Export("authorizationController:didCompleteWithAuthorization:")]
        public void DidComplete(ASAuthorizationController controller, ASAuthorization authorization)
        {
            var creds = authorization.GetCredential<ASAuthorizationAppleIdCredential>();
            tcsCredential?.TrySetResult(creds);
        }

        [Export("authorizationController:didCompleteWithError:")]
        public void DidComplete(ASAuthorizationController controller, NSError error)
        {
            tcsCredential?.TrySetResult(null);
            System.Console.WriteLine(error);
        }

        #endregion

        #region IASAuthorizationControllerPresentation Context Providing

        public UIWindow GetPresentationAnchor(ASAuthorizationController controller)
        {
            return UIApplication.SharedApplication.KeyWindow;
        }

        #endregion
    }
}