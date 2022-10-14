using CommonLayer.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace RepositoryLayer.Services
{
    public class UserRL : IUserRL
    {
        private readonly FundooDBContext fundooDBContext;
        public IConfiguration configuration { get; }
        public ResponseModel responseModel;

        public UserRL(FundooDBContext fundooDBContext, IConfiguration configuration, ResponseModel responseModel)
        {
            this.fundooDBContext = fundooDBContext;
            this.configuration = configuration;
            this.responseModel = responseModel;
        }

        public UserEntity UserRegistration(Registration registration)
        {
            try
            {
                UserEntity userEntity = new UserEntity();
                userEntity.firstName = registration.firstName;
                userEntity.lastName = registration.lastName;
                userEntity.email = registration.email;
                userEntity.password = EncryptPassword(registration.password);//calling EncryptPassword() method
                fundooDBContext.UserTable.Add(userEntity);
                int result = fundooDBContext.SaveChanges();
                if (result > 0)
                {
                    return userEntity;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string LoginUser(LoginModel loginModel)
        {
          //  string deccriptedPassword = DecryptPassword(password);
            try
            {
                string pass = EncryptPassword(loginModel.Password);
                var result = fundooDBContext.UserTable.Where(u => u.email == loginModel.Email && u.password == pass).FirstOrDefault();
                if (result != null)
                {
                    return GetJWTToken(loginModel.Email, result.userID);
                }  
                return null;
            }
            catch (Exception ex)
            {
                return null;
               // throw ex;
            }
        }
        private string GetJWTToken(string email, int userID)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(configuration["key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("email", email),
                    new Claim("userID",userID.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(3),

                SigningCredentials =
                new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public string ForgetPassword(string emailId)
        {
            try
            {
                var emailCheck = fundooDBContext.UserTable.FirstOrDefault(e => e.email == emailId);
                if (emailCheck != null)
                {
                    var token = GetJWTToken(emailCheck.email, emailCheck.userID);
                    MSMQModel mSMQModel = new MSMQModel();
                    mSMQModel.sendData2Queue(token);
                    return token.ToString();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public ResponseModel ResetPassword(ResetPasswordModel model, string email)
        {
            // ResponseModel responseModel = new ResponseModel();

            try
            {
                if (model.password.Equals(model.confirmPassword))
                {
                    UserEntity user = fundooDBContext.UserTable.Where(e => e.email == email).FirstOrDefault();
                    if (user != null)
                    {
                        user.password = model.confirmPassword;
                        int result = fundooDBContext.SaveChanges(); //return no of changes entries in database
                        if (result != 0)
                        {
                            responseModel.ResponseCode = "200";
                            responseModel.ResponseMassage = "Reset passord successfully";
                        }
                        else
                        {
                            responseModel.ResponseCode = "401";
                            responseModel.ResponseMassage = "Reset Password not change";
                        }
                    }
                    else
                    {
                        responseModel.ResponseCode = "402";
                        responseModel.ResponseMassage = "Please enter valid Email";
                    }
                }
                else
                {
                    responseModel.ResponseCode = "403";
                    responseModel.ResponseMassage = "Please check password and confirmpassword ";
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return responseModel;
        }
        /// <summary>
        /// this method converts the user given password into encoded format.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public string EncryptPassword(string password)
        {
            try
            {
                byte[] encode = new byte[password.Length];
                encode = Encoding.UTF8.GetBytes(password);
                string encPassword = Convert.ToBase64String(encode);
                return encPassword;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="encryptpwd"></param>
        /// <returns></returns>
        public string DecryptPassword(string encryptpwd)
        {
            try
            {
                UTF8Encoding encodepwd = new UTF8Encoding();
                Decoder Decode = encodepwd.GetDecoder();
                byte[] todecode_byte = Convert.FromBase64String(encryptpwd);
                int charCount = Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
                char[] decoded_char = new char[charCount];
                Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
                string decryptpwd = new String(decoded_char);
                return decryptpwd;
            }
            catch (Exception)
            {

                throw;
            }
        }


    }
}
