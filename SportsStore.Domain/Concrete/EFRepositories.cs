using System;
using System.Collections.Generic;
using System.Linq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Data;

namespace SportsStore.Domain.Concrete
{

    public class EFAddressRepository : IAddressRepository
    {
        Entities _context = new Entities();
        public IEnumerable<Address> Addresses => _context.Addresses;
        public IEnumerable<City> Cities => _context.Cities;

        public void SaveAddress(Address address)
        {
            if (address.Id == 0)
            {
                _context.Addresses.Add(address);
            }
            else
            {
                var dbEnty = _context.Addresses.Find(address.Id);
                if (dbEnty != null)
                    dbEnty.CityId = address.CityId;
                dbEnty.Code = address.Code;
                dbEnty.Street = address.Street;
                dbEnty.Number = address.Number;
            }
            _context.SaveChanges();
        }

        public Address DeleteAddress(int Id)
        {
            var dbEntry = _context.Addresses.Find(Id);
            if (dbEntry == null) return null;

            _context.Addresses.Remove(dbEntry);
            _context.SaveChanges();
            return dbEntry;
        }
    }


    public class EFCategoryRepository : ICategoryRepository
    {
        Entities _context = new Entities();
        public IEnumerable<Category> Categories => _context.Categories;

        public void SaveCategory(Category category)
        {
            if (category.Id == 0)
            {
                _context.Categories.Add(category);
            }
            else
            {
                var dbEnty = _context.Categories.Find(category.Id);
                if (dbEnty != null)
                    dbEnty.Name = category.Name;
            }
            _context.SaveChanges();
        }

        public Category DeleteCategory(int Id)
        {
            var dbEntry = _context.Categories.Find(Id);
            if (dbEntry == null) return null;

            _context.Categories.Remove(dbEntry);
            _context.SaveChanges();
            return dbEntry;
        }
    }


    public class EFCityRepository : ICityRepository
    {
        Entities _context = new Entities();
        public IEnumerable<City> Cities => _context.Cities;

        public void SaveCity(City city)
        {
            if (city.Id == 0)
            {
                _context.Cities.Add(city);
            }
            else
            {
                var dbEnty = _context.Cities.Find(city.Id);
                if (dbEnty != null)
                    dbEnty.Name = city.Name;
            }
            _context.SaveChanges();
        }

        public City DeleteCity(int Id)
        {
            var dbEntry = _context.Cities.Find(Id);
            if (dbEntry == null) return null;

            _context.Cities.Remove(dbEntry);
            _context.SaveChanges();
            return dbEntry;
        }
    }


    public class EFOrderRepository : IOrderRepository
    {
        Entities _context = new Entities();
        public IEnumerable<Order> Orders => _context.Orders;

        public void SaveOrder(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }
    }
    public class EFOrderListRepository : IOrderListRepository
    {
        Entities _context = new Entities();
        public IEnumerable<OrderList> OrdersList => _context.OrderLists;

        public void SaveOrderList(IEnumerable<OrderList> orderList)
        {
            _context.OrderLists.AddRange(orderList);
            _context.SaveChanges();
        }
    }


    public class EFRoleRepository : IRoleRepository
    {
        Entities _context = new Entities();
        public IEnumerable<Role> Roles => _context.Roles;

        public void SaveRole(Role role)
        {
            if (role.Id == 0)
            {
                _context.Roles.Add(role);
            }
            else
            {
                var dbEnty = _context.Roles.Find(role.Id);
                if (dbEnty != null)
                    dbEnty.Name = role.Name;
            }
            _context.SaveChanges();
        }

        public Role DeleteRole(int Id)
        {
            var dbEntry = _context.Roles.Find(Id);
            if (dbEntry == null) return null;

            _context.Roles.Remove(dbEntry);
            _context.SaveChanges();
            return dbEntry;
        }
    }


    public class EFUserRepository : IUserRepository
    {
        Entities _context = new Entities();
        public IEnumerable<User> Users => _context.Users;
        public IEnumerable<Address> Addresses => _context.Addresses;
        public IEnumerable<Role> Roles => _context.Roles;

        public void SaveUser(User user)
        {
            if (user.Id == 0)
            {
                _context.Users.Add(user);
            }
            else
            {
                var dbEnty = _context.Users.Find(user.Id);
                if (dbEnty != null)
                {
                    dbEnty.AddressId = user.AddressId;
                    dbEnty.Email = user.Email;
                    dbEnty.FirstName = user.Email;
                    dbEnty.LastName = user.LastName;
                    dbEnty.Password = user.Password;
                    dbEnty.Phone = user.Phone;
                    dbEnty.RoleId = user.RoleId;
                    dbEnty.Confirmed = user.Confirmed;
                }
            }
            _context.SaveChanges();
        }

        public User DeleteUser(int Id)
        {
            var dbEntry = _context.Users.Find(Id);
            if (dbEntry == null) return null;

            _context.Users.Remove(dbEntry);
            _context.SaveChanges();
            return dbEntry;
        }

        public void ConfirmEmail(string email)
        {
            var dbEntry = _context.Users.FirstOrDefault(u=>u.Email == email);
            if (dbEntry == null) return;

            dbEntry.Confirmed = true;
            _context.SaveChanges();
        }

        public void ChangePassword(int Id, string password)
        {
            var dbEntry = _context.Users.Find(Id);
            if (dbEntry == null) return;

            dbEntry.Password = password;
            _context.SaveChanges();
        }

        public string ResetPassword(int Id)
        {
            var random = new Random();
            const string input = "ABCDEFGHIJKLMNOPRSTUWZ";
            var buffer = new char[6];
            for (var i = 0; i < 6; i++)
                buffer[i] = input[random.Next(input.Length)];

            var newPassword = new string(buffer);
            ChangePassword(Id,newPassword);
            return newPassword;
        }
    }


    public class EFProductRepository : IProductRepository
    {
        Entities _context = new Entities();
        public IEnumerable<Product> Products => _context.Products;
        public IEnumerable<Category> Categories => _context.Categories;

        public void SaveProduct(Product product)
        {
            if (product.Id == 0)
            {
                _context.Products.Add(product);
            }
            else
            {
                var dbEntry = _context.Products.Find(product.Id);
                if (dbEntry != null)
                {
                    dbEntry.Name = product.Name;
                    dbEntry.Description = product.Description;
                    dbEntry.Price = product.Price;
                    dbEntry.CategoryId = product.CategoryId;
                    dbEntry.ImageData = product.ImageData;
                    dbEntry.ImageMimeType = product.ImageMimeType;
                }
            }
            _context.SaveChanges();
        }

        public Product DeleteProduct(int Id)
        {
            var dbEntry = _context.Products.Find(Id);
            if (dbEntry == null) return null;

            _context.Products.Remove(dbEntry);
            _context.SaveChanges();
            return dbEntry;
        }
    }

    public class EFStatusRepository : IStatusRepository
    {
        Entities _context = new Entities();
        public IEnumerable<Status> Statuses => _context.Status;

        public void SaveStatus(Status status)
        {
            if (status.Id == 0)
            {
                _context.Status.Add(status);
            }
            else
            {
                var dbEnty = _context.Status.Find(status.Id);
                if (dbEnty != null)
                { 
                    dbEnty.Name = status.Name;
                    dbEnty.Description = status.Description;
                }
            }
            _context.SaveChanges();
        }

        public Status DeleteStatus(int Id)
        {
            var dbEntry = _context.Status.Find(Id);
            if (dbEntry == null) return null;

            _context.Status.Remove(dbEntry);
            _context.SaveChanges();
            return dbEntry;
        }
    }

}
