namespace Dashboard.Utility
{
    public class JwtUtils : IJwtUtils
    {
        private readonly AppSettings _appSettings;
        private readonly byte[] _secret;

        public JwtUtils(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _secret = Encoding.UTF8.GetBytes(_appSettings.Secret);
        }

        public TokenResponse GenerateJwtToken(int userId, int? expires = 60)
        {
            int minutes = expires != null ? (int)expires : 60;
            JwtSecurityTokenHandler tokenHandler = new();
            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(new[] { new Claim("userId", userId.ToString()) }),
                Expires = DateTime.UtcNow.AddMinutes(minutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_secret), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return new TokenResponse(tokenHandler.WriteToken(token), tokenDescriptor.Expires ?? DateTime.UtcNow);
        }

        public int? ValidateJwtToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return null;
            }
            try
            {
                JwtSecurityTokenHandler tokenHandler = new();

                _ = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(_secret),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                JwtSecurityToken jwtToken = (JwtSecurityToken)validatedToken;
                int userId = int.Parse(jwtToken.Claims.First(x => x.Type == "userId").Value);

                // return account id from JWT token if validation successful
                return userId;
            }
            catch
            {
                // return null if validation fails
                return null;
            }
        }

        public RefreshToken GenerateRefreshToken(string ipAddress)
        {
            byte[] randomBytes = RandomGenerator.GenerateBytes(64);

            RefreshToken refreshToken = new()
            {
                Token = Convert.ToBase64String(randomBytes),
                Expires = DateTime.UtcNow.AddDays(_appSettings.RefreshTokenTTL),
                CreatedAt = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };

            return refreshToken;
        }
    }
}
