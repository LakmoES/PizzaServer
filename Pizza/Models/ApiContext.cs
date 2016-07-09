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
            Apis.Add(new API { requireToken = false, url = "Auth/Logout", type = "POST (сейчас есть GET)", parameters = "string token [Access token], int allSessions [Выйти из всех сессий (0/1)]", description = "Завершить сессию(-и). Второй параметр не является обязательным.", returns = "ok/wrong token" });
            Apis.Add(new API { requireToken = true, url = "Auth/NoMoreGuest", type = "POST (сейчас есть GET)", parameters = "string username [Имя пользователя], string password [Пароль], string email [Email адрес], string name [Имя], string surname [Фамилия]", description = "Перевод текущей учетной записи из гостевой в постоянную.", returns = "ok/not a guest/JsonArray(errors)" });

            Apis.Add(new API { requireToken = true, url = "User/AddTel", type = "POST (сейчас есть GET)", parameters = "string tel [Номер телефона]", description = "Добавление номера телефона текущему пользователю.", returns = "ok/wrong token" });
            Apis.Add(new API { requireToken = true, url = "User/AddAddress", type = "POST (сейчас есть GET)", parameters = "string address [Адрес]", description = "Добавление адреса текущему пользователю.", returns = "ok/wrong token" });
            Apis.Add(new API { requireToken = true, url = "User/GetTelList", type = "GET", parameters = "-", description = "Номера телефонов текущего пользователя.", returns = "JsonArray/wrong token" });
            Apis.Add(new API { requireToken = true, url = "User/GetAddressList", type = "GET", parameters = "-", description = "Адреса текущего пользователя.", returns = "JsonArray/wrong token" });
            Apis.Add(new API { requireToken = true, url = "User/Edit", type = "POST (сейчас есть GET)", parameters = "string password [Пароль], string email [Email], string name [Имя], string surname [Фамилия]", description = "Редактирование профиля пользователя.", returns = "ok/wrong token/nothing to edit" });
            Apis.Add(new API { requireToken = true, url = "User/RemoveTel", type = "POST (сейчас есть GET)", parameters = "int telID [ID номера телефона]", description = "Удаление определенного номера телефона текущего пользователя.", returns = "ok/wrong token" });
            Apis.Add(new API { requireToken = true, url = "User/RemoveAddress", type = "POST (сейчас есть GET)", parameters = "int addressID [ID адреса]", description = "Удаление конкретного адреса текущего пользователя.", returns = "ok/wrong token" });
            Apis.Add(new API { requireToken = true, url = "User/EditTel", type = "POST (сейчас есть GET)", parameters = "int telID [ID номера телефона], string tel [Новый номер]", description = "Изменение определенного номера телефона текущего пользователя.", returns = "ok/bad argument/number not found/wrong token" });
            Apis.Add(new API { requireToken = true, url = "User/EditAddress", type = "POST (сейчас есть GET)", parameters = "int addressID [ID номера телефона], string address [Новый адрес]", description = "Изменение конкретного адреса текущего пользователя.", returns = "ok/bad argument/address not found/wrong token" });

            Apis.Add(new API { requireToken = false, url = "Product/GetPage", type = "GET", parameters = "int page [Номер страницы], int pageSize [Количество товаров на странице], int category [id категории товаров]", description = "Постраничный вывод товаров. Категорию можно не указывать.", returns = "JsonArray/bad argument" });
            Apis.Add(new API { requireToken = false, url = "Product/Pages", type = "GET", parameters = "int pageSize [Количество товаров на странице], int category [id категории товаров]", description = "Количество страниц с товаром, если на каждой странице pageSize товаров. Категорию можно не указывать.", returns = "number/bad argument" });
            Apis.Add(new API { requireToken = false, url = "Product/GetCategoryName", type = "GET", parameters = "int category [id категории товаров]", description = "Возвращает имя категории по ее id.", returns = "Json/bad argument/false" });
            Apis.Add(new API { requireToken = false, url = "Product/GetCategoryList", type = "GET", parameters = "-", description = "Список всех категорий товаров.", returns = "JsonArray" });
            Apis.Add(new API { requireToken = false, url = "Product/GetByName", type = "GET", parameters = "string name [Часть названия товара], int page [Номер страницы], int pageSize [Количество товаров на странице]", description = "Постраничный вывод товаров, которые содержат в своем названии подстроку name.", returns = "JsonArray/bad argument" });            
        }
    }
}