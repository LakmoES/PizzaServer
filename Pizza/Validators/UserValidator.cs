using Pizza.Models;
using Pizza.Models.DBEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pizza.Validators
{
    public static class UserValidator
    {
        public static bool CheckAdditionUser(User user, out List<Error> errors)
        {
            errors = new List<Error>();
            bool checkFlag = true;

            if (user.username == null)
            {
                errors.Add(new Error { error = "Не указано имя пользователя"});
                checkFlag = false;
            }
            else
                if (user.username.Length < 1)
            {
                errors.Add(new Error { error = "Имя пользователя не может быть пустым" });
                checkFlag = false;
            }

            if (user.password == null)
            {
                errors.Add(new Error { error = "Не указан пароль" });
                checkFlag = false;
            }
            else
                if (user.password.Length < 6)
            {
                errors.Add(new Error { error = "Минимальная длина пароля 6 символов" });
                checkFlag = false;
            }

            var emailChecker = new EmailAddressAttribute();
            if (user.email == null)
            {
                errors.Add(new Error { error = "Не указан email" });
                checkFlag = false;
            }
            else
                if (!emailChecker.IsValid(user.email))
            {
                errors.Add(new Error { error = "Неверный формат email адреса" });
                checkFlag = false;
            }

            if (user.name == null)
            {
                errors.Add(new Error { error = "Не указано имя" });
                checkFlag = false;
            }
            else
                if (user.name.Length < 1)
            {
                errors.Add(new Error { error = "Имя не может быть пустым" });
                checkFlag = false;
            }

            return checkFlag;
        }

        public static bool CheckAdditionGuest(User user, out List<Error> errors)
        {
            errors = new List<Error>();
            bool checkFlag = true;

            if (user.username == null)
            {
                errors.Add(new Error { error = "Не указано имя пользователя" });
                checkFlag = false;
            }
            else
                if (user.username.Length < 1)
            {
                errors.Add(new Error { error = "Имя пользователя не может быть пустым" });
                checkFlag = false;
            }

            if (user.password == null)
            {
                errors.Add(new Error { error = "Не указан пароль" });
                checkFlag = false;
            }
            else
                if (user.password.Length < 6)
            {
                errors.Add(new Error { error = "Минимальная длина пароля 6 символов" });
                checkFlag = false;
            }

            return checkFlag;
        }

        public static bool CheckUpdate(User user, out List<Error> errors)
        {
            errors = new List<Error>();
            bool checkFlag = true;

            if (user.password != null)
                if (user.password.Length < 6)
                {
                    errors.Add(new Error { error = "Минимальная длина пароля 6 символов" });
                    checkFlag = false;
                }

            var emailChecker = new EmailAddressAttribute();
            if (user.email != null)
                if (!emailChecker.IsValid(user.email))
                {
                    errors.Add(new Error { error = "Неверный формат email адреса" });
                    checkFlag = false;
                }

            if (user.name != null)
                if (user.name.Length < 3)
                {
                    errors.Add(new Error { error = "Минимальная длина имени 3 символа" });
                    checkFlag = false;
                }

            if(user.surname != null)
                if (user.surname.Length < 3)
                {
                    errors.Add(new Error { error = "Минимальная длина фамилии 3 символа" });
                    checkFlag = false;
                }

            return checkFlag;
        }
    }
}