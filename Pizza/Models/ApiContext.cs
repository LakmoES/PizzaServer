using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pizza.Models
{
    public class ApiContext
    {
        public List<API> Apis { private set; get; }
        public ApiContext()
        {
            Apis = new List<API>();
            Apis.Add(new API { requireToken = false, url = "Auth/RegisterUser", type = "POST (сейчас есть GET)", parameters = "string username [Имя пользователя], string password [Пароль], string email [Email адрес], string name [Имя], string surname [Фамилия]", description = "Регистрация нового пользователя. Фамилию можно опустить.", returns = "ok/JsonArray(errors)" });
            Apis.Add(new API { requireToken = false, url = "Auth/NewGuest", type = "GET", parameters = "-", description = "Гостевая регистрация.", returns = "Json/JsonArray(errors)" });
            Apis.Add(new API { requireToken = false, url = "Auth/Login", type = "POST (сейчас есть GET)", parameters = "string username [Имя пользователя], string password [Пароль.md5]", description = "Авторизация. Получение access token", returns = "Json/false" });
            Apis.Add(new API { requireToken = false, url = "Auth/GetNewToken", type = "POST (сейчас есть GET)", parameters = "string token [Access token]", description = "Получение нового access token", returns = "Json/false" });

            Apis.Add(new API { requireToken = true, url = "User/AddTel", type = "POST (сейчас есть GET)", parameters = "string tel [Номер телефона]", description = "Добавление номера телефона текущему пользователю.", returns = "ok/wrong token" });
            Apis.Add(new API { requireToken = true, url = "User/AddAddress", type = "POST (сейчас есть GET)", parameters = "string address [Адрес]", description = "Добавление адреса текущему пользователю.", returns = "ok/wrong token" });
            Apis.Add(new API { requireToken = true, url = "User/GetTelList", type = "GET", parameters = "-", description = "Номера телефонов текущего пользователя.", returns = "JsonArray/wrong token" });
            Apis.Add(new API { requireToken = true, url = "User/GetAddressList", type = "GET", parameters = "-", description = "Адреса текущего пользователя.", returns = "JsonArray/wrong token" });
        }
    }
}