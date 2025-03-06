using System.ComponentModel.DataAnnotations;
using Application.Abstractions.MediatR;
using Domain.Abstractions.RepositoryInterfaces;
using Domain.Entities;
using Domain.Exceptions;
using Infrastructure.Utilities;

namespace Application.Features.Users.Commands.RegisterUser;
public class RegisterUserCommandHandler(IUserRepository userRepository) : ICommandHandler<RegisterUserCommand, RegisterUserResponse>
{
    public async Task<RegisterUserResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (await userRepository.ExistsAsync(u => u.Email == request.Email))
        {
            throw new DuplicateValueException("User already exists");
        }

        var user = User.Create(
            request.NickName, 
            request.Email, 
            request.FirstName, 
            request.LastName, 
            PasswordHasher.Hash(request.Password));

        await userRepository.Add(user);
    }
}
