using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Contrats.UnitOfWorks;
using LocationSystem.Application.Exceptions;
using LocationSystem.Application.Utilities;
using LocationSystem.Application.Utilities.Jwt;
using LocationSystem.Domain.Entities;

namespace LocationSystem.Application.Features.Auth.Login.Commands
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IUnitOfWork _unitOfWork;

        public LoginCommandHandler(IUserRepository userRepository, IJwtService jwtService, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _unitOfWork = unitOfWork;
        }

        public async Task<LoginResponseDto> Handle(LoginCommand request)
        {
            var loginRequest = request.Request;
            await _unitOfWork.BeginTransactionAsync();
            try
            {


                // 查询用户
                User? user = await _userRepository.GetUserByEmailAsync(loginRequest.Email);
                if (user == null)
                {
                    throw new NotFoundException("用户不存在");
                }

                // 检查用户是否被禁用
                if (user.IsDisabled)
                {
                    throw new ApplicationCustomException("账号已被禁用", 403);
                }

                // 验证密码
                if (!BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.PasswordHash))
                {
                    throw new ApplicationCustomException("密码错误", 401);
                }

                // 生成token
                var accessToken = _jwtService.GenerateAccessToken(user);
                var refreshToken = _jwtService.GenerateRefreshToken(user.Id);
                // 保存refresh token到数据库
                await _userRepository.SaveRefreshToken(user.Id, refreshToken);
                await _unitOfWork.CommitAsync();
                // 构建响应
                var response = new LoginResponseDto
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    UserInfo = new UserInfoDto
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Email = user.Email.Value,
                        UserType = user.UserType.ToString()
                    }
                };

                return response;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}