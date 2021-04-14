﻿using Dapper;
using DataAccess.Repositories.Interfaces;
using Domain.DTOs;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        public string ConnectionString { get; }

        public UserRepository(IConfiguration configuration)
        {
            this.ConnectionString = configuration.GetConnectionString("IdentityServer");
        }

        public async Task<IEnumerable<UserDto>> GetUsersAsync(IEnumerable<Guid> userIds)
        {
            const string GET_USERS_SQL = @"SELECT ID, USERNAME, DISPLAYNAME FROM ASPNETUSERS WHERE ID IN @userIds";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                var userDtos = await connection.QueryAsync<UserDto>(GET_USERS_SQL, new { userIds = userIds });
                return userDtos;
            }
        }

        public async Task<bool> DoesUserExistAsync(Guid userId)
        {
            const string USER_EXISTS_SQL = @"SELECT 1 FROM ASPNETUSERS WHERE ID=@userId";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //queryfirst throws exception if not exists
                try
                {
                    await connection.QueryFirstAsync<int>(USER_EXISTS_SQL, new { userId = userId });
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        public async Task<bool> DoesUserExistAsync(string username)
        {
            const string USER_EXISTS_SQL = @"SELECT 1 FROM ASPNETUSERS WHERE UserName=@username";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //queryfirst throws exception if not exists
                try
                {
                    await connection.QueryFirstAsync<int>(USER_EXISTS_SQL, new { username = username });
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public async Task<UserDto> GetUserAsync(Guid userId)
        {
            const string GET_USER_SQL = @"SELECT ID, USERNAME, DISPLAYNAME FROM ASPNETUSERS WHERE ID=@userId";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                var sasdf = await connection.QueryFirstAsync(GET_USER_SQL, new { userId = userId });
                var userDto = await connection.QueryFirstAsync<UserDto>(GET_USER_SQL, new { userId = userId });
                return userDto;
            }
        }

        public async Task<UserDto> GetUserAsync(string username)
        {
            const string GET_USER_SQL = @"SELECT ID, USERNAME, DISPLAYNAME FROM ASPNETUSERS WHERE UserName=@username";

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    var userDto = await connection.QueryFirstOrDefaultAsync<UserDto>(GET_USER_SQL, new { username = username });
                    return userDto;
                }
            }
            catch(Exception exc)
            {
                throw;
            }

        }
    }
}
