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
            Apis.Add(new API { requireToken = false, url = "Auth/RegisterUser", type = "POST", parameters = "string username [Имя пользователя], string password [Пароль], string email [Email адрес], string name [Имя], string surname [Фамилия]", description = "Регистрация нового пользователя. Фамилию можно опустить.", returns = "ok/JsonArray(errors)" });
            Apis.Add(new API { requireToken = false, url = "Auth/NewGuest", type = "GET", parameters = "-", description = "Гостевая регистрация.", returns = "Json/JsonArray(errors)" });
            Apis.Add(new API { requireToken = false, url = "Auth/Login", type = "POST", parameters = "string username [Имя пользователя], string password [Пароль]", description = "Авторизация. Получение access token", returns = "Json/false" });
            Apis.Add(new API { requireToken = true, url = "Auth/GetNewToken", type = "POST", parameters = "-", description = "Получение нового access token", returns = "Json/false" });
            Apis.Add(new API { requireToken = true, url = "Auth/Logout", type = "POST", parameters = "int allSessions [Выйти из всех сессий (0/1)]", description = "Завершить сессию(-и). Параметр не является обязательным.", returns = "ok/wrong token" });
            Apis.Add(new API { requireToken = true, url = "Auth/NoMoreGuest", type = "POST", parameters = "string username [Имя пользователя], string password [Пароль], string email [Email адрес], string name [Имя], string surname [Фамилия]", description = "Перевод текущей учетной записи из гостевой в постоянную.", returns = "ok/not a guest/JsonArray(errors)" });

            Apis.Add(new API { requireToken = true, url = "User/AddTel", type = "POST", parameters = "string tel [Номер телефона]", description = "Добавление номера телефона текущему пользователю.", returns = "ok/wrong token" });
            Apis.Add(new API { requireToken = true, url = "User/AddAddress", type = "POST", parameters = "string address [Адрес]", description = "Добавление адреса текущему пользователю.", returns = "ok/wrong token" });
            Apis.Add(new API { requireToken = true, url = "User/GetTelList", type = "GET", parameters = "-", description = "Номера телефонов текущего пользователя.", returns = "JsonArray/wrong token" });
            Apis.Add(new API { requireToken = true, url = "User/GetAddressList", type = "GET", parameters = "-", description = "Адреса текущего пользователя.", returns = "JsonArray/wrong token" });
            Apis.Add(new API { requireToken = true, url = "User/Edit", type = "POST", parameters = "string password [Пароль], string email [Email], string name [Имя], string surname [Фамилия]", description = "Редактирование профиля пользователя.", returns = "ok/wrong token/JsonArray(errors)" });
            Apis.Add(new API { requireToken = true, url = "User/RemoveTel", type = "POST", parameters = "int telID [ID номера телефона]", description = "Удаление определенного номера телефона текущего пользователя.", returns = "ok/wrong token" });
            Apis.Add(new API { requireToken = true, url = "User/RemoveAddress", type = "POST", parameters = "int addressID [ID адреса]", description = "Удаление конкретного адреса текущего пользователя.", returns = "ok/wrong token" });
            Apis.Add(new API { requireToken = true, url = "User/EditTel", type = "POST", parameters = "int telID [ID номера телефона], string tel [Новый номер]", description = "Изменение определенного номера телефона текущего пользователя.", returns = "ok/bad argument/number not found/wrong token" });
            Apis.Add(new API { requireToken = true, url = "User/EditAddress", type = "POST", parameters = "int addressID [ID номера телефона], string address [Новый адрес]", description = "Изменение конкретного адреса текущего пользователя.", returns = "ok/bad argument/address not found/wrong token" });
            Apis.Add(new API { requireToken = true, url = "User/GetInfo", type = "POST", parameters = "-", description = "Некоторые данные текущего пользователя.", returns = "Json/wrong token" });

            Apis.Add(new API { requireToken = false, url = "Product/GetPage", type = "GET", parameters = "char orderBy [Правило сортировки], int desc [Сортировка в обратом порядке], int page [Номер страницы], int pageSize [Количество товаров на странице], int category [id категории товаров]", description = "Постраничный вывод товаров. Категорию можно не указывать. Допустимые значения orderBy = c {cost}, t {category/type}, a {advertising}.", returns = "JsonArray/bad argument" });
            Apis.Add(new API { requireToken = false, url = "Product/Pages", type = "GET", parameters = "int pageSize [Количество товаров на странице], int category [id категории товаров]", description = "Количество страниц с товаром, если на каждой странице pageSize товаров. Категорию можно не указывать.", returns = "number/bad argument" });
            Apis.Add(new API { requireToken = false, url = "Product/GetCategoryName", type = "GET", parameters = "int category [id категории товаров]", description = "Возвращает имя категории по ее id.", returns = "Json/bad argument/false" });
            Apis.Add(new API { requireToken = false, url = "Product/GetCategoryList", type = "GET", parameters = "-", description = "Список всех категорий товаров.", returns = "JsonArray" });
            Apis.Add(new API { requireToken = false, url = "Product/GetByName", type = "GET", parameters = "string name [Часть названия товара], int page [Номер страницы], int pageSize [Количество товаров на странице]", description = "Постраничный вывод товаров, которые содержат в своем названии подстроку name.", returns = "JsonArray/bad argument" });
            Apis.Add(new API { requireToken = false, url = "Product/PagesByName", type = "GET", parameters = "string name [Подстрока из названия товара], int pageSize [Количество товаров на странице]", description = "Количество страниц с товаром, если на каждой странице pageSize товаров. Выбираются товары, содержащие в названии строку 'name'.", returns = "number/bad argument" });

            Apis.Add(new API { requireToken = true, url = "ShopCart/AddProduct", type = "POST", parameters = "int productID [ID товара], int amount [Количество]", description = "Добавление товара в корзину текущего пользователя.", returns = "ok/wrong token/bad argument/product not found" });
            Apis.Add(new API { requireToken = true, url = "ShopCart/RemoveProduct", type = "POST", parameters = "int productID [ID товара]", description = "Удаление товара из корзины текущего пользователя.", returns = "ok/wrong token/bad argument" });
            Apis.Add(new API { requireToken = true, url = "ShopCart/EditProduct", type = "POST", parameters = "int productID [ID товара], int amount [Количество]", description = "Изменение количества товара в корзине текущего пользователя.", returns = "ok/wrong token/bad argument/product not found" });
            Apis.Add(new API { requireToken = true, url = "ShopCart/ProductExists", type = "POST", parameters = "int productID [ID товара]", description = "Проверка наличия товара в корзине текущего пользователя.", returns = "Json/wrong token/bad argument" });
            Apis.Add(new API { requireToken = true, url = "ShopCart/Clear", type = "POST", parameters = "-", description = "Очистка корзины текущего пользователя.", returns = "ok/wrong token" });
            Apis.Add(new API { requireToken = true, url = "ShopCart/Show", type = "POST", parameters = "string promocode [Промокод]", description = "Список товаров в корзине текущего пользователя.", returns = "JsonArray/wrong token" });
            Apis.Add(new API { requireToken = true, url = "ShopCart/MakeOrder", type = "POST", parameters = "string promocode [Промокод], int addressID [ID адреса доставки]", description = "Сделать заказ товаров из корзины.", returns = "Json/wrong token/bad promocode/bad address/attempt to order unavailable" });
            Apis.Add(new API { requireToken = true, url = "ShopCart/OrderProduct", type = "POST", parameters = "string promocode [Промокод], int addressID [ID адреса доставки], int productID [ID товара], int amount [Количество единиц товара]", description = "Сделать заказ определенного товара.", returns = "Json/wrong token/bad promocode/bad address/attempt to order unavailable" });

            Apis.Add(new API { requireToken = true, url = "Order/GetPage", type = "POST", parameters = "int page [Номер страницы], int pageSize [Количество заказов на странице]", description = "Постраничный вывод заказов текущего пользователя.", returns = "JsonArray/wrong token/bad argument" });
            Apis.Add(new API { requireToken = true, url = "Order/Pages", type = "POST", parameters = "int pageSize [Количество заказов на странице]", description = "Количество страниц с заказами, если на каждой странице pageSize заказов.", returns = "number/wrong token/bad argument" });
            Apis.Add(new API { requireToken = true, url = "Order/Products", type = "POST", parameters = "int orderNO [ID заказа]", description = "Вывод товаров из определенного заказа текущего пользователя.", returns = "JsonArray/wrong token/bad argument" });
        }
    }
}