using CommonLayer.Model;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.interfaces
{
  public  interface IUserBL
    {
        public UserEntity UserRegistration(Registration registration);
        public string LoginUser(LoginModel loginModel);
        public string ForgetPassword(string emailId);
         public ResponseModel ResetPassword(ResetPasswordModel model, string email);
       // public ResponseModel ResetPassword(string email, string password, string confirmPassword); 

        }
}
