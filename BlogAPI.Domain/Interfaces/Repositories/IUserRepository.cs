﻿using BlogApi.Domain.Entities;

namespace BlogApi.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User> GetByIdAsync(Guid id);
    Task<User> GetByEmailAsync(string email);
    Task<User> AddAsync(User user);
}