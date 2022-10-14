using BusinessLayer.interfaces;
using CommonLayer.Model;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class UserBL : IUserBL
    {
        private readonly IUserRL userRL;
        public ResponseModel responseModel;
        public UserBL(IUserRL userRL, ResponseModel responseModel)
        {
            this.userRL = userRL;
            this.responseModel = responseModel;
        }

        public UserEntity UserRegistration(Registration registration)
        {
            try
            {
                return userRL.UserRegistration(registration);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public string LoginUser(LoginModel loginModel)
        {
            try
            {
                return this.userRL.LoginUser(loginModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string ForgetPassword(string emailId)
        {
            try
            {
                return this.userRL.ForgetPassword(emailId);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public ResponseModel ResetPassword(ResetPasswordModel model, string email)
        {
            try
            {
                responseModel = this.userRL.ResetPassword(model, email);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return responseModel;
        }

    }
}
