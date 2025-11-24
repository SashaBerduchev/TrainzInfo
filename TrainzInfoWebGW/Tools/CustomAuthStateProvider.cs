using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using TrainzInfoWebGW.Tools.DTO;
using static System.Net.WebRequestMethods;

namespace TrainzInfoWebGW.Tools
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        private ClaimsPrincipal _user;
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorage;

        public CustomAuthStateProvider(ILocalStorageService localStorage, HttpClient http)
        {
            _user = _anonymous;
            _localStorage = localStorage;
            _http = http;
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
            var token = await _localStorage.GetItemAsync<string>("authToken");

            if (!string.IsNullOrEmpty(token))
            {
                // Додаємо токен у заголовок
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                try
                {
                    var response = await _http.GetAsync("api/auth/getauthuser");

                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var userDto = JsonSerializer.Deserialize<UserDto>(json, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        var identity = new ClaimsIdentity(new[]
                        {
                        new Claim(ClaimTypes.Name, userDto.Email),
                        new Claim(ClaimTypes.Role, userDto.Role)
                    }, "apiauth");

                        _user = new ClaimsPrincipal(identity);
                    }
                    else
                    {
                        _user = new ClaimsPrincipal(new ClaimsIdentity());
                    }
                }
                catch
                {
                    _user = new ClaimsPrincipal(new ClaimsIdentity());
                }
            }
            else
            {
                _user = new ClaimsPrincipal(new ClaimsIdentity());
            }

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_user)));
        }

    }
}
