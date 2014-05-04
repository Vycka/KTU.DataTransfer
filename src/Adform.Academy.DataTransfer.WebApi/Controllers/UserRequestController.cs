﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using Adform.Academy.DataTransfer.Core.DTO.Models;
using Adform.Academy.DataTransfer.Core.DTO.NHibernate;
using Adform.Academy.DataTransfer.Logger.Events;
using Adform.Academy.DataTransfer.WebApi.Contracts.Users;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;

namespace Adform.Academy.DataTransfer.WebApi.Controllers
{
    [RoutePrefix("Adform.Academy.DataTransfer/v1/Users")]
    public class UserRequestController : ControllerBase
    {
        [Route("Get")]
        [HttpGet, HttpPost]
        public GetUserResponse Get(GetUserRequest request)
        {
            ISession session = SessionFactory.GetSession();
            
            var user = session.Get<User>(request.UserId);

            if (user == null)
            {
                return new GetUserResponse
                {
                    IsActive = false,
                    Success = false,
                    Message = "User not found"
                };
            }

            return new GetUserResponse
            {
                UserId = user.UserId,
                IsActive = user.IsActive,
                IsAdmin = user.IsAdmin,
                UserName = user.UserName
            };
        }

        [Route("Save")]
        [HttpGet, HttpPost]
        public SaveUserResponse Save(SaveUserRequest request)
        {
            if (CheckIfRemovingLastAdmin(request))
            {
                return new SaveUserResponse
                {
                    Success = false,
                    Message = "Cannot disable or remove administrator rights for last administrator user"
                };
            }

            var existingUserByName = GetUserByName(request.UserName);
            if (existingUserByName != null)
            {
                if (existingUserByName.UserId != request.UserId)
                    return new SaveUserResponse
                    {
                        Success = false,
                        Message = "Another user with that name already exists"
                    };
            }
            else
            {
                Logger.Log(new UserCreatedEvent(request.InvokerUserId, request.UserName));
            }
            

            ISession session = SessionFactory.GetSession();
            
            //TODO: Do Some Logging
            var user = new User
            {
                UserId = request.UserId,
                IsActive = request.IsActive,
                IsAdmin = request.IsAdmin,
                UserName = request.UserName
            };

            if (request.UserId == 0 || !String.IsNullOrEmpty(request.Password))
            {
                user.Password = ComputeSha256(request.Password);
            }
            else
            {
                var existingUser = session.Get<User>(request.UserId);

                Logger.Log(new UserModifiedEvent(existingUser, request.UserName, request.InvokerUserId));

                user.Password = existingUser.Password;
            }
            session.Merge(user);
            session.Flush();

            return new SaveUserResponse();
            
        }

        [Route("GetUserList")]
        [HttpGet, HttpPost]
        public GetUserListResponse GetUserList(GetUserRequest request)
        {
           ISession session = SessionFactory.GetSession();

            IList<UserListItem> userList = session.CreateCriteria(typeof(User)).SetProjection(Projections.ProjectionList()
                .Add(Projections.Property("UserId"), "UserId")
                .Add(Projections.Property("UserName"), "UserName")
                .Add(Projections.Property("IsActive"), "IsActive")
                .Add(Projections.Property("IsAdmin"), "IsAdmin"))
                .SetResultTransformer(Transformers.AliasToBean<UserListItem>())
                .List<UserListItem>();

            return new GetUserListResponse
            {
                Users = userList.ToList()
            };
        }

        [Route("CheckLogin")]
        [HttpGet, HttpPost]
        public CheckLoginResponse CheckLogin(CheckLoginRequest request)
        {
            var user = GetUserByName(request.UserName);

            if (user == null)
            {
                return new CheckLoginResponse
                {
                    Success = false
                };
            }


            var response = new CheckLoginResponse();
            response.Success = user.IsActive && ComputeSha256(request.Password) == user.Password;
            if (response.Success)
            {
                response.UserId = user.UserId;
                response.UserName = user.UserName;
                response.IsActive = user.IsActive;
                response.IsAdmin = user.IsAdmin;
            }

            return response;
        }

        private string ComputeSha256(string input)
        {
            SHA256 hasher = SHA256.Create();
            byte[] hashedResult = hasher.ComputeHash(Encoding.UTF8.GetBytes(input));

            var sBuilder = new StringBuilder();
            for (int i = 0; i < hashedResult.Length; i++)
            {
                sBuilder.Append(hashedResult[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        public User GetUserByName(string name)
        {
            ISession session = SessionFactory.GetSession();
            
            var user = session.CreateCriteria(typeof(User))
                .Add(Restrictions.Eq("UserName", name))
                .UniqueResult<User>();

            return user;
        }

        public bool CheckIfRemovingLastAdmin(SaveUserRequest request)
        {
            if (request.UserId == 0)
                return false; // Adding new user.. Whatever

            ISession session = SessionFactory.GetSession();

            // If editing user was admin but after editing its not
            var user = session.Get<User>(request.UserId);
            if (user.IsAdmin && user.IsActive)
            {
                if (!request.IsAdmin || !request.IsActive)
                {
                    int activeAdminsCount = (int)session.CreateCriteria(typeof (User))
                        .Add(Restrictions.Eq("IsAdmin", true))
                        .Add(Restrictions.Eq("IsActive", true))
                        .SetProjection(Projections.CountDistinct("UserId"))
                        .UniqueResult();

                    if (activeAdminsCount <= 1)
                        return true;
                }
            }
            return false;
        }
    }
}
