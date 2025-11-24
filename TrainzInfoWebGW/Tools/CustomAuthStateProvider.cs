using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace TrainzInfoWebGW.Tools
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        private ClaimsPrincipal _user;
        private readonly ILocalStorageService _localStorage;

        public CustomAuthStateProvider(ILocalStorageService localStorage)
        {
            _user = _anonymous;
            _localStorage = localStorage;
        }

        public void MarkUserAsAuthenticated(string email)
        {
            var identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, email)
                }, "apiauth");

            _user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_user)));
        }

        public void MarkUserAsLoggedOut()
        {
            _user = _anonymous;
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_user)));
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // Завжди повертаємо не null
            return Task.FromResult(new AuthenticationState(_user));
        }

        public async Task CheckAuthenticationAsync()
        {
            // читаємо токен
            var token = await _localStorage.GetItemAsync<string>("authToken");

            if (!string.IsNullOrEmpty(token))
            {
                var identity = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.Name, "UserFromServer") // можна розпарсити JWT
        }, "apiauth");

                _user = new ClaimsPrincipal(identity);
            }
            else
            {
                _user = new ClaimsPrincipal(new ClaimsIdentity());
            }

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_user)));
        }

    }
}
